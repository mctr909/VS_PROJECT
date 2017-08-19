#include "Main.h"

//*******************************************************************
// WinMain - ���C���G���g���|�C���g
//*******************************************************************
int WINAPI
WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpszCmdLine, int nCmdShow)
{
	hInst = hInstance;

	// �E�C���h�E�N���X�̓o�^
	WNDCLASSEX wc = {
		sizeof(WNDCLASSEX),
		CS_CLASSDC,
		MainWndProc,
		0L,
		0L,
		GetModuleHandle(NULL),
		NULL,
		NULL,
		NULL,
		NULL,
		CLASS_NAME,
		NULL
	};
	if (NULL == RegisterClassEx(&wc)) {
		return 0;
	}

	// �^�C�g���o�[�ƃE�C���h�E�g�̕����܂߂ăE�C���h�E�T�C�Y��ݒ�
	RECT rect;
	SetRect(&rect, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);
	AdjustWindowRect(&rect, WS_OVERLAPPEDWINDOW, FALSE);
	rect.right = rect.right - rect.left;
	rect.bottom = rect.bottom - rect.top;
	rect.top = 0;
	rect.left = 0;

	// �E�C���h�E�̐���
	HWND hWnd = CreateWindow(
		CLASS_NAME,
		WINDOW_TITLE,
		WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT,
		CW_USEDEFAULT,
		rect.right,
		rect.bottom,
		NULL,
		NULL,
		wc.hInstance,
		NULL
	);
	if (NULL == hWnd) {
		return 0;
	}

	// ���C���E�B���h�E��\������
	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	// �E�B���h�E���b�Z�[�W����������
	MSG msg;
	while (GetMessage(&msg, NULL, 0, 0)) {
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}

	return msg.wParam;
}

//*******************************************************************
// MainWndProc - ���C���E�B���h�E�̃��b�Z�[�W����
//*******************************************************************
LRESULT CALLBACK
MainWndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	// ���b�Z�[�W������
	switch (uMsg) {

	case WM_CREATE:
		return wmCreate(hWnd, uMsg, wParam, lParam);

	case WM_DESTROY:
		return wmDestroy(hWnd, uMsg, wParam, lParam);

	case WM_SIZE:
		SendMessage(hToolWnd, WM_SIZE, 0, 0);
		return DefWindowProc(hWnd, uMsg, wParam, lParam);

	case WM_PAINT:
		return wmPaint(hWnd, uMsg, wParam, lParam);

	case WM_QUERYNEWPALETTE: {	// �t�H�[�J�X�𓾂钼�O�Ɏ����̃p���b�g�����̉�
		HDC hDC = GetDC(hWnd);
		SelectPalette(hDC, hPalette, FALSE);
		if (RealizePalette(hDC)) {
			InvalidateRect(hWnd, NULL, TRUE);
		}
		ReleaseDC(hWnd, hDC);
		break;
	}

	case WM_PALETTECHANGED: {	// �ʂ̃A�v���P�[�V�������p���b�g��ύX����
		if ((HWND)wParam != hWnd) {
			HDC hDC = GetDC(hWnd);
			SelectPalette(hDC, hPalette, FALSE);
			if (RealizePalette(hDC)) {
				UpdateColors(hDC);
			}
			ReleaseDC(hWnd, hDC);
		}
		break;
	}

	case WM_USER:
		return wmUser(hWnd, uMsg, wParam, lParam);
	
	default:
		return DefWindowProc(hWnd, uMsg, wParam, lParam);
	}
	return (LRESULT)0;
}

