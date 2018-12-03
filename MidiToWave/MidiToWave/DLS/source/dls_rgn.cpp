#include "dls_rgn.h"

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
		mp_sampler = (CK_WSMP*)ptr;
		auto pLoop = ptr + sizeof(CK_WSMP);
		for (UINT i = 0; i < mp_sampler->LoopCount; ++i) {
			m_loops.push_back((WaveLoop*)pLoop);
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

DLS::RGN*
DLS::LRGN::GetRegion(BYTE note, BYTE velocity) {
	for (auto rgn : m_list) {
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
		m_list.push_back(new RGN(ptr, size));
		break;
	default:
		// "Unknown ListType"
		break;
	}
}