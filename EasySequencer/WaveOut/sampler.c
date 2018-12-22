#include <windows.h>
#include <stdio.h>
#include <math.h>
#include "sampler.h"

/******************************************************************************/
#define     CHORUS_PHASES   3
#define     DELAY_TAPS      1048576

LPBYTE      __pBuffer = NULL;
Int32       __sampleRate = 44100;

/******************************************************************************/
LPBYTE loadDLS(LPWSTR filePath, UInt32 *size, UInt32 sampleRate) {
    if (NULL == size) {
        return NULL;
    }

    if (sampleRate < 8000 && 192000 < sampleRate) {
        return NULL;
    }

    //
    __sampleRate = sampleRate;

    //
    if (NULL != __pBuffer) {
        free(__pBuffer);
        __pBuffer = NULL;
    }

    //
    FILE *fpDLS = NULL;
    _wfopen_s(&fpDLS, filePath, TEXT("rb"));
    if (NULL != fpDLS) {
        //
        fseek(fpDLS, 4, SEEK_SET);
        fread_s(size, sizeof(*size), sizeof(*size), 1, fpDLS);
        *size -= 8;

        //
        __pBuffer = (LPBYTE)malloc(*size);
        if (NULL != __pBuffer) {
            fseek(fpDLS, 12, SEEK_SET);
            fread_s(__pBuffer, *size, *size, 1, fpDLS);
        }

        //
        fclose(fpDLS);
    }

    return __pBuffer;
}

CHANNEL** createChannels(UInt32 count) {
    CHANNEL **channels = (CHANNEL**)malloc(sizeof(CHANNEL*) * count);
    for (UInt32 i = 0; i < count; ++i) {
        channels[i] = (CHANNEL*)malloc(sizeof(CHANNEL));
        memset(channels[i], 0, sizeof(CHANNEL));
    }

    //
    for (UInt32 i = 0; i < count; ++i) {
        memset(&channels[i]->delay, 0, sizeof(DELAY));

        DELAY *delay = &channels[i]->delay;
        delay->readIndex = 0;
        delay->writeIndex = 0;

        delay->pTapL = (double*)malloc(sizeof(double) * DELAY_TAPS);
        delay->pTapR = (double*)malloc(sizeof(double) * DELAY_TAPS);
        memset(delay->pTapL, 0, sizeof(double) * DELAY_TAPS);
        memset(delay->pTapR, 0, sizeof(double) * DELAY_TAPS);
    }

    //
    for (UInt32 i = 0; i < count; ++i) {
        memset(&channels[i]->chorus, 0, sizeof(CHORUS));

        CHORUS *chorus = &channels[i]->chorus;
        chorus->lfoK = 6.283 / __sampleRate;
        chorus->pPanL = (double*)malloc(sizeof(double) * CHORUS_PHASES);
        chorus->pPanR = (double*)malloc(sizeof(double) * CHORUS_PHASES);
        chorus->pLfoRe = (double*)malloc(sizeof(double) * CHORUS_PHASES);
        chorus->pLfoIm = (double*)malloc(sizeof(double) * CHORUS_PHASES);

        for (Int32 p = 0; p < CHORUS_PHASES; ++p) {
            chorus->pPanL[p] = cos(3.1416 * p / CHORUS_PHASES);
            chorus->pPanR[p] = sin(3.1416 * p / CHORUS_PHASES);
            chorus->pLfoRe[p] = cos(6.283 * p / CHORUS_PHASES);
            chorus->pLfoIm[p] = sin(6.283 * p / CHORUS_PHASES);
        }
    }

    return channels;
}

SAMPLER** createSamplers(UInt32 count) {
    SAMPLER** samplers = (SAMPLER**)malloc(sizeof(SAMPLER*) * count);
    for (UInt32 i = 0; i < count; ++i) {
        samplers[i] = (SAMPLER*)malloc(sizeof(SAMPLER));
        memset(samplers[i], 0, sizeof(SAMPLER));
    }

    return samplers;
}

/******************************************************************************/
inline extern void channelStep(CHANNEL *ch, double *waveL, double *waveR) {
    //
    double chWave = 0.0;
    filterStep(&ch->eq, ch->curAmp * ch->wave, &chWave);

    //
    ch->waveL = chWave * ch->panLeft;
    ch->waveR = chWave * ch->panRight;

    delayStep(ch, &ch->delay);
    chorusStep(ch, &ch->delay, &ch->chorus);

    ch->curAmp += 100 * (ch->tarAmp - ch->curAmp) / __sampleRate;

    ch->eq.cutoff += 100 * (ch->tarCutoff - ch->eq.cutoff) / __sampleRate;

    //
    *waveL += ch->waveL;
    *waveR += ch->waveR;
    ch->wave = 0.0;
}

