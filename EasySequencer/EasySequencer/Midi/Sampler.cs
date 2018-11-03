﻿namespace MIDI {
	unsafe public class Sampler {
		private Channel mChannel;
		private double mVelocity;
		private bool mOnKey;

		private WaveInfo mWaveInfo;
		private Filter mEnvFilter;

		private double mCurEnvAmp;
		private double mCurEnvCutoff;
		private double mCurTime;
		private double mCurIndex;

		public int ChannelNo {
			get { return (null == mChannel) ? 0 : mChannel.No; }
		}

		public byte NoteNo { get; private set; }

		public bool IsActive { get; private set; }

		public bool IsSuspend { get; private set; }

		public Sampler() {
			mChannel = null;
			NoteNo = 0;
			mVelocity = 0.0;

			IsActive = false;
			mOnKey = false;

			mWaveInfo.EnvAmp = new Envelope();
			mWaveInfo.EnvCutoff = new Envelope();
			mEnvFilter = new Filter(1.0, 0.0);

			mCurEnvAmp = 0.0;
			mCurEnvCutoff = 1.0;
			mCurTime = 0.0;
			mCurIndex = 0.0;
		}

		public void NoteOff() {
			mOnKey = false;
		}

		public void NoteOn(Channel channel, byte noteNo, byte velocity) {
			IsSuspend = true;
			mChannel = channel;
			NoteNo = noteNo;
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

			//
			mEnvFilter.Clear();
			mEnvFilter.Cutoff = mCurEnvCutoff;
			mEnvFilter.Resonance = mWaveInfo.Resonance;

			//
			mOnKey = true;
			IsActive = true;
			IsSuspend = false;
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

				if (mCurEnvAmp < 0.01) {
					IsActive = false;
				}
			}

			//
			mCurTime += Const.DeltaTime;
			mCurIndex += mWaveInfo.Delta * mChannel.PitchD;
			if (mWaveInfo.LoopEnd < mCurIndex) {
				mCurIndex -= mWaveInfo.LoopLength;
				if (!mWaveInfo.LoopEnable) {
					IsActive = false;
				}
			}
		}
	}
}