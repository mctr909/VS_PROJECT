using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;

namespace DLS
{
	unsafe public struct SFMT_
	{
		public UInt16 FormatTag;
		public UInt16 Channels;
		public UInt32 SamplesPerSec;
		public UInt32 AvgBytesPerSec;
		public UInt16 BlockAlign;
		public UInt16 BitsPerSample;
		public UInt16 ExSize;

		public SFMT_(byte* pBuff)
		{
			FormatTag = *(UInt16*)pBuff;
			pBuff += 2;
			Channels = *(UInt16*)pBuff;
			pBuff += 2;
			SamplesPerSec = *(UInt32*)pBuff;
			pBuff += 4;
			AvgBytesPerSec = *(UInt32*)pBuff;
			pBuff += 4;
			BlockAlign = *(UInt16*)pBuff;
			pBuff += 2;
			BitsPerSample = *(UInt16*)pBuff;
			pBuff += 2;
			ExSize = *(UInt16*)pBuff;
		}

		public byte[] Bytes
		{
			get {
				var buff = new byte[18];
				byte* pBuff;
				fixed (byte* p = &buff[0]) pBuff = p;

				*(UInt16*)pBuff = FormatTag;
				pBuff += 2;
				*(UInt16*)pBuff = Channels;
				pBuff += 2;
				*(UInt32*)pBuff = SamplesPerSec;
				pBuff += 4;
				*(UInt32*)pBuff = AvgBytesPerSec;
				pBuff += 4;
				*(UInt16*)pBuff = BlockAlign;
				pBuff += 2;
				*(UInt16*)pBuff = BitsPerSample;
				pBuff += 2;
				*(UInt16*)pBuff = ExSize;

				return buff;
			}
		}
	}

	unsafe public class CWAVE
	{
		public SFMT_ Format;
		public CWSMP Sampler;
		public CINFO Info;
		public byte[] Samples;

		public CWAVE(byte* buff, UInt32 termAddr)
		{
			while ((UInt32)buff < termAddr) {
				var chunkType = *(CHUNK_TYPE*)buff;
				buff += 4;
				var chunkSize = *(UInt32*)buff;
				buff += 4;

				switch (chunkType) {
				case CHUNK_TYPE.FMT_:
					Format = new SFMT_(buff);
					break;
				case CHUNK_TYPE.WSMP:
					Sampler = new CWSMP(buff);
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
			var listType = *(LIST_TYPE*)buff;
			buff += 4;
			var termAddr = (UInt32)buff + chunkSize - 4;

			switch (listType) {
			case LIST_TYPE.INFO:
				Info = new CINFO(buff, termAddr);
				break;
			default:
				throw new Exception();
			}
		}

		public Byte[] Bytes
		{
			get {
				using (var ms = new MemoryStream())
				using (var bw = new BinaryWriter(ms)) {
					var format = Format.Bytes;
					var sampler = Sampler.Bytes;
					var info = Info.Bytes;

					bw.Write((UInt32)CHUNK_TYPE.FMT_);
					bw.Write((UInt32)format.Length);
					bw.Write(format);

					bw.Write((UInt32)CHUNK_TYPE.WSMP);
					bw.Write((UInt32)sampler.Length);
					bw.Write(sampler);

					bw.Write((UInt32)CHUNK_TYPE.DATA);
					bw.Write((UInt32)Samples.Length);
					bw.Write(Samples);

					bw.Write((UInt32)CHUNK_TYPE.LIST);
					bw.Write((UInt32)info.Length + 4);
					bw.Write((UInt32)LIST_TYPE.INFO);
					bw.Write(info);

					return ms.ToArray();
				}
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

			if (0 < Sampler.List.Count) {
				for (var i = 0; i < Sampler[0].Begin; ++i) {
					bmw.Write(bmr.ReadInt16());
				}
				for (int j = 0; j < 100; ++j) {
					msr.Seek(2 * Sampler[0].Begin, SeekOrigin.Begin);
					for (var i = 0; i < Sampler[0].Length; ++i) {
						bmw.Write(bmr.ReadInt16());
					}
				}
				while (msr.Position < msr.Length) {
					bmw.Write(bmr.ReadInt16());
				}
			}
			else {
				while (msr.Position < msr.Length) {
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

	unsafe public class CWVPL : LIST<CWAVE>
	{
		public CWVPL(byte* buff, UInt32 termAddr)
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
					Add(new CWAVE(buff, termAddr));
					break;
				default:
					throw new Exception();
			}
		}

		public byte[] Bytes
		{
			get
			{
				using (var ms = new MemoryStream())
				using (var bw = new BinaryWriter(ms))
				{
					foreach (var wav in List)
					{
						var wavb = wav.Bytes;
						bw.Write((UInt32)CHUNK_TYPE.LIST);
						bw.Write((UInt32)wavb.Length + 4);
						bw.Write((UInt32)LIST_TYPE.WAVE);
						bw.Write(wavb);
					}
					return ms.ToArray();
				}
			}
		}

		public Dictionary<uint, string> UseWaveList(CLINS instPool)
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

	unsafe public class CPTBL : LIST<UInt32>
	{
		private UInt32 mChunkSize;
		private UInt32 mListCount;

		public CPTBL(byte* buff)
		{
			mChunkSize = *(UInt32*)buff;
			buff += 4;
			mListCount = *(UInt32*)buff;
			buff += 4;
			for (var i = 0; i < mListCount; ++i) {
				Add(*(UInt32*)buff);
				buff += 4;
			}
		}

		public CPTBL(CWVPL wavePool)
		{
			mChunkSize = 8;
			mListCount = (UInt32)wavePool.List.Count;

			UInt32 pos = 0;
			foreach (var wav in wavePool.List) {
				Add(pos);
				pos += (UInt32)wav.Bytes.Length + 12;
			}
		}

		public byte[] Bytes
		{
			get {
				using (var ms = new MemoryStream())
				using (var bw = new BinaryWriter(ms)) {
					bw.Write(mChunkSize);
					bw.Write(mListCount);
					foreach (var off in List) {
						bw.Write(off);
					}
					return ms.ToArray();
				}
			}
		}
	}
}
