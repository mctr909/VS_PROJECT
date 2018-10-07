using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

namespace DLS {
	unsafe public class LRGN : Chunk {
		public Dictionary<int, RGN> List = new Dictionary<int, RGN>();

		public LRGN() { }

		public LRGN(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

		protected override void LoadList(byte* ptr, byte* endPtr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.RGN_:
				List.Add(List.Count, new RGN(ptr, endPtr));
				break;
			default:
				throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((uint)mList.Type))));
			}
		}

		public new byte[] Bytes {
			get {
				var ms = new MemoryStream();
				var bw = new BinaryWriter(ms);
				foreach (var rgn in List) {
					bw.Write(rgn.Value.Bytes);
				}

				var ms2 = new MemoryStream();
				var bw2 = new BinaryWriter(ms2);
				if (0 < ms.Length) {
					bw2.Write((uint)CK_CHUNK.TYPE.LIST);
					bw2.Write((uint)(ms.Length + 4));
					bw2.Write((uint)CK_LIST.TYPE.LRGN);
					bw2.Write(ms.ToArray());
				}

				return ms2.ToArray();
			}
		}
	}

	unsafe public class RGN : Chunk {
		public CK_RGNH Header;
		public CK_WSMP Sampler;
		public Dictionary<int, WaveLoop> Loops = new Dictionary<int, WaveLoop>();
		public CK_WLNK WaveLink;
		public LART Articulations = new LART();

		public RGN(byte noteLow = 0, byte noteHigh = 127, byte velocityLow = 0, byte velocityHigh = 127) {
			Header.Key.Low = noteLow;
			Header.Key.High = noteHigh;
			Header.Velocity.Low = velocityLow;
			Header.Velocity.High = velocityHigh;
		}

		public RGN(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

		protected override void LoadChunk(byte* ptr) {
			switch (mChunk.Type) {
			case CK_CHUNK.TYPE.RGNH:
				Header = (CK_RGNH)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_RGNH));
				if (mChunk.Size < sizeof(CK_RGNH)) {
					Header.Layer = 0;
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
				throw new Exception(string.Format("Unknown ChunkType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((uint)mChunk.Type))));
			}
		}

		protected override void LoadList(byte* ptr, byte* endPtr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.LART:
			case CK_LIST.TYPE.LAR2:
				Articulations = new LART(ptr, endPtr);
				break;
			default:
				throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((uint)mList.Type))));
			}
		}

		protected override void WriteChunk(BinaryWriter bw) {
			mList.Type = CK_LIST.TYPE.RGN_;

			var data = Header.Bytes;
			bw.Write((uint)CK_CHUNK.TYPE.RGNH);
			bw.Write(data.Length);
			bw.Write(data);

			data = Sampler.Bytes;
			bw.Write((uint)CK_CHUNK.TYPE.WSMP);
			bw.Write((uint)(data.Length + Sampler.LoopCount * sizeof(WaveLoop)));
			bw.Write(data);
			foreach (var loop in Loops.Values) {
				bw.Write(loop.Bytes);
			}

			data = WaveLink.Bytes;
			bw.Write((uint)CK_CHUNK.TYPE.WLNK);
			bw.Write(data.Length);
			bw.Write(data);
		}

		protected override void WriteList(BinaryWriter bw) {
			bw.Write(Articulations.Bytes);
		}
	}
}