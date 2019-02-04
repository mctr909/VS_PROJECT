#pragma once
#include <windows.h>
#include "dls_chunk.h"

namespace DLS {
	class WAVE : Chunk {
	public:
		CK_FMT *mp_format = NULL;
		CK_WSMP *mp_sampler = NULL;
		LPBYTE mp_data = NULL;
		UINT m_dataSize = 0;
		WaveLoop *mp_loops = NULL;

	public:
		WAVE(LPBYTE ptr, UINT size) { Load(ptr, size); }
		~WAVE();

	protected:
		void LoadChunk(LPBYTE ptr) override;
		void LoadList(LPBYTE ptr, UINT size) override;
	};

	class WVPL : Chunk {
	public:
		UINT m_listCount = 0;
		WAVE **mpp_list = NULL;

	public:
		WVPL(LPBYTE ptr, UINT size) { Load(ptr, size); }
		~WVPL();

	protected:
		void LoadList(LPBYTE ptr, UINT size) override;
	};
}