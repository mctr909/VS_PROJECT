﻿using System;
using System.IO;

namespace DLS
{
	unsafe public class LRGN : LIST<RGN_>
	{
		public LRGN() { }

		public LRGN(byte* buff, UInt32 termAddr)
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
				case LIST_TYPE.RGN_:
				case LIST_TYPE.RGN2:
					Add(new RGN_(buff, termAddr));
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
					foreach(var rgn in List)
					{
						var rgnb = rgn.Bytes;
						bw.Write((UInt32)CHUNK_TYPE.LIST);
						bw.Write((UInt32)rgnb.Length + 4);
						bw.Write((UInt32)LIST_TYPE.RGN_);
						bw.Write(rgnb);
					}
					return ms.ToArray();
				}
			}
		}
	}

	unsafe public struct RGN_
	{
		public RGNH RegionHeader;
		public WLNK WaveLink;
		public WSMP Sampler;
		public LART ArtPool;

		public RGN_(byte* buff, UInt32 termAddr)
		{
			RegionHeader = new RGNH();
			WaveLink = new WLNK();
			Sampler = new WSMP();
			ArtPool = new LART();

			while ((UInt32)buff < termAddr)
			{
				var chunkType = *(CHUNK_TYPE*)buff; buff += 4;
				var chunkSize = *(UInt32*)buff; buff += 4;

				switch (chunkType)
				{
					case CHUNK_TYPE.RGNH:
						RegionHeader = new RGNH(buff, chunkSize);
						break;
					case CHUNK_TYPE.WSMP:
						Sampler = new WSMP(buff);
						break;
					case CHUNK_TYPE.WLNK:
						WaveLink = new WLNK(buff);
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
				case LIST_TYPE.LART:
				case LIST_TYPE.LAR2:
					ArtPool = new LART(buff, termAddr);
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
					var hd = RegionHeader.Bytes;
					var smp = Sampler.Bytes;
					var lnk = WaveLink.Bytes;
					var art = ArtPool.Bytes;

					bw.Write((UInt32)CHUNK_TYPE.RGNH);
					bw.Write((UInt32)hd.Length);
					bw.Write(hd);

					bw.Write((UInt32)CHUNK_TYPE.WSMP);
					bw.Write((UInt32)smp.Length);
					bw.Write(smp);

					bw.Write((UInt32)CHUNK_TYPE.WLNK);
					bw.Write((UInt32)lnk.Length);
					bw.Write(lnk);

					if (0 < art.Length && 0 < ArtPool.Art.List.Count)
					{
						bw.Write((UInt32)CHUNK_TYPE.LIST);
						bw.Write((UInt32)art.Length + 4);
						bw.Write((UInt32)LIST_TYPE.LART);
						bw.Write(art);
					}

					return ms.ToArray();
				}
			}
		}
	}

	unsafe public struct RGNH
	{
		public UInt16 KeyRangeLow;
		public UInt16 KeyRangeHigh;
		public UInt16 VelocityRangeLow;
		public UInt16 VelocityRangeHigh;
		public UInt16 Options;
		public UInt16 KeyGroup;
		public UInt16 Layer;

		public RGNH(byte* buff, UInt32 size)
		{
			KeyRangeLow       = *(UInt16*)buff; buff += 2;
			KeyRangeHigh      = *(UInt16*)buff; buff += 2;
			VelocityRangeLow  = *(UInt16*)buff; buff += 2;
			VelocityRangeHigh = *(UInt16*)buff; buff += 2;
			Options           = *(UInt16*)buff; buff += 2;
			KeyGroup          = *(UInt16*)buff; buff += 2;

			if (14 <= size)
			{
				Layer = *(UInt16*)buff;
			}
			else
			{
				Layer = 0;
			}
		}

		public byte[] Bytes
		{
			get
			{
				var buff = new byte[12];
				byte* pBuff;
				fixed (byte* p = &buff[0]) pBuff = p;

				*(UInt16*)pBuff = KeyRangeLow;			pBuff += 2;
				*(UInt16*)pBuff = KeyRangeHigh;			pBuff += 2;
				*(UInt16*)pBuff = VelocityRangeLow;		pBuff += 2;
				*(UInt16*)pBuff = VelocityRangeHigh;	pBuff += 2;
				*(UInt16*)pBuff = Options;				pBuff += 2;
				*(UInt16*)pBuff = KeyGroup;

				return buff;
			}
		}
	}

	unsafe public struct WLNK
	{
		public UInt16 Options;
		public UInt16 PhaseGroup;
		public UInt32 Channel;
		public UInt32 WaveIndex;

		public WLNK(byte* buff)
		{
			Options    = *(UInt16*)buff; buff += 2;
			PhaseGroup = *(UInt16*)buff; buff += 2;
			Channel    = *(UInt32*)buff; buff += 4;
			WaveIndex  = *(UInt32*)buff;
		}

		public byte[] Bytes
		{
			get
			{
				var buff = new byte[12];
				byte* pBuff;
				fixed (byte* p = &buff[0]) pBuff = p;

				*(UInt16*)pBuff = Options;		pBuff += 2;
				*(UInt16*)pBuff = PhaseGroup;	pBuff += 2;
				*(UInt32*)pBuff = Channel;		pBuff += 4;
				*(UInt32*)pBuff = WaveIndex;

				return buff;
			}
		}
	}
}
