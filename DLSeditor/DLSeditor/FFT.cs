using System;
using System.Collections.Generic;

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

		for (int i = 0; i < N; ++i) {
			Re[i] *= Math.Sin(Math.PI * i / N);
		}

		Execute();

		for (int i = 0; i < N; ++i) {
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

		var threshold = maxCorrelation * 0.9;
		var mainPeak = peaks.Find(x => threshold <= x.Value);

		return SampleRate / mainPeak.Index;
	}
}
