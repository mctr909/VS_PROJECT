﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class LART : Chunk {
		public ART ART;

		public LART() { }

		public LART(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

		protected override unsafe void LoadChunk(byte* ptr) {
			switch (mChunk.Type) {
			case CK_CHUNK.TYPE.ART1:
			case CK_CHUNK.TYPE.ART2:
				ART = new ART(ptr);
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((uint)mChunk.Type))));
			}
		}

		public new byte[] Bytes {
			get {
				if (null == ART) {
					return new byte[0];
				}

				var ms = new MemoryStream();
				var bw = new BinaryWriter(ms);

				bw.Write((uint)CK_CHUNK.TYPE.ART1);
				bw.Write((uint)(sizeof(CK_ART1) + ART.List.Count * sizeof(Connection)));
				bw.Write((uint)8);
				bw.Write((uint)ART.List.Count);
				foreach (var art in ART.List) {
					bw.Write(art.Value.Bytes);
				}

				var ms2 = new MemoryStream();
				var bw2 = new BinaryWriter(ms2);
				if (0 < ms.Length) {
					bw2.Write((uint)CK_CHUNK.TYPE.LIST);
					bw2.Write((uint)(ms.Length + 4));
					bw2.Write((uint)CK_LIST.TYPE.LART);
					bw2.Write(ms.ToArray());
				}

				return ms2.ToArray();
			}
		}
	}

	unsafe public class ART {
		public Dictionary<int, Connection> List = new Dictionary<int, Connection>();

		public ART() { }

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