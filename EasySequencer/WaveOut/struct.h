#pragma once
typedef unsigned	int		UInt32;
typedef signed		int		Int32;
typedef unsigned	short	UInt16;
typedef signed		short	Int16;
typedef unsigned	char	bool;

#define true	((bool)1)
#define false	((bool)0)

#pragma pack(4)
typedef struct {
	Int32 writeIndex;
	Int32 readIndex;
	double *pTapL;
	double *pTapR;
} DELAY_VALUES;
#pragma

#pragma pack(4)
typedef struct {
	double lfoK;
	double *pMixL;
	double *pMixR;
	double *pLfoRe;
	double *pLfoIm;
} CHORUS_VALUES;
#pragma

#pragma pack(4)
typedef struct CHANNEL {
	double wave;
	double pitch;
	double hold;
	double delayDepth;
	double delayRate;
	double chorusDepth;
	double chorusRate;
	double tarAmp;
	double curAmp;
	double panLeft;
	double panRight;
} CHANNEL;
#pragma

#pragma pack(4)
typedef struct SAMPLER {
	UInt16 channelNo;
	UInt16 noteNo;

	bool onKey;
	bool isActive;

	UInt32 pcmAddr;
	UInt32 pcmLength;

	bool loopEnable;
	UInt32 loopBegin;
	UInt32 loopLength;

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