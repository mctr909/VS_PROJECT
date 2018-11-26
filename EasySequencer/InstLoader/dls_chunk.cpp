#include "dls_chunk.h"

void
DLS::Chunk::Load(LPBYTE ptr, UINT size) {
	for (UINT cur = 0; cur < size; ) {
		mp_chunk = (CK_CHUNK*)ptr;
		ptr += sizeof(CK_CHUNK);

		if (CHUNK_TYPE_LIST == mp_chunk->Type) {
			mp_list = (CK_LIST*)ptr;
			LoadList(ptr + sizeof(CK_LIST), mp_chunk->Size);
		}
		else {
			LoadChunk(ptr);
		}

		ptr += mp_chunk->Size;
		cur += mp_chunk->Size;
	}
}