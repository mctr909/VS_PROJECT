#include "fft_filter.h"
#include <stdlib.h>
#include <string.h>
#include <math.h>

#define PI 3.14159265

FftFilter::FftFilter(int dft_size, int overlap_size) {
    DFT_N = dft_size;
    WAV_N = overlap_size;
    mpRe = (double*)malloc(sizeof(double) * DFT_N);
    mpIm = (double*)malloc(sizeof(double) * DFT_N);
    mpGain = (double*)malloc(sizeof(double) * DFT_N);
    mpPrevI = (double*)malloc(sizeof(double) * WAV_N);
    mpPrevO = (double*)malloc(sizeof(double) * WAV_N);
    memset(mpRe, 0, sizeof(double) * DFT_N);
    memset(mpIm, 0, sizeof(double) * DFT_N);
    memset(mpPrevI, 0, sizeof(double) * WAV_N);
    memset(mpPrevO, 0, sizeof(double) * WAV_N);

    for (int w = 0; w < DFT_N; w++) {
        mpGain[w] = 1.0;
    }
}

FftFilter::~FftFilter() {
    free(mpRe);
    free(mpIm);
    free(mpPrevI);
    free(mpPrevO);
    mpRe = NULL;
    mpIm = NULL;
    mpPrevI = NULL;
    mpPrevO = NULL;
}

void
FftFilter::fft() {
    int m, mh, i, j, k;
    double wr, wi, xr, xi;
    auto theta = -2.0 * PI / DFT_N;

    for (m = DFT_N; 1 <= (mh = m >> 1); m = mh) {
        for (i = 0; i < mh; ++i) {
            wr = cos(theta * i);
            wi = sin(theta * i);
            for (j = i; j < DFT_N; j += m) {
                k = j + mh;
                xr = mpRe[j] - mpRe[k];
                xi = mpIm[j] - mpIm[k];
                mpRe[j] += mpRe[k];
                mpIm[j] += mpIm[k];
                mpRe[k] = wr * xr - wi * xi;
                mpIm[k] = wr * xi + wi * xr;
            }
        }
        theta *= 2;
    }
    i = 0;
    for (j = 1; j < DFT_N - 1; ++j) {
        for (k = DFT_N >> 1; k > (i ^= k); k >>= 1);
        if (j < i) {
            xr = mpRe[j];
            xi = mpIm[j];
            mpRe[j] = mpRe[i];
            mpIm[j] = mpIm[i];
            mpRe[i] = xr;
            mpIm[i] = xi;
        }
        mpRe[j] /= DFT_N;
        mpIm[j] /= DFT_N;
    }
    mpRe[0] /= DFT_N;
    mpIm[0] /= DFT_N;
    mpRe[j] /= DFT_N;
    mpIm[j] /= DFT_N;
}

void
FftFilter::ifft() {
    int m, mh, i, j, k;
    double wr, wi, xr, xi;
    auto theta = 2.0 * PI / DFT_N;

    for (m = DFT_N; 1 <= (mh = m >> 1); m = mh) {
        for (i = 0; i < mh; ++i) {
            wr = cos(theta * i);
            wi = sin(theta * i);
            for (j = i; j < DFT_N; j += m) {
                k = j + mh;
                xr = mpRe[j] - mpRe[k];
                xi = mpIm[j] - mpIm[k];
                mpRe[j] += mpRe[k];
                mpIm[j] += mpIm[k];
                mpRe[k] = wr * xr - wi * xi;
                mpIm[k] = wr * xi + wi * xr;
            }
        }
        theta *= 2;
    }
    i = 0;
    for (j = 1; j < DFT_N - 1; ++j) {
        for (k = DFT_N >> 1; k > (i ^= k); k >>= 1);
        if (j < i) {
            xr = mpRe[j];
            xi = mpIm[j];
            mpRe[j] = mpRe[i];
            mpIm[j] = mpIm[i];
            mpRe[i] = xr;
            mpIm[i] = xi;
        }
    }
}

void
FftFilter::Execute(double *pInput, double *pOutput) {
    memset(mpRe, 0, sizeof(double) * DFT_N);
    memset(mpIm, 0, sizeof(double) * DFT_N);
    for (int t = 0, u = DFT_N - WAV_N * 2, v = u + WAV_N; t < WAV_N; t++, u++, v++) {
        auto wt = 0.5 * PI * t / WAV_N;
        mpRe[u] = mpPrevI[t] * sin(wt);
        mpRe[v] = pInput[t] * cos(wt);
        mpPrevI[t] = pInput[t];
    }
    fft();
    for (int w = 0; w < DFT_N; w++) {
        mpRe[w] *= mpGain[w];
        mpIm[w] *= mpGain[w];
    }
    ifft();
    for (int t = 0, u = DFT_N - WAV_N * 2; t < WAV_N; t++, u++) {
        auto wt = 0.5 * PI * t / WAV_N;
        pOutput[t] = mpRe[u] * sin(wt) + mpPrevO[t] * cos(wt);
        mpPrevO[t] = mpRe[u + WAV_N];
    }
}

void
FftFilter::test() {
    FILE *fp = NULL;
    fopen_s(&fp, "C:\\Users\\9004054911\\source\\repos\\FFT\\test2\\test.csv", "w");

    const int DAT_N = 44100;
    auto inp = new double[WAV_N];
    auto out = new double[WAV_N];
    auto tm = 0.0;

    for (int k = 0; k < 50; k++) {
        for (int t = 0, u = DFT_N - WAV_N * 2, v = u + WAV_N; t < WAV_N; t++, u++, v++) {
            if (tm < 0.5) {
                inp[t] = tm * 2;
            } else {
                inp[t] = tm * 2 - 2;
            }
            tm += 60.3113 / DAT_N;
            if (1 <= tm) {
                tm -= 1.0;
            }
        }
        Execute(inp, out);
        for (int t = 0; t < WAV_N; t++) {
            fprintf(fp, "%f, %f\n", inp[t], out[t]);
        }
    }
    fclose(fp);
}
