#pragma once
#include <windows.h>
#include <stdio.h>
#include <mmsystem.h>
#include <DSound.h>
#include <stdlib.h>
#include <math.h>

#include "struct.h"

#pragma comment (lib, "winmm.lib")
#pragma comment (lib, "DSound.lib")

/******************************************************************************/
#define BUFFER_COUNT		16
#define CHANNEL_COUNT		16
#define SAMPLER_COUNT		128
#define DEVICE_LIST_SIZE	1024

/******************************************************************************/
HWAVEOUT		g_hWaveOut = NULL;
WAVEFORMATEX	g_waveFmt = { 0 };
WAVEHDR			g_waveHdr[BUFFER_COUNT] = { NULL };

UINT			g_bufferLength = 4096;
UINT			g_sampleRate = 44100;
UINT			g_delayTaps = (UINT)(0.5 * g_sampleRate);
UINT			g_chorusPhases = 5;
UINT			g_deviceListLength = 0;
WCHAR			g_deviceList[DEVICE_LIST_SIZE] = { '\0' };

LPBYTE			gp_buffer = NULL;
CHANNEL			**gpp_channels = NULL;
SAMPLER			**gpp_samplers = NULL;

BOOL			g_isStop = true;
BOOL			g_isMute = true;
BOOL			g_issueMute = false;

double			g_waveL = 0.0;
double			g_waveR = 0.0;

/******************************************************************************/
typedef struct {
	int writeIndex;
	int readIndex;
	DOUBLE *pTapL = NULL;
	DOUBLE *pTapR = NULL;
} DELAY_VALUES;

typedef struct {
	DOUBLE lfoK;
	DOUBLE *pLfoRe = NULL;
	DOUBLE *pLfoIm = NULL;
} CHORUS_VALUES;

DELAY_VALUES gp_delay[CHANNEL_COUNT] = { 0 };
CHORUS_VALUES gp_chorus[CHANNEL_COUNT] = { 0 };

/******************************************************************************/
#ifdef __cplusplus
extern "C" {
	__declspec(dllexport) LPWSTR WINAPI WaveOutList();
	__declspec(dllexport) BOOL WINAPI WaveOutOpen(UINT deviceId, UINT sampleRate, UINT bufferLength);
	__declspec(dllexport) VOID WINAPI WaveOutClose();
	__declspec(dllexport) CHANNEL** WINAPI GetChannelPtr();
	__declspec(dllexport) SAMPLER** WINAPI GetSamplerPtr();
	__declspec(dllexport) VOID WINAPI Attach(LPBYTE ptr, INT size);
}
#endif

/******************************************************************************/
void CALLBACK DSEnumProc(LPGUID lpGUID, LPCTSTR lpszDesc, LPCTSTR lpszDrvName, LPVOID lpContext);
void CALLBACK WaveOutProc(HWAVEOUT hwo, UINT uMsg);

/******************************************************************************/
inline void channelStep(CHANNEL &ch);
inline void samplerStep(SAMPLER &smpl);
