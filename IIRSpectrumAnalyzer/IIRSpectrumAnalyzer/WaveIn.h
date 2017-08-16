#pragma once
#include <windows.h>

class WaveIn
{
	//*******************************************************************
	// �����o�萔
	//*******************************************************************
public:
	typedef INT16 BUFFER_TYPE;
	static const UINT32 SAMPLE_RATE		= 44100;
	static const UINT32 SAMPLES			= 882;
	static const UINT16 CHANNELS		= 2;
	static const bool   TO_MONO			= true;
	static const UINT32 BUFFER_LENGTH	= (TO_MONO ? 1 : CHANNELS) * SAMPLES;
	static const UINT32 BUFFER_SIZE		= CHANNELS * sizeof(BUFFER_TYPE) * SAMPLES;

private:
	static const UINT32 BUFFERS = 4;

	//*******************************************************************
	// �����o�ϐ�
	//*******************************************************************
public:
	bool		IsWaveOpen = false;
	BUFFER_TYPE	m_pBuffer[BUFFER_LENGTH];

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
	bool OpenDevice(HWND);
	void CloseDevice();
	void StartRecording();
	static void CALLBACK WaveInProc(HWAVEIN, UINT, DWORD, DWORD, DWORD);
};
