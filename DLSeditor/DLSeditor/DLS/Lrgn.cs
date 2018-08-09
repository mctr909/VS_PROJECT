using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class LRGN : Chunk {
		public Dictionary<int, RGN> List = new Dictionary<int, RGN>();

		public LRGN() { }

		public LRGN(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override void LoadList(byte* ptr, UInt32 endAddr) {
			switch (ListId) {
			case LIST_ID.RGN_:
				List.Add(List.Count, new RGN(ptr, endAddr));
				break;
			default:
				throw new Exception(string.Format("Unknown ListId [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)ListId))));
			}
		}
	}

	unsafe public class RGN : Chunk {
		public CK_RGNH RegionHeader;
		public CK_WSMP Sampler;
		public WaveLoop[] Loops;
		public CK_WLNK WaveLink;
		public LART Articulations;

		public RGN(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override unsafe void LoadChunk(Byte* ptr) {
			switch (ChunkId) {
			case CHUNK_ID.RGNH:
				RegionHeader = (CK_RGNH)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_RGNH));
				if (Size < sizeof(CK_RGNH)) {
					RegionHeader.Layer = 0;
				}
				break;
			case CHUNK_ID.WSMP:
				Sampler = (CK_WSMP)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_WSMP));
				Loops = new WaveLoop[Sampler.LoopCount];
				var pLoop = ptr + sizeof(CK_WSMP);
				for (var i = 0; i < Loops.Length; ++i) {
					Loops[i] = (WaveLoop)Marshal.PtrToStructure((IntPtr)pLoop, typeof(WaveLoop));
					pLoop += sizeof(WaveLoop);
				}
				break;
			case CHUNK_ID.WLNK:
				WaveLink = (CK_WLNK)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_WLNK));
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkId [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)ChunkId))));
			}
		}

		protected override unsafe void LoadList(byte* ptr, UInt32 endAddr) {
			switch (ListId) {
			case LIST_ID.LART:
			case LIST_ID.LAR2:
				Articulations = new LART(ptr, endAddr);
				break;
			default:
				throw new Exception(string.Format("Unknown ListId [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)ListId))));
			}
		}
	}
}