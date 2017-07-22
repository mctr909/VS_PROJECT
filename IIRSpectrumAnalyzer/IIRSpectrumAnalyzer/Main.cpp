#include "Main.h"

//*******************************************************************
// WinMain - メインエントリポイント
//*******************************************************************
int WINAPI
WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpszCmdLine, int nCmdShow)
{
	hInst = hInstance;

	// ウインドウクラスの登録
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

	// タイトルバーとウインドウ枠の分を含めてウインドウサイズを設定
	RECT rect;
	SetRect(&rect, 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);
	AdjustWindowRect(&rect, WS_OVERLAPPEDWINDOW, FALSE);
	rect.right = rect.right - rect.left;
	rect.bottom = rect.bottom - rect.top;
	rect.top = 0;
	rect.left = 0;

	// ウインドウの生成
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

	// メインウィンドウを表示する
	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	// ウィンドウメッセージを処理する
	MSG msg;
	while (GetMessage(&msg, NULL, 0, 0)) {
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}

	return msg.wParam;
}

//*******************************************************************
// MainWndProc - メインウィンドウのメッセージ処理
//*******************************************************************
LRESULT CALLBACK
MainWndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	// メッセージを処理
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

	case WM_HSCROLL:
		if ((HWND)(lParam) == hTrkGamma) {
			gGamma = 1.0 / (SendMessage(hTrkGamma, TBM_GETPOS, 0, 0) + 1.0);
		}
		return(DefWindowProc(hWnd, uMsg, wParam, lParam));

	case WM_QUERYNEWPALETTE: {	// フォーカスを得る直前に自分のパレットを実体化
		HDC hDC = GetDC(hWnd);
		SelectPalette(hDC, hPalette, FALSE);
		if (RealizePalette(hDC)) {
			InvalidateRect(hWnd, NULL, TRUE);
		}
		ReleaseDC(hWnd, hDC);
		break;
	}

	case WM_PALETTECHANGED: {	// 別のアプリケーションがパレットを変更した
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
wmCreate(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	DWORD			pallet[PALLET_COLORS];
	LPLOGPALETTE	lpPal;
	LPDWORD			dp;
	LPDWORD			sp;
	HDC				hDC;
	int				i, j, k;
	double			width;
	double			freq;

	// トラックバーコントロール
	hTrkGamma = CreateWindowEx(
		0,								// ExStyle
		TRACKBAR_CLASS,					// ClassName
		L"",							// Caption
		WS_CHILD | WS_VISIBLE | TBS_AUTOTICKS,
		0, 0,							// Left, Top
		TRK_GAMMA_WIDTH, MENU_HEIGHT,	// Width, Height
		hWnd,
		(HMENU)ID_TRK_GAMMA,
		(HINSTANCE)GetWindowLong(hWnd, GWL_HINSTANCE),
		NULL
	);
	SendMessage(hTrkGamma, TBM_SETRANGE, TRUE, MAKELONG(0, 100));	// トラックバーの範囲
	SendMessage(hTrkGamma, TBM_SETPAGESIZE, 0, 10);					// マウスクリック時の移動量
	SendMessage(hTrkGamma, TBM_SETLINESIZE, 0, 1);					// 矢印キーを押した時の移動量
	SendMessage(hTrkGamma, TBM_SETPOS, TRUE, 20);					// ポジション
	SendMessage(hTrkGamma, TBM_SETTICFREQ, 10, 0);					// 目盛りのサイズ
	
	// カラーパレットを作成する。
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

	// DIBバッファ lpBits の構造 (サイズDRAW_WIDTH * DRAW_HEIGHT * 2面)
	// ※ DIBヘッダ lpBitmap の画像サイズは DRAW_WIDTH * DRAW_HEIGHT
	// lpBits -> +-------------+ 高さ
	//           |             | DRAW_HEIGHT	スペクトル表示用バッファ
	//           +-------------+
	//           |             | DRAW_HEIGHT	履歴スクロールバッファ
	//           +-------------+
	//           横幅 DRAW_WIDTH

	// DIBバッファを作成する。
	i = sizeof(BITMAPINFOHEADER) + PALLET_COLORS * 4;
	lpBitmap = (LPBITMAPINFO)GlobalAlloc(GPTR, i + DRAW_WIDTH * DRAW_HEIGHT * 2);
	lpBits = (LPBYTE)lpBitmap + i;
	ZeroMemory(lpBits, DRAW_WIDTH * DRAW_HEIGHT * 2);
	
	// DIBヘッダ
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
	
	// 表示バッファ(DDB)を作成する
	hDC = GetDC(hWnd);
	hMemBitmap = CreateCompatibleBitmap(hDC, DRAW_WIDTH, DRAW_HEIGHT * 2 + GAUGE_HEIGHT);
	ReleaseDC(hWnd, hDC);

	// ゲージを描画する
	DrawGauge(hWnd, hMemBitmap);

	// 録音デバイスクラス
	cWaveIn = new WaveIn(hWnd);

	// IIRフィルタクラス
	cIIR = new IIRFilter(WaveIn::SAMPLE_RATE, BANKS);

	// フィルタ定数の設定
	for (i = 0; i < BANKS; ++i)
	{
		freq = PITCH * pow(2.0, i / (12.0 * NOTE_DIV));
		width = 10.0 - (16.0 * i / BANKS);
		if (width < 2.0) {
			width = 2.0;
		}
		cIIR->Bandpass(i, freq, width / (12.0 * NOTE_DIV));
	}

	// プロセスの優先順位を HIGH にする
	SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS);

	InvalidateRect(hWnd, NULL, FALSE);
	return (LRESULT)0;
}

