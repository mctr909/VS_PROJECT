using System;
using System.IO;

namespace DLS
{
	unsafe public struct SINSH
	{
		private UInt32 mRegions;
		private UInt32 mBank;
		private UInt32 mInstrument;

		public SINSH(byte* buff)
		{
			mRegions = *(UInt32*)buff;
			buff += 4;
			mBank = *(UInt32*)buff;
			buff += 4;
			mInstrument = *(UInt32*)buff;
		}

		#region プロパティ
		public bool IsDrum
		{
			get {
				return 0 < (mBank & 0x80000000);
			}
			set {
				mBank |= value ? 0x80000000 : 0;
			}
		}

		public byte ProgramNo
		{
			get {
				return (byte)mInstrument;
			}
			set {
				mInstrument = value;
			}
		}

		public byte BankMSB
		{
			get {
				return (byte)((mBank & 0x00007F00) >> 8);
			}
			set {
				mBank |= (UInt32)(value & 0x7F) << 8;
			}
		}

		public byte BankLSB
		{
			get {
				return (byte)(mBank & 0x0000007F);
			}
			set {
				mBank |= (UInt32)(value & 0x7F);
			}
		}

		public byte[] Bytes
		{
			get {
				var buff = new byte[12];
				byte* pBuff;
				fixed (byte* p = &buff[0]) pBuff = p;

				*(UInt32*)pBuff = mRegions;
				pBuff += 4;
				*(UInt32*)pBuff = mBank;
				pBuff += 4;
				*(UInt32*)pBuff = mInstrument;

				return buff;
			}
		}
		#endregion
	}

	unsafe public class CINS_
	{
		public SINSH InstHeader;
		public CLRGN RegionPool;
		public CLART ArtPool;
		public CINFO Info;

		public CINS_()
		{
			InstHeader = new SINSH();
			RegionPool = new CLRGN();
			ArtPool = new CLART();
			Info = new CINFO();
		}

		public CINS_(byte* buff, UInt32 termAddr)
		{
			InstHeader = new SINSH();
			RegionPool = new CLRGN();
			ArtPool = new CLART();
			Info = new CINFO();

			while ((UInt32)buff < termAddr) {
				var chunkType = *(CHUNK_TYPE*)buff;
				buff += 4;
				var chunkSize = *(UInt32*)buff;
				buff += 4;

				switch (chunkType) {
				case CHUNK_TYPE.INSH:
					InstHeader = new SINSH(buff);
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
			case LIST_TYPE.LRGN:
				RegionPool = new CLRGN(buff, termAddr);
				break;
			case LIST_TYPE.LART:
				ArtPool = new CLART(buff, termAddr);
				break;
			case LIST_TYPE.INFO:
				Info = new CINFO(buff, termAddr);
				break;
			default:
				throw new Exception();
			}
		}

		public byte[] Bytes
		{
			get {
				using (var ms = new MemoryStream())
				using (var bw = new BinaryWriter(ms)) {
					var hd = InstHeader.Bytes;
					var rgn = RegionPool.Bytes;
					var art = ArtPool.Bytes;
					var info = Info.Bytes;

					bw.Write((UInt32)CHUNK_TYPE.INSH);
					bw.Write((UInt32)hd.Length);
					bw.Write(hd);

					bw.Write((UInt32)CHUNK_TYPE.LIST);
					bw.Write((UInt32)rgn.Length + 4);
					bw.Write((UInt32)LIST_TYPE.LRGN);
					bw.Write(rgn);

					if (0 < art.Length && 0 < ArtPool.Art.List.Count) {
						bw.Write((UInt32)CHUNK_TYPE.LIST);
						bw.Write((UInt32)art.Length + 4);
						bw.Write((UInt32)LIST_TYPE.LART);
						bw.Write(art);
					}

					bw.Write((UInt32)CHUNK_TYPE.LIST);
					bw.Write((UInt32)info.Length + 4);
					bw.Write((UInt32)LIST_TYPE.INFO);
					bw.Write(info);

					return ms.ToArray();
				}
			}
		}
	}

	unsafe public class CLINS : LIST<CINS_>
	{
		public CLINS() { }

		public CLINS(byte* buff, UInt32 termAddr)
		{
			while ((UInt32)buff < termAddr) {
				var chunkType = *(CHUNK_TYPE*)buff;
				buff += 4;
				var chunkSize = *(UInt32*)buff;
				buff += 4;

				switch (chunkType) {
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
			case LIST_TYPE.INS_:
				Add(new CINS_(buff, termAddr));
				break;
			default:
				throw new Exception();
			}

		}

		public byte[] Bytes
		{
			get {
				using (var ms = new MemoryStream())
				using (var bw = new BinaryWriter(ms)) {
					foreach (var ins in List) {
						var insb = ins.Bytes;
						bw.Write((UInt32)CHUNK_TYPE.LIST);
						bw.Write((UInt32)insb.Length + 4);
						bw.Write((UInt32)LIST_TYPE.INS_);
						bw.Write(insb);
					}
					return ms.ToArray();
				}
			}
		}
	}
}