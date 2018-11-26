#pragma once
#include <windows.h>
#include "dls_chunk.h"
#include "dls_rgn.h"
#include "dls_art.h"

namespace DLS {
	class INS : Chunk {
	public:
		CK_INSH *mp_header = NULL;
		LRGN *mp_regions = NULL;
		LART *mp_articulations = NULL;

	public:
		INS(LPBYTE ptr, UINT size);
		~INS();

	protected:
		void LoadChunk(LPBYTE ptr) override;
		void LoadList(LPBYTE ptr, UINT size) override;
	};

	class LINS : Chunk {
	private:
		UINT m_listCount = 0;
		UINT m_listIndex = 0;
		INS **mpp_list = NULL;

	public:
		LINS(LPBYTE ptr, UINT size, UINT instCount);
		~LINS();

		INS* GetInst(MidiLocale &locale);

	protected:
		void LoadList(LPBYTE ptr, UINT size) override;
	};
}