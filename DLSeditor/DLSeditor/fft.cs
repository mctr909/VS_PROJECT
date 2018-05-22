using System;
using System.IO;

unsafe public class FFT
{
	private int mDataLen;
	private int mDataLenHalf;
	private int mSampleRate;
	private double mScale;

	private int[] mWK;
	private double[] mTable;

	private double* mpRe;
	private double* mpIm;

	public double[] Window;
	public double[] Re;
	public double[] Im;

	public FFT(int dataLen, int sampleRate)
	{
		mDataLen = dataLen;
		mDataLenHalf = dataLen >> 1;
		mSampleRate = sampleRate;
		mScale = 8.0 * Math.Atan(1.0) / dataLen;

		Window = new double[dataLen];
		for (int i = 0; i < dataLen; ++i)
		{
			Window[i] = (0.5 - 0.5 * Math.Cos(Math.PI * i / mDataLenHalf));
		}

		Re = new double[dataLen];
		Im = new double[dataLen];
		fixed (double* p = &Re[0]) mpRe = p;
		fixed (double* p = &Im[0]) mpIm = p;

		mWK = new int[dataLen];
		mTable = new double[2 * dataLen];

		// 2進数の最上位の値が1になるまで繰り返し
		for (int ns = mDataLenHalf; 1 <= ns; ns >>= 1)
		{
			// Lの値を0からdetaLenまで、2×最上位の値のstepで繰り返し
			for (int L = 0, M = 0; L < mDataLen; L += ns << 1, M += 2)
			{
				// 回転子 = 前の回転子のindex/2
				var th1 = mWK[L] >> 1;
				var th2 = th1 + mDataLenHalf;
				mTable[M] = Math.Cos(th1 * mScale);
				mTable[M + 1] = Math.Sin(th1 * mScale);

				// i0の値をLの値から(L + 2進数の最上位)の値まで繰り返し
				for (int i0 = L; i0 < L + ns; ++i0)
				{
					var i1 = i0 + ns; // i1 = i0 + 2進数の最上位の値
					mWK[i0] = th1;
					mWK[i1] = th2;
				}
			}
		}
	}

	private void HighCut(double begin)
	{
		var ma = (int)(begin * mDataLen / mSampleRate);

		if (mDataLenHalf < ma) ma = mDataLenHalf;

		int mb = mDataLen - ma;

		for (int i = ma; i < mb; ++i)
		{
			var temp = Re[i];
			Re[i] = Im[i];
			Im[i] = temp;
		}
	}

	private void LowCut(double end)
	{
		var ma = (int)(end * mDataLen / mSampleRate);

		if (mDataLenHalf < ma) ma = mDataLenHalf;

		int mb = mDataLen - ma;

		for (int i = 0; i < ma; ++i)
		{
			Re[i] = 0.0;
			Im[i] = 0.0;
		}

		for (int i = mb; i < mDataLen; ++i)
		{
			Re[i] = 0.0;
			Im[i] = 0.0;
		}
	}

	private void fft()
	{
		int ns;
		int L, M;
		int i0, i1;

		double c, s;
		double reTemp, imTemp;

		double* pRe0;
		double* pRe1;
		double* pIm0;
		double* pIm1;

		for (ns = mDataLenHalf; 1 <= ns; ns >>= 1)
		{
			for (L = 0, M = 0; L < mDataLen; L += ns << 1, M += 2)
			{
				c = mTable[M];
				s = mTable[M + 1];

				for (i0 = L; i0 < L + ns; ++i0)
				{
					i1 = i0 + ns;

					pRe0 = mpRe + i0;
					pIm0 = mpIm + i0;
					pRe1 = mpRe + i1;
					pIm1 = mpIm + i1;

					reTemp = *pRe1 * c + *pIm1 * s;
					imTemp = *pIm1 * c - *pRe1 * s;

					*pRe1 = *pRe0 - reTemp;
					*pIm1 = *pIm0 - imTemp;
					*pRe0 += reTemp;
					*pIm0 += imTemp;
				}
			}
		}

		// 正規化
		for (i0 = 0; i0 < mDataLen; ++i0)
		{
			*(mpRe + i0) /= mDataLen;
			*(mpIm + i0) /= mDataLen;
		}

		// ビット反転
		for (i0 = 0; i0 < mDataLen; ++i0)
		{
			if (i0 < mWK[i0])
			{
				i1 = mWK[i0];

				pRe0 = mpRe + i0;
				pRe1 = mpRe + i1;
				pIm0 = mpIm + i0;
				pIm1 = mpIm + i1;

				reTemp = *pRe0;
				*pRe0 = *pRe1;
				*pRe1 = reTemp;

				reTemp = *pIm0;
				*pIm0 = *pIm1;
				*pIm1 = reTemp;
			}
		}
	}

