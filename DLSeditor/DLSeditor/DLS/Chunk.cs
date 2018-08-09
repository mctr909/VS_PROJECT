using System;

namespace DLS {
	unsafe public class Chunk {
		protected enum CHUNK_ID : UInt32 {
			COLH = 0x686C6F63,
			VERS = 0x73726576,
			MSYN = 0x6E79736D,
			PTBL = 0x6C627470,
			LIST = 0x5453494C,
			DLID = 0x64696C64,
			GUID = 0x64697567,
			INSH = 0x68736E69,
			RGNH = 0x686E6772,
			WSMP = 0x706D7377,
			WLNK = 0x6B6E6C77,
			ART1 = 0x31747261,
			ART2 = 0x32747261,
			FMT_ = 0x20746D66,
			DATA = 0x61746164,
			WAVU = 0x75766177,
			WAVH = 0x68766177,
			SMPL = 0x6C706D73,
			FACT = 0x74636166,
			CUE_ = 0x20657563,
			ADTL = 0x6C746461
		}

		protected enum LIST_ID : UInt32 {
			LINS = 0x736E696C,
			WVPL = 0x6C707677,
			INFO = 0x4F464E49,
			INS_ = 0x20736E69,
			WAVE = 0x65766177,
			LRGN = 0x6E67726C,
			LART = 0x7472616C,
			LAR2 = 0x3272616C,
			RGN_ = 0x206E6772,
			RGN2 = 0x326E6772
		}

		protected CHUNK_ID ChunkId;
		protected LIST_ID ListId;
		protected UInt32 Size;

		protected Chunk() { }

		protected Chunk(byte* ptr, UInt32 endAddr) {
			while ((UInt32)ptr < endAddr) {
				ChunkId = *(CHUNK_ID*)ptr;
				ptr += 4;
				Size = *(UInt32*)ptr;
				ptr += 4;

				if (CHUNK_ID.LIST == ChunkId) {
					ListId = *(LIST_ID*)ptr;
					LoadList(ptr + 4, (UInt32)ptr + Size);
				}
				else {
					LoadChunk(ptr);
				}

				ptr += Size;
			}
		}

		protected virtual void LoadChunk(byte* ptr) { }

		protected virtual void LoadList(byte* ptr, UInt32 endAddr) { }
	}
}