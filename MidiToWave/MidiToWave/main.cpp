#include "main.h"

/******************************************************************************/
LPWSTR WINAPI WaveOutList() {
	g_deviceListLength = 0;
	memset(g_deviceList, '\0', sizeof(g_deviceList));
	
	DirectSoundEnumerate((LPDSENUMCALLBACK)DSEnumProc, NULL);

	return g_deviceList;
}

BOOL WINAPI WaveOutOpen(UINT deviceId, UINT sampleRate, UINT bufferLength) {
	if (NULL != g_whdl) {
		WaveOutClose();
	}

	g_sampleRate = sampleRate;
	g_bufferLength = bufferLength;

	g_wfe.wFormatTag = 1;
	g_wfe.nChannels = 2;
	g_wfe.wBitsPerSample = 16;
	g_wfe.nSamplesPerSec = g_sampleRate;
	g_wfe.nBlockAlign = g_wfe.nChannels * g_wfe.wBitsPerSample >> 3;
	g_wfe.nAvgBytesPerSec = g_sampleRate * g_wfe.nBlockAlign;
	g_wfe.cbSize = 0;

	if (MMSYSERR_NOERROR != waveOutOpen(&g_whdl, deviceId, &g_wfe, (DWORD)WaveCallback, 0, CALLBACK_FUNCTION)) {
		return false;
	}

	g_isPlay = true;

	for (int i = 0; i < CHANNEL_COUNT; ++i) {
		if (NULL == g_channels[i]) {
			g_channels[i] = (CHANNEL*)malloc(sizeof(CHANNEL));
			memset(g_channels[i], 0, sizeof(CHANNEL));
		}
	}

	for (int i = 0; i < SAMPLER_COUNT; ++i) {
		if (NULL == g_samplers[i]) {
			g_samplers[i] = (SAMPLER*)malloc(sizeof(SAMPLER));
			memset(g_samplers[i], 0, sizeof(SAMPLER));
		}
	}

	for (int i = 0; i < BUFFER_COUNT; ++i) {
		g_whdr[i].dwBufferLength = g_bufferLength * g_wfe.nBlockAlign;
		g_whdr[i].lpData = (LPSTR)malloc(g_whdr[i].dwBufferLength);
		g_whdr[i].dwLoops = 0;
		g_whdr[i].dwFlags = 0;

		memset(g_whdr[i].lpData, 0, g_whdr[i].dwBufferLength);

		waveOutPrepareHeader(g_whdl, &g_whdr[i], sizeof(WAVEHDR));
		waveOutWrite(g_whdl, &g_whdr[i], sizeof(WAVEHDR));
	}

	return true;
}

VOID WINAPI WaveOutClose() {
	if (NULL == g_whdl) {
		return;
	}

	g_isPlay = false;

	for (int i = 0; i < CHANNEL_COUNT; ++i) {
		if (NULL != g_channels[i]) {
			free(g_channels[i]);
			g_channels[i] = NULL;
		}
	}

	for (int i = 0; i < SAMPLER_COUNT; ++i) {
		if (NULL != g_samplers[i]) {
			free(g_samplers[i]);
			g_samplers[i] = NULL;
		}
	}

	for (int i = 0; i < BUFFER_COUNT; ++i) {
		waveOutUnprepareHeader(g_whdl, &g_whdr[i], sizeof(g_whdr[i]));
		if (NULL != g_whdr[i].lpData) {
			free(g_whdr[i].lpData);
			g_whdr[i].lpData = NULL;
		}
	}

	waveOutReset(g_whdl);
	waveOutClose(g_whdl);
	g_whdl = NULL;
}

VOID WINAPI SendMidi(LPBYTE message) {
	BYTE ch = (*message) & 0x0F;
	BYTE b1 = *(message + 1);
	BYTE b2 = *(message + 2);

	switch ((*message) & 0xF0) {
	case 0x80:
		noteOff(g_channels[ch], b1);
		break;
	case 0x90:
		noteOn(g_channels[ch], b1, b2);
		break;
	case 0xB0:
		ctrlChange(g_channels[ch], b1, b2);
		break;
	case 0xC0:
		programChange(g_channels[ch], b1);
		break;
	case 0xE0:
		pitchBend(g_channels[ch], b1, b2);
		break;
	case 0xF0:
		sysEx(message);
		break;
	default:
		break;
	}
}

