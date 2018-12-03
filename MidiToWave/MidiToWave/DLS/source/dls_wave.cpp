#include "dls_wave.h"

void
DLS::WAVE::LoadChunk(LPBYTE ptr) {
	switch (mp_chunk->Type) {
	case CHUNK_TYPE_DLID:
	case CHUNK_TYPE_GUID:
		break;
	case CHUNK_TYPE_FMT_:
		mp_format = (CK_FMT*)ptr;
		break;
	case CHUNK_TYPE_DATA:
		m_dataSize = mp_chunk->Size;
		mp_data = ptr;
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
	}
}

void
DLS::WAVE::LoadList(LPBYTE ptr, UINT size) {
	switch (mp_list->Type) {
	case LIST_TYPE_INFO:
		break;
	}
}

void
DLS::WVPL::LoadList(LPBYTE ptr, UINT size) {
	switch (mp_list->Type) {
	case LIST_TYPE_WAVE:
		m_list.push_back(new WAVE(ptr, size));
		break;
	default:
		break;
	}
}