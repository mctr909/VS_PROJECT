#pragma once
#include "WaveIn.h"

WaveIn::WaveIn(HWND hWnd)
{
	for (UINT32 i = 0; i < BUFFERS; ++i) {
		m_whdr[i] = NULL;
	}

	// �J�n
	if (!IsWaveOpen) {
		IsWaveOpen = OpenDevice(hWnd);
		if (!IsWaveOpen) return;
		StartRecording();
	}
}

WaveIn::~WaveIn()
{
	// WAVE�f�o�C�X�����
	CloseDevice();
}

//*******************************************************************
// �^���f�o�C�X���J�� - ����������TRUE��Ԃ�
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
// �^���f�o�C�X�����
//*******************************************************************
void
WaveIn::CloseDevice()
{
	// �^�����~����
	waveInStop(m_hWaveIn);
	waveInReset(m_hWaveIn);

	// �w�b�_��񏀔���Ԃɂ���
	for (UINT32 i = 0; i<BUFFERS; ++i) {
		waveInUnprepareHeader(m_hWaveIn, m_whdr[i], sizeof(WAVEHDR));
	}

	// �^���f�o�C�X�����
	waveInClose(m_hWaveIn);

	//*******************************************************************
	// ���蓖�Ă��^���o�b�t�@�����
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
// �^���J�n
//*******************************************************************
void
WaveIn::StartRecording()
{
	//*******************************************************************
	// �^���o�b�t�@���m�ۂ���
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

		// �o�b�t�@�u���b�N���������āA���̓L���[�ɒǉ�����
		for (UINT32 i = 0; i < BUFFERS; ++i) {
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

//*******************************************************************
// LPWAVEHDR��lpData�̒l���o�b�t�@�ɃZ�b�g
//*******************************************************************
void
WaveIn::SetBuffer(LPARAM lParam)
{
	UINT32 i, j, k;

	for (i = 0; i < BUFFERS; ++i) {
		if (((LPWAVEHDR)lParam)->lpData == m_whdr[i]->lpData) {
			if (TO_MONO) {
				// ���m������[(L + R) / 2]���ăo�b�t�@�ɃZ�b�g
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
