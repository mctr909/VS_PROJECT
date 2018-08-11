namespace MIDI {
	unsafe public class Sampler {
		private Channel mChannel;
		private byte mNoteNo;
		private double mDelta;
		private double mVelocity;

		private WaveInfo mWaveInfo;

		private bool mIsActive;
		private bool mOnKey;

		private Envelope mEnvAmp;
		private Envelope mEnvCutoff;

		private double mCurEnvAmp;
		private double mCurEnvCutoff;

		private Filter mEnvFilter;

		private Filter mCtrlFilter;
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

			mEnvAmp = new Envelope();
			mEnvCutoff = new Envelope();

			mCurEnvAmp = 0.0;
			mCurEnvCutoff = 1.0;

			mEnvFilter = new Filter(1.0, 0.0);

			mCtrlFilter = new Filter(1.0, 0.0);
			mCurCtrlCutoff = 1.0;
			mCurCtrlResonance = 0.0;

			mCurTime = 0.0;
			mCurIndex = 0.0;
		}

		public void NoteOn(Channel channel, byte noteNo, byte velocity) {
			mChannel = channel;
			mNoteNo = noteNo;
			mCurTime = 0.0;
			mCurIndex = 0.0;
			mWaveInfo = channel.WaveList[noteNo];
			mVelocity = mWaveInfo.Gain * Const.Amp[velocity];

			var note = noteNo - mWaveInfo.BaseNoteNo;
			if (note < 0) {
				mDelta = mWaveInfo.Delta / Const.SemiTone[-note];
			}
			else {
				mDelta = mWaveInfo.Delta * Const.SemiTone[note];
			}

			mIsActive = true;
			mOnKey = true;

			//
			mEnvAmp = mWaveInfo.EnvAmp;
			mEnvCutoff = mWaveInfo.EnvCutoff;

			mCurEnvAmp = mEnvAmp.LevelA;
			mCurEnvCutoff = mEnvCutoff.LevelA;

			mEnvFilter.Clear();
			mEnvFilter.Cutoff = mCurEnvCutoff;
			mEnvFilter.Resonance = mWaveInfo.Resonance;

			//
			mCurCtrlCutoff = mChannel.FcD;
			mCurCtrlResonance = mChannel.FqD;

			mCtrlFilter.Clear();
			mCtrlFilter.Cutoff = mCurCtrlCutoff;
			mCtrlFilter.Resonance = mCurCtrlResonance;
		}

		public void NoteOff() {
			mOnKey = false;
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
			var wave = mVelocity * mCurEnvAmp * (mWaveInfo.Buff[cur] * dt + mWaveInfo.Buff[pre] * (1.0 - dt)) / 32768.0;

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
				if (mCurTime < (mEnvAmp.TimeA + mEnvAmp.TimeH)) {
					mCurEnvAmp += (mEnvAmp.LevelH - mCurEnvAmp) * mEnvAmp.DeltaA;
				}
				else {
					mCurEnvAmp += (mEnvAmp.LevelS - mCurEnvAmp) * mEnvAmp.DeltaD;
				}

				if (mCurTime < (mEnvCutoff.TimeA + mEnvCutoff.TimeH)) {
					mCurEnvCutoff += (mEnvCutoff.LevelH - mCurEnvCutoff) * mEnvCutoff.DeltaA;
				}
				else {
					mCurEnvCutoff += (mEnvCutoff.LevelS - mCurEnvCutoff) * mEnvCutoff.DeltaD;
				}
			}
			else {
				mCurEnvAmp -= mCurEnvAmp * mEnvAmp.DeltaR * mChannel.Hld;
				mCurEnvCutoff += (mEnvCutoff.LevelR - mCurEnvCutoff) * mEnvCutoff.DeltaR;

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
			mCurIndex += mDelta * mChannel.PitchD;
			if (mWaveInfo.LoopEnd <= mCurIndex) {
				mCurIndex = mWaveInfo.LoopBegin + mCurIndex - (int)mCurIndex;
				if (!mWaveInfo.LoopEnable) {
					mIsActive = false;
				}
			}
		}
	}
}