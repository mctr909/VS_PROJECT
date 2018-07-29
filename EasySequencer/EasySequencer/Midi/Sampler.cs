﻿namespace MIDI {
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

		private Filter mFilter;
		private double mCurAmp;
		private double mCurCutoff;
		private double mCurResonance;

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

			mFilter = new Filter(1.0, 0.0);
			mCurAmp = 0.0;
			mCurCutoff = 0.0;
			mCurResonance = 0.0;

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

			var note = noteNo - mWaveInfo.BaseNote;
			if (note < 0) {
				mDelta = mWaveInfo.Delta / Const.SemiTone[-note];
			}
			else {
				mDelta = mWaveInfo.Delta * Const.SemiTone[note];
			}

			mIsActive = true;
			mOnKey = true;

			mEnvAmp = channel.EnvAmp;
			mCurAmp = mEnvAmp.ALevel;

			mEnvCutoff = channel.EnvCutoff;
			mCurCutoff = mEnvCutoff.ALevel * mChannel.FcD;

			var tempCutoff = mCurCutoff;
			if (1.0 < tempCutoff) {
				tempCutoff = 1.0;
			}

			var tempResonance = mChannel.FqD;
			if (1.0 < tempResonance) {
				tempResonance = 1.0;
			}

			mFilter.Clear();
			mFilter.Cutoff = tempCutoff;
			mFilter.Resonance = tempResonance;
		}

		public void NoteOff() {
			mOnKey = false;
		}

		public void NoteOutput() {
			if (null == mWaveInfo.Buff) {
				return;
			}

			var cur = (int)mCurIndex;
			var pre = cur - 1;
			var dt = mCurIndex - cur;
			if (pre < 0) {
				pre += (int)mWaveInfo.LoopEnd;
			}

			double wave = mVelocity * mCurAmp * (mWaveInfo.Buff[cur] * dt + mWaveInfo.Buff[pre] * (1.0 - dt)) / 32768.0;

			mFilter.Cutoff = mCurCutoff;
			mFilter.Resonance = mCurResonance;
			mFilter.Step(wave, ref wave);

			mChannel.Wave += wave;

			if (mOnKey) {
				if (mCurTime < mEnvAmp.ATime) {
					mCurAmp += (mEnvAmp.DLevel - mCurAmp) * mEnvAmp.ADelta * Const.DeltaTime;
				}
				else {
					mCurAmp += (mEnvAmp.SLevel - mCurAmp) * mEnvAmp.DDelta * Const.DeltaTime;
				}

				if (mCurTime < mEnvCutoff.ATime) {
					mCurCutoff += (mEnvCutoff.DLevel * mChannel.FcD - mCurCutoff) * mEnvCutoff.ADelta * Const.DeltaTime;
				}
				else {
					mCurCutoff += (mEnvCutoff.SLevel * mChannel.FcD - mCurCutoff) * mEnvCutoff.DDelta * Const.DeltaTime;
				}
			}
			else {
				mCurAmp -= mCurAmp * mEnvAmp.RDelta * mChannel.Hld * Const.DeltaTime;
				mCurCutoff += (mEnvCutoff.RLevel * mChannel.FcD - mCurCutoff) * mEnvCutoff.RDelta * Const.DeltaTime;

				if (mCurAmp < 0.001) {
					mIsActive = false;
				}
			}

			if (1.0 < mCurCutoff) {
				mCurCutoff = 1.0;
			}

			mCurResonance += 10.0 * (mChannel.FqD - mCurResonance) * Const.DeltaTime;
			if (1.0 < mCurResonance) {
				mCurResonance = 1.0;
			}

			mCurTime += Const.DeltaTime;
			mCurIndex += mDelta * mChannel.PitchD;
			if (mWaveInfo.LoopEnd <= mCurIndex) {
				mCurIndex = mWaveInfo.LoopBegin + mCurIndex - (int)mCurIndex;
				if (!mWaveInfo.LoopEnable) {
					mIsActive = false;
				}
			}
		}

		public void DrumOutput() {
			if(null == mWaveInfo.Buff) {
				return;
			}

			var cur = (int)mCurIndex;
			var pre = cur - 1;
			var dt = mCurIndex - cur;
			if (pre < 0) {
				pre += (int)mWaveInfo.LoopEnd;
			}

			mChannel.Wave += mVelocity * mCurAmp * (mWaveInfo.Buff[cur] * dt + mWaveInfo.Buff[pre] * (1.0 - dt)) / 32768.0;

			mCurAmp -= mCurAmp * 6.0 * Const.DeltaTime;
			if (mCurAmp < 0.001) {
				mIsActive = false;
			}

			mCurTime += Const.DeltaTime;
			mCurIndex += mDelta * mChannel.PitchD;
			if (mWaveInfo.LoopEnd <= mCurIndex) {
				mCurIndex = mWaveInfo.LoopBegin + mCurIndex - (int)mCurIndex;
				if (!mWaveInfo.LoopEnable) {
					mIsActive = false;
					return;
				}
			}
		}
	}
}