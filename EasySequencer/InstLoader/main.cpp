#include "main.h"
#include "const.h"

/******************************************************************************/
BOOL WINAPI LoadDLS(LPWSTR filePath) {
	//
	if (NULL != gp_dls) {
		delete gp_dls;
		gp_dls = NULL;
	}

	gp_dls = new DLS::DLS(filePath);

	return true;
}

VOID WINAPI SendMidi(LPBYTE message) {
	BYTE ch = *message & 0x0F;
	BYTE b1 = *message + 1;
	BYTE b2 = *message + 2;

	switch (*message & 0xF0) {
	case 0x80:
		noteOff(gpp_channels[ch], b1);
		break;
	case 0x90:
		noteOn(gpp_channels[ch], b1, b2);
		break;
	case 0xB0:
		ctrlChange(gpp_channels[ch], b1, b2);
		break;
	case 0xC0:
		programChange(gpp_channels[ch], b1);
		break;
	case 0xE0:
		pitchBend(gpp_channels[ch], b1, b2);
		break;
	case 0xF0:
		break;
	default:
		break;
	}
}

/******************************************************************************/
void initChannel(CHANNEL *pCh, UINT channelNo) {
	if (NULL == pCh) {
		return;
	}

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

	programChange(pCh, 0);
}

void noteOff(CHANNEL *pCh, BYTE noteNo) {
	if (NULL == pCh) {
		return;
	}

	for (UINT i = 0; i < SAMPLER_COUNT; ++i) {
		if (NULL == gpp_samplers[i] || NULL == gpp_samplers[i]->pChannel) {
			continue;
		}
		if (gpp_samplers[i]->pChannel->no == pCh->no && gpp_samplers[i]->noteNo == noteNo) {
			if (KEY_STATUS_HOLD != pCh->keyBoard[noteNo]) {
				pCh->keyBoard[noteNo] = KEY_STATUS_OFF;
				gpp_samplers[i]->onKey = false;
			}
		}
	}
}

void noteOn(CHANNEL *pCh, BYTE noteNo, BYTE velocity) {
	if (NULL == pCh) {
		return;
	}

	if (0 == velocity) {
		noteOff(pCh, noteNo);
		return;
	}

	SAMPLER *pSmpl = NULL;
	for (UINT i = 0; i < SAMPLER_COUNT; ++i) {
		pSmpl = gpp_samplers[i];
		if (!pSmpl->isActive) {
			pSmpl->pChannel = pCh;
			pSmpl->noteNo = noteNo;

			pSmpl->tarAmp = velocity / 127.0;
			pSmpl->curAmp = 0.0;

			pCh->keyBoard[noteNo] = KEY_STATUS_ON;

			auto pRgn = pCh->pLrgn->GetRegion(noteNo, velocity);
			if (NULL == pRgn) {
				continue;
			}
			auto pWave = gp_dls->mp_wavePool->GetWave(*pRgn);
			if (NULL == pWave) {
				continue;
			}

			pSmpl->pWave = pWave;

			if (NULL == pRgn->mp_sampler || 0 == pRgn->mp_sampler->LoopCount) {
				if (NULL == pWave->mp_sampler || 0 == pWave->mp_sampler->LoopCount) {
					pSmpl->loop.Start = 0;
					pSmpl->loop.Length = pWave->m_dataSize / pWave->mp_format->BlockAlign;
					pSmpl->hasLoop = false;
				}
				else {
					pSmpl->loop = pWave->mp_loops[0];
					pSmpl->hasLoop = true;
				}
			}
			else {
				pSmpl->loop = pRgn->mp_loops[0];
				pSmpl->hasLoop = true;
			}

			//
			{
				pSmpl->delta = (double)pWave->mp_format->SampleRate / g_sampleRate;
				INT diffNote = 0;
				INT fineTune = 0;
				if (NULL == pRgn->mp_sampler) {
					diffNote = noteNo - pWave->mp_sampler->UnityNote;
					fineTune = pWave->mp_sampler->FineTune;
				}
				else {
					diffNote = noteNo - pRgn->mp_sampler->UnityNote;
					fineTune = pRgn->mp_sampler->FineTune;
				}

				pSmpl->delta *= pow(2.0, fineTune / 1200.0);

				if (diffNote < 0) {
					pSmpl->delta /= SemiTone[-diffNote];
				}
				else {
					pSmpl->delta *= SemiTone[diffNote];
				}
			}

			if (NULL == pRgn->mp_articulations) {
				pSmpl->pLart = pSmpl->pChannel->pLart;
			}
			else {
				pSmpl->pLart = pRgn->mp_articulations;
			}

			//pSmpl->delta = 8.1758 * pow(2.0, noteNo / 12.0) / g_sampleRate;

			pSmpl->index = 0.0;
			pSmpl->time = 0.0;

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

	DLS::MidiLocale locale;
	locale.BankFlags = (9 == pCh->no) ? 0x80 : 0x00;
	locale.ProgramNo = value;
	locale.BankMSB = pCh->ctrl.bankMSB;
	locale.BankLSB = pCh->ctrl.bankLSB;

	if (NULL == gp_dls) {
		pCh->pLrgn = NULL;
		pCh->pLart = NULL;
	}
	else {
		auto pInst = gp_dls->mp_instruments->GetInst(locale);
		if (NULL == pInst) {
			locale.BankMSB = 0;
			locale.BankLSB = 0;
			pInst = gp_dls->mp_instruments->GetInst(locale);
		}
		if (NULL == pInst) {
			locale.ProgramNo = 0;
			pInst = gp_dls->mp_instruments->GetInst(locale);
		}
		if (NULL != pInst) {
			pCh->pLrgn = pInst->mp_regions;
			pCh->pLart = pInst->mp_articulations;
		}
	}
}

void pitchBend(CHANNEL *pCh, BYTE lsb, BYTE msb) {
	if (NULL == pCh) {
		return;
	}

	auto temp = (short)((lsb | (msb << 7)) - 8192) * pCh->ctrl.bendRange;
	if (temp < 0) {
		temp = -temp;
		pCh->pitch = 1.0 / (SemiTone[temp >> 13] * PitchMSB[(temp >> 7) % 64] * PitchLSB[temp % 128]);
	}
	else {
		pCh->pitch = SemiTone[temp >> 13] * PitchMSB[(temp >> 7) % 64] * PitchLSB[temp % 128];
	}
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

	pCh->ctrl.rpnMSB = 0xFF;
	pCh->ctrl.rpnLSB = 0xFF;
}
