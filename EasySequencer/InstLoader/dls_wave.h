#pragma once
#include <windows.h>
#include "dls_chunk.h"
#include "dls_rgn.h"

namespace DLS {
	class WAVE : Chunk {
	public:
		CK_FMT *mp_format = NULL;
		CK_WSMP *mp_sampler = NULL;
		LPBYTE mp_data = NULL;
		UINT m_dataSize = 0;
		WaveLoop *mp_loops = NULL;

	public:
		WAVE(LPBYTE ptr, UINT size);
		~WAVE();

	protected:
		void LoadChunk(LPBYTE ptr) override;
		void LoadList(LPBYTE ptr, UINT size) override;
	};

	class WVPL : Chunk {
	private:
		UINT m_listCount = 0;
		UINT m_listIndex = 0;
		WAVE **mpp_list = NULL;

	public:
		WVPL(LPBYTE ptr, UINT size, UINT waveCount);
		~WVPL();

		WAVE* GetWave(RGN &region);

	protected:
		void LoadList(LPBYTE ptr, UINT size) override;
	};
}