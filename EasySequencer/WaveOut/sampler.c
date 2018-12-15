#include <windows.h>
#include <stdio.h>
#include <math.h>
#include "sampler.h"

/******************************************************************************/
LPBYTE			gp_dlsBuffer = NULL;

Int32			g_sampleRate = 44100;
Int32			g_delayTaps = 44100;
Int32			g_chorusPhases = 3;

/******************************************************************************/
LPBYTE loadDLS(LPWSTR filePath, UInt32 *size, UInt32 sampleRate) {
	if (NULL == size) {
		return NULL;
	}

	if (sampleRate < 8000 && 192000 < sampleRate) {
		return NULL;
	}

	//
	g_sampleRate = sampleRate;

	//
	if (NULL != gp_dlsBuffer) {
		free(gp_dlsBuffer);
		gp_dlsBuffer = NULL;
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
		gp_dlsBuffer = (LPBYTE)malloc(*size);
		if (NULL != gp_dlsBuffer) {
			fseek(fpDLS, 12, SEEK_SET);
			fread_s(gp_dlsBuffer, *size, *size, 1, fpDLS);
		}

		//
		fclose(fpDLS);
	}

	return gp_dlsBuffer;
}

CHANNEL** createChannels(UInt32 count) {
	CHANNEL **channels = (CHANNEL**)malloc(sizeof(CHANNEL*) * count);
	for (UInt32 i = 0; i < count; ++i) {
		channels[i] = (CHANNEL*)malloc(sizeof(CHANNEL));
		memset(channels[i], 0, sizeof(CHANNEL));
	}

	//
	for (UInt32 i = 0; i < count; ++i) {
		channels[i]->pDelay = (DELAY_VALUES*)malloc(sizeof(DELAY_VALUES));
		memset(channels[i]->pDelay, 0, sizeof(DELAY_VALUES));

		DELAY_VALUES *delay = channels[i]->pDelay;
		delay->readIndex = 0;
		delay->writeIndex = 0;

		delay->pTapL = (double*)malloc(sizeof(double) * g_delayTaps);
		delay->pTapR = (double*)malloc(sizeof(double) * g_delayTaps);
		memset(delay->pTapL, 0, sizeof(double) * g_delayTaps);
		memset(delay->pTapR, 0, sizeof(double) * g_delayTaps);
	}

	//
	for (UInt32 i = 0; i < count; ++i) {
		channels[i]->pChorus = (CHORUS_VALUES*)malloc(sizeof(CHORUS_VALUES));
		memset(channels[i]->pChorus, 0, sizeof(CHORUS_VALUES));

		CHORUS_VALUES *chorus = channels[i]->pChorus;
		chorus->lfoK = 6.283 / g_sampleRate;
		chorus->pPanL = (double*)malloc(sizeof(double) * g_chorusPhases);
		chorus->pPanR = (double*)malloc(sizeof(double) * g_chorusPhases);
		chorus->pLfoRe = (double*)malloc(sizeof(double) * g_chorusPhases);
		chorus->pLfoIm = (double*)malloc(sizeof(double) * g_chorusPhases);

		for (Int32 p = 0; p < g_chorusPhases; ++p) {
			chorus->pPanL[p] = cos(3.1416 * p / g_chorusPhases);
			chorus->pPanR[p] = sin(3.1416 * p / g_chorusPhases);
			chorus->pLfoRe[p] = cos(6.283 * p / g_chorusPhases);
			chorus->pLfoIm[p] = sin(6.283 * p / g_chorusPhases);
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

bool isIdle() {
	return (NULL == gp_dlsBuffer);
}

/******************************************************************************/
inline extern void channelStep(CHANNEL *ch, double *waveL, double *waveR) {
	//
	double chWave = 0.0;
	filterStep(&ch->eq, ch->curAmp * ch->wave, &chWave);

	//
	ch->waveL = chWave * ch->panLeft;
	ch->waveR = chWave * ch->panRight;

	delayStep(ch, ch->pDelay);
	chorusStep(ch, ch->pDelay, ch->pChorus);

	ch->curAmp += 100 * (ch->tarAmp - ch->curAmp) / g_sampleRate;

	ch->eq.cutoff += 100 * (ch->tarCutoff - ch->eq.cutoff) / g_sampleRate;

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
			smpl->curAmp += (smpl->envAmp.levelD - smpl->curAmp) * smpl->envAmp.deltaA / g_sampleRate;
		}
		else {
			smpl->curAmp += (smpl->envAmp.levelS - smpl->curAmp) * smpl->envAmp.deltaD / g_sampleRate;
		}

		if (smpl->time < smpl->envEq.hold) {
			smpl->eq.cutoff += (smpl->envEq.levelD - smpl->eq.cutoff) * smpl->envEq.deltaA / g_sampleRate;
		}
		else {
			smpl->eq.cutoff += (smpl->envEq.levelS - smpl->eq.cutoff) * smpl->envEq.deltaD / g_sampleRate;
		}
	}
	else {
		if (ch->hold < 10.0) {
			smpl->curAmp -= smpl->curAmp * ch->hold / g_sampleRate;
		}
		else {
			smpl->curAmp -= smpl->curAmp * smpl->envAmp.deltaR / g_sampleRate;
		}

		smpl->eq.cutoff += (smpl->envEq.levelR - smpl->eq.cutoff) * smpl->envEq.deltaR / g_sampleRate;

		if (smpl->curAmp < 0.001) {
			smpl->isActive = false;
		}
	}

	//
	Int16 *pcm = (Int16*)(gp_dlsBuffer + smpl->pcmAddr);
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
	smpl->time += 1.0 / g_sampleRate;

	//
	if ((smpl->loopBegin + smpl->loopLength) < smpl->index) {
		smpl->index -= smpl->loopLength;
		if (!smpl->loopEnable) {
			smpl->isActive = false;
		}
	}
}

/******************************************************************************/
inline void delayStep(CHANNEL *ch, DELAY_VALUES *delay) {
	++delay->writeIndex;
	if (g_delayTaps <= delay->writeIndex) {
		delay->writeIndex = 0;
	}

	delay->readIndex = delay->writeIndex - (Int32)(ch->delayRate * g_sampleRate);
	if (delay->readIndex < 0) {
		delay->readIndex += g_delayTaps;
	}

	double delayL = ch->delayDepth * delay->pTapL[delay->readIndex];
	double delayR = ch->delayDepth * delay->pTapR[delay->readIndex];

	ch->waveL += (0.7 * delayL + 0.3 * delayR);
	ch->waveR += (0.7 * delayR + 0.3 * delayL);

	delay->pTapL[delay->writeIndex] = ch->waveL;
	delay->pTapR[delay->writeIndex] = ch->waveR;
}

inline void chorusStep(CHANNEL *ch, DELAY_VALUES *delay, CHORUS_VALUES *chorus) {
	double chorusL = 0.0;
	double chorusR = 0.0;
	double index;
	Int32 indexCur;
	Int32 indexPre;
	double dt;

	for (Int32 ph = 0; ph < g_chorusPhases; ++ph) {
		index = delay->writeIndex - (0.5 - 0.45 * chorus->pLfoRe[ph]) * g_sampleRate * 0.05;
		indexCur = (Int32)index;
		indexPre = indexCur - 1;
		dt = index - indexCur;

		if (indexCur < 0) {
			indexCur += g_delayTaps;
		}
		if (g_delayTaps <= indexCur) {
			indexCur -= g_delayTaps;
		}

		if (indexPre < 0) {
			indexPre += g_delayTaps;
		}
		if (g_delayTaps <= indexPre) {
			indexPre -= g_delayTaps;
		}

		chorusL += (delay->pTapL[indexCur] * dt + delay->pTapL[indexPre] * (1.0 - dt)) * chorus->pPanL[ph];
		chorusR += (delay->pTapR[indexCur] * dt + delay->pTapR[indexPre] * (1.0 - dt)) * chorus->pPanR[ph];

		chorus->pLfoRe[ph] -= chorus->lfoK * ch->chorusRate * chorus->pLfoIm[ph];
		chorus->pLfoIm[ph] += chorus->lfoK * ch->chorusRate * chorus->pLfoRe[ph];
	}

	ch->waveL += chorusL * ch->chorusDepth / g_chorusPhases;
	ch->waveR += chorusR * ch->chorusDepth / g_chorusPhases;
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