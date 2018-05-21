using System;

namespace DLS
{
	unsafe public class LINS : LIST<INS_>
	{
		private LINS() { }

		public LINS(byte* buff, UInt32 termAddr)
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
				case LIST_TYPE.INS_:
					Add(new INS_(buff, termAddr));
					break;
				default:
					throw new Exception();
			}

		}
	}

	unsafe public class INS_
	{
		public INSH InstHeader;
		public LRGN RegionPool;
		public LART ArtPool;
		public INFO Info;

		private INS_() { }

		public INS_(byte* buff, UInt32 termAddr)
		{
			InstHeader = new INSH();
			RegionPool = null;
			ArtPool = null;
			Info = null;

			while ((UInt32)buff < termAddr)
			{
				var chunkType = *(CHUNK_TYPE*)buff; buff += 4;
				var chunkSize = *(UInt32*)buff; buff += 4;

				switch (chunkType)
				{
					case CHUNK_TYPE.INSH:
						InstHeader = new INSH(buff);
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
				case LIST_TYPE.LRGN:
					RegionPool = new LRGN(buff, termAddr);
					break;
				case LIST_TYPE.LART:
					ArtPool = new LART(buff, termAddr);
					break;
				case LIST_TYPE.INFO:
					Info = new INFO(buff, termAddr);
					break;
				default:
					throw new Exception();
			}
		}
	}

	unsafe public struct INSH
	{
		private UInt32 mRegions;
		private UInt32 mBank;
		private UInt32 mInstrument;

		public INSH(byte* buff)
		{
			mRegions	= *(UInt32*)buff; buff += 4;
			mBank		= *(UInt32*)buff; buff += 4;
			mInstrument	= *(UInt32*)buff;
		}

		#region プロパティ
		public bool IsDrum
		{
			get { return 0 < (mBank & 0x80000000); }
			set { mBank |= value ? 0x80000000 : 0; }
		}

		public byte ProgramNo
		{
			get { return (byte)mInstrument; }
			set { mInstrument = value; }
		}

		public byte BankMSB
		{
			get { return (byte)((mBank & 0x00007F00) >> 8); }
			set { mBank |= (UInt32)(value & 0x7F) << 8; }
		}

		public byte BankLSB
		{
			get { return (byte)(mBank & 0x0000007F); }
			set { mBank |= (UInt32)(value & 0x7F); }
		}
		#endregion
	}
}
