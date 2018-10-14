using System;
using System.IO;

namespace DLSeditor {
	unsafe public class WavePlayback : WaveOutLib {
		public int mLoopBegin;
		public int mLoopEnd;
		private short[] mWave;
		private double mDelta;
		private double mTime;

		public Spectrum Spectrum;

		public WavePlayback() {
			mWave = new short[1];
			Stop();
		}

		public void SetValue(DLS.WAVE wave) {
			mWave = new short[8 * wave.Data.Length / wave.Format.Bits];

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

			mDelta = (double)wave.Format.SampleRate / SampleRate;
			mTime = 0.0;
		}

		public void Stop() {
			mWave = new short[1];
			mLoopBegin = 0;
			mLoopEnd = 0;
			mDelta = 0.0;
			mTime = 0.0;
			Spectrum = new Spectrum((uint)SampleRate, 27.5, 12, 112);
		}

		protected override void SetData() {
			for (var i = 0; i < BufferSize; i += 2) {
				var wave = ((int)mTime < mWave.Length) ? (mWave[(int)mTime] / 32768.0) : 0.0;
				WaveBuffer[i] = (short)(32767 * wave);
				WaveBuffer[i + 1] = (short)(32767 * wave);

				for (uint b = 0; b < Spectrum.Banks; ++b) {
					Spectrum.Filtering(b, wave);
				}

				mTime += mDelta;
				if (mLoopEnd <= mTime) {
					mTime = mLoopBegin + mTime - mLoopEnd;
				}
			}

			Spectrum.SetLevel();
		}
	}
}