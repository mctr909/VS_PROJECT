#pragma once
#include <windows.h>
#include <vector>
#include "dls_chunk.h"

namespace DLS {
	class WAVE : Chunk {
	public:
		CK_FMT *mp_format = NULL;
		CK_WSMP *mp_sampler = NULL;
		LPBYTE mp_data = NULL;
		UINT m_dataSize = 0;
		std::vector<WaveLoop*> m_loops;

	public:
		WAVE(LPBYTE ptr, UINT size) { Load(ptr, size); }

	protected:
		void LoadChunk(LPBYTE ptr) override;
		void LoadList(LPBYTE ptr, UINT size) override;
	};

	class WVPL : Chunk {
	public:
		std::vector<WAVE*> m_list;

	public:
		WVPL(LPBYTE ptr, UINT size) { Load(ptr, size); }

	protected:
		void LoadList(LPBYTE ptr, UINT size) override;
	};
}