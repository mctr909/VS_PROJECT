namespace MIDI {
	unsafe public class Sampler {
		private Channel mChannel;
		private byte mNoteNo;
		private double mVelocity;

		private bool mIsActive;
		private bool mOnKey;

		private WaveInfo mWaveInfo;
		private Filter mEnvFilter;
		private Filter mCtrlFilter;

		private double mCurEnvAmp;
		private double mCurEnvCutoff;
		private double mCurCtrlCutoff;
		private double mCurCtrlResonance;
		private double mCurTime;
		private double mCurIndex;

		public int ChannelNo {
			get { return (null == mChannel) ? 0 : mChannel.No; }
		}

		public byte NoteNo {
			get { return mNoteNo; }
		}

		public bool IsActive {
			get { return mIsActive; }
		}

		public Sampler() {
			mChannel = null;
			mNoteNo = 0;
			mVelocity = 0.0;

			mIsActive = false;
			mOnKey = false;

			mWaveInfo.EnvAmp = new Envelope();
			mWaveInfo.EnvCutoff = new Envelope();
			mEnvFilter = new Filter(1.0, 0.0);
			mCtrlFilter = new Filter(1.0, 0.0);

			mCurEnvAmp = 0.0;
			mCurEnvCutoff = 1.0;
			mCurCtrlCutoff = 1.0;
			mCurCtrlResonance = 0.0;
			mCurTime = 0.0;
			mCurIndex = 0.0;
		}

		public void NoteOff() {
			mOnKey = false;
		}

		public void NoteOn(Channel channel, byte noteNo, byte velocity) {
			mChannel = channel;
			mNoteNo = noteNo;
			mWaveInfo = channel.WaveList[noteNo];
			mVelocity = mWaveInfo.Gain * Const.Amp[velocity] / 32768.0;

			var note = noteNo - mWaveInfo.BaseNoteNo;
			if (note < 0) {
				mWaveInfo.Delta /= Const.SemiTone[-note];
			}
			else {
				mWaveInfo.Delta *= Const.SemiTone[note];
			}

			//
			mCurTime = 0.0;
			mCurIndex = 0.0;
			mCurEnvAmp = mWaveInfo.EnvAmp.LevelA;
			mCurEnvCutoff = mWaveInfo.EnvCutoff.LevelA;
			mCurCtrlCutoff = mChannel.FcD;
			mCurCtrlResonance = mChannel.FqD;

			//
			mEnvFilter.Clear();
			mEnvFilter.Cutoff = mCurEnvCutoff;
			mEnvFilter.Resonance = mWaveInfo.Resonance;

			mCtrlFilter.Clear();
			mCtrlFilter.Cutoff = mCurCtrlCutoff;
			mCtrlFilter.Resonance = mCurCtrlResonance;

			//
			mIsActive = true;
			mOnKey = true;
		}

		public void Output() {
			if (null == mWaveInfo.Buff) {
				return;
			}

			//
			var cur = (int)mCurIndex;
			var pre = cur - 1;
			var dt = mCurIndex - cur;
			if (pre < 0) {
				pre += (int)mWaveInfo.LoopEnd;
			}

			//
			var wave = mVelocity * mCurEnvAmp * (mWaveInfo.Buff[cur] * dt + mWaveInfo.Buff[pre] * (1.0 - dt));

			//
			mEnvFilter.Cutoff = mCurEnvCutoff;
			mEnvFilter.Step(wave, ref wave);

			//
			mCtrlFilter.Cutoff = mCurCtrlCutoff;
			mCtrlFilter.Resonance = mCurCtrlResonance;
			mCtrlFilter.Step(wave, ref wave);

			//
			mChannel.Wave += wave;

			//
			if (mOnKey) {
				if (mCurTime < (mWaveInfo.EnvAmp.TimeA + mWaveInfo.EnvAmp.TimeH)) {
					mCurEnvAmp += (mWaveInfo.EnvAmp.LevelH - mCurEnvAmp) * mWaveInfo.EnvAmp.DeltaA;
				}
				else {
					mCurEnvAmp += (mWaveInfo.EnvAmp.LevelS - mCurEnvAmp) * mWaveInfo.EnvAmp.DeltaD;
				}

				if (mCurTime < (mWaveInfo.EnvCutoff.TimeA + mWaveInfo.EnvCutoff.TimeH)) {
					mCurEnvCutoff += (mWaveInfo.EnvCutoff.LevelH - mCurEnvCutoff) * mWaveInfo.EnvCutoff.DeltaA;
				}
				else {
					mCurEnvCutoff += (mWaveInfo.EnvCutoff.LevelS - mCurEnvCutoff) * mWaveInfo.EnvCutoff.DeltaD;
				}
			}
			else {
				mCurEnvAmp -= mCurEnvAmp * mWaveInfo.EnvAmp.DeltaR * mChannel.Hld;
				mCurEnvCutoff += (mWaveInfo.EnvCutoff.LevelR - mCurEnvCutoff) * mWaveInfo.EnvCutoff.DeltaR;

				if (mCurEnvAmp < 0.001) {
					mIsActive = false;
				}
			}

			//
			mCurCtrlCutoff += 10.0 * (mChannel.FcD - mCurCtrlCutoff) * Const.DeltaTime;
			if (1.0 < mCurCtrlCutoff) {
				mCurCtrlCutoff = 1.0;
			}
			mCurCtrlResonance += 10.0 * (mChannel.FqD - mCurCtrlResonance) * Const.DeltaTime;
			if (1.0 < mCurCtrlResonance) {
				mCurCtrlResonance = 1.0;
			}

			//
			mCurTime += Const.DeltaTime;
			mCurIndex += mWaveInfo.Delta * mChannel.PitchD;
			if (mWaveInfo.LoopEnd < mCurIndex) {
				mCurIndex -= mWaveInfo.LoopLength;
				if (!mWaveInfo.LoopEnable) {
					mIsActive = false;
				}
			}
		}
	}
}