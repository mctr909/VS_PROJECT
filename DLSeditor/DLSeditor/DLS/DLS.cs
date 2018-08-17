using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace DLS {
	unsafe public class DLS : Chunk {
		private CK_VERS mVersion;
		private UInt32 mMSYN = 1;
		private Dictionary<int, UInt32> mWaveCue = new Dictionary<int, UInt32>();
		private CK_DLID mDlId;

		public INFO Text = new INFO();
		public LINS Instruments = new LINS();
		public WVPL WavePool = new WVPL();

		public DLS() { }

		public DLS(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override unsafe void LoadChunk(byte* ptr) {
			switch (mChunk.Type) {
			case CK_CHUNK.TYPE.COLH:
				break;
			case CK_CHUNK.TYPE.VERS:
				mVersion = (CK_VERS)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_VERS));
				break;
			case CK_CHUNK.TYPE.MSYN:
				mMSYN = *(UInt32*)ptr;
				break;
			case CK_CHUNK.TYPE.PTBL:
				var ptbl = (CK_PTBL)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_PTBL));
				ptr += sizeof(CK_PTBL);
				for (var i = 0; i < ptbl.Count; ++i) {
					mWaveCue.Add(mWaveCue.Count, *(UInt32*)ptr);
					ptr += sizeof(UInt32);
				}
				break;
			case CK_CHUNK.TYPE.DLID:
				mDlId = (CK_DLID)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_DLID));
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)mChunk.Type))));
			}
		}

		protected override unsafe void LoadList(byte* ptr, UInt32 endAddr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.LINS:
				Instruments = new LINS(ptr, endAddr);
				break;
			case CK_LIST.TYPE.WVPL:
				WavePool = new WVPL(ptr, endAddr);
				break;
			case CK_LIST.TYPE.INFO:
				Text = new INFO(ptr, endAddr);
				break;
			default:
				throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((UInt32)mList.Type))));
			}
		}

		public void Save(string filePath) {
			var fs = new FileStream(filePath, FileMode.Create);
			var fw = new BinaryWriter(fs);

			// COLH
			fw.Write((UInt32)CK_CHUNK.TYPE.COLH);
			fw.Write((UInt32)4);
			fw.Write((UInt32)Instruments.List.Count);


		}
	}
}