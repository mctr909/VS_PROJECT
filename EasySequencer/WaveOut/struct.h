#pragma once
#include <windows.h>

#pragma pack(4)
typedef struct {
	DWORD no;
	DOUBLE wave;
	DOUBLE pitch;
	DOUBLE hold;
	DOUBLE delayDepth;
	DOUBLE delayRate;
	DOUBLE chorusDepth;
	DOUBLE chorusRate;
	DOUBLE tarAmp;
	DOUBLE curAmp;
	DOUBLE panLeft;
	DOUBLE panRight;
} CHANNEL;
#pragma

#pragma pack(4)
typedef struct {
	USHORT channelNo;
	USHORT noteNo;

	BOOLEAN onKey;
	BOOLEAN isActive;

	DWORD pcmAddr;
	DWORD pcmLength;

	BOOLEAN loopEnable;
	DWORD loopBegin;
	DWORD loopLength;

	DOUBLE tarAmp;

	DOUBLE envAmp;
	DOUBLE envAmpDeltaA;
	DOUBLE envAmpDeltaD;
	DOUBLE envAmpDeltaR;
	DOUBLE envAmpLevel;
	DOUBLE envAmpHold;

	DOUBLE gain;
	DOUBLE delta;

	DOUBLE index;
	DOUBLE time;
} SAMPLER;
#pragma