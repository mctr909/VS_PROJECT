using System;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class DLS : Chunk {
		private UInt32 mInstCount;
		private CK_VERS mVersion;
		private UInt32 mMSYN;
		private CK_PTBL mPtbl;
		private UInt32[] mWaveCue;
		private CK_DLID mDlId;

		public LINS Instruments;
		public WVPL WavePool;
		public INFO Info;

		public DLS() {
			mMSYN = 1;
			Instruments = new LINS();
			WavePool = new WVPL();
		}

		public DLS(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override unsafe void LoadChunk(Byte* ptr) {
			switch (ChunkId) {
			case CHUNK_ID.COLH:
				mInstCount = *(UInt32*)ptr;
				break;
			case CHUNK_ID.VERS:
				mVersion = (CK_VERS)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_VERS));
				break;
			case CHUNK_ID.MSYN:
				mMSYN = *(UInt32*)ptr;
				break;
			case CHUNK_ID.PTBL:
				mPtbl = (CK_PTBL)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_PTBL));
				ptr += sizeof(CK_PTBL);
				mWaveCue = new UInt32[mPtbl.Cues];
				for (var i = 0; i < mPtbl.Cues; ++i) {
					mWaveCue[i] = *(UInt32*)ptr;
					ptr += sizeof(UInt32);
				}
				break;
			case CHUNK_ID.DLID:
				mDlId = (CK_DLID)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_DLID));
				break;
			default:
				throw new Exception();
			}
		}

		protected override unsafe void LoadList(byte* ptr, UInt32 endAddr) {
			switch (ListId) {
			case LIST_ID.LINS:
				Instruments = new LINS(ptr, endAddr);
				break;
			case LIST_ID.WVPL:
				WavePool = new WVPL(ptr, endAddr);
				break;
			case LIST_ID.INFO:
				Info = new INFO(ptr, endAddr);
				break;
			default:
				throw new Exception();
			}
		}
	}
}