	private void ifft()
	{
		int ns;
		int L, M;
		int i0, i1;

		double c, s;
		double reTemp, imTemp;

		double* pRe0;
		double* pRe1;
		double* pIm0;
		double* pIm1;

		for (ns = mDataLenHalf; 1 <= ns; ns >>= 1)
		{
			for (L = 0, M = 0; L < mDataLen; L += ns << 1, M += 2)
			{
				c = mTable[M];
				s = mTable[M + 1];

				for (i0 = L; i0 < L + ns; ++i0)
				{
					i1 = i0 + ns;

					pRe0 = mpRe + i0;
					pIm0 = mpIm + i0;
					pRe1 = mpRe + i1;
					pIm1 = mpIm + i1;

					reTemp = *pRe1 * c - *pIm1 * s;
					imTemp = *pIm1 * c + *pRe1 * s;

					*pRe1 = *pRe0 - reTemp;
					*pIm1 = *pIm0 - imTemp;
					*pRe0 += reTemp;
					*pIm0 += imTemp;
				}
			}
		}

		for (i0 = 0; i0 < mDataLen; ++i0)
		{
			if (i0 < mWK[i0])
			{
				i1 = mWK[i0];

				pRe0 = mpRe + i0;
				pRe1 = mpRe + i1;
				pIm0 = mpIm + i0;
				pIm1 = mpIm + i1;

				reTemp = *pRe0;
				*pRe0 = *pRe1;
				*pRe1 = reTemp;

				reTemp = *pIm0;
				*pIm0 = *pIm1;
				*pIm1 = reTemp;
			}
		}
	}

	public byte[] Comp(ref byte[] data)
	{
		var mr = new MemoryStream(new byte[data.Length + 2 * mDataLen]);
		var br = new BinaryReader(mr);

		var mw = new MemoryStream();
		var bw = new BinaryWriter(mw);

		var wave = new double[mDataLen];
		var delayEnv = new double[mDataLenHalf];

		int i, j;
		Int16 output;

		mr.Seek(mDataLen, SeekOrigin.Begin);
		mr.Write(data, 0, data.Length);
		mr.Seek(0, SeekOrigin.Begin);

		while (mr.Position < mr.Length)
		{
			for (i = 0, j = mDataLenHalf; i < mDataLenHalf; ++i, ++j)
			{
				wave[i] = wave[j];
				if (mr.Position < mr.Length)
				{
					wave[j] = br.ReadInt16() / 32768.0;
				}
				else
				{
					wave[j] = 0.0;
				}

				Im[i] = 0.0;
				Im[j] = 0.0;
				Re[i] = wave[i];
				Re[j] = wave[j];
			}

			fft();
			LowCut(20.0);
			HighCut(6000.0);
			ifft();

			for (i = 0, j = mDataLenHalf; i < mDataLenHalf; ++i, ++j)
			{
				output = (Int16)(32768 * (Re[i] * Window[i] + delayEnv[i] * Window[j]));
				delayEnv[i] = Re[j];

				if (32767 < output) output = 32767;
				if (output < -32768) output = -32768;

				bw.Write(output);
			}
		}

		return mw.ToArray();
	}
}
