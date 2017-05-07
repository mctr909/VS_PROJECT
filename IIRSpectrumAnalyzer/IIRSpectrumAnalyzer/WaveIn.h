#pragma once
#include <windows.h>

class WaveIn
{
	//*******************************************************************
	// メンバ定数
	//*******************************************************************
public:
	static const UINT32 SAMPLE_RATE = 44100;
	static const UINT32 SAMPLES = 768;

private:
	static const UINT32 BUFFERS = 4;
	static const UINT32 BUFFER_SIZE = SAMPLES * sizeof(INT16);


	//*******************************************************************
	// メンバ変数
	//*******************************************************************
public:
	bool		IsWaveOpen = false;
	INT16		m_pBuffer[SAMPLES];

private:
	HWAVEIN		m_hWaveIn		= NULL;
	HGLOBAL		m_hgRecBuffer	= NULL;
	LPWAVEHDR	m_whdr[BUFFERS];

	//*******************************************************************
	// 関数プロトタイプ
	//*******************************************************************
public:
	WaveIn(HWND);
	~WaveIn();

public:
	MMRESULT Reuse(LPARAM);
	void SetBuffer(LPARAM);

private:
	BOOL OpenDevice(HWND);
	void CloseDevice();
	void StartRecording();
	static void CALLBACK WaveInProc(HWAVEIN, UINT, DWORD, DWORD, DWORD);
};
