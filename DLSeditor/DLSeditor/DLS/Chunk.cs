using System;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class Chunk {
		protected CK_CHUNK mChunk;
		protected CK_LIST mList;

		protected Chunk() { }

		protected Chunk(byte* ptr, UInt32 endAddr) {
			while ((UInt32)ptr < endAddr) {
				mChunk = (CK_CHUNK)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_CHUNK));
				ptr += sizeof(CK_CHUNK);

				if (CK_CHUNK.TYPE.LIST == mChunk.Type) {
					mList = (CK_LIST)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_LIST));
					LoadList(ptr + sizeof(CK_LIST), (UInt32)ptr + mChunk.Size);
				}
				else {
					LoadChunk(ptr);
				}

				ptr += mChunk.Size;
			}
		}

		protected virtual void LoadChunk(byte* ptr) { }

		protected virtual void LoadList(byte* ptr, UInt32 endAddr) { }
	}
}