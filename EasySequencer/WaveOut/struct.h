#pragma once
#include <windows.h>
#include "main.h"

#pragma pack(4)
typedef struct {
	UINT no;
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

	bool onKey;
	bool isActive;

	UINT pcmAddr;
	UINT pcmLength;

	bool loopEnable;
	UINT loopBegin;
	UINT loopLength;

	double tarAmp;

	double envAmp;
	double envAmpDeltaA;
	double envAmpDeltaD;
	double envAmpDeltaR;
	double envAmpLevel;
	double envAmpHold;

	double gain;
	double delta;

	double index;
	double time;
} SAMPLER;
#pragma