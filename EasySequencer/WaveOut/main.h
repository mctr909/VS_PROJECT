#pragma once
#include <windows.h>
#include <mmsystem.h>
#include <DSound.h>

#include "struct.h"

#pragma comment (lib, "winmm.lib")
#pragma comment (lib, "DSound.lib")

/******************************************************************************/
#define BUFFER_COUNT        16
#define DEVICE_LIST_SIZE    1024
#define CHANNEL_COUNT       16
#define SAMPLER_COUNT       256

/******************************************************************************/
__declspec(dllexport) LPWSTR WINAPI WaveOutList();
__declspec(dllexport) BOOL WINAPI WaveOutOpen(UInt32 deviceId, UInt32 sampleRate, UInt32 bufferLength);
__declspec(dllexport) VOID WINAPI WaveOutClose();
__declspec(dllexport) CHANNEL** WINAPI GetChannelPtr();
__declspec(dllexport) SAMPLER** WINAPI GetSamplerPtr();
__declspec(dllexport) LPBYTE WINAPI LoadDLS(LPWSTR filePath, UInt32 *size, UInt32 sampleRate);

/******************************************************************************/
void CALLBACK DSEnumProc(LPGUID lpGUID, LPCTSTR lpszDesc, LPCTSTR lpszDrvName, LPVOID lpContext);
void CALLBACK WaveOutProc(HWAVEOUT hwo, UInt32 uMsg);
