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

UInt32			g_bufferLength = 4096;
UInt32			g_sampleRate = 44100;
UInt32			g_delayTaps = 44100;
UInt32			g_chorusPhases = 3;
UInt32			g_deviceListLength = 0;
WCHAR			g_deviceList[DEVICE_LIST_SIZE] = { '\0' };

LPBYTE			gp_buffer = NULL;
CHANNEL			**gpp_channels = NULL;
SAMPLER			**gpp_samplers = NULL;

bool			g_isStop = true;
bool			g_isMute = true;
bool			g_issueMute = false;

DELAY_VALUES	gp_delay[CHANNEL_COUNT] = { 0 };
CHORUS_VALUES	gp_chorus[CHANNEL_COUNT] = { 0 };

double			g_waveL = 0.0;
double			g_waveR = 0.0;
double			g_chWaveC = 0.0;
double			g_chWaveL = 0.0;
double			g_chWaveR = 0.0;

/******************************************************************************/
__declspec(dllexport) LPWSTR WINAPI WaveOutList();
__declspec(dllexport) BOOL WINAPI WaveOutOpen(UInt32 deviceId, UInt32 sampleRate, UInt32 bufferLength);
__declspec(dllexport) VOID WINAPI WaveOutClose();
__declspec(dllexport) CHANNEL** WINAPI GetChannelPtr();
__declspec(dllexport) SAMPLER** WINAPI GetSamplerPtr();
__declspec(dllexport) LPBYTE WINAPI LoadDLS(LPWSTR filePath, UInt32 *size);

/******************************************************************************/
void CALLBACK DSEnumProc(LPGUID lpGUID, LPCTSTR lpszDesc, LPCTSTR lpszDrvName, LPVOID lpContext);
void CALLBACK WaveOutProc(HWAVEOUT hwo, UInt32 uMsg);

/******************************************************************************/
inline void channelStep(CHANNEL *ch, UInt32 no);
inline void delayStep(CHANNEL *ch, DELAY_VALUES *delay);
inline void chorusStep(CHANNEL *ch, DELAY_VALUES *delay, CHORUS_VALUES *chorus);
inline void samplerStep(SAMPLER *smpl);
