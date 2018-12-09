#include "main.h"

/******************************************************************************/
LPWSTR WINAPI WaveOutList() {
	g_deviceListLength = 0;
	memset(g_deviceList, '\0', sizeof(g_deviceList));

	DirectSoundEnumerate((LPDSENUMCALLBACK)DSEnumProc, NULL);

	return g_deviceList;
}

BOOL WINAPI WaveOutOpen(UInt32 deviceId, UInt32 sampleRate, UInt32 bufferLength) {
	if (NULL != g_hWaveOut) {
		WaveOutClose();
	}

	//
	g_sampleRate = sampleRate;
	g_bufferLength = bufferLength;

	//
	g_waveFmt.wFormatTag = WAVE_FORMAT_PCM;
	g_waveFmt.nChannels = 2;
	g_waveFmt.wBitsPerSample = 16;
	g_waveFmt.nSamplesPerSec = g_sampleRate;
	g_waveFmt.nBlockAlign = g_waveFmt.nChannels * g_waveFmt.wBitsPerSample / 8;
	g_waveFmt.nAvgBytesPerSec = g_waveFmt.nSamplesPerSec * g_waveFmt.nBlockAlign;

	//
	if (MMSYSERR_NOERROR != waveOutOpen(
		&g_hWaveOut,
		deviceId,
		&g_waveFmt,
		(DWORD_PTR)WaveOutProc,
		(DWORD_PTR)g_waveHdr,
		CALLBACK_FUNCTION
	)) {
		return false;
	}

	//
	g_isStop = false;

	//
	for (UInt32 n = 0; n < BUFFER_COUNT; ++n) {
		g_waveHdr[n].dwBufferLength = g_bufferLength * g_waveFmt.nBlockAlign;
		g_waveHdr[n].dwFlags = WHDR_BEGINLOOP | WHDR_ENDLOOP;
		g_waveHdr[n].dwLoops = 0;
		g_waveHdr[n].dwUser = 0;
		if (NULL == g_waveHdr[n].lpData) {
			g_waveHdr[n].lpData = (LPSTR)malloc(g_bufferLength * g_waveFmt.nBlockAlign);
			if (NULL != g_waveHdr[n].lpData) {
				memset(g_waveHdr[n].lpData, 0, g_bufferLength * g_waveFmt.nBlockAlign);
				waveOutPrepareHeader(g_hWaveOut, &g_waveHdr[n], sizeof(WAVEHDR));
				waveOutWrite(g_hWaveOut, &g_waveHdr[n], sizeof(WAVEHDR));
			}
		}
	}

	return true;
}

VOID WINAPI WaveOutClose() {
	if (NULL == g_hWaveOut) {
		return;
	}

	//
	g_isStop = true;

	//
	for (UInt32 n = 0; n < BUFFER_COUNT; ++n) {
		waveOutUnprepareHeader(g_hWaveOut, &g_waveHdr[n], sizeof(WAVEHDR));
	}
	waveOutReset(g_hWaveOut);
	waveOutClose(g_hWaveOut);
	g_hWaveOut = NULL;
}

CHANNEL** WINAPI GetChannelPtr() {
	if (NULL == gpp_channels) {
		gpp_channels = (CHANNEL**)malloc(sizeof(CHANNEL*) * CHANNEL_COUNT);
		for (UInt32 i = 0; i < CHANNEL_COUNT; ++i) {
			gpp_channels[i] = (CHANNEL*)malloc(sizeof(CHANNEL));
			memset(gpp_channels[i], 0, sizeof(CHANNEL));
		}
	}

	return gpp_channels;
}

SAMPLER** WINAPI GetSamplerPtr() {
	if (NULL == gpp_samplers) {
		gpp_samplers = (SAMPLER**)malloc(sizeof(SAMPLER*) * SAMPLER_COUNT);
		for (UInt32 i = 0; i < SAMPLER_COUNT; ++i) {
			gpp_samplers[i] = (SAMPLER*)malloc(sizeof(SAMPLER));
			memset(gpp_samplers[i], 0, sizeof(SAMPLER));
		}
	}

	return gpp_samplers;
}

