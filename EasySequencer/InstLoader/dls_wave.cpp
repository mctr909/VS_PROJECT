#include "dls_wave.h"

DLS::WAVE::WAVE(LPBYTE ptr, UINT size) {
	Load(ptr, size);
}

DLS::WAVE::~WAVE() {
	if (NULL != mp_loops) {
		free(mp_loops);
		mp_loops = NULL;
	}
}

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
	}
}

DLS::WVPL::WVPL(LPBYTE ptr, UINT size, UINT waveCount) {
	m_listCount = waveCount;
	mpp_list = (WAVE**)malloc(sizeof(WAVE*) * m_listCount);
	Load(ptr, size);
}

DLS::WVPL::~WVPL() {
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

DLS::WAVE*
DLS::WVPL::GetWave(RGN &region) {
	return mpp_list[region.mp_waveLink->TableIndex];
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
		if (m_listIndex < m_listCount) {
			mpp_list[m_listIndex] = new WAVE(ptr, size);
			++m_listIndex;
		}
		break;
	default:
		break;
	}
}