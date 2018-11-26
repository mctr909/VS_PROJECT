#pragma once
#include <windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#include "struct.h"
#include "dls.h"

/******************************************************************************/
UINT			g_sampleRate = 44100;
DLS::DLS		*gp_dls = NULL;
CHANNEL			*gpp_channels[CHANNEL_COUNT] = { NULL };
SAMPLER			*gpp_samplers[SAMPLER_COUNT] = { NULL };

/******************************************************************************/
#ifdef __cplusplus
extern "C" {
	__declspec(dllexport) BOOL WINAPI LoadDLS(LPWSTR filePath);
	__declspec(dllexport) VOID WINAPI SendMidi(LPBYTE message);
}
#endif

/******************************************************************************/
void initChannel(CHANNEL *pCh, UINT channelNo);
void noteOff(CHANNEL *pCh, BYTE noteNo);
void noteOn(CHANNEL *pCh, BYTE noteNo, BYTE velocity);
void ctrlChange(CHANNEL *pCh, BYTE type, BYTE b1);
void programChange(CHANNEL *pCh, BYTE value);
void pitchBend(CHANNEL *pCh, BYTE lsb, BYTE msb);

/******************************************************************************/
void setAmp(CHANNEL *pCh, BYTE vol, BYTE exp);
void setPan(CHANNEL *pCh, BYTE value);
void setHold(CHANNEL *pCh, BYTE value);
void rpn(CHANNEL *pCh, BYTE b1);
