using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS
{
	unsafe public class File
	{
		private UInt32 mID;
		private UInt32 mSize;
		private UInt32 mType;
		private UInt32 mInstCount;
		private byte[] mVersion;
		private UInt32 mMSYN;

		public PTBL WaveOffsetPool;
		public LINS InstPool;
		public WVPL WavePool;
		public INFO Info;

		private File() { }

		public File(string filePath)
		{
			byte[] buff = null;
			byte* ptr = null;

			using (var fs = new FileStream(filePath, FileMode.Open))
			using (var br = new BinaryReader(fs)) {
				mID = br.ReadUInt32();
				mSize = br.ReadUInt32();
				mType = br.ReadUInt32();

				buff = new byte[mSize - 4];
				fs.Read(buff, 0, buff.Length);
				fs.Close();
			}

			fixed (byte* p = &buff[0]) ptr = p;
			var termAddr = (UInt32)ptr + (UInt32)buff.Length;
			ReadChunk(ptr, termAddr);

			int size = 0;
			foreach (var wave in WavePool.List) {
				size += wave.Samples.Length;
			}

			var idx = 0;
			var temp = new byte[size];
			foreach (var wave in WavePool.List) {
				for (int i = 0; i < wave.Samples.Length; ++i) {
					temp[idx] = wave.Samples[i];
					++idx;
				}
			}
		}

		private void ReadChunk(byte* buff, UInt32 termAddr)
		{
			while ((UInt32)buff < termAddr)
			{
				var chunkType = *(CHUNK_TYPE*)buff; buff += 4;
				var chunkSize = *(UInt32*)buff; buff += 4;

				switch (chunkType)
				{
					case CHUNK_TYPE.COLH:
						mInstCount = *(UInt32*)buff;
						break;
					case CHUNK_TYPE.VERS:
						mVersion = new byte[chunkSize];
						Marshal.Copy((IntPtr)buff, mVersion, 0, mVersion.Length);
						break;
					case CHUNK_TYPE.MSYN:
						mMSYN = *(UInt32*)buff;
						break;
					case CHUNK_TYPE.PTBL:
						WaveOffsetPool = new PTBL(buff);
						break;
					case CHUNK_TYPE.LIST:
						ReadLIST(buff, chunkSize);
						break;
					default:
						throw new Exception();
				}

				buff += chunkSize;
			}
		}

		private void ReadLIST(byte* buff, UInt32 chunkSize)
		{
			var listType = *(LIST_TYPE*)buff; buff += 4;
			var termAddr = (UInt32)buff + chunkSize - 4;

			switch (listType)
			{
				case LIST_TYPE.LINS:
					InstPool = new LINS(buff, termAddr);
					break;
				case LIST_TYPE.WVPL:
					WavePool = new WVPL(buff, termAddr);
					break;
				case LIST_TYPE.INFO:
					Info = new INFO(buff, termAddr);
					break;
				default:
					throw new Exception();
			}
		}
	}
}
