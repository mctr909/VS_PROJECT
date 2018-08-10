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
			switch (mList.Type) {
			case CK_LIST.TYPE.RGN_:
				List.Add(List.Count, new RGN(ptr, endAddr));
				break;
			default:
				throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)mList.Type))));
			}
		}
	}

	unsafe public class RGN : Chunk {
		public CK_RGNH RegionHeader;
		public CK_WSMP Sampler;
		public Dictionary<int, WaveLoop> Loops = new Dictionary<int, WaveLoop>();
		public CK_WLNK WaveLink;
		public LART Articulations;

		public RGN(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override unsafe void LoadChunk(Byte* ptr) {
			switch (mChunk.Type) {
			case CK_CHUNK.TYPE.RGNH:
				RegionHeader = (CK_RGNH)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_RGNH));
				if (mChunk.Size < sizeof(CK_RGNH)) {
					RegionHeader.Layer = 0;
				}
				break;
			case CK_CHUNK.TYPE.WSMP:
				Sampler = (CK_WSMP)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_WSMP));
				var pLoop = ptr + sizeof(CK_WSMP);
				for (var i = 0; i < Sampler.LoopCount; ++i) {
					Loops.Add(Loops.Count, (WaveLoop)Marshal.PtrToStructure((IntPtr)pLoop, typeof(WaveLoop)));
					pLoop += sizeof(WaveLoop);
				}
				break;
			case CK_CHUNK.TYPE.WLNK:
				WaveLink = (CK_WLNK)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_WLNK));
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)mChunk.Type))));
			}
		}

		protected override unsafe void LoadList(byte* ptr, UInt32 endAddr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.LART:
			case CK_LIST.TYPE.LAR2:
				Articulations = new LART(ptr, endAddr);
				break;
			default:
				throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)mList.Type))));
			}
		}
	}
}