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
			switch (mList.Type) {
			case CK_LIST.TYPE.INS_:
				List.Add(List.Count, new INS(ptr, endAddr));
				break;
			default:
				throw new Exception(string.Format("Unknown ListId [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)mList.Type))));
			}
		}
	}

	unsafe public class INS : Chunk {
		public CK_INSH Header;
		public LRGN Regions = new LRGN();
		public LART Articulations = new LART();
		public INFO Text = new INFO();

		public INS() { }

		public INS(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override unsafe void LoadChunk(Byte* ptr) {
			switch (mChunk.Type) {
			case CK_CHUNK.TYPE.INSH:
				Header = (CK_INSH)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_INSH));
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)mChunk.Type))));
			}
		}

		protected override unsafe void LoadList(byte* ptr, UInt32 endAddr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.LRGN:
				Regions = new LRGN(ptr, endAddr);
				break;
			case CK_LIST.TYPE.LART:
			case CK_LIST.TYPE.LAR2:
				Articulations = new LART(ptr, endAddr);
				break;
			case CK_LIST.TYPE.INFO:
				Text = new INFO(ptr, endAddr);
				break;
			default:
				throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)mList.Type))));
			}
		}
	}
}