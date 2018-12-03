using System.Collections.Generic;

namespace DLS {
	unsafe public class INS_ : Chunk {
		public CK_INSH* pHeader = null;
		public LRGN regions = null;
		public LART articulations = null;

		public INS_(byte* ptr, uint size) {
			Load(ptr, size);
		}

		protected override void LoadChunk(byte* ptr) {
			switch (mp_chunk->type) {
			case CHUNK_TYPE.INSH:
				pHeader = (CK_INSH*)ptr;
				break;
			default:
				// "Unknown ChunkType"
				break;
			}
		}

		protected override void LoadList(byte* ptr, uint size) {
			switch (mp_list->type) {
			case LIST_TYPE.LRGN:
				if (null != regions) {
					regions.Dispose();
					regions = null;
				}
				regions = new LRGN(ptr, size, pHeader->regions);
				break;
			case LIST_TYPE.LART:
			case LIST_TYPE.LAR2:
				if (null != articulations) {
					articulations.Dispose();
					articulations = null;
				}
				articulations = new LART(ptr, size);
				break;
			case LIST_TYPE.INFO:
				break;
			default:
				// "Unknown ListType"
				break;
			}
		}
	}

	unsafe public class LINS : Chunk {
		public HashSet<INS_> List = null;
		private uint m_listCount = 0;
		private uint m_listIndex = 0;

		public LINS(byte* ptr, uint size, uint instCount) {
			m_listCount = instCount;
			List = new HashSet<INS_>();
			Load(ptr, size);
		}

		protected override void LoadList(byte* ptr, uint size) {
			switch (mp_list->type) {
			case LIST_TYPE.INS_:
				if (m_listIndex < m_listCount) {
					List.Add(new INS_(ptr, size));
					++m_listIndex;
				}
				break;
			default:
				// "Unknown ListId"
				break;
			}
		}
	}
}