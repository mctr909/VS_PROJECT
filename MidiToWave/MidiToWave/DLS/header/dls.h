#pragma once
#include <windows.h>
#include "dls_chunk.h"
#include "dls_ins.h"
#include "dls_wave.h"

namespace DLS {
	class DLS : Chunk {
	private:
		UINT m_msyn = 1;
		CK_VERS *mp_version = NULL;
		LINS *mp_instruments = NULL;
		WVPL *mp_wavePool = NULL;
		LPBYTE mp_dlsBuffer = NULL;

	public:
		DLS(LPWSTR filePath);
		~DLS();

		INS* GetInst(MidiLocale &locale);
		WAVE* GetWave(RGN &region);

	protected:
		void LoadChunk(LPBYTE ptr);
		void LoadList(LPBYTE ptr, UINT size);
	};
}