LRESULT
wmCreate(HWND& hWnd, UINT& uMsg, WPARAM& wParam, LPARAM& lParam)
{
	DWORD			pallet[PALLET_COLORS];
	LPLOGPALETTE	lpPal;
	LPDWORD			dp;
	LPDWORD			sp;
	HDC				hDC;
	int				i, j, k;
	double			width;
	double			freq;

	// �J���[�p���b�g���쐬����B
	for (i = 0, j = 0, k = 0; i < PALLET_COLORS; ++i, j += 1280 / PALLET_COLORS, k = j % 256) {
		if (j < 256) {
			// Black -> Blue
			pallet[i] = k;
		}
		else if (j < 512) {
			// Blue -> LightGreen
			pallet[i] = (k << 8) | 0xFF;
		}
		else if (j < 768) {
			// LightGreen -> Green
			pallet[i] = 0xFFFF - k;
		}
		else if (j < 1024) {
			// Green -> Yellow
			pallet[i] = (k << 16) | 0xFF00;
		}
		else if (j < 1280) {
			// Yellow -> Red
			pallet[i] = 0xFFFF00 - (k << 8);
		}
	}

	lpPal = (LPLOGPALETTE)GlobalAlloc(GPTR, sizeof(LOGPALETTE) + (PALLET_COLORS * sizeof(PALETTEENTRY)));
	lpPal->palVersion = 0x300;
	lpPal->palNumEntries = PALLET_COLORS;
	dp = (DWORD*)lpPal->palPalEntry;
	sp = (DWORD*)pallet;
	for (i = 0; i < PALLET_COLORS; ++i) {
		*(dp++) = ((DWORD)PC_NOCOLLAPSE << 24) | (DWORD)RGBQUAD2COLORREF(*(sp++));
	}
	hPalette = CreatePalette(lpPal);
	GlobalFree(lpPal);

	// DIB�o�b�t�@ lpBits �̍\�� (�T�C�YDRAW_WIDTH * DRAW_HEIGHT * 2��)
	// �� DIB�w�b�_ lpBitmap �̉摜�T�C�Y�� DRAW_WIDTH * DRAW_HEIGHT
	// lpBits -> +-------------+ ����
	//           |             | DRAW_HEIGHT	�X�y�N�g���\���p�o�b�t�@
	//           +-------------+
	//           |             | DRAW_HEIGHT	�����X�N���[���o�b�t�@
	//           +-------------+
	//           ���� DRAW_WIDTH

	// DIB�o�b�t�@���쐬����B
	i = sizeof(BITMAPINFOHEADER) + PALLET_COLORS * 4;
	lpBitmap = (LPBITMAPINFO)GlobalAlloc(GPTR, i + DRAW_WIDTH * DRAW_HEIGHT * 2);
	lpBits = (LPBYTE)lpBitmap + i;
	ZeroMemory(lpBits, DRAW_WIDTH * DRAW_HEIGHT * 2);
	
	// DIB�w�b�_
	lpBitmap->bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	lpBitmap->bmiHeader.biWidth = DRAW_WIDTH;
	lpBitmap->bmiHeader.biHeight = DRAW_HEIGHT;
	lpBitmap->bmiHeader.biPlanes = 1;
	lpBitmap->bmiHeader.biBitCount = 8;
	lpBitmap->bmiHeader.biCompression = BI_RGB;
	lpBitmap->bmiHeader.biSizeImage = DRAW_WIDTH * DRAW_HEIGHT;
	lpBitmap->bmiHeader.biXPelsPerMeter = 0;
	lpBitmap->bmiHeader.biYPelsPerMeter = 0;
	lpBitmap->bmiHeader.biClrUsed = PALLET_COLORS;
	lpBitmap->bmiHeader.biClrImportant = PALLET_COLORS;
	CopyMemory(&lpBitmap->bmiColors, pallet, PALLET_COLORS * 4);
	
	// �\���o�b�t�@(DDB)���쐬����
	hDC = GetDC(hWnd);
	hMemBitmap = CreateCompatibleBitmap(hDC, DRAW_WIDTH, DRAW_HEIGHT * 2 + GAUGE_HEIGHT);
	ReleaseDC(hWnd, hDC);

	// �Q�[�W��`�悷��
	DrawGauge(hWnd, hMemBitmap);

	// �^���f�o�C�X�N���X
	cWaveIn = new WaveIn(hWnd);

	// IIR�t�B���^�N���X
	cIIR = new IIRFilter(WaveIn::SAMPLE_RATE, BANKS);

	// �t�B���^�萔�̐ݒ�
	for (i = 0; i < BANKS; ++i)
	{
		freq = PITCH * pow(2.0, i / (12.0 * NOTE_DIV));
		width = 2.5 - (3.0 * i / BANKS);
		if (width < 1.0) {
			width = 1.0;
		}
		cIIR->Bandpass(i, freq, width / 12.0);
	}

	// �v���Z�X�̗D�揇�ʂ� HIGH �ɂ���
	SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS);

	InvalidateRect(hWnd, NULL, FALSE);
	return (LRESULT)0;
}

LRESULT
wmDestroy(HWND& hWnd, UINT& uMsg, WPARAM& wParam, LPARAM& lParam)
{
	// �v���Z�X�̗D�揇�ʂ� NORMAL �ɖ߂�
	// �i�����Ȃ��΃E�B���h�E��������܂łɎ��Ԃ������邱�Ƃ�����j
	SetPriorityClass(GetCurrentProcess(), NORMAL_PRIORITY_CLASS);

	// �m�ۂ��Ă������������J��
	GlobalFree(lpBitmap);
	DeleteObject(hMemBitmap);

	delete cWaveIn;
	cWaveIn = NULL;

	delete cIIR;
	cIIR = NULL;

	PostQuitMessage(0);
	return (LRESULT)0;
}

