using System;
using System.IO;

namespace DLSeditor {
	unsafe public class WavePlayback : WaveOutLib {
		public int mLoopBegin;
		public int mLoopEnd;
		public double mPitch;

		private short[] mWave;
		private int mSampleRate;
		private double mDelta;
		private double mTime;
		private int mFftIndex;
		private FFT mFft;

		public WavePlayback() {
			mWave = new short[1];
			mFft = new FFT(16384, SampleRate);
			Stop();
		}

		public void SetValue(DLS.WAVE wave) {
			mWave = new short[8 * wave.Data.Length / wave.Format.Bits];
			mSampleRate = (int)wave.Format.SampleRate;
			var br = new BinaryReader(new MemoryStream(wave.Data));

			switch (wave.Format.Bits) {
			case 8:
				for (var i = 0; i < mWave.Length; ++i) {
					mWave[i] = (short)((br.ReadByte() - 128) * 256);
				}
				break;
			case 16:
				for (var i = 0; i < mWave.Length; ++i) {
					mWave[i] = br.ReadInt16();
				}
				break;
			default:
				return;
			}
		}

		public void Play() {
			mDelta = (double)mSampleRate / SampleRate;
			mTime = 0.0;
		}

		public void Stop() {
			mLoopBegin = 0;
			mLoopEnd = 0;
			mDelta = 0.0;
			mTime = 0.0;
		}

		protected override void SetData() {
			for (var i = 0; i < BufferSize; i += 2) {
				var wave = ((int)mTime < mWave.Length) ? (mWave[(int)mTime] / 32768.0) : 0.0;
				WaveBuffer[i] = (short)(32767 * wave);
				WaveBuffer[i + 1] = (short)(32767 * wave);

				mFft.Re[mFftIndex] = wave;
				mFft.Im[mFftIndex] = 0.0;
				++mFftIndex;
				if(16384 <= mFftIndex) {
					mFftIndex = 0;
					mPitch = mFft.Pitch();
				}

				mTime += mDelta;
				if (mLoopEnd <= mTime) {
					mTime = mLoopBegin + mTime - mLoopEnd;
				}
			}
		}
	}
}