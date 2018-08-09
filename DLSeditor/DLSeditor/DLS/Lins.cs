using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class LINS : Chunk {
		public Dictionary<int, INS> List = new Dictionary<int, INS>();

		public LINS() { }

		public LINS(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override void LoadList(byte* ptr, UInt32 endAddr) {
			switch (ListId) {
			case LIST_ID.INS_:
				List.Add(List.Count, new INS(ptr, endAddr));
				break;
			default:
				throw new Exception(string.Format("Unknown ListId [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)ListId))));
			}
		}
	}

	unsafe public class INS : Chunk {
		public CK_INSH InstHeader;
		public LRGN Regions;
		public LART Articulations;
		public INFO Info;

		public INS(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override unsafe void LoadChunk(Byte* ptr) {
			switch (ChunkId) {
			case CHUNK_ID.INSH:
				InstHeader = (CK_INSH)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_INSH));
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkId [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)ChunkId))));
			}
		}

		protected override unsafe void LoadList(byte* ptr, UInt32 endAddr) {
			switch (ListId) {
			case LIST_ID.LRGN:
				Regions = new LRGN(ptr, endAddr);
				break;
			case LIST_ID.LART:
			case LIST_ID.LAR2:
				Articulations = new LART(ptr, endAddr);
				break;
			case LIST_ID.INFO:
				Info = new INFO(ptr, endAddr);
				break;
			default:
				throw new Exception(string.Format("Unknown ListId [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)ListId))));
			}
		}
	}
}