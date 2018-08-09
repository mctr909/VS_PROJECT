using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class LART : Chunk {
		public Dictionary<int, ART> List = new Dictionary<int, ART>();

		public LART(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override unsafe void LoadChunk(Byte* ptr) {
			switch (ChunkId) {
			case CHUNK_ID.ART1:
			case CHUNK_ID.ART2:
				List.Add(List.Count, new ART(ptr));
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkId [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)ChunkId))));
			}
		}
	}

	unsafe public struct ART {
		public CK_ART1 Info;
		public ConnectionBlock[] Connections;

		public ART(byte* ptr) {
			Info = (CK_ART1)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_ART1));
			ptr += sizeof(CK_ART1);

			Connections = new ConnectionBlock[Info.Blocks];
			for (var i = 0; i < Info.Blocks; ++i) {
				Connections[i] = (ConnectionBlock)Marshal.PtrToStructure((IntPtr)ptr, typeof(ConnectionBlock));
				ptr += sizeof(ConnectionBlock);
			}
		}
	}
}