#pragma once
#include <windows.h>
#include <vector>
#include "dls_chunk.h"
#include "dls_rgn.h"
#include "dls_art.h"

namespace DLS {
	class INS : Chunk {
	public:
		CK_INSH *mp_header = NULL;
		LRGN *mp_regions = NULL;
		LART *mp_articulations = NULL;

	public:
		INS(LPBYTE ptr, UINT size) { Load(ptr, size); }

	protected:
		void LoadChunk(LPBYTE ptr) override;
		void LoadList(LPBYTE ptr, UINT size) override;
	};

	class LINS : Chunk {
	public:
		std::vector<INS*> m_list;

	public:
		LINS(LPBYTE ptr, UINT size) { Load(ptr, size); }

	protected:
		void LoadList(LPBYTE ptr, UINT size) override;
	};
}