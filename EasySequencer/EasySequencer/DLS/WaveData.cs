using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;

namespace DLS
{
	unsafe public class PTBL : LIST<UInt32>
	{
		private UInt32 mChunkSize;
		private UInt32 mListCount;

		public PTBL(byte* buff)
		{
			mChunkSize = *(UInt32*)buff; buff += 4;
			mListCount = *(UInt32*)buff; buff += 4;
			for (var i = 0; i < mListCount; ++i)
			{
				Add(*(UInt32*)buff);
				buff += 4;
			}
		}
	}

	unsafe public class WVPL : LIST<WAVE>
	{
		public WVPL(byte* buff, UInt32 termAddr)
		{
			while ((UInt32)buff < termAddr)
			{
				var chunkType = *(CHUNK_TYPE*)buff; buff += 4;
				var chunkSize = *(UInt32*)buff; buff += 4;

				switch (chunkType)
				{
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
				case LIST_TYPE.WAVE:
					Add(new WAVE(buff, termAddr));
					break;
				default:
					throw new Exception();
			}
		}

		public Dictionary<uint, string> UseWaveList(LINS instPool)
		{
			var list = new Dictionary<uint, string>();
			foreach(var inst in instPool.List)
			{
				foreach(var region in inst.RegionPool.List)
				{
					var idx = region.WaveLink.WaveIndex;
					if (!list.ContainsKey(idx))
					{
						list.Add(idx, this[(int)idx].Info.Name);
					}
				}
			}
			return list;
		}
	}

	unsafe public class WAVE
	{
		public FMT_ Format;
		public WSMP Sampler;
		public INFO Info;
		public byte[] Samples;

		public WAVE(byte* buff, UInt32 termAddr)
		{
			while ((UInt32)buff < termAddr)
			{
				var chunkType = *(CHUNK_TYPE*)buff; buff += 4;
				var chunkSize = *(UInt32*)buff; buff += 4;

				switch (chunkType)
				{
					case CHUNK_TYPE.FMT_:
						Format = new FMT_(buff);
						break;
					case CHUNK_TYPE.WSMP:
						Sampler = new WSMP(buff);
						break;
					case CHUNK_TYPE.DATA:
						Samples = new byte[chunkSize];
						Marshal.Copy((IntPtr)buff, Samples, 0, Samples.Length);
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
				case LIST_TYPE.INFO:
					Info = new INFO(buff, termAddr);
					break;
				default:
					throw new Exception();
			}
		}

		public void ToFile(string filePath)
		{
			FileStream fs = new FileStream(Path.Combine(filePath, Info.Name + ".wav"), FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);

			var msr = new MemoryStream(Samples);
			var bmr = new BinaryReader(msr);
			var msw = new MemoryStream();
			var bmw = new BinaryWriter(msw);

			if (0 < Sampler.List.Count)
			{
				for (var i = 0; i < Sampler[0].Begin; ++i)
				{
					bmw.Write(bmr.ReadInt16());
				}
				for (int j = 0; j < 100; ++j)
				{
					msr.Seek(2 * Sampler[0].Begin, SeekOrigin.Begin);
					for (var i = 0; i < Sampler[0].Length; ++i)
					{
						bmw.Write(bmr.ReadInt16());
					}
				}
				while (msr.Position < msr.Length)
				{
					bmw.Write(bmr.ReadInt16());
				}
			}
			else
			{
				while (msr.Position < msr.Length)
				{
					bmw.Write(bmr.ReadInt16());
				}
			}

			// RIFF
			bw.Write((UInt32)RIFF_CONST.ID);
			bw.Write((UInt32)0);
			bw.Write((UInt32)RIFF_CONST.TYPE_WAVE);

			// fmt
			bw.Write((UInt32)CHUNK_TYPE.FMT_);
			bw.Write((UInt32)16);
			bw.Write(Format.FormatTag);
			bw.Write(Format.Channels);
			bw.Write(Format.SamplesPerSec);
			bw.Write(Format.AvgBytesPerSec);
			bw.Write(Format.BlockAlign);
			bw.Write(Format.BitsPerSample);

			// data
			bw.Write((UInt32)CHUNK_TYPE.DATA);
			bw.Write((UInt32)msw.Length);
			bw.Write(msw.ToArray());

			fs.Seek(4, SeekOrigin.Begin);
			bw.Write((UInt32)(fs.Length - 8));

			fs.Close();
		}
	}

	unsafe public struct FMT_
	{
		public UInt16 FormatTag;
		public UInt16 Channels;
		public UInt32 SamplesPerSec;
		public UInt32 AvgBytesPerSec;
		public UInt16 BlockAlign;
		public UInt16 BitsPerSample;
		public UInt16 ExSize;

		public FMT_(byte* pBuff)
		{
			FormatTag		= *(UInt16*)pBuff; pBuff += 2;
			Channels		= *(UInt16*)pBuff; pBuff += 2;
			SamplesPerSec	= *(UInt32*)pBuff; pBuff += 4;
			AvgBytesPerSec	= *(UInt32*)pBuff; pBuff += 4;
			BlockAlign		= *(UInt16*)pBuff; pBuff += 2;
			BitsPerSample	= *(UInt16*)pBuff; pBuff += 2;
			ExSize			= *(UInt16*)pBuff;
		}

		public byte[] Bytes
		{
			get
			{
				var buff = new byte[18];
				byte* pBuff;
				fixed (byte* p = &buff[0]) pBuff = p;

				*(UInt16*)pBuff = FormatTag;		pBuff += 2;
				*(UInt16*)pBuff = Channels;			pBuff += 2;
				*(UInt32*)pBuff = SamplesPerSec;	pBuff += 4;
				*(UInt32*)pBuff = AvgBytesPerSec;	pBuff += 4;
				*(UInt16*)pBuff = BlockAlign;		pBuff += 2;
				*(UInt16*)pBuff = BitsPerSample;	pBuff += 2;
				*(UInt16*)pBuff = ExSize;

				return buff;
			}
		}
	}
}