LRESULT
wmPaint(HWND& hWnd, UINT& uMsg, WPARAM& wParam, LPARAM& lParam)
{
	PAINTSTRUCT ps;
	HDC			hMemDC;
	RECT		r;

	BeginPaint(hWnd, &ps);

	// �p���b�g��I�����Ď��̉�����B
	SelectPalette(ps.hdc, hPalette, FALSE);
	RealizePalette(ps.hdc);

	GetClientRect(hWnd, &r);

	// DIB�o�b�t�@�̓��e��`��
	//  �� ��������ɕ`�悵�ăX�N���[���ɓ]�ʂ����x��Ԃ������Ă���̂́A
	//       1. �������}����
	//       2. ���̃r�f�I�J�[�h�ŁAStretchDIBits����StretchBlt�̕�������
	//     �ł��邽�߁B
	hMemDC = CreateCompatibleDC(ps.hdc);
	SelectObject(hMemDC, hMemBitmap);

	// �㔼���i�X�y�N�g���\���j���o�b�t�@�ɕ`��
	SetDIBitsToDevice(
		hMemDC,
		0, 0,
		DRAW_WIDTH, DRAW_HEIGHT,
		0, 0,
		0, DRAW_HEIGHT,
		lpBits,
		lpBitmap,
		DIB_RGB_COLORS
	);

	// ���������o�b�t�@�ɕ`��
	SetDIBitsToDevice(
		hMemDC,
		0, DRAW_HEIGHT + GAUGE_HEIGHT,
		DRAW_WIDTH, DRAW_HEIGHT,
		0, 0,
		0,
		DRAW_HEIGHT,
		lpBits + DRAW_WIDTH * DRAW_HEIGHT,
		lpBitmap,
		DIB_RGB_COLORS
	);

	// �g��`�悷��
	DrawEdge(ps.hdc, &r, EDGE_SUNKEN, BF_RECT);

	// �o�b�t�@�̓��e���E�B���h�E�ɓ]��
	r.left += GetSystemMetrics(SM_CXBORDER);
	r.right -= GetSystemMetrics(SM_CXBORDER);
	r.top += GetSystemMetrics(SM_CYBORDER);
	r.bottom -= GetSystemMetrics(SM_CYBORDER);
	SetStretchBltMode(ps.hdc, STRETCH_ORSCANS);
	StretchBlt(
		ps.hdc,
		r.left, r.top,
		r.right - r.left, r.bottom - r.top,
		hMemDC,
		0, 0,
		DRAW_WIDTH, GAUGE_HEIGHT + DRAW_HEIGHT * 2,
		SRCCOPY
	);

	DeleteDC(hMemDC);
	EndPaint(hWnd, &ps);

	return (LRESULT)0;
}

LRESULT
wmUser(HWND& hWnd, UINT& uMsg, WPARAM& wParam, LPARAM& lParam)
{
	if ((NULL == cWaveIn->m_pBuffer) || (FALSE == cWaveIn->IsWaveOpen)) {
		InvalidateRect(hWnd, NULL, TRUE);
		return (LRESULT)0;
	}

	// �o�b�t�@�ɒl���Z�b�g
	cWaveIn->SetBuffer(lParam);

	// �f�[�^�o�b�t�@�u���b�N���ė��p����
	cWaveIn->Reuse(lParam);

	// �`�揈��
	PlotSpectrum(hWnd);

	// �`�揈�����ɗ��܂����`��v�����폜����
	MSG msg;
	while (PeekMessage(&msg, hWnd, WM_USER, WM_USER, PM_REMOVE)) {
		// �f�[�^�o�b�t�@�u���b�N���ė��p����
		MMRESULT rc = cWaveIn->Reuse(msg.lParam);
		if (MMSYSERR_NOERROR != rc) {
			break;
		}
	}

	InvalidateRect(hWnd, NULL, TRUE);
	return (LRESULT)0;
}