/******************************************************************************/
void CALLBACK DSEnumProc(LPGUID lpGUID, LPCTSTR lpszDesc, LPCTSTR lpszDrvName, LPVOID lpContext) {
	if (g_deviceListLength + _mbstrlen((const char*)lpszDesc) < DEVICE_LIST_SIZE) {
		g_deviceListLength += wsprintf(&g_deviceList[g_deviceListLength], TEXT("%s\n"), lpszDesc);
	}
}

void CALLBACK WaveCallback(HWND hWnd, UINT msg, UINT dwUser, LPWAVEHDR waveHdr, UINT dwParam2) {
	switch (msg) {
	case MM_WOM_OPEN:
		break;
	case MM_WOM_CLOSE:
		break;
	case MM_WOM_DONE:
		if (!g_isPlay) {
			break;
		}

		UINT b, t, s, ch;
		short *pWave;

		for (b = 0; b < BUFFER_COUNT; ++b) {
			if (0 == (g_whdr[b].dwFlags & WHDR_INQUEUE)) {
				pWave = (short*)g_whdr[b].lpData;
				for (t = 0; t < g_bufferLength; ++t) {
					for (s = 0; s < SAMPLER_COUNT; ++s) {
						if (g_samplers[s]->isActive) {
							samplerStep(g_channels[g_samplers[s]->channelNo], g_samplers[s]);
						}
					}

					g_waveL = 0.0;
					g_waveR = 0.0;
					for (ch = 0; ch < CHANNEL_COUNT; ++ch) {
						channelStep(g_channels[ch]);
					}

					if (g_waveL < -1.0) g_waveL = -1.0;
					if (g_waveR < -1.0) g_waveR = -1.0;
					if (1.0 < g_waveL) g_waveL = 1.0;
					if (1.0 < g_waveR) g_waveR = 1.0;
					*pWave = (short)(32767 * g_waveL); ++pWave;
					*pWave = (short)(32767 * g_waveR); ++pWave;
				}

				waveOutWrite(g_whdl, &g_whdr[b], sizeof(WAVEHDR));
				break;
			}
		}

		break;
	}
}

/******************************************************************************/
inline void channelStep(CHANNEL *ch) {
	g_waveL += ch->amp * ch->panL * ch->wave;
	g_waveR += ch->amp * ch->panR * ch->wave;
}

inline void samplerStep(CHANNEL *ch, SAMPLER *smpl) {
	if (smpl->onKey) {
		if (smpl->time < ch->envAmp.hold) {
			smpl->curAmp += (1.0 - smpl->curAmp) * ch->envAmp.attack / g_sampleRate;
		}
		else {
			smpl->curAmp += (ch->envAmp.sustain - smpl->curAmp) * ch->envAmp.decay / g_sampleRate;
		}
	}
	else {
		smpl->curAmp -= smpl->curAmp * ch->envAmp.releace / g_sampleRate;
		if (smpl->curAmp < 1.0 / 2000.0) {
			smpl->isActive = false;
		}
	}

	ch->wave += smpl->curAmp;

	smpl->index += smpl->delta * ch->pitch;
	smpl->time += 1.0 / g_sampleRate;
}

/******************************************************************************/
void noteOff(CHANNEL *ch, BYTE noteNo) {
	for (UINT i = 0; i < SAMPLER_COUNT; ++i) {
		if (g_samplers[i]->channelNo == ch->no && g_samplers[i]->noteNo == noteNo) {
			g_samplers[i]->onKey = false;
		}
	}
}

void noteOn(CHANNEL *ch, BYTE noteNo, BYTE velocity) {
	noteOff(ch, noteNo);

	if (0 == noteNo) {
		return;
	}

	for (UINT i = 0; i < SAMPLER_COUNT; ++i) {
		if (!g_samplers[i]->isActive) {
			g_samplers[i]->channelNo = ch->no;
			g_samplers[i]->noteNo = noteNo;

			g_samplers[i]->delta = 1.0 / g_sampleRate;
			g_samplers[i]->curAmp = 0.0;
			g_samplers[i]->index = 0.0;
			g_samplers[i]->time = 0.0;

			g_samplers[i]->onKey = true;
			g_samplers[i]->isActive = true;
			return;
		}
	}
}

void ctrlChange(CHANNEL *ch, BYTE type, BYTE value) {

}

void programChange(CHANNEL *ch, BYTE value) {

}

void pitchBend(CHANNEL *ch, BYTE lsb, BYTE msb) {

}

void sysEx(LPBYTE message) {
}