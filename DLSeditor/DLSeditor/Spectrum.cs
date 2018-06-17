using System;

public class Spectrum
{
	private class BANK
	{
		public double a1;
		public double a2;
		public double b0;
		public double b1;
		public double b2;

		public double aDelay1;
		public double aDelay2;
		public double bDelay1;
		public double bDelay2;

		public double amplitude;
		public double attenuation;
	}

	private double mFreqToOmega = 0.0;
	private BANK[] mBanks;
	private double[] mLevel;
	private double mMax;

	private readonly double mScale;
	private readonly double mAttenuation;

	public int Banks
	{
		get {
			return mBanks.Length;
		}
	}

	public double[] Level
	{
		get {
			return mLevel;
		}
	}

	public double Max
	{
		get {
			return mMax;
		}
	}

	public Spectrum(UInt32 sampleRate, double baseFreq, UInt32 octDiv, UInt32 banks)
	{
		mFreqToOmega = 8.0 * Math.Atan(1.0) / sampleRate;
		mBanks = new BANK[banks];
		mLevel = new double[banks];
		for (UInt32 b = 0; b < banks; ++b) {
			mBanks[b] = new BANK();
			var w = 6.0 - 12.0 * b / banks;
			if (w < 0.75) {
				w = 0.75;
			}
			Bandpass(b, baseFreq * Math.Pow(2.0, (double)b / octDiv), w / 12.0, 1.0 / w);
		}
		mScale = 32768.0;
		mAttenuation = 0.75;
	}

	public void Filtering(UInt32 bankNo, double input)
	{
		var bank = mBanks[bankNo];

		var output
			= bank.b0 * input
			+ bank.b1 * bank.bDelay1
			+ bank.b2 * bank.bDelay2
			- bank.a1 * bank.aDelay1
			- bank.a2 * bank.aDelay2
		;

		bank.aDelay2 = bank.aDelay1;
		bank.aDelay1 = output;
		bank.bDelay2 = bank.bDelay1;
		bank.bDelay1 = input;

		bank.amplitude += output * output * bank.attenuation;
	}

	public void SetLevel()
	{
		mMax *= 1.0 - 1.0 / 256.0;

		for (uint b = 0; b < mBanks.Length; ++b) {
			var s = 0.92 - mAttenuation * b / mBanks.Length;
			if (s < 0.05) {
				s = 0.05;
			}
			mBanks[b].amplitude *= s;

			mLevel[b] = mScale * mBanks[b].amplitude / s;
			if (mLevel[b] < 1.0) {
				mLevel[b] = 1.0;
			}
			mLevel[b] = Math.Log10(mLevel[b]) / Math.Log10(mScale);
			if (mMax < mLevel[b]) {
				mMax = mLevel[b];
			}
		}
	}

	private void Bandpass(UInt32 bankNo, double freq, double oct, double att)
	{
		double omega = freq * mFreqToOmega;
		double x = Math.Log(2.0) / 2.0 * oct * omega / Math.Sin(omega);
		double alpha = Math.Sin(omega) * (Math.Exp(x) - Math.Exp(-x)) / 2.0;
		double a0 = 1.0 + alpha;

		var bank = mBanks[bankNo];
		bank.a1 = -2.0 * Math.Cos(omega) / a0;
		bank.a2 = (1.0 - alpha) / a0;
		bank.b0 = alpha / a0;
		bank.b1 = 0.0;
		bank.b2 = -alpha / a0;
		bank.attenuation = att;
	}
}