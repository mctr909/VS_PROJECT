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
/* �萔
/******************************************************************************/
const LPWSTR CLASS_NAME    = L"Spectrum";	// �E�B���h�E�N���X��
const LPWSTR WINDOW_TITLE  = L"Spectrum";	// �^�C�g��

const UINT32 NOTES          = 116;
const UINT32 NOTE_DIV       = 5;
const UINT32 START_NOTE     = 0;
const UINT32 BANKS          = NOTES * NOTE_DIV;

const DOUBLE PITCH          = 27.5 * pow(2.0, (START_NOTE - 0.25) / 12.0);
const DOUBLE SIGMA          = 60.0;
const DOUBLE ADJUST_AMP     = 1.0;
const UINT32 DFT_LENGTH     = 6000;
const UINT32 WINDOW_WIDTH   = 466;
const UINT32 WINDOW_HEIGHT  = 480;

const UINT32 DRAW_STEPX     = sizeof(DWORD);
const UINT32 DRAW_WIDTH     = (BANKS * DRAW_STEPX);
const UINT32 DRAW_HEIGHT    = 149;
const UINT32 GAUGE_HEIGHT   = 20;
const UINT32 PALLET_COLORS  = 64;

/******************************************************************************/
/* �O���[�o���ϐ�
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
/* �֐��v���g�^�C�v
/******************************************************************************/
int WINAPI WinMain(HINSTANCE, HINSTANCE, LPSTR, int);

LRESULT CALLBACK mainWndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT wmCreate(HWND&, UINT&, WPARAM&, LPARAM&);
LRESULT wmDestroy(HWND&, UINT&, WPARAM&, LPARAM&);
LRESULT wmPaint(HWND&, UINT&, WPARAM&, LPARAM&);
LRESULT wmUser(HWND&, UINT&, WPARAM&, LPARAM&);

void createPallet(HWND&);
void drawGauge(HWND, HBITMAP);
void plotSpectrum(HWND);
