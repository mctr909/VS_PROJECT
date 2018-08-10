using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class LART : Chunk {
		public Dictionary<int, ART> List = new Dictionary<int, ART>();

		public LART(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override unsafe void LoadChunk(Byte* ptr) {
			switch (mChunk.Type) {
			case CK_CHUNK.TYPE.ART1:
			case CK_CHUNK.TYPE.ART2:
				List.Add(List.Count, new ART(ptr));
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)mChunk.Type))));
			}
		}
	}

	unsafe public class ART {
		public Dictionary<int, Connection> List = new Dictionary<int, Connection>();

		public ART(byte* ptr) {
			var info = (CK_ART1)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_ART1));
			ptr += sizeof(CK_ART1);

			for (var i = 0; i < info.Count; ++i) {
				List.Add(List.Count, (Connection)Marshal.PtrToStructure((IntPtr)ptr, typeof(Connection)));
				ptr += sizeof(Connection);
			}
		}
	}
}