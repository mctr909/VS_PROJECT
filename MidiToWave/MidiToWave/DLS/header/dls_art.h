#pragma once
#include <windows.h>
#include <vector>
#include "dls_chunk.h"

namespace DLS {
	class ART {
	public:
		std::vector<Connection*> m_list;

	public:
		ART(LPBYTE ptr);
	};

	class LART : Chunk {
	public:
		ART *mp_art = NULL;

	public:
		LART(LPBYTE ptr, UINT size) { Load(ptr, size); }

	protected:
		void LoadChunk(LPBYTE ptr) override;
	};
}