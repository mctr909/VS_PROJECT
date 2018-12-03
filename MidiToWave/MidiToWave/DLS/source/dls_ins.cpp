#include "dls_ins.h"

void
DLS::INS::LoadChunk(LPBYTE ptr) {
	switch (mp_chunk->Type) {
	case CHUNK_TYPE_INSH:
		mp_header = (CK_INSH*)ptr;
		break;
	default:
		// "Unknown ChunkType"
		break;
	}
}

void
DLS::INS::LoadList(LPBYTE ptr, UINT size) {
	switch (mp_list->Type) {
	case LIST_TYPE_LRGN:
		mp_regions = new LRGN(ptr, size);
		break;
	case LIST_TYPE_LART:
	case LIST_TYPE_LAR2:
		mp_articulations = new LART(ptr, size);
		break;
	case LIST_TYPE_INFO:
		break;
	default:
		// "Unknown ListType"
		break;
	}
}

void
DLS::LINS::LoadList(LPBYTE ptr, UINT size) {
	switch (mp_list->Type) {
	case LIST_TYPE_INS_:
		m_list.push_back(new INS(ptr, size));
		break;
	default:
		// "Unknown ListId"
		break;
	}
}