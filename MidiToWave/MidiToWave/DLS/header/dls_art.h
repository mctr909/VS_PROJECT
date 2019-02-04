#pragma once
#include <windows.h>
#include "dls_chunk.h"

namespace DLS {
	class ART {
	public:
		UINT m_listCount = 0;
		Connection* mp_list = NULL;

	public:
		ART(LPBYTE ptr);
		~ART();
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