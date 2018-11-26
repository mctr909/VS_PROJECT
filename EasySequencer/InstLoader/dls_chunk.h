#pragma once
#include "dls_const.h"
#include "dls_struct.h"

namespace DLS {
	class Chunk {
	protected:
		CK_CHUNK *mp_chunk;
		CK_LIST *mp_list;

	protected:
		Chunk() {}
		void Load(LPBYTE ptr, UINT size);
		virtual void LoadChunk(LPBYTE ptr) {}
		virtual void LoadList(LPBYTE ptr, UINT size) {}
	};
}