namespace MIDI {
	unsafe public class Sampler {
		private Channel mChannel;
		private byte mNoteNo;
		private double mDelta;
		private double mVelocity;

		private WaveInfo mWaveInfo;

		private bool mIsActive;
		private bool mOnKey;

		private Filter mFilter;
		private double mCurAmp;
		private double mCurCutoff;
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

			mFilter = new Filter(1.0, 0.0);
			mCurAmp = 0.0;
			mCurCutoff = 0.0;
			mCurTime = 0.0;
			mCurIndex = 0.0;
		}

		public void NoteOn(Channel channel, byte noteNo, byte velocity) {
			mChannel = channel;
			mNoteNo = noteNo;
			mWaveInfo = channel.WaveList[noteNo];
			mVelocity = mWaveInfo.Gain * Const.Amp[velocity];

			var note = noteNo - mWaveInfo.BaseNote;
			if (note < 0) {
				mDelta = mWaveInfo.Delta / Const.SemiTone[-note];
			}
			else {
				mDelta = mWaveInfo.Delta * Const.SemiTone[note];
			}

			mIsActive = true;
			mOnKey = true;

			mCurAmp = channel.EnvAmp.ALevel;
			mCurCutoff = channel.EnvCutoff.ALevel * mChannel.Fc;

			var tempCutoff = mCurCutoff;
			if (1.0 < tempCutoff) {
				tempCutoff = 1.0;
			}

			var tempResonance = mChannel.Fq;
			if (1.0 < tempResonance) {
				tempResonance = 1.0;
			}

			mFilter.Clear();
			mFilter.Cutoff = tempCutoff;
			mFilter.Resonance = tempResonance;

			mCurTime = 0.0;
			mCurIndex = 0.0;
		}

		public void NoteOff() {
			mOnKey = false;
		}

		public void NoteOutput() {
			if (mOnKey) {
				if (mCurTime < mChannel.EnvAmp.ATime) {
					mCurAmp += (mChannel.EnvAmp.DLevel - mCurAmp) * mChannel.EnvAmp.ADelta * mChannel.DeltaTime;
				}
				else {
					mCurAmp += (mChannel.EnvAmp.SLevel - mCurAmp) * mChannel.EnvAmp.DDelta * mChannel.DeltaTime;
				}

				if (mCurTime < mChannel.EnvCutoff.ATime) {
					mCurCutoff += (mChannel.EnvCutoff.DLevel * mChannel.Fc - mCurCutoff) * mChannel.EnvCutoff.ADelta * mChannel.DeltaTime;
				}
				else {
					mCurCutoff += (mChannel.EnvCutoff.SLevel * mChannel.Fc - mCurCutoff) * mChannel.EnvCutoff.DDelta * mChannel.DeltaTime;
				}
			}
			else {
				mCurAmp -= mCurAmp * mChannel.EnvAmp.RDelta * mChannel.Hld * mChannel.DeltaTime;
				mCurCutoff += (mChannel.EnvCutoff.RLevel * mChannel.Fc - mCurCutoff) * mChannel.EnvCutoff.RDelta * mChannel.DeltaTime;
				if (mCurAmp < 0.001) {
					mIsActive = false;
				}
			}

			mCurIndex += mDelta * mChannel.PitchD;
			if (mWaveInfo.LoopEnd <= mCurIndex) {
				mCurIndex = mWaveInfo.LoopBegin + mCurIndex - (int)mCurIndex;
			}

			var indexCur = (int)mCurIndex;
			var indexPre = indexCur - 1;
			var dt = mCurIndex - indexCur;
			if (indexPre < 0) {
				indexPre += (int)mWaveInfo.LoopEnd;
			}

			double temp = mCurAmp * mVelocity * (mWaveInfo.Buff[indexCur] * dt + mWaveInfo.Buff[indexPre] * (1.0 - dt)) / 32768.0;

			var tempCutoff = mCurCutoff;
			if (1.0 < tempCutoff) {
				tempCutoff = 1.0;
			}

			var tempResonance = mChannel.Fq;
			if (1.0 < tempResonance) {
				tempResonance = 1.0;
			}

			mFilter.Cutoff = tempCutoff;
			mFilter.Resonance = tempResonance;
			mFilter.Step(temp, ref temp);

			mChannel.Wave += temp;
			mCurTime += mChannel.DeltaTime;
		}

		public void DrumOutput() {
			if (mOnKey) {
				mCurAmp = 1.0;
			}
			else {
				mCurAmp -= mCurAmp * 4.0 * mChannel.DeltaTime;
				if (mCurAmp < 0.001) {
					mIsActive = false;
				}
			}

			mCurIndex += mDelta * mChannel.PitchD;
			if (mWaveInfo.LoopEnd <= mCurIndex) {
				mIsActive = false;
				return;
			}

			var indexCur = (int)mCurIndex;
			var indexPre = indexCur - 1;
			var dt = mCurIndex - indexCur;
			if (indexPre < 0) {
				indexPre += mWaveInfo.Buff.Length;
			}

			mChannel.Wave += mVelocity * mCurAmp * (mWaveInfo.Buff[indexCur] * dt + mWaveInfo.Buff[indexPre] * (1.0 - dt)) / 32768.0;
			mCurTime += mChannel.DeltaTime;
		}
	}
}