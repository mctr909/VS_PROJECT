#include "dls_art.h"

DLS::ART::ART(LPBYTE ptr) {
	CK_ART1 info;
	memcpy(&info, ptr, sizeof(CK_ART1));
	ptr += sizeof(CK_ART1);

	for (UINT i = 0; i < info.Count; ++i) {
		m_list.push_back((Connection*)ptr);
		ptr += sizeof(Connection);
	}
}

void
DLS::LART::LoadChunk(LPBYTE ptr) {
	switch (mp_chunk->Type) {
	case CHUNK_TYPE_ART1:
	case CHUNK_TYPE_ART2:
		mp_art = new ART(ptr);
		break;
	default:
		// "Unknown ChunkType"
		break;
	}
}