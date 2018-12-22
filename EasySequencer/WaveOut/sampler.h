#pragma once
#include "struct.h"

/******************************************************************************/
LPBYTE loadDLS(LPWSTR filePath, UInt32 *size, UInt32 sampleRate);
CHANNEL** createChannels(UInt32 count);
SAMPLER** createSamplers(UInt32 count);

/******************************************************************************/
inline extern void channelStep(CHANNEL *ch, double *waveL, double *waveR);
inline extern void samplerStep(CHANNEL **chs, SAMPLER *smpl);

/******************************************************************************/
inline void delayStep(CHANNEL *ch, DELAY *delay);
inline void chorusStep(CHANNEL *ch, DELAY *delay, CHORUS *chorus);
inline void filterStep(FILTER *filter, double input, double *output);
