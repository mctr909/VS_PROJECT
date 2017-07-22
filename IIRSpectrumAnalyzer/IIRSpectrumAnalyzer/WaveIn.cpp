#pragma once
#include "WaveIn.h"

WaveIn::WaveIn(HWND hWnd)
{
	for (UINT32 i = 0; i < BUFFERS; ++i) {
		m_whdr[i] = NULL;
	}

	// 開始
	if (!IsWaveOpen) {
		IsWaveOpen = OpenDevice(hWnd);
		if (!IsWaveOpen) return;
		StartRecording();
	}
}

WaveIn::~WaveIn()
{
	// WAVEデバイスを閉じる
	CloseDevice();
}

//*******************************************************************
// 録音デバイスを開く - 成功したらTRUEを返す
//*******************************************************************
bool
WaveIn::OpenDevice(HWND hWnd)
{
	WAVEFORMATEX wfe;
	wfe.wFormatTag = WAVE_FORMAT_PCM;			// PCM without any compression
	wfe.nChannels = CHANNELS;
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
	// 録音を停止する
	waveInStop(m_hWaveIn);
	waveInReset(m_hWaveIn);

	// ヘッダを非準備状態にする
	for (UINT32 i = 0; i<BUFFERS; ++i) {
		waveInUnprepareHeader(m_hWaveIn, m_whdr[i], sizeof(WAVEHDR));
	}

	// 録音デバイスを閉じる
	waveInClose(m_hWaveIn);

	//*******************************************************************
	// 割り当てた録音バッファを解放
	//*******************************************************************
	if (m_hgRecBuffer != NULL) {
		GlobalFree(m_hgRecBuffer);
	}

	for (UINT32 i = 0; i < BUFFERS; i++) {
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
	//*******************************************************************
	// 録音バッファを確保する
	//*******************************************************************
	if ((m_hgRecBuffer = GlobalAlloc(GPTR, (sizeof(WAVEHDR) + BUFFER_SIZE) * BUFFERS)) == NULL) {
		waveInClose(m_hWaveIn);
		IsWaveOpen = false;
		return;
	}

	//
	{
		LPSTR h = (LPSTR)m_hgRecBuffer;
		for (UINT32 i = 0; i < BUFFERS; ++i) {
			m_whdr[i] = (LPWAVEHDR)h;
			h += sizeof(WAVEHDR);
			m_whdr[i]->lpData = h;
			m_whdr[i]->dwBufferLength = BUFFER_SIZE;
			h += m_whdr[i]->dwBufferLength;
		}
	}

	//
	{
		MMRESULT rc;

		// バッファブロックを準備して、入力キューに追加する
		for (UINT32 i = 0; i < BUFFERS; ++i) {
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

//*******************************************************************
// LPWAVEHDRのlpDataの値をバッファにセット
//*******************************************************************
void
WaveIn::SetBuffer(LPARAM lParam)
{
	UINT32 i, j, k;

	for (i = 0; i < BUFFERS; ++i) {
		if (((LPWAVEHDR)lParam)->lpData == m_whdr[i]->lpData) {
			if (TO_MONO) {
				// モノラル化[(L + R) / 2]してバッファにセット
				BUFFER_TYPE* buff = (BUFFER_TYPE*)m_whdr[i & (BUFFERS - 1)]->lpData;
				for (j = 0, k = 0; j < SAMPLES; ++j, k += 2) {
					m_pBuffer[j] = (buff[k] + buff[k + 1]) >> 1;
				}
			}
			else {
				memcpy_s(
					m_pBuffer, BUFFER_SIZE,
					m_whdr[i & (BUFFERS - 1)]->lpData, BUFFER_SIZE
				);
			}
			break;
		}
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
