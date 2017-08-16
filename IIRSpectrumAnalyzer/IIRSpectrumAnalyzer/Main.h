#pragma once
#include <windows.h>
#include <commctrl.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#include "WaveIn.h"
#include "IIRFilter.h"

#pragma comment (lib, "winmm.lib")

#define RGBQUAD2COLORREF(p) (COLORREF)( \
	(DWORD)(BYTE)HIWORD((DWORD)(p)) | ((DWORD)(p) & 0xff00) | ((DWORD)(BYTE)(DWORD)(p)<<16) \
)

const LPWSTR CLASS_NAME			= L"IIR Spectrum";	// ウィンドウクラス名
const LPWSTR WINDOW_TITLE		= L"IIR Spectrum";	// タイトル

const UINT32 NOTES				= 120;
const UINT32 NOTE_DIV			= 3;
const UINT32 START_NOTE			= 7;
const UINT32 BANKS				= NOTES * NOTE_DIV;
const DOUBLE PITCH				= 13.75 * pow(2.0, (START_NOTE - 0.25) / 12.0);

const UINT32 WINDOW_WIDTH		= 434;
const UINT32 WINDOW_HEIGHT		= 320;

const UINT32 DRAW_STEPX			= sizeof(DWORD);
const UINT32 DRAW_WIDTH			= (BANKS * DRAW_STEPX);
const UINT32 DRAW_HEIGHT		= 96;
const UINT32 GAUGE_HEIGHT		= 16;
const UINT32 PALLET_COLORS		= 64;

//*******************************************************************
// グローバル変数
//*******************************************************************
HINSTANCE		hInst		= NULL;
HWND			hToolWnd	= NULL;
HPALETTE		hPalette	= NULL;
HBITMAP			hMemBitmap	= NULL;

WaveIn*			cWaveIn		= NULL;
IIRFilter*		cIIR		= NULL;
LPBITMAPINFO	lpBitmap	= NULL;
LPBYTE			lpBits		= NULL;

double			gAvgLevel	= 32768.0;

//*******************************************************************
// 関数プロトタイプ
//*******************************************************************
int WINAPI WinMain(HINSTANCE, HINSTANCE, LPSTR, int);

LRESULT CALLBACK MainWndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT wmCreate(HWND&, UINT&, WPARAM&, LPARAM&);
LRESULT wmDestroy(HWND&, UINT&, WPARAM&, LPARAM&);
LRESULT wmPaint(HWND&, UINT&, WPARAM&, LPARAM&);
LRESULT wmUser(HWND&, UINT&, WPARAM&, LPARAM&);

void DrawGauge(HWND, HBITMAP);
void PlotSpectrum(HWND);
