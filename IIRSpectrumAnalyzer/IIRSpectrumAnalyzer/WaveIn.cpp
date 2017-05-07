#pragma once
#include "WaveIn.h"

WaveIn::WaveIn(HWND hWnd)
{
	for (int i = 0; i < BUFFERS; i++) {
		m_whdr[i] = NULL;
	}

	// 開始
	if (!IsWaveOpen) {
		IsWaveOpen = OpenDevice(hWnd);
		if (!IsWaveOpen) {
			return;
		}
		StartRecording();
	}
}

WaveIn::~WaveIn()
{
	// WAVEデバイスを閉じる
	CloseDevice();
}

//*******************************************************************
// データバッファブロックを再利用する
//*******************************************************************
MMRESULT
WaveIn::Reuse(LPARAM lParam)
{
	MMRESULT rc = waveInPrepareHeader(m_hWaveIn, (LPWAVEHDR)lParam, sizeof(WAVEHDR));
	if (MMSYSERR_NOERROR == rc) {
		rc = waveInAddBuffer(m_hWaveIn, (LPWAVEHDR)lParam, sizeof(WAVEHDR));
	}

	if (MMSYSERR_NOERROR != rc) {
		CloseDevice();
	}

	return rc;
}

void
WaveIn::SetBuffer(LPARAM lParam)
{
	for (int i = 0; i<BUFFERS; ++i) {
		if (((LPWAVEHDR)lParam)->lpData == m_whdr[i]->lpData) {
			CopyMemory(
				m_pBuffer,
				m_whdr[i & (BUFFERS - 1)]->lpData,
				BUFFER_SIZE
			);
			break;
		}
	}
}

//*******************************************************************
// 録音デバイスを開く - 成功したらTRUEを返す
//*******************************************************************
BOOL
WaveIn::OpenDevice(HWND hWnd)
{
	WAVEFORMATEX wfe;
	wfe.wFormatTag = WAVE_FORMAT_PCM;			// PCM without any compression
	wfe.nChannels = 1;							// 1channel (Monaural)
	wfe.nSamplesPerSec = SAMPLE_RATE;			// (Hz)
	wfe.wBitsPerSample = sizeof(INT16) * 8;		// (bits/channel)
	wfe.nBlockAlign = (WORD)(wfe.nChannels * wfe.wBitsPerSample / 8);
	wfe.nAvgBytesPerSec = wfe.nSamplesPerSec * wfe.nBlockAlign;
	wfe.cbSize = 0;

	m_hWaveIn = NULL;
	return (waveInOpen(
		&m_hWaveIn,
		WAVE_MAPPER,
		&wfe,
		(DWORD)(void*)WaveInProc,
		(DWORD)hWnd,
		CALLBACK_FUNCTION
	) == MMSYSERR_NOERROR);
}

//*******************************************************************
// 録音デバイスを閉じる
//*******************************************************************
void
WaveIn::CloseDevice()
{
	int i;

	// 録音を停止する
	waveInStop(m_hWaveIn);
	waveInReset(m_hWaveIn);

	// ヘッダを非準備状態にする
	for (i = 0; i<BUFFERS; i++) {
		waveInUnprepareHeader(m_hWaveIn, m_whdr[i], sizeof(WAVEHDR));
	}

	waveInClose(m_hWaveIn);

	//*******************************************************************
	// 割り当てた録音バッファを解放
	//*******************************************************************
	if (m_hgRecBuffer != NULL) {
		GlobalFree(m_hgRecBuffer);
	}

	for (i = 0; i < BUFFERS; i++) {
		m_whdr[i] = NULL;
	}

	IsWaveOpen = false;
}

//*******************************************************************
// 録音開始
//*******************************************************************
void
WaveIn::StartRecording()
{
	MMRESULT rc;
	int		 i;
	LPSTR	 h;

	//*******************************************************************
	// 録音バッファを確保する
	//*******************************************************************
	if ((m_hgRecBuffer = GlobalAlloc(GPTR, (sizeof(WAVEHDR) + BUFFER_SIZE) * BUFFERS)) == NULL) {
		waveInClose(m_hWaveIn);
		IsWaveOpen = false;
		return;
	}

	h = (LPSTR)m_hgRecBuffer;
	for (i = 0; i < BUFFERS; ++i) {
		m_whdr[i] = (LPWAVEHDR)h;
		h += sizeof(WAVEHDR);
		m_whdr[i]->lpData = h;
		m_whdr[i]->dwBufferLength = BUFFER_SIZE;
		h += m_whdr[i]->dwBufferLength;
	}

	// バッファブロックを準備して、入力キューに追加する
	for (i = 0; i < BUFFERS; ++i) {
		rc = waveInPrepareHeader(m_hWaveIn, m_whdr[i], sizeof(WAVEHDR));

		// 入力キューにバッファを追加する
		if (MMSYSERR_NOERROR == rc) {
			rc = waveInAddBuffer(m_hWaveIn, m_whdr[i], sizeof(WAVEHDR));
		}
	}

	// 録音を開始する
	if (MMSYSERR_NOERROR == rc) {
		rc = waveInStart(m_hWaveIn);
	}

	if (MMSYSERR_NOERROR != rc) {
		CloseDevice();	 // 割り当てられたメモリを解放する
		return;
	}
}

//*******************************************************************
// WaveInコールバック関数
// (注) この関数内では限られたAPIしか呼べず早急に復帰する必要がある
//*******************************************************************
void CALLBACK
WaveIn::WaveInProc(HWAVEIN hwi, UINT uMsg, DWORD dwInstance, DWORD dwParam1, DWORD dwParam2)
{
	switch (uMsg)
	{
	case WIM_DATA:
		// メッセージをポストして、受信した入力ブロックを処理する
		PostMessage((HWND)dwInstance, WM_USER, 0, dwParam1);
		break;

	default:
		break;
	}
}
