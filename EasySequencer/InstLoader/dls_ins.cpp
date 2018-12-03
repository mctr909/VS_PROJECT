#include "dls_ins.h"

DLS::INS::INS(LPBYTE ptr, UINT size) {
	Load(ptr, size);
}

DLS::INS::~INS() {
	if (NULL != mp_regions) {
		delete mp_regions;
		mp_regions = NULL;
	}

	if (NULL != mp_articulations) {
		delete mp_articulations;
		mp_articulations = NULL;
	}
}

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
		if (NULL != mp_regions) {
			delete mp_regions;
			mp_regions = NULL;
		}
		mp_regions = new LRGN(ptr, size, mp_header->Regions);
		break;
	case LIST_TYPE_LART:
	case LIST_TYPE_LAR2:
		if (NULL != mp_articulations) {
			delete mp_articulations;
			mp_articulations = NULL;
		}
		mp_articulations = new LART(ptr, size);
		break;
	case LIST_TYPE_INFO:
		break;
	default:
		// "Unknown ListType"
		break;
	}
}

DLS::LINS::LINS(LPBYTE ptr, UINT size, UINT instCount) {
	m_listCount = instCount;
	mpp_list = (INS**)malloc(sizeof(INS*) * m_listCount);
	Load(ptr, size);
}

DLS::LINS::~LINS() {
	for (UINT i = 0; i < m_listCount; ++i) {
		if (NULL == mpp_list[i]) {
			delete mpp_list[i];
			mpp_list[i] = NULL;
		}
	}

	if (NULL != mpp_list) {
		free(mpp_list);
		mpp_list = NULL;
	}
}

DLS::INS*
DLS::LINS::GetInst(MidiLocale &locale) {
	for (UINT i = 0; i < m_listCount; ++i) {
		auto ins = mpp_list[i];
		auto hdLocale = ins->mp_header->Locale;

		if (hdLocale.BankFlags == locale.BankFlags &&
			hdLocale.ProgramNo == locale.ProgramNo &&
			hdLocale.BankMSB == locale.BankMSB &&
			hdLocale.BankLSB == locale.BankLSB
		) {
			return ins;
		}
	}
	return NULL;
}

void
DLS::LINS::LoadList(LPBYTE ptr, UINT size) {
	switch (mp_list->Type) {
	case LIST_TYPE_INS_:
		if (m_listIndex < m_listCount) {
			mpp_list[m_listIndex] = new INS(ptr, size);
			++m_listIndex;
		}
		break;
	default:
		// "Unknown ListId"
		break;
	}
}