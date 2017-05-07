#pragma once
#include <windows.h>

class WaveIn
{
	//*******************************************************************
	// �����o�萔
	//*******************************************************************
public:
	static const UINT32 SAMPLE_RATE = 44100;
	static const UINT32 SAMPLES = 768;

private:
	static const UINT32 BUFFERS = 4;
	static const UINT32 BUFFER_SIZE = SAMPLES * sizeof(INT16);


	//*******************************************************************
	// �����o�ϐ�
	//*******************************************************************
public:
	bool		IsWaveOpen = false;
	INT16		m_pBuffer[SAMPLES];

private:
	HWAVEIN		m_hWaveIn		= NULL;
	HGLOBAL		m_hgRecBuffer	= NULL;
	LPWAVEHDR	m_whdr[BUFFERS];

	//*******************************************************************
	// �֐��v���g�^�C�v
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
