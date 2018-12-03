#pragma once
#include <windows.h>
#include "dls.h"

/******************************************************************************/
#define DEVICE_LIST_SIZE	256
#define BUFFER_COUNT		16
#define CHANNEL_COUNT		16
#define SAMPLER_COUNT		128
#define KEY_COUNT			128

/******************************************************************************/
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