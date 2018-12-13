#pragma once
#include "struct.h"

/******************************************************************************/
#define CHANNEL_COUNT		16
#define SAMPLER_COUNT		256

/******************************************************************************/
LPBYTE loadDLS(LPWSTR filePath, UInt32 *size, UInt32 sampleRate);
CHANNEL** getChannelPtr();
SAMPLER** getSamplerPtr();
bool isIdle();

/******************************************************************************/
inline extern void channelStep(UInt32 no, double *waveL, double *waveR);
inline extern void samplerStep(UInt32 no);

/******************************************************************************/
inline void delayStep(CHANNEL *ch, DELAY_VALUES *delay);
inline void chorusStep(CHANNEL *ch, DELAY_VALUES *delay, CHORUS_VALUES *chorus);
inline void filterStep(FILTER *filter, double input, double *output);
