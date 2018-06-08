using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS
{
	unsafe public class File
	{
		private byte[] mBuff;
		private UInt32 mID;
		private UInt32 mSize;
		private UInt32 mType;
		private UInt32 mInstCount;
		private byte[] mVersion;
		private UInt32 mMSYN;

		public LINS InstList;
		public WVPL WavePool;
		public INFO Info;

		public File()
		{
			mID = (UInt32)RIFF_CONST.ID;
			mSize = 0;
			mType = (UInt32)RIFF_CONST.TYPE_WAVE;

			mInstCount = 0;
			mVersion = new byte[8];
			mMSYN = 1;

			InstList = new LINS();
			WavePool = new WVPL();
		}

		public File(string filePath)
		{
			using (var fs = new FileStream(filePath, FileMode.Open))
			using (var br = new BinaryReader(fs)) {
				mID = br.ReadUInt32();
				mSize = br.ReadUInt32();
				mType = br.ReadUInt32();

				mBuff = new byte[mSize - 4];
				fs.Read(mBuff, 0, mBuff.Length);
				fs.Close();
			}

			fixed (byte* p = &mBuff[0])
			{
				var ptr = p;
				var endAddr = (UInt32)(ptr + mBuff.Length);

				while ((UInt32)ptr < endAddr) {
					var chunkType = *(ChunkID*)ptr;
					ptr += 4;
					var chunkSize = *(UInt32*)ptr;
					ptr += 4;

					switch (chunkType) {
					case ChunkID.COLH:
						mInstCount = *(UInt32*)ptr;
						break;
					case ChunkID.VERS:
						mVersion = new byte[chunkSize];
						Marshal.Copy((IntPtr)ptr, mVersion, 0, mVersion.Length);
						break;
					case ChunkID.MSYN:
						mMSYN = *(UInt32*)ptr;
						break;
					case ChunkID.PTBL:
						break;
					case ChunkID.LIST:
						ReadList(ptr, chunkSize);
						break;
					case ChunkID.DLID:
						break;
					default:
						throw new Exception();
					}

					ptr += chunkSize;
				}
			}
		}

		private void ReadList(byte* ptr, UInt32 size)
		{
			var listType = *(ListID*)ptr;
			var endAddr = (UInt32)ptr + size;
			ptr += 4;

			switch (listType) {
			case ListID.LINS:
				InstList = new LINS(ptr, endAddr);
				break;
			case ListID.WVPL:
				WavePool = new WVPL(ptr, endAddr);
				break;
			case ListID.INFO:
				Info = new INFO(ptr, endAddr);
				break;
			default:
				throw new Exception();
			}
		}
	}
}
