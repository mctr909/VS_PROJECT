using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class Chunk {
		protected CK_CHUNK mChunk;
		protected CK_LIST mList;

		protected Chunk() { }

		protected Chunk(byte* ptr, byte* endPtr) {
			while (ptr < endPtr) {
				mChunk = (CK_CHUNK)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_CHUNK));
				ptr += sizeof(CK_CHUNK);

				if (CK_CHUNK.TYPE.LIST == mChunk.Type) {
					mList = (CK_LIST)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_LIST));
					LoadList(ptr + sizeof(CK_LIST), ptr + mChunk.Size);
				}
				else {
					LoadChunk(ptr);
				}

				ptr += mChunk.Size;
			}
		}

		public byte[] Bytes {
			get {
				var ms = new MemoryStream();
				var bw = new BinaryWriter(ms);
				WriteChunk(bw);
				WriteList(bw);

				var ms2 = new MemoryStream();
				var bw2 = new BinaryWriter(ms2);
				bw2.Write((uint)CK_CHUNK.TYPE.LIST);
				bw2.Write((uint)(ms.Length + 4));
				bw2.Write((uint)mList.Type);
				bw2.Write(ms.ToArray());

				return ms2.ToArray();
			}
		}

		protected virtual void LoadChunk(byte* ptr) { }

		protected virtual void LoadList(byte* ptr, byte* endPtr) { }

		protected virtual void WriteChunk(BinaryWriter bw) { }

		protected virtual void WriteList(BinaryWriter bw) { }
	}
}
