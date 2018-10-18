using System;
using System.Collections.Generic;

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

	private double[] mLevel;
	private BANK[] mBanks;
	private double mFreqToOmega;
	private double mMax;

	private readonly double mScale;
	private readonly double mAttenuation;

	public double[] Level
	{
		get {
			return mLevel;
		}
	}

	public int Banks
	{
		get {
			return mLevel.Length;
		}
	}

	public double Max
	{
		get {
			return mMax;
		}
	}

	public Spectrum(uint sampleRate, double baseFreq, uint octDiv, uint banks)
	{
		mFreqToOmega = 8.0 * Math.Atan(1.0) / sampleRate;
		mBanks = new BANK[banks];
		mLevel = new double[banks];
		for (uint bankNo = 0; bankNo < banks; ++bankNo) {
			mBanks[bankNo] = new BANK();
			var width = 4.0 - 8.0 * bankNo / banks;
			if (width < 0.66) {
				width = 0.66;
			}

			var omega = Math.Pow(2.0, (double)bankNo / octDiv) * baseFreq * mFreqToOmega;
			var x = Math.Log(2.0) / 2.0 * width * omega / Math.Sin(omega) / 12.0;
			var alpha = Math.Sin(omega) * (Math.Exp(x) - Math.Exp(-x)) / 2.0;
			var a0 = 1.0 + alpha;
			var bank = mBanks[bankNo];
			bank.a1 = -2.0 * Math.Cos(omega) / a0;
			bank.a2 = (1.0 - alpha) / a0;
			bank.b0 = alpha / a0;
			bank.b1 = 0.0;
			bank.b2 = -alpha / a0;
			bank.attenuation = 1.0 / width;
		}
		mScale = 32768.0;
		mAttenuation = 0.75;
	}

	public void Filtering(uint bankNo, double input)
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
		mMax *= 1.0 - 1.0 / 1024.0;

		for (uint b = 0; b < mBanks.Length; ++b) {
			var s = 1.0 - mAttenuation * b / mBanks.Length;
			if (s < 0.01) {
				s = 0.01;
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
}

public class FFT {
	public double[] Re;
	public double[] Im;
	private int SampleRate;

	private struct PeakPoint {
		public int Index { get; set; }
		public double Value { get; set; }

		public PeakPoint(int index, double value) {
			Index = index;
			Value = value;
		}
	}

	public FFT(int length, int sampleRate) {
		Re = new double[length];
		Im = new double[length];
		SampleRate = sampleRate;
	}

	public void Execute() {
		int N = Re.Length;
		int m, mh, i, j, k;
		double wr, wi, xr, xi;
		double theta = -2.0 * Math.PI / N;

		for (m = N; 1 <= (mh = m >> 1); m = mh) {
			for (i = 0; i < mh; ++i) {
				wr = Math.Cos(theta * i);
				wi = Math.Sin(theta * i);
				for (j = i; j < N; j += m) {
					k = j + mh;
					xr = Re[j] - Re[k];
					xi = Im[j] - Im[k];
					Re[j] += Re[k];
					Im[j] += Im[k];
					Re[k] = wr * xr - wi * xi;
					Im[k] = wr * xi + wi * xr;
				}
			}
			theta *= 2;
		}

		i = 0;
		for (j = 1; j < N - 1; ++j) {
			for (k = N >> 1; k > (i ^= k); k >>= 1) ;
			if (j < i) {
				xr = Re[j];
				xi = Im[j];
				Re[j] = Re[i];
				Im[j] = Im[i];
				Re[i] = xr;
				Im[i] = xi;
			}
			Re[j] /= N;
			Im[j] /= N;
		}
		Re[0] /= N;
		Im[0] /= N;
		Re[N - 1] /= N;
		Im[N - 1] /= N;
	}

	public void IExecute() {
		int N = Re.Length;
		int m, mh, i, j, k;
		double wr, wi, xr, xi;
		double theta = 2.0 * Math.PI / N;

		for (m = N; 1 <= (mh = m >> 1); m = mh) {
			for (i = 0; i < mh; ++i) {
				wr = Math.Cos(theta * i);
				wi = Math.Sin(theta * i);
				for (j = i; j < N; j += m) {
					k = j + mh;
					xr = Re[j] - Re[k];
					xi = Im[j] - Im[k];
					Re[j] += Re[k];
					Im[j] += Im[k];
					Re[k] = wr * xr - wi * xi;
					Im[k] = wr * xi + wi * xr;
				}
			}
			theta *= 2;
		}

		i = 0;
		for (j = 1; j < N - 1; ++j) {
			for (k = N >> 1; k > (i ^= k); k >>= 1) ;
			if (j < i) {
				xr = Re[j];
				xi = Im[j];
				Re[j] = Re[i];
				Im[j] = Im[i];
				Re[i] = xr;
				Im[i] = xi;
			}
		}
	}

	public double Pitch() {
		var N = Re.Length;
		var nsdf = new double[N];

		Execute();
		for (int i = 0; i < Re.Length; ++i) {
			Re[i] = Re[i] * Re[i] + Im[i] * Im[i];
			Im[i] = 0.0;
		}
		IExecute();

		for (var i = 0; i < N; ++i) {
			if (0.0 < Math.Abs(Re[0])) {
				nsdf[i] = Re[i] / Re[0];
			}
			else {
				nsdf[i] = 0.0;
			}
		}

		// ピーク検出
		var maxCorrelation = 0.0;
		var peaks = new List<PeakPoint>();
		for (var i = 1; i < N; ++i) {
			if (0.0 < nsdf[i] && nsdf[i - 1] <= 0.0) {
				var currentMax = new PeakPoint(i, nsdf[i]);
				++i;

				for (; i < N; ++i) {
					if (currentMax.Value < nsdf[i]) {
						currentMax.Index = i;
						currentMax.Value = nsdf[i];
					}
					else if (nsdf[i] < 0.0) {
						break;
					}
				}

				peaks.Add(currentMax);
				if (maxCorrelation < currentMax.Value) {
					maxCorrelation = currentMax.Value;
				}
			}
		}

		if (0 == peaks.Count) {
			return 0.0;
		}

		var threshold = maxCorrelation * 0.8;
		var mainPeak = peaks.Find(x => threshold <= x.Value);

		return SampleRate / mainPeak.Index;
	}
}
