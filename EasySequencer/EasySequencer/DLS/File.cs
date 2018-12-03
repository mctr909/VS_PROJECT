using System;
using System.IO;

namespace DLS {
	unsafe public class File : Chunk {
		public LINS instruments = null;
		public WVPL wavePool = null;
		public byte[] buffer = null;
		public byte* bufferAddr = null;

		private CK_COLH m_colh;
		private CK_PTBL m_ptbl;
		private CK_VERS* mp_version = null;

		public File(string filePath) {
			uint size = 0;
			var fs = new FileStream(filePath, FileMode.Open);
			var br = new BinaryReader(fs);

			fs.Seek(4, SeekOrigin.Begin);
			size = br.ReadUInt32();

			buffer = new byte[size];
			fs.Seek(12, SeekOrigin.Begin);
			br.Read(buffer, 0, (int)size);

			fixed (byte* p = &buffer[0]) {
				bufferAddr = p;
				Load(p, size);
			}

			br.Close();
			br.Dispose();
			fs.Dispose();
		}

		public override void Dispose() {
			instruments.Dispose();
			wavePool.Dispose();
		}

		protected override void LoadChunk(byte* ptr) {
			switch (mp_chunk->type) {
			case CHUNK_TYPE.COLH:
				m_colh = *(CK_COLH*)ptr;
				break;
			case CHUNK_TYPE.VERS:
				mp_version = (CK_VERS*)ptr;
				break;
			case CHUNK_TYPE.MSYN:
				break;
			case CHUNK_TYPE.PTBL:
				m_ptbl = *(CK_PTBL*)ptr;
				break;
			case CHUNK_TYPE.DLID:
				break;
			default:
				//"Unknown ChunkType"
				break;
			}
		}

		protected override void LoadList(byte* ptr, uint size) {
			switch (mp_list->type) {
			case LIST_TYPE.LINS:
				instruments = new LINS(ptr, size, m_colh.instruments);
				break;
			case LIST_TYPE.WVPL:
				wavePool = new WVPL(ptr, size, m_ptbl.count);
				break;
			case LIST_TYPE.INFO:
				break;
			default:
				// "Unknown ListType"
				break;
			}
		}
	}
}