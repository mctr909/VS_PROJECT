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
	for (UINT i = 0; i < CHANNEL_COUNT; ++i) {
		if (NULL == gp_channels[i]) {
			gp_channels[i] = (CHANNEL*)malloc(sizeof(CHANNEL));
			if (NULL != gp_channels[i]) {
				initChannel(gp_channels[i], i);
			}
		}
	}

	//
	for (UINT i = 0; i < SAMPLER_COUNT; ++i) {
		if (NULL == gp_samplers[i]) {
			gp_samplers[i] = (SAMPLER*)malloc(sizeof(SAMPLER));
			if (NULL != gp_samplers[i]) {
				memset(gp_samplers[i], 0, sizeof(SAMPLER));
			}
		}
	}
	
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

	//
	for (UINT i = 0; i < CHANNEL_COUNT; ++i) {
		if (NULL != gp_channels[i]) {
			free(gp_channels[i]);
			gp_channels[i] = NULL;
		}
	}

	//
	for (UINT i = 0; i < SAMPLER_COUNT; ++i) {
		if (NULL != gp_samplers[i]) {
			free(gp_samplers[i]);
			gp_samplers[i] = NULL;
		}
	}
}

BOOL WINAPI DlsLoad(LPWSTR filePath) {
	//
	g_issueMute = true;
	while (!g_isMute && !g_isStop) {
		Sleep(100);
	}

	//
	Panic();

	//
	if (NULL != gp_dls) {
		delete gp_dls;
		gp_dls = NULL;
	}

	gp_dls = new DLS::DLS(filePath);

	DLS::MidiLocale l = { 0 };
	l.ProgramNo = 2;
	auto b = gp_dls->GetInst(l);
	auto c = b->mp_regions->GetRegion(20, 60);
	//auto d = gp_dls->GetWave(*c);

	//
	g_issueMute = false;

	return true;
}

VOID WINAPI SendMidi(LPBYTE message) {
	if (g_issueMute || g_isStop) {
		return;
	}

	BYTE ch = *message & 0x0F;
	BYTE b1 = *(message + 1);
	BYTE b2 = *(message + 2);

	switch (*message & 0xF0) {
	case 0x80:
		noteOff(gp_channels[ch], b1);
		break;
	case 0x90:
		noteOn(gp_channels[ch], b1, b2);
		break;
	case 0xB0:
		ctrlChange(gp_channels[ch], b1, b2);
		break;
	case 0xC0:
		programChange(gp_channels[ch], b1);
		break;
	case 0xE0:
		pitchBend(gp_channels[ch], b1, b2);
		break;
	case 0xF0:
		sysEx(message);
		break;
	default:
		break;
	}
}

