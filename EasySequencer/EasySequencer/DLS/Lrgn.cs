using System;

namespace DLS
{
	unsafe public class LRGN : LIST<RGN_>
	{
		private LRGN() { }

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
			ArtPool = null;

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

			if (14 <= size) {
				Layer = *(UInt16*)buff;
			}
			else {
				Layer = 0;
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
	}
}
