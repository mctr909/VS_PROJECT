#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include "dft.h"

/******************************************************************************/
#define BEGIN_THRESHOLD (1 / 32768.0)

/******************************************************************************/
int    _stop = 1;
int    _stoped = 1;
int    _notes = 0;
int    _dftLength = 0;
int    *_pBeginTbl = NULL;
int    *_pBeginWave = NULL;
double *_pWaveBuff = NULL;
double *_pTbl = NULL;

/******************************************************************************/
int dft::init(double baseFreq, double sigma, int notes, int octDiv, int dftLength, int sampleRate) {
    double pi;
    double sigma2;
    int idx;

    _stop = 1;
    if (!_stoped) {
        return 0;
    }

    purge();

    // create
    _pWaveBuff = (double*)malloc(sizeof(double)*dftLength);
    _pTbl = (double*)malloc(sizeof(double)*notes*dftLength * 2);
    _pBeginTbl = (int*)malloc(sizeof(int)*notes);
    _pBeginWave = (int*)malloc(sizeof(int)*notes);

    // clear
    memset(_pWaveBuff, 0, sizeof(double)*dftLength);

    // set value
    pi = 4.0*atan(1.0);
    sigma2 = sigma * sigma;
    idx = 0;
    for (int no = 0; no < notes; ++no) {
        double w = baseFreq * pow(2.0, (double)no / octDiv) / sampleRate;
        double v = 2.0*sqrt(pi)*w / sigma;

        double integ = 0.0;
        for (int t = 0; t < dftLength; ++t) {
            double pwt = pi * w*(2 * t - dftLength + 1);
            integ += v * exp(-pwt * pwt / sigma2) * (0.5 - 0.5*cos(2 * pi*(t + 0.5) / dftLength));
        }
        v /= integ;

        double mx = 0.0;
        _pBeginTbl[no] = idx;
        _pBeginWave[no] = 0;
        for (int t = 0; t < dftLength; ++t) {
            double pwt = pi * w*(2 * t - dftLength + 1);
            double ex = v * exp(-pwt * pwt / sigma2) * (0.5 - 0.5*cos(2 * pi*(t + 0.5) / dftLength));
            if (mx < ex) {
                mx = ex;
            }
            if (mx < BEGIN_THRESHOLD) {
                _pBeginTbl[no] = idx;
                _pBeginWave[no] = t;
            }
            _pTbl[idx]     = cos(pwt) * ex;
            _pTbl[idx + 1] = sin(pwt) * ex;
            idx += 2;
        }
    }

    _notes = notes;
    _dftLength = dftLength;
    _stop = 0;
    _stoped = 0;

    return 1;
}

void dft::purge() {
    if (NULL != _pWaveBuff) {
        free(_pWaveBuff);
        _pWaveBuff = NULL;
    }
    if (NULL != _pTbl) {
        free(_pTbl);
        _pTbl = NULL;
    }
    if (NULL != _pBeginTbl) {
        free(_pBeginTbl);
        _pBeginTbl = NULL;
    }
    if (NULL != _pBeginWave) {
        free(_pBeginWave);
        _pBeginWave = NULL;
    }
}

void dft::exec(short *pInput, double *pAmp, int samples) {
    if (_stop) {
        _stoped = 1;
        return;
    }

    memcpy(_pWaveBuff, _pWaveBuff + samples, sizeof(double)*(_dftLength - samples));
    for (int a = 0, b = _dftLength - samples; b < _dftLength; a += 2, ++b) {
        _pWaveBuff[b] = (pInput[a] + pInput[a + 1]) / 65536.0;
    }

    for (int no = 0; no < _notes; ++no) {
        double re = 0.0;
        double im = 0.0;
        int end = _dftLength - _pBeginWave[no];
        for (int t = _pBeginWave[no], u = _pBeginTbl[no]; t < end; ++t, u += 2) {
            re -= _pWaveBuff[t] * _pTbl[u];
            im += _pWaveBuff[t] * _pTbl[u + 1];
        }
        pAmp[no] = sqrt(re * re + im * im);
    }
}
