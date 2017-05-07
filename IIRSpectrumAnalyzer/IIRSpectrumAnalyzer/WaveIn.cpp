#pragma once
#include "WaveIn.h"

WaveIn::WaveIn(HWND hWnd)
{
	for (int i = 0; i < BUFFERS; i++) {
		m_whdr[i] = NULL;
	}

	// �J�n
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
	// WAVE�f�o�C�X�����
	CloseDevice();
}

//*******************************************************************
// �f�[�^�o�b�t�@�u���b�N���ė��p����
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
// �^���f�o�C�X���J�� - ����������TRUE��Ԃ�
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
// �^���f�o�C�X�����
//*******************************************************************
void
WaveIn::CloseDevice()
{
	int i;

	// �^�����~����
	waveInStop(m_hWaveIn);
	waveInReset(m_hWaveIn);

	// �w�b�_��񏀔���Ԃɂ���
	for (i = 0; i<BUFFERS; i++) {
		waveInUnprepareHeader(m_hWaveIn, m_whdr[i], sizeof(WAVEHDR));
	}

	waveInClose(m_hWaveIn);

	//*******************************************************************
	// ���蓖�Ă��^���o�b�t�@�����
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
// �^���J�n
//*******************************************************************
void
WaveIn::StartRecording()
{
	MMRESULT rc;
	int		 i;
	LPSTR	 h;

	//*******************************************************************
	// �^���o�b�t�@���m�ۂ���
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

	// �o�b�t�@�u���b�N���������āA���̓L���[�ɒǉ�����
	for (i = 0; i < BUFFERS; ++i) {
		rc = waveInPrepareHeader(m_hWaveIn, m_whdr[i], sizeof(WAVEHDR));

		// ���̓L���[�Ƀo�b�t�@��ǉ�����
		if (MMSYSERR_NOERROR == rc) {
			rc = waveInAddBuffer(m_hWaveIn, m_whdr[i], sizeof(WAVEHDR));
		}
	}

	// �^�����J�n����
	if (MMSYSERR_NOERROR == rc) {
		rc = waveInStart(m_hWaveIn);
	}

	if (MMSYSERR_NOERROR != rc) {
		CloseDevice();	 // ���蓖�Ă�ꂽ���������������
		return;
	}
}

//*******************************************************************
// WaveIn�R�[���o�b�N�֐�
// (��) ���̊֐����ł͌���ꂽAPI�����Ăׂ����}�ɕ��A����K�v������
//*******************************************************************
void CALLBACK
WaveIn::WaveInProc(HWAVEIN hwi, UINT uMsg, DWORD dwInstance, DWORD dwParam1, DWORD dwParam2)
{
	switch (uMsg)
	{
	case WIM_DATA:
		// ���b�Z�[�W���|�X�g���āA��M�������̓u���b�N����������
		PostMessage((HWND)dwInstance, WM_USER, 0, dwParam1);
		break;

	default:
		break;
	}
}
