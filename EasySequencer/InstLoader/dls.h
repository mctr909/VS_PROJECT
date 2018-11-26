#pragma once
#include <windows.h>
#include <vector>
#include "dls_chunk.h"
#include "dls_ins.h"
#include "dls_wave.h"

namespace DLS {
	class DLS : Chunk {
	public:
		LINS *mp_instruments = NULL;
		WVPL *mp_wavePool = NULL;

	private:
		UINT m_msyn = 1;
		CK_COLH m_colh = { 0 };
		CK_PTBL m_ptbl = { 0 };
		CK_VERS *mp_version = NULL;
		LPBYTE mp_dlsBuffer = NULL;

	public:
		DLS(LPWSTR filePath);
		~DLS();

	protected:
		void LoadChunk(LPBYTE ptr);
		void LoadList(LPBYTE ptr, UINT size);
	};
}