VOID WINAPI Panic() {
	for (int s = 0; s < SAMPLER_COUNT; ++s) {
		SAMPLER *smpl = gp_samplers[s];
		if (NULL == smpl) {
			continue;
		}
		smpl->onKey = false;
		smpl->isActive = false;

		CHANNEL *ch = gp_channels[smpl->channelNo];
		if (NULL == ch) {
			continue;
		}
		ch->keyBoard[smpl->noteNo] = KEY_STATUS_OFF;
	}
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
		for (b = 0; b < BUFFER_COUNT; ++b) {
			if (0 == (g_waveHdr[b].dwFlags & WHDR_INQUEUE)) {
				pWave = (short*)g_waveHdr[b].lpData;
				for (t = 0; t < g_bufferLength; ++t) {
					for (s = 0; s < SAMPLER_COUNT; ++s) {
						samplerStep(gp_samplers[s]);
					}

					g_waveL = 0.0;
					g_waveR = 0.0;
					for (ch = 0; ch < CHANNEL_COUNT; ++ch) {
						channelStep(gp_channels[ch]);
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
inline void channelStep(CHANNEL *pCh) {
	g_waveL += pCh->amp * pCh->panLeft * pCh->wave;
	g_waveR += pCh->amp * pCh->panRight * pCh->wave;
	pCh->wave = 0.0;
}

inline void samplerStep(SAMPLER *pSmpl) {
	if (!pSmpl->isActive) {
		return;
	}

	if (pSmpl->onKey) {
		pSmpl->curAmp += (pSmpl->tarAmp - pSmpl->curAmp) * 2.718 / g_sampleRate;
	}
	else {
		pSmpl->curAmp -= pSmpl->curAmp * 2.718 / g_sampleRate;
		if (pSmpl->curAmp < 0.0001) {
			pSmpl->isActive = false;
		}
	}

	pSmpl->re -= 6.283 * pSmpl->delta * pSmpl->im;
	pSmpl->im += 6.283 * pSmpl->delta * pSmpl->re;

	pSmpl->index += pSmpl->delta;
	pSmpl->time += 1.0 / 44100.0;

	gp_channels[pSmpl->channelNo]->wave += pSmpl->im * pSmpl->curAmp;
}

/******************************************************************************/
void initChannel(CHANNEL *pCh, UINT channelNo) {
	memset(pCh, 0, sizeof(CHANNEL));
	pCh->no = channelNo;
	pCh->wave = 0.0;

	pCh->isDrum = false;
	pCh->programNo = 0;
	pCh->ctrl.bankMSB = 0;
	pCh->ctrl.bankLSB = 0;

	setAmp(pCh, 100, 100);
	setPan(pCh, 64);

	pCh->ctrl.hold = 0;

	pCh->ctrl.res = 64;
	pCh->ctrl.cut = 64;

	pCh->ctrl.rel = 64;
	pCh->ctrl.atk = 64;

	pCh->ctrl.vibRate = 64;
	pCh->ctrl.vibDepth = 64;
	pCh->ctrl.vibDelay = 64;

	pCh->ctrl.rev = 0;
	pCh->ctrl.cho = 0;
	pCh->ctrl.del = 0;

	pCh->ctrl.nrpnLSB = 0xFF;
	pCh->ctrl.nrpnMSB = 0xFF;
	pCh->ctrl.rpnLSB = 0xFF;
	pCh->ctrl.rpnMSB = 0xFF;

	pCh->ctrl.bendRange = 2;
	pitchBend(pCh, 0, 0);
}

void noteOff(CHANNEL *pCh, BYTE noteNo) {
	if (NULL == pCh) {
		return;
	}

	for (UINT i = 0; i < SAMPLER_COUNT; ++i) {
		if (gp_samplers[i]->channelNo == pCh->no && gp_samplers[i]->noteNo == noteNo) {
			if (KEY_STATUS_HOLD != pCh->keyBoard[noteNo]) {
				pCh->keyBoard[noteNo] = KEY_STATUS_OFF;
				gp_samplers[i]->onKey = false;
			}
		}
	}
}

void noteOn(CHANNEL *pCh, BYTE noteNo, BYTE velocity) {
	if (NULL == pCh) {
		return;
	}

	noteOff(pCh, noteNo);

	if (0 == velocity) {
		return;
	}

	SAMPLER *pSmpl = NULL;
	for (UINT i = 0; i < SAMPLER_COUNT; ++i) {
		pSmpl = gp_samplers[i];
		if (!pSmpl->isActive) {
			pSmpl->channelNo = pCh->no;
			pSmpl->noteNo = noteNo;

			pSmpl->delta = 8.1758 * pow(2.0, noteNo / 12.0) / 44100;
			pSmpl->index = 0.0;
			pSmpl->time = 0.0;

			pSmpl->tarAmp = velocity / 127.0;
			pSmpl->curAmp = 0.0;

			pSmpl->re = 1.0;
			pSmpl->im = 0.0;

			pCh->keyBoard[noteNo] = KEY_STATUS_ON;

			pSmpl->onKey = true;
			pSmpl->isActive = true;
			return;
		}
	}
}

void ctrlChange(CHANNEL *pCh, BYTE type, BYTE b1) {
	if (NULL == pCh) {
		return;
	}

	switch (type) {
	case 0x00:
		pCh->ctrl.bankMSB = b1; break;
	case 0x20:
		pCh->ctrl.bankLSB = b1; break;

	case 0x06:
		rpn(pCh, b1);
		nrpn(pCh, b1);
		break;

	case 0x07:
		setAmp(pCh, b1, pCh->ctrl.exp); break;
	case 0x0A:
		setPan(pCh, b1); break;
	case 0x0B:
		setAmp(pCh, pCh->ctrl.vol, b1); break;

	case 0x40:
		setHold(pCh, b1); break;

	case 0x47:
		pCh->ctrl.res = b1; break;
	case 0x4A:
		pCh->ctrl.cut = b1; break;

	case 0x48:
		pCh->ctrl.rel = b1; break;
	case 0x49:
		pCh->ctrl.atk = b1; break;

	case 0x4C:
		pCh->ctrl.vibRate = b1; break;
	case 0x4D:
		pCh->ctrl.vibDepth = b1; break;
	case 0x4E:
		pCh->ctrl.vibDelay = b1; break;

	case 0x5B:
		pCh->ctrl.rev = b1; break;
	case 0x5D:
		pCh->ctrl.cho = b1; break;
	case 0x5E:
		pCh->ctrl.del = b1; break;

	case 0x62:
		pCh->ctrl.nrpnLSB = b1; break;
	case 0x63:
		pCh->ctrl.nrpnMSB = b1; break;
	case 0x64:
		pCh->ctrl.rpnLSB = b1; break;
	case 0x65:
		pCh->ctrl.rpnMSB = b1; break;

	case 0x79:
		initChannel(pCh, pCh->no);
		break;
	}
}

void programChange(CHANNEL *pCh, BYTE value) {
	if (NULL == pCh) {
		return;
	}

	pCh->programNo = value;
	pCh->ctrl.bankMSB;
	pCh->ctrl.bankLSB;
}

void pitchBend(CHANNEL *pCh, BYTE lsb, BYTE msb) {
	if (NULL == pCh) {
		return;
	}

	pCh->ctrl.bankLSB = lsb;
	pCh->ctrl.bankMSB = msb;
	pCh->pitch = pow(2.0, pCh->ctrl.bendRange / 12.0 * ((lsb | msb << 7) - 8192.0) / 8192.0);
}

void sysEx(LPBYTE message) {

}

/******************************************************************************/
void setAmp(CHANNEL *pCh, BYTE vol, BYTE exp) {
	pCh->ctrl.vol = vol;
	pCh->ctrl.exp = exp;
	pCh->amp = vol / 127.0 * exp / 127.0;
}

void setPan(CHANNEL *pCh, BYTE value) {
	pCh->ctrl.pan = value;
	pCh->panLeft = 1.0;
	pCh->panRight = 1.0;
}

void setHold(CHANNEL *pCh, BYTE value) {
	if (value < 64) {
		for (int k = 0; k < KEY_COUNT; ++k) {
			if (KEY_STATUS_HOLD == pCh->keyBoard[k]) {
				pCh->keyBoard[k] = KEY_STATUS_OFF;
				noteOff(pCh, k);
			}
		}
	}
	else {
		for (int k = 0; k < KEY_COUNT; ++k) {
			if (KEY_STATUS_ON == pCh->keyBoard[k]) {
				pCh->keyBoard[k] = KEY_STATUS_HOLD;
			}
		}
	}

	pCh->ctrl.hold = value;
}

void rpn(CHANNEL *pCh, BYTE b1) {
	switch (pCh->ctrl.rpnLSB | pCh->ctrl.rpnMSB << 8) {
	case 0x0000:
		pCh->ctrl.bendRange = b1; break;
	default:
		break;
	}

	pCh->ctrl.rpnLSB = 0xFF;
	pCh->ctrl.rpnMSB = 0xFF;
}

void nrpn(CHANNEL *pCh, BYTE b1) {
	pCh->ctrl.nrpnLSB = 0xFF;
	pCh->ctrl.nrpnMSB = 0xFF;
}