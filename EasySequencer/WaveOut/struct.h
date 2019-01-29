#pragma once
typedef unsigned    int     UInt32;
typedef signed      int     Int32;
typedef unsigned    short   UInt16;
typedef signed      short   Int16;
typedef unsigned    char    bool;

#define true    ((bool)1)
#define false   ((bool)0)

#pragma pack(4)
typedef struct DELAY {
    double depth;
    double rate;
    double *pTapL;
    double *pTapR;
    Int32 writeIndex;
    Int32 readIndex;
} DELAY;
#pragma

#pragma pack(4)
typedef struct CHORUS {
    double depth;
    double rate;
    double lfoK;
    double *pPanL;
    double *pPanR;
    double *pLfoRe;
    double *pLfoIm;
} CHORUS;
#pragma

#pragma pack(4)
typedef struct FILTER {
    double cutoff;
    double resonance;
    double pole00;
    double pole01;
    double pole02;
    double pole03;
    double pole10;
    double pole11;
    double pole12;
    double pole13;
} FILTER;
#pragma

#pragma pack(4)
typedef struct ENVELOPE {
    double levelA;
    double levelD;
    double levelS;
    double levelR;
    double deltaA;
    double deltaD;
    double deltaR;
    double hold;
} ENVELOPE;
#pragma

#pragma pack(4)
typedef struct CHANNEL {
    double wave;
    double waveL;
    double waveR;

    double pitch;
    double holdDelta;

    double panLeft;
    double panRight;

    double tarCutoff;
    double tarAmp;
    double curAmp;

    FILTER eq;
    DELAY delay;
    CHORUS chorus;
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

	double gain;
	double delta;

	double index;
	double time;

	double tarAmp;
	double curAmp;

	ENVELOPE envAmp;
	ENVELOPE envEq;

	FILTER eq;
} SAMPLER;
#pragma