#include "dls_rgn.h"

DLS::RGN::RGN(LPBYTE ptr, UINT size) {
	Load(ptr, size);
}

DLS::RGN::~RGN() {
	if (NULL != mp_loops) {
		free(mp_loops);
		mp_loops = NULL;
	}
}

void
DLS::RGN::LoadChunk(LPBYTE ptr) {
	switch (mp_chunk->Type) {
	case CHUNK_TYPE_RGNH:
		mp_header = (CK_RGNH*)ptr;
		if (mp_chunk->Size < sizeof(CK_RGNH)) {
			mp_header->Layer = 0;
		}
		break;
	case CHUNK_TYPE_WSMP:
	{
		if (NULL != mp_loops) {
			free(mp_loops);
			mp_loops = NULL;
		}

		mp_sampler = (CK_WSMP*)ptr;
		auto pLoop = ptr + sizeof(CK_WSMP);
		mp_loops = (WaveLoop*)malloc(sizeof(WaveLoop) * mp_sampler->LoopCount);
		for (UINT i = 0; i < mp_sampler->LoopCount; ++i) {
			mp_loops[i] = *(WaveLoop*)pLoop;
			pLoop += sizeof(WaveLoop);
		}
	}
	break;
	case CHUNK_TYPE_WLNK:
		mp_waveLink = (CK_WLNK*)ptr;
		break;
	default:
		// "Unknown ChunkType"
		break;
	}
}

void
DLS::RGN::LoadList(LPBYTE ptr, UINT size) {
	switch (mp_list->Type) {
	case LIST_TYPE_LART:
	case LIST_TYPE_LAR2:
		mp_articulations = new LART(ptr, size);
		break;
	default:
		// "Unknown ListType"
		break;
	}
}

DLS::LRGN::LRGN(LPBYTE ptr, UINT size, UINT regionCount) {
	m_listCount = regionCount;
	mpp_list = (RGN**)malloc(sizeof(RGN*) * m_listCount);
	Load(ptr, size);
}

DLS::LRGN::~LRGN() {
	for (UINT i = 0; i < m_listCount; ++i) {
		if (NULL != mpp_list[i]) {
			delete mpp_list[i];
			mpp_list[i] = NULL;
		}
	}

	if (NULL != mpp_list) {
		free(mpp_list);
		mpp_list = NULL;
	}
}

DLS::RGN*
DLS::LRGN::GetRegion(BYTE note, BYTE velocity) {
	for (UINT i = 0; i < m_listCount; ++i) {
		auto rgn = mpp_list[i];
		auto k = rgn->mp_header->Key;
		auto v = rgn->mp_header->Velocity;
		if (k.Low <= note && note <= k.High &&
			v.Low <= velocity && velocity <= v.High
		) {
			return rgn;
		}
	}
	return NULL;
}

void
DLS::LRGN::LoadList(LPBYTE ptr, UINT size) {
	switch (mp_list->Type) {
	case LIST_TYPE_RGN_:
		if (m_listIndex < m_listCount) {
			mpp_list[m_listIndex] = new RGN(ptr, size);
			++m_listIndex;
		}
		break;
	default:
		// "Unknown ListType"
		break;
	}
}