#include "dls.h"

DLS::DLS::DLS(LPWSTR filePath) {
	UINT size = 0;
	FILE *fp = NULL;
	_wfopen_s(&fp, filePath, TEXT("r"));

	if (NULL != fp) {
		fseek(fp, 4, SEEK_SET);
		fread(&size, 4, 1, fp);

		if (NULL != mp_dlsBuffer) {
			free(mp_dlsBuffer);
			mp_dlsBuffer = NULL;
		}

		mp_dlsBuffer = (LPBYTE)malloc(size);

		fseek(fp, 12, SEEK_SET);
		fread(mp_dlsBuffer, size, 1, fp);

		Load(mp_dlsBuffer, size);

		fclose(fp);
	}
}

DLS::DLS::~DLS() {
	if (NULL != mp_dlsBuffer) {
		free(mp_dlsBuffer);
		mp_dlsBuffer = NULL;
	}
}

DLS::INS*
DLS::DLS::GetInst(MidiLocale &locale) {
	if (NULL == mp_instruments) {
		return NULL;
	}

	for (auto ins : mp_instruments->m_list) {
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

DLS::WAVE*
DLS::DLS::GetWave(RGN &region) {
	if (NULL == mp_wavePool) {
		return NULL;
	}

	return mp_wavePool->m_list[region.mp_waveLink->TableIndex];
}

void
DLS::DLS::LoadChunk(LPBYTE ptr) {
	switch (mp_chunk->Type) {
	case CHUNK_TYPE_COLH:
		break;
	case CHUNK_TYPE_VERS:
		mp_version = (CK_VERS*)ptr;
		break;
	case CHUNK_TYPE_MSYN:
		break;
	case CHUNK_TYPE_PTBL:
		break;
	case CHUNK_TYPE_DLID:
		break;
	default:
		//"Unknown ChunkType"
		break;
	}
}

void
DLS::DLS::LoadList(LPBYTE ptr, UINT size) {
	switch (mp_list->Type) {
	case LIST_TYPE_LINS:
		mp_instruments = new LINS(ptr, size);
		break;
	case LIST_TYPE_WVPL:
		mp_wavePool = new WVPL(ptr, size);
		break;
	case LIST_TYPE_INFO:
		break;
	default:
		// "Unknown ListType"
		break;
	}
}