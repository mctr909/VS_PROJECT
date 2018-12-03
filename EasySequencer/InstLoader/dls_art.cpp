#include "dls_art.h"

DLS::ART::ART(LPBYTE ptr) {
	CK_ART1 info;
	memcpy(&info, ptr, sizeof(CK_ART1));
	ptr += sizeof(CK_ART1);

	m_listCount = info.Count;
	mp_list = (Connection*)malloc(sizeof(Connection) * m_listCount);

	for (UINT i = 0; i < info.Count; ++i) {
		mp_list[i] = *(Connection*)ptr;
		ptr += sizeof(Connection);
	}
}

DLS::ART::~ART() {
	if (NULL != mp_list) {
		free(mp_list);
		mp_list = NULL;
	}
}

DLS::LART::LART(LPBYTE ptr, UINT size) {
	Load(ptr, size);
}

DLS::LART::~LART() {
	if (NULL != mp_art) {
		delete mp_art;
		mp_art = NULL;
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