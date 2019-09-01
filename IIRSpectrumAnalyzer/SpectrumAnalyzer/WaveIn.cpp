#pragma once
#include "WaveIn.h"

WaveIn::WaveIn(HWND hWnd) {
    for (UINT32 i = 0; i < BUFFERS; ++i) {
        mWhdr[i] = NULL;
    }

    // �J�n
    if (!IsWaveOpen) {
        IsWaveOpen = OpenDevice(hWnd);
        if (!IsWaveOpen) return;
        StartRecording();
    }
}

WaveIn::~WaveIn() {
    // WAVE�f�o�C�X�����
    CloseDevice();
}

//*******************************************************************
// �^���f�o�C�X���J�� - ����������TRUE��Ԃ�
//*******************************************************************
bool
WaveIn::OpenDevice(HWND hWnd) {
    WAVEFORMATEX wfe;
    wfe.wFormatTag = WAVE_FORMAT_PCM;			// PCM without any compression
    wfe.nChannels = CHANNELS;
    wfe.nSamplesPerSec = SAMPLE_RATE;			// (Hz)
    wfe.wBitsPerSample = sizeof(INT16) * 8;		// (bits/channel)
    wfe.nBlockAlign = (WORD)(wfe.nChannels * wfe.wBitsPerSample / 8);
    wfe.nAvgBytesPerSec = wfe.nSamplesPerSec * wfe.nBlockAlign;
    wfe.cbSize = 0;

    mhWaveIn = NULL;
    return (waveInOpen(
        &mhWaveIn,
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
WaveIn::CloseDevice() {
    // �^�����~����
    waveInStop(mhWaveIn);
    waveInReset(mhWaveIn);

    // �w�b�_��񏀔���Ԃɂ���
    for (UINT32 i = 0; i < BUFFERS; ++i) {
        waveInUnprepareHeader(mhWaveIn, mWhdr[i], sizeof(WAVEHDR));
    }

    // �^���f�o�C�X�����
    waveInClose(mhWaveIn);

    //*******************************************************************
    // ���蓖�Ă��^���o�b�t�@�����
    //*******************************************************************
    if (mhgRecBuffer != NULL) {
        GlobalFree(mhgRecBuffer);
    }

    for (UINT32 i = 0; i < BUFFERS; i++) {
        mWhdr[i] = NULL;
    }

    IsWaveOpen = false;
}

//*******************************************************************
// �^���J�n
//*******************************************************************
void
WaveIn::StartRecording() {
    //*******************************************************************
    // �^���o�b�t�@���m�ۂ���
    //*******************************************************************
    if ((mhgRecBuffer = GlobalAlloc(GPTR, (sizeof(WAVEHDR) + BUFFER_SIZE) * BUFFERS)) == NULL) {
        waveInClose(mhWaveIn);
        IsWaveOpen = false;
        return;
    }

    //
    {
        LPSTR h = (LPSTR)mhgRecBuffer;
        for (UINT32 i = 0; i < BUFFERS; ++i) {
            mWhdr[i] = (LPWAVEHDR)h;
            h += sizeof(WAVEHDR);
            mWhdr[i]->lpData = h;
            mWhdr[i]->dwBufferLength = BUFFER_SIZE;
            h += mWhdr[i]->dwBufferLength;
        }
    }

    //
    {
        MMRESULT rc;

        // �o�b�t�@�u���b�N���������āA���̓L���[�ɒǉ�����
        for (UINT32 i = 0; i < BUFFERS; ++i) {
            rc = waveInPrepareHeader(mhWaveIn, mWhdr[i], sizeof(WAVEHDR));

            // ���̓L���[�Ƀo�b�t�@��ǉ�����
            if (MMSYSERR_NOERROR == rc) {
                rc = waveInAddBuffer(mhWaveIn, mWhdr[i], sizeof(WAVEHDR));
            }
        }

        // �^�����J�n����
        if (MMSYSERR_NOERROR == rc) {
            rc = waveInStart(mhWaveIn);
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
WaveIn::Reuse(LPARAM lParam) {
    MMRESULT rc = waveInPrepareHeader(mhWaveIn, (LPWAVEHDR)lParam, sizeof(WAVEHDR));
    if (MMSYSERR_NOERROR == rc) {
        rc = waveInAddBuffer(mhWaveIn, (LPWAVEHDR)lParam, sizeof(WAVEHDR));
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
WaveIn::SetBuffer(LPARAM lParam) {
    for (UINT32 i = 0; i < BUFFERS; ++i) {
        if (((LPWAVEHDR)lParam)->lpData == mWhdr[i]->lpData) {
            mpBuffer = mWhdr[i & (BUFFERS - 1)]->lpData;
            break;
        }
    }
}

//*******************************************************************
// WaveIn�R�[���o�b�N�֐�
// (��) ���̊֐����ł͌���ꂽAPI�����Ăׂ����}�ɕ��A����K�v������
//*******************************************************************
void CALLBACK
WaveIn::WaveInProc(HWAVEIN hwi, UINT uMsg, DWORD dwInstance, DWORD dwParam1, DWORD dwParam2) {
    switch (uMsg) {
    case WIM_DATA:
        // ���b�Z�[�W���|�X�g���āA��M�������̓u���b�N����������
        PostMessage((HWND)dwInstance, WM_USER, 0, dwParam1);
        break;

    default:
        break;
    }
}
