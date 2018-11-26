#pragma once
#include <windows.h>
#include "dls_chunk.h"
#include "dls_art.h"

namespace DLS {
	class RGN : Chunk {
	public:
		CK_RGNH *mp_header = NULL;
		CK_WSMP *mp_sampler = NULL;
		CK_WLNK *mp_waveLink = NULL;
		LART *mp_articulations = NULL;
		WaveLoop *mp_loops = NULL;

	public:
		RGN(LPBYTE ptr, UINT size);
		~RGN();

	protected:
		void LoadChunk(LPBYTE ptr) override;
		void LoadList(LPBYTE ptr, UINT size) override;
	};

	class LRGN : Chunk {
	private:
		UINT m_listCount = 0;
		UINT m_listIndex = 0;
		RGN **mpp_list = NULL;

	public:
		LRGN(LPBYTE ptr, UINT size, UINT regionCount);
		~LRGN();

		RGN* GetRegion(BYTE note, BYTE velocity);

	protected:
		void LoadList(LPBYTE ptr, UINT size) override;
	};
}