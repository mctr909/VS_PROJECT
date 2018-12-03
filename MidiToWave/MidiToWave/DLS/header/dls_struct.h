#pragma once
#include "dls_const.h"

namespace DLS {
	typedef struct {
		BYTE BankLSB;
		BYTE BankMSB;
		BYTE Reserve1;
		BYTE BankFlags;
		BYTE ProgramNo;
		BYTE Reserve2;
		BYTE Reserve3;
		BYTE Reserve4;
	} MidiLocale;

	typedef struct {
		USHORT Low;
		USHORT High;
	} Range;

	typedef struct {
		SRC_TYPE Source;
		SRC_TYPE Control;
		DST_TYPE Destination;
		TRN_TYPE Transform;
		INT Scale;
	} Connection;

	typedef struct {
		UINT Size;
		UINT Type;
		UINT Start;
		UINT Length;
	} WaveLoop;

	typedef struct {
		CHUNK_TYPE Type;
		UINT Size;
	} CK_CHUNK;

	typedef struct {
		LIST_TYPE Type;
	} CK_LIST;

	typedef struct {
		UINT MSB;
		UINT LSB;
	} CK_VERS;

	typedef struct {
		UINT Instruments;
	} CK_COLH;

	typedef struct {
		UINT Regions;
		MidiLocale Locale;
	} CK_INSH;

	typedef struct {
		Range Key;
		Range Velocity;
		USHORT Options;
		USHORT KeyGroup;
		USHORT Layer;
	} CK_RGNH;

	typedef struct {
		UINT Size;
		UINT Count;
	} CK_ART1;

	typedef struct {
		USHORT Options;
		USHORT PhaseGroup;
		UINT Channel;
		UINT TableIndex;
	} CK_WLNK;

	typedef struct {
		UINT Size;
		USHORT UnityNote;
		short FineTune;
		int GainInt;
		UINT Options;
		UINT LoopCount;
	} CK_WSMP;

	typedef struct {
		UINT Size;
		UINT Count;
	} CK_PTBL;

	typedef struct {
		USHORT Tag;
		USHORT Channels;
		UINT SampleRate;
		UINT BytesPerSec;
		USHORT BlockAlign;
		USHORT Bits;
	} CK_FMT;

	typedef struct {
		INFO_TYPE Type;
		UINT Size;
	} CK_INFO;
}