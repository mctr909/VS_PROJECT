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

		public File()
		{
			mID		= (UInt32)RIFF_CONST.ID;
			mSize	= 0;
			mType	= (UInt32)RIFF_CONST.TYPE_WAVE;

			mInstCount	= 0;
			mVersion	= new byte[8];
			mMSYN		= 1;

			WaveOffsetPool = null;
			InstPool = new LINS();
			WavePool = null;
			Info = new INFO();
		}

		public File(string filePath)
		{
			byte[] buff = null;
			byte* ptr = null;

			using (var fs = new FileStream(filePath, FileMode.Open))
			using (var br = new BinaryReader(fs))
			{
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
		}

		public File(string filePath, LINS instPool, WVPL wavePool, INFO info)
		{
			InstPool = instPool;
			WavePool = wavePool;
			Info = info;

			mInstCount = (UInt32)instPool.List.Count;
			mVersion = new byte[8];
			mMSYN = 1;

			using (var fs = new FileStream(filePath, FileMode.Create))
			using (var bw = new BinaryWriter(fs))
			{
				var ins = InstPool.Bytes;
				var wav = WavePool.Bytes;
				var off = new PTBL(WavePool).Bytes;
				var inf = Info.Bytes;

				var ls = WavePool.UseWaveList(InstPool);

				bw.Write((UInt32)RIFF_CONST.ID);
				bw.Write((UInt32)0);
				bw.Write((UInt32)RIFF_CONST.TYPE_DLS_);

				bw.Write((UInt32)CHUNK_TYPE.COLH);
				bw.Write((UInt32)sizeof(UInt32));
				bw.Write(mInstCount);

				bw.Write((UInt32)CHUNK_TYPE.VERS);
				bw.Write((UInt32)mVersion.Length);
				bw.Write(mVersion);

				bw.Write((UInt32)CHUNK_TYPE.MSYN);
				bw.Write((UInt32)sizeof(UInt32));
				bw.Write(mMSYN);

				bw.Write((UInt32)CHUNK_TYPE.LIST);
				bw.Write((UInt32)ins.Length + 4);
				bw.Write((UInt32)LIST_TYPE.LINS);
				bw.Write(ins);

				bw.Write((UInt32)CHUNK_TYPE.PTBL);
				bw.Write((UInt32)off.Length);
				bw.Write(off);

				bw.Write((UInt32)CHUNK_TYPE.LIST);
				bw.Write((UInt32)wav.Length + 4);
				bw.Write((UInt32)LIST_TYPE.WVPL);
				bw.Write(wav);

				bw.Write((UInt32)CHUNK_TYPE.LIST);
				bw.Write((UInt32)inf.Length + 4);
				bw.Write((UInt32)LIST_TYPE.INFO);
				bw.Write(inf);

				fs.Seek(4, SeekOrigin.Begin);
				bw.Write((UInt32)fs.Length - 8);
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