inline extern void samplerStep(CHANNEL **chs, SAMPLER *smpl) {
    if (NULL == chs || NULL == smpl || !smpl->isActive) {
        return;
    }

    CHANNEL *ch = chs[smpl->channelNo];
    if (NULL == ch) {
        return;
    }

    if (smpl->onKey) {
        if (smpl->time < smpl->envAmp.hold) {
            smpl->curAmp += (smpl->envAmp.levelD - smpl->curAmp) * smpl->envAmp.deltaA / __sampleRate;
        }
        else {
            smpl->curAmp += (smpl->envAmp.levelS - smpl->curAmp) * smpl->envAmp.deltaD / __sampleRate;
        }

        if (smpl->time < smpl->envEq.hold) {
            smpl->eq.cutoff += (smpl->envEq.levelD - smpl->eq.cutoff) * smpl->envEq.deltaA / __sampleRate;
        }
        else {
            smpl->eq.cutoff += (smpl->envEq.levelS - smpl->eq.cutoff) * smpl->envEq.deltaD / __sampleRate;
        }
    }
    else {
        if (ch->hold < 10.0) {
            smpl->curAmp -= smpl->curAmp * ch->hold / __sampleRate;
        }
        else {
            smpl->curAmp -= smpl->curAmp * smpl->envAmp.deltaR / __sampleRate;
        }

        smpl->eq.cutoff += (smpl->envEq.levelR - smpl->eq.cutoff) * smpl->envEq.deltaR / __sampleRate;

        if (smpl->curAmp < 0.001) {
            smpl->isActive = false;
        }
    }

    //
    Int16 *pcm = (Int16*)(__pBuffer + smpl->pcmAddr);
    Int32 cur = (Int32)smpl->index;
    Int32 pre = cur - 1;
    double dt = smpl->index - cur;
    if (pre < 0) {
        pre = 0;
    }
    if (smpl->pcmLength <= cur) {
        cur = 0;
        pre = 0;
        smpl->index = 0.0;
        if (!smpl->loopEnable) {
            smpl->isActive = false;
        }
    }

    //
    filterStep(
        &smpl->eq,
        (pcm[cur] * dt + pcm[pre] * (1.0 - dt)) * smpl->gain * smpl->tarAmp * smpl->curAmp / 32768.0,
        &ch->wave
    );

    //
    smpl->index += smpl->delta * ch->pitch;
    smpl->time += 1.0 / __sampleRate;

    //
    if ((smpl->loopBegin + smpl->loopLength) < smpl->index) {
        smpl->index -= smpl->loopLength;
        if (!smpl->loopEnable) {
            smpl->isActive = false;
        }
    }
}

/******************************************************************************/
inline void delayStep(CHANNEL *ch, DELAY *delay) {
    ++delay->writeIndex;
    if (DELAY_TAPS <= delay->writeIndex) {
        delay->writeIndex = 0;
    }

    delay->readIndex = delay->writeIndex - (Int32)(delay->rate * __sampleRate);
    if (delay->readIndex < 0) {
        delay->readIndex += DELAY_TAPS;
    }

    double delayL = delay->depth * delay->pTapL[delay->readIndex];
    double delayR = delay->depth * delay->pTapR[delay->readIndex];

    ch->waveL += (0.7 * delayL + 0.3 * delayR);
    ch->waveR += (0.7 * delayR + 0.3 * delayL);

    delay->pTapL[delay->writeIndex] = ch->waveL;
    delay->pTapR[delay->writeIndex] = ch->waveR;
}

inline void chorusStep(CHANNEL *ch, DELAY *delay, CHORUS *chorus) {
    double chorusL = 0.0;
    double chorusR = 0.0;
    double index;
    Int32 indexCur;
    Int32 indexPre;
    double dt;

    for (Int32 ph = 0; ph < CHORUS_PHASES; ++ph) {
        index = delay->writeIndex - (0.5 - 0.45 * chorus->pLfoRe[ph]) * __sampleRate * 0.05;
        indexCur = (Int32)index;
        indexPre = indexCur - 1;
        dt = index - indexCur;

        if (indexCur < 0) {
            indexCur += DELAY_TAPS;
        }
        if (DELAY_TAPS <= indexCur) {
            indexCur -= DELAY_TAPS;
        }

        if (indexPre < 0) {
            indexPre += DELAY_TAPS;
        }
        if (DELAY_TAPS <= indexPre) {
            indexPre -= DELAY_TAPS;
        }

        chorusL += (delay->pTapL[indexCur] * dt + delay->pTapL[indexPre] * (1.0 - dt)) * chorus->pPanL[ph];
        chorusR += (delay->pTapR[indexCur] * dt + delay->pTapR[indexPre] * (1.0 - dt)) * chorus->pPanR[ph];

        chorus->pLfoRe[ph] -= chorus->lfoK * chorus->rate * chorus->pLfoIm[ph];
        chorus->pLfoIm[ph] += chorus->lfoK * chorus->rate * chorus->pLfoRe[ph];
    }

    ch->waveL += chorusL * chorus->depth / CHORUS_PHASES;
    ch->waveR += chorusR * chorus->depth / CHORUS_PHASES;
}

inline void filterStep(FILTER *filter, double input, double *output) {
    double fk = filter->cutoff * 1.16;
    double fki = 1.0 - fk;

    input -= filter->pole03 * (1.0 - fk * fk * 0.15) * 4.0 * filter->resonance;
    input *= (fk * fk) * (fk * fk) * 0.35013;

    filter->pole00 = input + 0.3 * filter->pole10 + fki * filter->pole00;
    filter->pole01 = filter->pole00 + 0.3 * filter->pole11 + fki * filter->pole01;
    filter->pole02 = filter->pole01 + 0.3 * filter->pole12 + fki * filter->pole02;
    filter->pole03 = filter->pole02 + 0.3 * filter->pole13 + fki * filter->pole03;

    filter->pole10 = input;
    filter->pole11 = filter->pole00;
    filter->pole12 = filter->pole01;
    filter->pole13 = filter->pole02;

    *output += filter->pole03;
}