LRESULT
wmDestroy(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	// プロセスの優先順位を NORMAL に戻す
	// （さもなくばウィンドウが閉じられるまでに時間がかかることがある）
	SetPriorityClass(GetCurrentProcess(), NORMAL_PRIORITY_CLASS);

	// 確保していたメモリを開放
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
wmPaint(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	PAINTSTRUCT ps;
	HDC			hMemDC;
	RECT		r;

	BeginPaint(hWnd, &ps);

	// パレットを選択して実体化する。
	SelectPalette(ps.hdc, hPalette, FALSE);
	RealizePalette(ps.hdc);

	GetClientRect(hWnd, &r);
	r.top = MENU_HEIGHT;

	// DIBバッファの内容を描画
	//  ※ メモリ上に描画してスクリーンに転写する二度手間をかけているのは、
	//       1. ちらつきを抑える
	//       2. 大抵のビデオカードで、StretchDIBitsよりもStretchBltの方が高速
	//     であるため。

	hMemDC = CreateCompatibleDC(ps.hdc);
	SelectObject(hMemDC, hMemBitmap);

	// 上半分（スペクトル表示）をバッファに描画
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

	// 下半分をバッファに描画
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

	// 枠を描画する
	DrawEdge(ps.hdc, &r, EDGE_SUNKEN, BF_RECT);

	// バッファの内容をウィンドウに転写
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
wmUser(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	if ((NULL == cWaveIn->m_pBuffer) || (FALSE == cWaveIn->IsWaveOpen)) {
		InvalidateRect(hWnd, NULL, TRUE);
		return (LRESULT)0;
	}

	//
	cWaveIn->SetBuffer(lParam);

	// データバッファブロックを再利用する
	cWaveIn->Reuse(lParam);

	// 描画処理
	PlotSpectrum(hWnd);

	// 描画処理中に溜まった描画要求を削除する
	MSG msg;
	while (PeekMessage(&msg, hWnd, WM_USER, WM_USER, PM_REMOVE)) {
		// データバッファブロックを再利用する
		MMRESULT rc = cWaveIn->Reuse(msg.lParam);
		if (MMSYSERR_NOERROR != rc) {
			break;
		}
	}

	InvalidateRect(hWnd, NULL, TRUE);
	return (LRESULT)0;
}

//*******************************************************************
//  目盛を描画
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

	// 音階ゲージを描画
	HBRUSH hbr;
	int i;

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
// スペクトルをDIBバッファに描画する
//*******************************************************************
void
PlotSpectrum(HWND hWnd)
{
	int b, d;
	double mxLevel;
	static INT16 buff[WaveIn::SAMPLES];

	// スクロールバッファを 1 pixel スクロールダウン
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

	// スペクトル描画
	ZeroMemory(lpBits, DRAW_WIDTH * DRAW_HEIGHT);

	// モノラル化 = L + R
	for (b = 0, d = 0; b < WaveIn::SAMPLES; ++b, d += 2) {
		buff[b] = (cWaveIn->m_pBuffer[d] + cWaveIn->m_pBuffer[d + 1]) / 2;
	}

	mxLevel = 0.0;

	for (b = 0; b < BANKS; ++b) {
		// 入力音声をフィルタにかけて振幅を算出
		{
			register int t;
			register double wave;
			register double amp = 0.0;

			for (t = 0; t < WaveIn::SAMPLES; ++t) {
				cIIR->Exec(b, buff[t], &wave);
				amp += wave * wave;
			}

			if (mxLevel < amp) {
				mxLevel = amp;
			}

			amp /= gAvgLevel;
			amp = (1.0 + gGamma) * amp / (amp + gGamma);

			d = (int)(DRAW_HEIGHT * amp);
			if (DRAW_HEIGHT <= d) {
				d = DRAW_HEIGHT - 1;
			}
		}

		// DIBバッファ上に棒グラフを描画
		{
			// DWORD値を使って、一度に４ピクセルずつ描画する
			register LPDWORD pM4 = (LPDWORD)lpBits + b;
			register int j;
			for (j = 0; j < d; ++j) {
				register DWORD ltmp = 8 + j * 55 / DRAW_HEIGHT;
				ltmp |= (ltmp << 8) | (ltmp << 16) | (ltmp << 24);
				*pM4 = ltmp;
				pM4 += BANKS;
			}
		}

		// スクロールバッファにも描画
		{
			register DWORD ltmp = d * PALLET_COLORS / DRAW_HEIGHT;

			ltmp |= (ltmp << 8) | (ltmp << 16) | (ltmp << 24);
			*((DWORD *)(lpBits + DRAW_WIDTH * (DRAW_HEIGHT * 2 - 1)) + b) = ltmp;
		}
	}

	if (gAvgLevel < mxLevel) {
		gAvgLevel = mxLevel;
	}
	else {
		gAvgLevel *= 1.0 - 2.0 / (WaveIn::SAMPLE_RATE / WaveIn::SAMPLES);
	}
	if (gAvgLevel < 32768.0) {
		gAvgLevel = 32768.0;
	}
}
