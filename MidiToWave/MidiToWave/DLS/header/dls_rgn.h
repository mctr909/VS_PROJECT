#pragma once
#include <windows.h>
#include <vector>
#include "dls_chunk.h"
#include "dls_art.h"

namespace DLS {
	class RGN : Chunk {
	public:
		CK_RGNH *mp_header = NULL;
		CK_WSMP *mp_sampler = NULL;
		CK_WLNK *mp_waveLink = NULL;
		LART *mp_articulations = NULL;
		std::vector<WaveLoop*> m_loops;

	public:
		RGN(LPBYTE ptr, UINT size) { Load(ptr, size); }

	protected:
		void LoadChunk(LPBYTE ptr) override;
		void LoadList(LPBYTE ptr, UINT size) override;
	};

	class LRGN : Chunk {
	public:
		std::vector<RGN*> m_list;

	public:
		LRGN(LPBYTE ptr, UINT size) { Load(ptr, size); }
		RGN* GetRegion(BYTE note, BYTE velocity);

	protected:
		void LoadList(LPBYTE ptr, UINT size) override;
	};
}