#include "main.h"

/******************************************************************************/
LPWSTR WINAPI WaveOutList() {
	g_deviceListLength = 0;
	memset(g_deviceList, '\0', sizeof(g_deviceList));

	DirectSoundEnumerate((LPDSENUMCALLBACK)DSEnumProc, NULL);

	return g_deviceList;
}

BOOL WINAPI WaveOutOpen(UINT deviceId, UINT sampleRate, UINT bufferLength) {
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
	for (int n = 0; n < BUFFER_COUNT; ++n) {
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
	for (int n = 0; n < BUFFER_COUNT; ++n) {
		waveOutUnprepareHeader(g_hWaveOut, &g_waveHdr[n], sizeof(WAVEHDR));
	}
	waveOutReset(g_hWaveOut);
	waveOutClose(g_hWaveOut);
	g_hWaveOut = NULL;
}

CHANNEL** WINAPI GetChannelPtr() {
	if (NULL == gpp_channels) {
		gpp_channels = (CHANNEL**)malloc(sizeof(CHANNEL*) * CHANNEL_COUNT);
		for (UINT i = 0; i < CHANNEL_COUNT; ++i) {
			gpp_channels[i] = (CHANNEL*)malloc(sizeof(CHANNEL));
			memset(gpp_channels[i], 0, sizeof(CHANNEL));
		}
	}

	return gpp_channels;
}

SAMPLER** WINAPI GetSamplerPtr() {
	if (NULL == gpp_samplers) {
		gpp_samplers = (SAMPLER**)malloc(sizeof(SAMPLER*) * SAMPLER_COUNT);
		for (UINT i = 0; i < SAMPLER_COUNT; ++i) {
			gpp_samplers[i] = (SAMPLER*)malloc(sizeof(SAMPLER));
			memset(gpp_samplers[i], 0, sizeof(SAMPLER));
		}
	}

	return gpp_samplers;
}

VOID WINAPI Attach(LPBYTE ptr, INT size) {
	//
	g_issueMute = true;
	while (!g_isMute) {
		Sleep(100);
	}

	if (NULL != gp_buffer) {
		free(gp_buffer);
		gp_buffer = NULL;
	}

	gp_buffer = (LPBYTE)malloc(size);
	memcpy_s(gp_buffer, size, ptr, size);

	//
	for (int i = 0; i < CHANNEL_COUNT; ++i) {
		auto delay = &gp_delay[i];

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

		delay->pTapL = (DOUBLE*)malloc(sizeof(DOUBLE) * g_delayTaps);
		delay->pTapR = (DOUBLE*)malloc(sizeof(DOUBLE) * g_delayTaps);
		memset(delay->pTapL, 0, sizeof(DOUBLE) * g_delayTaps);
		memset(delay->pTapR, 0, sizeof(DOUBLE) * g_delayTaps);
	}

	//
	for (int i = 0; i < CHANNEL_COUNT; ++i) {
		auto chorus = &gp_chorus[i];
		if (NULL != chorus->pLfoRe) {
			free(chorus->pLfoRe);
			chorus->pLfoRe = NULL;
		}
		if (NULL != chorus->pLfoIm) {
			free(chorus->pLfoIm);
			chorus->pLfoIm = NULL;
		}

		chorus->lfoK = 6.283 / g_sampleRate;
		chorus->pLfoRe = (DOUBLE*)malloc(sizeof(DOUBLE) * g_chorusPhases);
		chorus->pLfoIm = (DOUBLE*)malloc(sizeof(DOUBLE) * g_chorusPhases);
		for (int p = 0; p < g_chorusPhases; ++p) {
			chorus->pLfoRe[p] = cos(3.1416 * p / g_chorusPhases);
			chorus->pLfoIm[p] = sin(3.1416 * p / g_chorusPhases);
		}
	}

	//
	g_issueMute = false;
}

/******************************************************************************/
void CALLBACK DSEnumProc(LPGUID lpGUID, LPCTSTR lpszDesc, LPCTSTR lpszDrvName, LPVOID lpContext) {
	if (g_deviceListLength + _mbstrlen((const char*)lpszDesc) < DEVICE_LIST_SIZE) {
		g_deviceListLength += wsprintf(&g_deviceList[g_deviceListLength], TEXT("%s\n"), lpszDesc);
	}
}

void CALLBACK WaveOutProc(HWAVEOUT hwo, UINT uMsg) {
	static UINT b, t, s, ch;
	static short* pWave = NULL;

	switch (uMsg) {
	case MM_WOM_OPEN:
		break;
	case MM_WOM_CLOSE:
		for (int n = 0; n < BUFFER_COUNT; ++n) {
			free(g_waveHdr[n].lpData);
			g_waveHdr[n].lpData = NULL;
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
				pWave = (short*)g_waveHdr[b].lpData;
				for (t = 0; t < g_bufferLength; ++t) {
					for (s = 0; s < SAMPLER_COUNT; ++s) {
						samplerStep(*gpp_samplers[s]);
					}

					g_waveL = 0.0;
					g_waveR = 0.0;
					for (ch = 0; ch < CHANNEL_COUNT; ++ch) {
						channelStep(*gpp_channels[ch]);
					}

					if (1.0 < g_waveL) g_waveL = 1.0;
					if (g_waveL < -1.0) g_waveL = -1.0;
					if (1.0 < g_waveR) g_waveR = 1.0;
					if (g_waveR < -1.0) g_waveR = -1.0;
					*pWave = (short)(g_waveL * 32767); ++pWave;
					*pWave = (short)(g_waveR * 32767); ++pWave;
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
inline void channelStep(CHANNEL &ch) {
	auto waveC = ch.curAmp *ch.wave;
	auto waveL = waveC * ch.panLeft;
	auto waveR = waveC * ch.panRight;

	auto delay = &gp_delay[ch.no];
	auto chorus = &gp_chorus[ch.no];

	// Delay
	{
		++delay->writeIndex;
		if (g_delayTaps <= delay->writeIndex) {
			delay->writeIndex = 0;
		}

		delay->readIndex = delay->writeIndex - (INT)(ch.delayRate * g_sampleRate);
		if (delay->readIndex < 0) {
			delay->readIndex += g_delayTaps;
		}

		auto fbL = ch.delayDepth * delay->pTapL[delay->readIndex];
		auto fbR = ch.delayDepth * delay->pTapR[delay->readIndex];

		waveL += (0.75 * fbL + 0.25 * fbR);
		waveR += (0.75 * fbR + 0.25 * fbL);

		delay->pTapL[delay->writeIndex] = waveL;
		delay->pTapR[delay->writeIndex] = waveR;
	}

	// Chorus
	{
		auto chorusL = 0.0;
		auto chorusR = 0.0;

		for (int ph = 0; ph < g_chorusPhases; ++ph) {
			auto index = delay->writeIndex - (0.5 - 0.48 * chorus->pLfoRe[ph]) * g_sampleRate * 0.05;
			auto indexCur = (int)index;
			auto indexPre = indexCur - 1;
			auto dt = index - indexCur;

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

			chorusL += delay->pTapL[indexCur] * dt + delay->pTapL[indexPre] * (1.0 - dt);
			chorusR += delay->pTapR[indexCur] * dt + delay->pTapR[indexPre] * (1.0 - dt);

			chorus->pLfoRe[ph] -= chorus->lfoK * ch.chorusRate * chorus->pLfoIm[ph];
			chorus->pLfoIm[ph] += chorus->lfoK * ch.chorusRate * chorus->pLfoRe[ph];
		}

		waveL += chorusL * ch.chorusDepth / g_chorusPhases;
		waveR += chorusR * ch.chorusDepth / g_chorusPhases;
	}

	ch.curAmp += 100 * (ch.tarAmp - ch.curAmp) / g_sampleRate;

	//
	g_waveL += waveL;
	g_waveR += waveR;
	ch.wave = 0.0;
}

inline void samplerStep(SAMPLER &smpl) {
	auto pCh = gpp_channels[smpl.channelNo];

	if (!smpl.isActive || NULL == pCh || NULL == gp_buffer) {
		return;
	}

	if (smpl.onKey) {
		if (smpl.time < smpl.envAmpHold) {
			smpl.envAmp += (1.0 - smpl.envAmp) * smpl.envAmpDeltaA / g_sampleRate;
		}
		else {
			smpl.envAmp += (smpl.envAmpLevel - smpl.envAmp) * smpl.envAmpDeltaD / g_sampleRate;
		}
	}
	else {
		if (pCh->hold < 10.0) {
			smpl.envAmp -= smpl.envAmp * pCh->hold / g_sampleRate;
		}
		else {
			smpl.envAmp -= smpl.envAmp * smpl.envAmpDeltaR / g_sampleRate;
		}
		if (smpl.envAmp < 0.0001) {
			smpl.isActive = false;
		}
	}

	//
	auto pcm = (short*)(gp_buffer + smpl.pcmAddr);
	auto cur = (int)smpl.index;
	auto pre = cur - 1;
	auto dt = smpl.index - cur;
	if (pre < 0) {
		pre = 0;
	}
	if (smpl.pcmLength <= cur) {
		cur = 0;
		pre = 0;
		smpl.index = 0.0;
		if (!smpl.loopEnable) {
			smpl.isActive = false;
		}
	}

	//
	pCh->wave += (pcm[cur] * dt + pcm[pre] * (1.0 - dt)) * smpl.gain * smpl.tarAmp * smpl.envAmp / 32768.0;

	//
	smpl.index += smpl.delta * pCh->pitch;
	smpl.time += 1.0 / g_sampleRate;

	//
	if ((smpl.loopBegin + smpl.loopLength) < smpl.index) {
		smpl.index -= smpl.loopLength;
		if (!smpl.loopEnable) {
			smpl.isActive = false;
		}
	}
}
