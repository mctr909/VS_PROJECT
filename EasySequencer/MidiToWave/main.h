#pragma once
#include <windows.h>
#include <stdio.h>
#include <mmsystem.h>
#include <DSound.h>
#include <stdlib.h>
#include <math.h>

#include "dls.h"

#pragma comment (lib, "winmm.lib")
#pragma comment (lib, "DSound.lib")

/******************************************************************************/
#define DEVICE_LIST_SIZE	256
#define BUFFER_COUNT		16
#define CHANNEL_COUNT		16
#define SAMPLER_COUNT		128
#define KEY_COUNT			128

enum KEY_STATUS : BYTE {
	KEY_STATUS_OFF,
	KEY_STATUS_ON,
	KEY_STATUS_HOLD
};

/******************************************************************************/
typedef struct {
	BYTE bankMSB;
	BYTE bankLSB;

	BYTE vol;
	BYTE exp;
	BYTE pan;

	BYTE rev;
	BYTE cho;
	BYTE del;

	BYTE res;
	BYTE cut;
	BYTE atk;
	BYTE rel;

	BYTE vibRate;
	BYTE vibDepth;
	BYTE vibDelay;

	BYTE bendRange;
	BYTE hold;

	BYTE nrpnMSB;
	BYTE nrpnLSB;
	BYTE rpnMSB;
	BYTE rpnLSB;
} CONTROL;

typedef struct {
	UINT no;

	BYTE isDrum;
	BYTE programNo;
	BYTE pitchMSB;
	BYTE pitchLSB;

	CONTROL ctrl;

	KEY_STATUS keyBoard[KEY_COUNT] = { KEY_STATUS_OFF };

	DLS::LRGN *pLrgn = NULL;
	DLS::LART *pLart = NULL;

	double wave;
	double amp;
	double panLeft;
	double panRight;
	double pitch;
} CHANNEL;

typedef struct {
	CHANNEL *pChannel = NULL;
	UINT noteNo;
	BOOL onKey;
	BOOL isActive;

	DLS::WAVE *pWave = NULL;
	DLS::LART *pLart = NULL;
	DLS::WaveLoop loop;
	BOOL hasLoop = false;

	double delta;
	double index;
	double time;
	double tarAmp;
	double curAmp;
} SAMPLER;

/******************************************************************************/
HWAVEOUT		g_hWaveOut = NULL;
WAVEFORMATEX	g_waveFmt = { 0 };
WAVEHDR			g_waveHdr[BUFFER_COUNT] = { NULL };

UINT			g_bufferLength = 4096;
UINT			g_sampleRate = 44100;
UINT			g_deviceListLength = 0;
WCHAR			g_deviceList[DEVICE_LIST_SIZE] = { '\0' };

CHANNEL			*gp_channels[CHANNEL_COUNT] = { NULL };
SAMPLER			*gp_samplers[SAMPLER_COUNT] = { NULL };

DLS::DLS		*gp_dls = NULL;

BOOL			g_isStop = true;
BOOL			g_isMute = false;
BOOL			g_issueMute = false;

double			g_waveL = 0.0;
double			g_waveR = 0.0;

/******************************************************************************/
#ifdef __cplusplus
extern "C" {
	__declspec(dllexport) LPWSTR WINAPI WaveOutList();
	__declspec(dllexport) BOOL WINAPI WaveOutOpen(UINT deviceId, UINT sampleRate, UINT bufferLength);
	__declspec(dllexport) VOID WINAPI WaveOutClose();
	__declspec(dllexport) BOOL WINAPI DlsLoad(LPWSTR filePath);
	__declspec(dllexport) VOID WINAPI SendMidi(UINT message);
	__declspec(dllexport) VOID WINAPI Panic();
}
#endif

/******************************************************************************/
void CALLBACK DSEnumProc(LPGUID lpGUID, LPCTSTR lpszDesc, LPCTSTR lpszDrvName, LPVOID lpContext);
void CALLBACK WaveOutProc(HWAVEOUT hwo, UINT uMsg);

/******************************************************************************/
inline void channelStep(CHANNEL *pCh);
inline void samplerStep(SAMPLER *pSmpl);

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
