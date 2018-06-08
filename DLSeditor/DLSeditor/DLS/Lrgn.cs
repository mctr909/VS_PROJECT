using System;

namespace DLS
{
	unsafe public struct Range
	{
		public UInt16 Low;
		public UInt16 High;
	}

	unsafe public struct RGNH
	{
		public Range RangeKey;
		public Range RangeVelocity;
		public UInt16 Options;
		public UInt16 KeyGroup;
		public UInt16 Layer;

		public RGNH(byte* pre, UInt32 size)
		{
			RangeKey.Low = *(UInt16*)pre;
			pre += 2;
			RangeKey.High = *(UInt16*)pre;
			pre += 2;

			RangeVelocity.Low = *(UInt16*)pre;
			pre += 2;
			RangeVelocity.High = *(UInt16*)pre;
			pre += 2;

			Options = *(UInt16*)pre;
			pre += 2;
			KeyGroup = *(UInt16*)pre;
			pre += 2;

			if (14 <= size) {
				Layer = *(UInt16*)pre;
			}
			else {
				Layer = 0;
			}
		}
	}

	unsafe public struct Loop
	{
		public UInt32 Size;
		public UInt32 Type;
		public UInt32 Start;
		public UInt32 Length;

		public Loop(byte* ptr)
		{
			Size = *(UInt32*)ptr;
			ptr += 4;
			Type = *(UInt32*)ptr;
			ptr += 4;
			Start = *(UInt32*)ptr;
			ptr += 4;
			Length = *(UInt32*)ptr;
		}
	}

	unsafe public struct WSMP
	{
		public UInt32 Size;
		public UInt16 UnityNoote;
		public Int16 FineTune;
		private Int32 GainInt;
		public UInt32 Options;
		public UInt32 LoopCount;
		public Loop[] Loops;

		public WSMP(byte* ptr)
		{
			Size = *(UInt32*)ptr;
			ptr += 4;
			UnityNoote = *(UInt16*)ptr;
			ptr += 2;
			FineTune = *(Int16*)ptr;
			ptr += 2;
			GainInt = *(Int32*)ptr;
			ptr += 4;
			Options = *(UInt32*)ptr;
			ptr += 4;
			LoopCount = *(UInt32*)ptr;
			ptr += 4;

			Loops = new Loop[LoopCount];
			for (var i = 0; i < LoopCount; ++i) {
				Loops[i] = new Loop(ptr);
				ptr += sizeof(Loop);
			}
		}

		public double Gain
		{
			get {
				return Math.Pow(10.0, GainInt / (200 * 65536.0));
			}
			set {
				GainInt = (Int32)(Math.Log10(value) * 200 * 65536);
			}
		}
	}

	unsafe public struct WLNK
	{
		public UInt16 Options;
		public UInt16 PhaseGroup;
		public UInt32 Channel;
		public UInt32 WaveIndex;

		public WLNK(byte* ptr)
		{
			Options = *(UInt16*)ptr;
			ptr += 2;
			PhaseGroup = *(UInt16*)ptr;
			ptr += 2;
			Channel = *(UInt32*)ptr;
			ptr += 4;
			WaveIndex = *(UInt32*)ptr;
		}
	}

	unsafe public class RGN
	{
		public RGNH RegionHeader;
		public WSMP Sampler;
		public WLNK WaveLink;
		public LART Articulations;

		public RGN(byte* ptr, UInt32 endAddr)
		{
			while ((UInt32)ptr < endAddr) {
				var chunkType = *(ChunkID*)ptr;
				ptr += 4;
				var chunkSize = *(UInt32*)ptr;
				ptr += 4;

				switch (chunkType) {
				case ChunkID.RGNH:
					RegionHeader = new RGNH(ptr, chunkSize);
					break;
				case ChunkID.WSMP:
					Sampler = new WSMP(ptr);
					break;
				case ChunkID.WLNK:
					WaveLink = new WLNK(ptr);
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
			case ListID.LART:
			case ListID.LAR2:
				Articulations = new LART(ptr, endAddr);
				break;
			default:
				throw new Exception();
			}
		}
	}

	unsafe public class LRGN : List<RGN>
	{
		public LRGN() {}

		public LRGN(byte* ptr, UInt32 endAddr)
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
			case ListID.RGN_:
				Add(new RGN(ptr, endAddr));
				break;
			default:
				throw new Exception();
			}
		}
	}
}