LPBYTE WINAPI LoadDLS(LPWSTR filePath, UInt32 *size) {
	//
	g_issueMute = true;
	while (!g_isMute) {
		Sleep(100);
	}

	//
	if (NULL != gp_buffer) {
		free(gp_buffer);
		gp_buffer = NULL;
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
		gp_buffer = (LPBYTE)malloc(*size);
		if (NULL != gp_buffer) {
			fseek(fpDLS, 12, SEEK_SET);
			fread_s(gp_buffer, *size, *size, 1, fpDLS);
		}

		//
		fclose(fpDLS);
	}

	//
	for (UInt32 i = 0; i < CHANNEL_COUNT; ++i) {
		DELAY_VALUES *delay = &gp_delay[i];

		delay->readIndex = 0;
		delay->writeIndex = 0;

		if (NULL != delay->pTapL) {
			free(delay->pTapL);
			delay->pTapL = NULL;
		}
		if (NULL != delay->pTapR) {
			free(delay->pTapR);
			delay->pTapR = NULL;
		}

		delay->pTapL = (double*)malloc(sizeof(double) * g_delayTaps);
		delay->pTapR = (double*)malloc(sizeof(double) * g_delayTaps);
		memset(delay->pTapL, 0, sizeof(double) * g_delayTaps);
		memset(delay->pTapR, 0, sizeof(double) * g_delayTaps);
	}

	//
	for (UInt32 i = 0; i < CHANNEL_COUNT; ++i) {
		CHORUS_VALUES *chorus = &gp_chorus[i];
		if (NULL != chorus->pPanL) {
			free(chorus->pPanL);
			chorus->pPanL = NULL;
		}
		if (NULL != chorus->pPanR) {
			free(chorus->pPanR);
			chorus->pPanR = NULL;
		}
		if (NULL != chorus->pLfoRe) {
			free(chorus->pLfoRe);
			chorus->pLfoRe = NULL;
		}
		if (NULL != chorus->pLfoIm) {
			free(chorus->pLfoIm);
			chorus->pLfoIm = NULL;
		}

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

	//
	g_issueMute = false;

	return gp_buffer;
}

/******************************************************************************/
void CALLBACK DSEnumProc(LPGUID lpGUID, LPCTSTR lpszDesc, LPCTSTR lpszDrvName, LPVOID lpContext) {
	if (g_deviceListLength + _mbstrlen((const char*)lpszDesc) < DEVICE_LIST_SIZE) {
		g_deviceListLength += wsprintf(&g_deviceList[g_deviceListLength], TEXT("%s\n"), lpszDesc);
	}
}

void CALLBACK WaveOutProc(HWAVEOUT hwo, UInt32 uMsg) {
	static Int32 b, t, s, ch;
	static Int16* pWave = NULL;

	switch (uMsg) {
	case MM_WOM_OPEN:
		break;
	case MM_WOM_CLOSE:
		for (b = 0; b < BUFFER_COUNT; ++b) {
			free(g_waveHdr[b].lpData);
			g_waveHdr[b].lpData = NULL;
		}
		break;
	case MM_WOM_DONE:
		//
		if (g_isStop) {
			break;
		}

		//
		if (g_issueMute) {
			for (b = 0; b < BUFFER_COUNT; ++b) {
				if (0 == (g_waveHdr[b].dwFlags & WHDR_INQUEUE)) {
					memset(g_waveHdr[b].lpData, 0, g_waveFmt.nBlockAlign * g_bufferLength);
					waveOutWrite(g_hWaveOut, &g_waveHdr[b], sizeof(WAVEHDR));
				}
			}
			g_isMute = true;
			break;
		}
		else {
			g_isMute = false;
		}

		//
		if (NULL == gpp_samplers || NULL == gpp_channels) {
			for (b = 0; b < BUFFER_COUNT; ++b) {
				if (0 == (g_waveHdr[b].dwFlags & WHDR_INQUEUE)) {
					memset(g_waveHdr[b].lpData, 0, g_waveFmt.nBlockAlign * g_bufferLength);
					waveOutWrite(g_hWaveOut, &g_waveHdr[b], sizeof(WAVEHDR));
				}
			}
			break;
		}

		//
		for (b = 0; b < BUFFER_COUNT; ++b) {
			if (0 == (g_waveHdr[b].dwFlags & WHDR_INQUEUE)) {
				pWave = (Int16*)g_waveHdr[b].lpData;
				for (t = 0; t < g_bufferLength; ++t) {
					for (s = 0; s < SAMPLER_COUNT; ++s) {
						samplerStep(gpp_samplers[s]);
					}

					g_waveL = 0.0;
					g_waveR = 0.0;
					for (ch = 0; ch < CHANNEL_COUNT; ++ch) {
						channelStep(gpp_channels[ch], ch);
					}

					if (1.0 < g_waveL) g_waveL = 1.0;
					if (g_waveL < -1.0) g_waveL = -1.0;
					if (1.0 < g_waveR) g_waveR = 1.0;
					if (g_waveR < -1.0) g_waveR = -1.0;
					*pWave = (Int16)(g_waveL * 32767); ++pWave;
					*pWave = (Int16)(g_waveR * 32767); ++pWave;
				}
				waveOutWrite(g_hWaveOut, &g_waveHdr[b], sizeof(WAVEHDR));
			}
		}
		break;
	default:
		break;
	}
}

/******************************************************************************/
inline void channelStep(CHANNEL *ch, UInt32 no) {
	//
	g_chWaveC = 0;
	filterStep(&ch->eq, ch->curAmp * ch->wave, &g_chWaveC);

	//
	g_chWaveL = g_chWaveC * ch->panLeft;
	g_chWaveR = g_chWaveC * ch->panRight;

	delayStep(ch, &gp_delay[no]);
	chorusStep(ch, &gp_delay[no], &gp_chorus[no]);

	ch->curAmp += 100 * (ch->tarAmp - ch->curAmp) / g_sampleRate;

	ch->eq.cutoff += 100 * (ch->tarCutoff - ch->eq.cutoff) / g_sampleRate;

	//
	g_waveL += g_chWaveL;
	g_waveR += g_chWaveR;
	ch->wave = 0.0;
}

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

	g_chWaveL += (0.7 * delayL + 0.3 * delayR);
	g_chWaveR += (0.7 * delayR + 0.3 * delayL);

	delay->pTapL[delay->writeIndex] = g_chWaveL;
	delay->pTapR[delay->writeIndex] = g_chWaveR;
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

	g_chWaveL += chorusL * ch->chorusDepth / g_chorusPhases;
	g_chWaveR += chorusR * ch->chorusDepth / g_chorusPhases;
}

inline void samplerStep(SAMPLER *smpl) {
	if (NULL == smpl || !smpl->isActive) {
		return;
	}

	CHANNEL *ch = gpp_channels[smpl->channelNo];
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

		if (smpl->curAmp < 0.0001) {
			smpl->isActive = false;
		}
	}

	//
	Int16 *pcm = (Int16*)(gp_buffer + smpl->pcmAddr);
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