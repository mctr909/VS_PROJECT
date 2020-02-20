#pragma once
#include <stdio.h>

class FftFilter {
public:
    int DFT_N = 0;
    int WAV_N = 0;

public:
    double *mpRe = NULL;
    double *mpIm = NULL;

private:
    double *mpGain = NULL;
    double *mpPrevI = NULL;
    double *mpPrevO = NULL;

public:
    FftFilter(int dft_size, int overlap_size);
    ~FftFilter();

private:
    void fft();
    void ifft();

public:
    void Execute(double *pInput, double *pOutput);
    void test();
};
