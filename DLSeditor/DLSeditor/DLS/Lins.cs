using System;

namespace DLS
{
	unsafe public struct INSH
	{
		public UInt32 Regions;
		public Byte BankMSB;
		public Byte BankLSB;
		public UInt16 Flags;
		public UInt32 ProgramNo;

		public INSH(byte* ptr)
		{
			Regions = *(UInt32*)ptr;
			ptr += 4;
			BankMSB = *ptr;
			ptr += 1;
			BankLSB = *ptr;
			ptr += 1;
			Flags = *(UInt16*)ptr;
			ptr += 2;
			ProgramNo = *(UInt32*)ptr;
		}
	}

	unsafe public class INS
	{
		public INSH InstHeader;
		public LRGN Regions;
		public LART Articulations;
		public INFO Info;

		public INS()
		{
			Regions = new LRGN();
			Articulations = new LART();
			Info = new INFO();
		}

		public INS(byte* ptr, UInt32 endAddr)
		{
			while ((UInt32)ptr < endAddr) {
				var chunkType = *(ChunkID*)ptr;
				ptr += 4;
				var chunkSize = *(UInt32*)ptr;
				ptr += 4;

				switch (chunkType) {
				case ChunkID.INSH:
					InstHeader = new INSH(ptr);
					break;
				case ChunkID.LIST:
					ReadList(ptr, chunkSize);
					break;
				default:
					throw new Exception();
				}

				ptr += chunkSize;
			}
		}

		private void ReadList(byte* ptr, UInt32 size)
		{
			var listType = *(ListID*)ptr;
			var endAddr = (UInt32)ptr + size;
			ptr += 4;

			switch (listType) {
			case ListID.LRGN:
				Regions = new LRGN(ptr, endAddr);
				break;
			case ListID.LART:
			case ListID.LAR2:
				Articulations = new LART(ptr, endAddr);
				break;
			case ListID.INFO:
				Info = new INFO(ptr, endAddr);
				break;
			default:
				throw new Exception();
			}
		}
	}

	unsafe public class LINS : List<INS>
	{
		public LINS() {}

		public LINS(byte* ptr, UInt32 endAddr)
		{
			while ((UInt32)ptr < endAddr) {
				var chunkType = *(ChunkID*)ptr;
				ptr += 4;
				var chunkSize = *(UInt32*)ptr;
				ptr += 4;

				switch (chunkType) {
				case ChunkID.LIST:
					ReadList(ptr, chunkSize);
					break;
				default:
					throw new Exception();
				}

				ptr += chunkSize;
			}
		}

		private void ReadList(byte* ptr, UInt32 size)
		{
			var listType = *(ListID*)ptr;
			var endAddr = (UInt32)ptr + size;
			ptr += 4;

			switch (listType) {
			case ListID.INS_:
				Add(new INS(ptr, endAddr));
				break;
			default:
				throw new Exception();
			}
		}
	}
}
