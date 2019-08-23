#pragma once
#include <windows.h>
#include <commctrl.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#include "WaveIn.h"
#include "dft.h"

#pragma comment (lib, "winmm.lib")

#define RGBQUAD2COLORREF(p) (COLORREF)( \
	(DWORD)(BYTE)HIWORD((DWORD)(p)) | ((DWORD)(p) & 0xff00) | ((DWORD)(BYTE)(DWORD)(p)<<16) \
)

/******************************************************************************/
/* 定数
/******************************************************************************/
const LPWSTR CLASS_NAME    = L"Spectrum";	// ウィンドウクラス名
const LPWSTR WINDOW_TITLE  = L"Spectrum";	// タイトル

const UINT32 NOTES         = 118;
const UINT32 NOTE_DIV      = 4;
const UINT32 START_NOTE    = 12;
const UINT32 BANKS         = NOTES * NOTE_DIV;
const DOUBLE PITCH         = 13.75 * pow(2.0, (START_NOTE - 0.25) / 12.0);
const DOUBLE SIGMA         = 48.0;
const UINT32 WINDOW_WIDTH  = 474;
const UINT32 WINDOW_HEIGHT = 320;

const UINT32 DRAW_STEPX    = sizeof(DWORD);
const UINT32 DRAW_WIDTH    = (BANKS * DRAW_STEPX);
const UINT32 DRAW_HEIGHT   = 128;
const UINT32 GAUGE_HEIGHT  = 16;
const UINT32 PALLET_COLORS = 64;

/******************************************************************************/
/* グローバル変数
/******************************************************************************/
HINSTANCE       hInst      = NULL;
HWND            hToolWnd   = NULL;
HPALETTE        hPalette   = NULL;
HBITMAP         hMemBitmap = NULL;

WaveIn*         cWaveIn    = NULL;
double          *gpAmp     = NULL;
LPBITMAPINFO    lpBitmap   = NULL;
LPBYTE          lpBits     = NULL;

double          gAvgLevel  = 1.0;

/******************************************************************************/
/* 関数プロトタイプ
/******************************************************************************/
int WINAPI WinMain(HINSTANCE, HINSTANCE, LPSTR, int);

LRESULT CALLBACK mainWndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT wmCreate(HWND&, UINT&, WPARAM&, LPARAM&);
LRESULT wmDestroy(HWND&, UINT&, WPARAM&, LPARAM&);
LRESULT wmPaint(HWND&, UINT&, WPARAM&, LPARAM&);
LRESULT wmUser(HWND&, UINT&, WPARAM&, LPARAM&);

void drawGauge(HWND, HBITMAP);
void plotSpectrum(HWND);
