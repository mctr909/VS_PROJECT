#pragma once
#include <windows.h>
#include <mmsystem.h>
#include <DSound.h>
#include <stdlib.h>

#pragma comment (lib, "winmm.lib")
#pragma comment (lib, "DSound.lib")

/******************************************************************************/
#define DEVICE_LIST_SIZE	4096
#define BUFFER_COUNT		2
#define CHANNEL_COUNT		16
#define SAMPLER_COUNT		128

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

	BYTE cut;
	BYTE res;

	BYTE atk;
	BYTE rel;

	BYTE vibRate;
	BYTE vibDepth;
	BYTE vibDelay;

	BYTE rpnMSB;
	BYTE rpnLSB;

	BYTE bendRange;
} CTRL;

typedef struct {
	double attack;
	double hold;
	double decay;
	double sustain;
	double releace;
} ENVELOPE;

typedef struct {
	BYTE no;
	double wave;
	CTRL ctrl;

	ENVELOPE envAmp;

	double amp;
	double panL;
	double panR;
	double pitch;
} CHANNEL;

typedef struct {
	BYTE channelNo;
	BYTE noteNo;
	BOOL isActive;
	BOOL onKey;

	double curAmp;
	double curCutoff;
	double delta;
	double index;
	double time;
} SAMPLER;

/******************************************************************************/
BOOL			g_isPlay = false;
HWAVEOUT		g_whdl = NULL;
WAVEHDR			g_whdr[BUFFER_COUNT] = { NULL };
WAVEFORMATEX	g_wfe = { 0 };

UINT			g_bufferLength = 4096;
UINT			g_sampleRate = 44100;
UINT			g_deviceListLength = 0;
WCHAR			g_deviceList[DEVICE_LIST_SIZE] = { '\0' };

CHANNEL			*g_channels[CHANNEL_COUNT] = { NULL };
SAMPLER			*g_samplers[SAMPLER_COUNT] = { NULL };

double			g_waveL = 0.0;
double			g_waveR = 0.0;

/******************************************************************************/
#ifdef __cplusplus
extern "C" {
	__declspec(dllexport) LPWSTR WINAPI WaveOutList();
	__declspec(dllexport) BOOL WINAPI WaveOutOpen(UINT deviceId, UINT sampleRate, UINT bufferLength);
	__declspec(dllexport) VOID WINAPI WaveOutClose();
	__declspec(dllexport) VOID WINAPI SendMidi(LPBYTE message);
}
#endif

/******************************************************************************/
void CALLBACK DSEnumProc(LPGUID lpGUID, LPCTSTR lpszDesc, LPCTSTR lpszDrvName, LPVOID lpContext);
void CALLBACK WaveCallback(HWND hWnd, UINT msg, UINT dwUser, LPWAVEHDR waveHdr, UINT dwParam2);

/******************************************************************************/
inline void channelStep(CHANNEL *ch);
inline void samplerStep(CHANNEL *ch, SAMPLER *smpl);

/******************************************************************************/
void noteOff(CHANNEL *ch, BYTE noteNo);
void noteOn(CHANNEL *ch, BYTE noteNo, BYTE velocity);
void ctrlChange(CHANNEL *ch, BYTE type, BYTE value);
void programChange(CHANNEL *ch, BYTE value);
void pitchBend(CHANNEL *ch, BYTE lsb, BYTE msb);
void sysEx(LPBYTE message);
