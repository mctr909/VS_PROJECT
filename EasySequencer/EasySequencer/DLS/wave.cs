using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class WAVE : Chunk {
		public CK_FMT* pFormat = null;
		public CK_WSMP* pSampler = null;
		public WaveLoop* pLoops = null;
		public uint pcmAddr = 0;
		public uint dataSize = 0;

		public WAVE(byte* ptr, uint size) {
			Load(ptr, size);
		}

		public override void Dispose() {
			if (null != pLoops) {
				Marshal.FreeHGlobal((IntPtr)pLoops);
				pLoops = null;
			}
		}

		protected override void LoadChunk(byte* ptr) {
			switch (mp_chunk->type) {
			case CHUNK_TYPE.DLID:
			case CHUNK_TYPE.GUID:
				break;
			case CHUNK_TYPE.FMT_:
				pFormat = (CK_FMT*)ptr;
				break;
			case CHUNK_TYPE.DATA:
				pcmAddr = (uint)((IntPtr)ptr).ToInt64();
				dataSize = mp_chunk->size;
				break;
			case CHUNK_TYPE.WSMP: {
					if (null != pLoops) {
						Marshal.FreeHGlobal((IntPtr)pLoops);
						pLoops = null;
					}

					pSampler = (CK_WSMP*)ptr;
					var pLoop = ptr + sizeof(CK_WSMP);
					pLoops = (WaveLoop*)Marshal.AllocHGlobal(sizeof(WaveLoop) * (int)pSampler->loopCount);
					for (uint i = 0; i < pSampler->loopCount; ++i) {
						pLoops[i] = *(WaveLoop*)pLoop;
						pLoop += sizeof(WaveLoop);
					}
				}
				break;
			default:
				// "Unknown ChunkType"
				break;
			}
		}

		protected override void LoadList(byte* ptr, uint size) {
			switch (mp_list->type) {
			case LIST_TYPE.INFO:
				break;
			}
		}
	}

	unsafe public class WVPL : Chunk {
		public Dictionary<int, WAVE> List = null;
		private uint m_listCount = 0;
		private uint m_listIndex = 0;

		public WVPL(byte* ptr, uint size, uint waveCount) {
			m_listCount = waveCount;
			List = new Dictionary<int, WAVE>();
			Load(ptr, size);
		}

		public override void Dispose() {
			foreach (var wave in List.Values) {
				if (null != wave) {
					wave.Dispose();
				}
			}
		}

		protected override void LoadList(byte* ptr, uint size) {
			switch (mp_list->type) {
			case LIST_TYPE.WAVE:
				if (m_listIndex < m_listCount) {
					List.Add(List.Count, new WAVE(ptr, size));
					++m_listIndex;
				}
				break;
			default:
				break;
			}
		}
	}
}
