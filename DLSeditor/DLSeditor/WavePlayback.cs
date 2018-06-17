using System;
using System.IO;

namespace DLSeditor
{
	public class WavePlayback : WaveOutLib
	{
		private short[] mWave;
		private uint mLoopBegin;
		private uint mLoopEnd;
		private double mDelta;
		private double mTime;

		public Spectrum Spectrum;

		public WavePlayback()
		{
			mWave = new short[1];
			Stop();
		}

		//public int Position
		//{
		//	get {
		//		return (int)mTime;
		//	}
		//}

		public void SetValue(DLS.WAVE wave)
		{
			mWave = new short[8 * wave.Data.Length / wave.Format.BitsPerSample];
			mLoopBegin = 0;
			mLoopEnd = (uint)mWave.Length;

			var br = new BinaryReader(new MemoryStream(wave.Data));

			switch (wave.Format.BitsPerSample) {
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

			if (0 < wave.Samplers.LoopCount) {
				mLoopBegin = wave.Samplers.Loops[0].Start;
				mLoopEnd = mLoopBegin + wave.Samplers.Loops[0].Length;
			}

			mDelta = (double)wave.Format.SampleRate / SampleRate;
			mTime = 0.0;
		}

		public void Stop()
		{
			mWave = new short[1];
			mLoopBegin = 0;
			mLoopEnd = 0;
			mDelta = 0.0;
			mTime = 0.0;
			Spectrum = new Spectrum((uint)SampleRate, 27.5, 12, 112);
		}

		protected override void SetData()
		{
			for (var i = 0; i < BufferSize; i += 2) {
				var wave = ((int)mTime < mWave.Length) ? (mWave[(int)mTime] / 32768.0) : 0.0;
				WaveBuffer[i] = (short)(32767 * wave);
				WaveBuffer[i + 1] = (short)(32767 * wave);

				for (UInt32 b = 0; b < Spectrum.Banks; ++b) {
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