//*******************************************************************
//  �ڐ���`��
//*******************************************************************
void
DrawGauge(HWND hWnd, HBITMAP hMemBitmap)
{
	RECT r;
	HDC hDC, hWindowDC;

	hWindowDC = GetDC(hWnd);
	hDC = CreateCompatibleDC(hWindowDC);
	ReleaseDC(hWnd, hWindowDC);
	SelectObject(hDC, hMemBitmap);

	r.left = 0;
	r.right = DRAW_WIDTH;
	r.top = DRAW_HEIGHT;
	r.bottom = r.top + GAUGE_HEIGHT;

	// ���K�Q�[�W��`��
	HBRUSH hbr;
	UINT32 i;

	FillRect(hDC, &r, (HBRUSH)GetStockObject(BLACK_BRUSH));
	for (i = 0; i < BANKS; ++i) {
		COLORREF c;
		int octAlt = ((i / NOTE_DIV + START_NOTE + 9) / 12 & 1);

		switch ((i / NOTE_DIV + START_NOTE) % 12) {
		case 1:
		case 4:
		case 6:
		case 9:
		case 11:
			c = PALETTERGB(64, 64, 64);
			break;
		default:
			if (octAlt) {
				c = PALETTERGB(255, 255, 255);
			}
			else {
				c = PALETTERGB(128, 255, 128);
			}
		}

		r.left = i * DRAW_STEPX;
		r.right = r.left + (DRAW_STEPX * NOTE_DIV);

		hbr = CreateSolidBrush(c);
		FillRect(hDC, &r, hbr);
		DeleteObject(hbr);
	}

	DeleteDC(hDC);
}

//*******************************************************************
// �X�y�N�g����DIB�o�b�t�@�ɕ`�悷��
//*******************************************************************
void
PlotSpectrum(HWND hWnd)
{
	UINT32 b, d;
	double maxLevel = 0.0;

	// �X�N���[���o�b�t�@�� 3pixel �X�N���[���_�E��
	MoveMemory(
		lpBits + DRAW_WIDTH * DRAW_HEIGHT,
		lpBits + DRAW_WIDTH * DRAW_HEIGHT + DRAW_WIDTH,
		DRAW_WIDTH * (DRAW_HEIGHT - 1)
	);
	MoveMemory(
		lpBits + DRAW_WIDTH * DRAW_HEIGHT,
		lpBits + DRAW_WIDTH * DRAW_HEIGHT + DRAW_WIDTH,
		DRAW_WIDTH * (DRAW_HEIGHT - 1)
	);
	MoveMemory(
		lpBits + DRAW_WIDTH * DRAW_HEIGHT,
		lpBits + DRAW_WIDTH * DRAW_HEIGHT + DRAW_WIDTH,
		DRAW_WIDTH * (DRAW_HEIGHT - 1)
	);

	// �X�y�N�g���`��
	ZeroMemory(lpBits, DRAW_WIDTH * DRAW_HEIGHT);

	for (b = 0; b < BANKS; ++b) {
		// ���͔g�`���t�B���^�ɂ����ĐU�����Z�o
		{
			register UINT32 t;
			register double filteredWave;
			register double amplitude = 0.0;

			for (t = 0; t < WaveIn::SAMPLES; ++t) {
				cIIR->Exec(b, cWaveIn->m_pBuffer[t], &filteredWave);
				amplitude += filteredWave * filteredWave;
			}

			if (maxLevel < amplitude) {
				maxLevel = amplitude;
			}

			amplitude = static_cast<INT32>(16384 * amplitude / gAvgLevel);
			if (amplitude < 1.0) {
				amplitude = 1.0;
			}

			amplitude = 1.2 * (log10(amplitude) / log10(16384.0) - 0.2);
			if (amplitude < 0.0) {
				amplitude = 0.0;
			}

			d = static_cast<UINT32>(DRAW_HEIGHT * amplitude);
			if (DRAW_HEIGHT <= d) {
				d = DRAW_HEIGHT - 1;
			}
		}

		// DIB�o�b�t�@��ɖ_�O���t��`��
		{
			// UINT32�l���g���āA��x�ɂS�s�N�Z�����`�悷��
			register UINT32 *pix4 = (UINT32*)lpBits + b;
			register UINT32 j;
			for (j = 0; j < d; ++j) {
				register UINT32 tmp = 8 + j * 55 / DRAW_HEIGHT;
				tmp |= (tmp << 8) | (tmp << 16) | (tmp << 24);
				*pix4 = tmp;
				pix4 += BANKS;
			}
		}

		// �X�N���[���o�b�t�@�ɂ��`��
		{
			register UINT32 tmp = d * PALLET_COLORS / DRAW_HEIGHT;
			tmp |= (tmp << 8) | (tmp << 16) | (tmp << 24);
			*((UINT32 *)(lpBits + DRAW_WIDTH * (DRAW_HEIGHT * 2 - 1)) + b) = tmp;
		}
	}

	if (gAvgLevel < maxLevel) {
		gAvgLevel = maxLevel;
	}
	else {
		gAvgLevel *= 1.0 - 2.0 / (WaveIn::SAMPLE_RATE / WaveIn::SAMPLES);
	}

	if (gAvgLevel < 32768.0) {
		gAvgLevel = 32768.0;
	}
}
