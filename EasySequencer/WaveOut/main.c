﻿#include "main.h"
#include "sampler.h"

/******************************************************************************/
HWAVEOUT		g_hWaveOut = NULL;
WAVEFORMATEX	g_waveFmt = { 0 };
WAVEHDR			g_waveHdr[BUFFER_COUNT] = { NULL };

Int32			g_bufferLength = 4096;
Int32			g_deviceListLength = 0;
WCHAR			g_deviceList[DEVICE_LIST_SIZE] = { '\0' };

bool			g_isStop = true;
bool			g_isMute = true;
bool			g_issueMute = false;

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
	g_bufferLength = bufferLength;

	//
	g_waveFmt.wFormatTag = WAVE_FORMAT_PCM;
	g_waveFmt.nChannels = 2;
	g_waveFmt.wBitsPerSample = 16;
	g_waveFmt.nSamplesPerSec = sampleRate;
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
	return getChannelPtr();
}

SAMPLER** WINAPI GetSamplerPtr() {
	return getSamplerPtr();
}

LPBYTE WINAPI LoadDLS(LPWSTR filePath, UInt32 *size, UInt32 sampleRate) {
	//
	g_issueMute = true;
	while (!g_isMute) {
		Sleep(100);
	}

	LPBYTE buffer = loadDLS(filePath, size, sampleRate);

	//
	g_issueMute = false;

	return buffer;
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
	static double waveL;
	static double waveR;

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
		if (isIdle()) {
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
						samplerStep(s);
					}

					waveL = 0.0;
					waveR = 0.0;
					for (ch = 0; ch < CHANNEL_COUNT; ++ch) {
						channelStep(ch, &waveL, &waveR);
					}

					if (1.0 < waveL) waveL = 1.0;
					if (waveL < -1.0) waveL = -1.0;
					if (1.0 < waveR) waveR = 1.0;
					if (waveR < -1.0) waveR = -1.0;
					*pWave = (Int16)(waveL * 32767); ++pWave;
					*pWave = (Int16)(waveR * 32767); ++pWave;
				}
				waveOutWrite(g_hWaveOut, &g_waveHdr[b], sizeof(WAVEHDR));
			}
		}
		break;
	default:
		break;
	}
}
