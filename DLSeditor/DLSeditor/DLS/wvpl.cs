using System;
using System.Runtime.InteropServices;
using System.IO;

namespace DLS
{
	unsafe public struct FMT_
	{
		public UInt16 FormatTag;
		public UInt16 Channels;
		public UInt32 SampleRate;
		public UInt32 BytesPerSec;
		public UInt16 BlockAlign;
		public UInt16 BitsPerSample;

		public FMT_(byte* ptr)
		{
			FormatTag = *(UInt16*)ptr;
			ptr += 2;
			Channels = *(UInt16*)ptr;
			ptr += 2;
			SampleRate = *(UInt32*)ptr;
			ptr += 4;
			BytesPerSec = *(UInt32*)ptr;
			ptr += 4;
			BlockAlign = *(UInt16*)ptr;
			ptr += 2;
			BitsPerSample = *(UInt16*)ptr;
		}
	}

	unsafe public class WAVE
	{
		public FMT_ Format;
		public WSMP Samplers;
		public byte[] Data;
		public INFO Info;

		public WAVE(byte* ptr, UInt32 endAddr)
		{
			while ((UInt32)ptr < endAddr) {
				var chunkType = *(ChunkID*)ptr;
				ptr += 4;
				var chunkSize = *(UInt32*)ptr;
				ptr += 4;

				switch (chunkType) {
				case ChunkID.FMT_:
					Format = new FMT_(ptr);
					break;
				case ChunkID.WSMP:
					Samplers = new WSMP(ptr);
					break;
				case ChunkID.DATA:
					Data = new byte[chunkSize];
					Marshal.Copy((IntPtr)ptr, Data, 0, Data.Length);
					break;
				case ChunkID.LIST:
					ReadList(ptr, chunkSize);
					break;
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
			case ListID.INFO:
				Info = new INFO(ptr, endAddr);
				break;
			}
		}

		public void ToFile(string filePath)
		{
			if (16 != Format.BitsPerSample) {
				return;
			}

			FileStream fs = new FileStream(filePath, FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);

			var msr = new MemoryStream(Data);
			var bmr = new BinaryReader(msr);
			var msw = new MemoryStream();
			var bmw = new BinaryWriter(msw);

			if (0 < Samplers.Loops.Length) {
				for (var i = 0; i < Samplers.Loops[0].Start; ++i) {
					bmw.Write(bmr.ReadInt16());
				}
				for (int j = 0; j < 100; ++j) {
					msr.Seek(2 * Samplers.Loops[0].Start, SeekOrigin.Begin);
					for (var i = 0; i < Samplers.Loops[0].Length; ++i) {
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
			bw.Write((UInt32)ChunkID.FMT_);
			bw.Write((UInt32)16);
			bw.Write(Format.FormatTag);
			bw.Write(Format.Channels);
			bw.Write(Format.SampleRate);
			bw.Write(Format.BytesPerSec);
			bw.Write(Format.BlockAlign);
			bw.Write(Format.BitsPerSample);


			// data
			bw.Write((UInt32)ChunkID.DATA);
			bw.Write((UInt32)msw.Length);
			bw.Write(msw.ToArray());

			fs.Seek(4, SeekOrigin.Begin);
			bw.Write((UInt32)(fs.Length - 8));

			fs.Close();
		}
	}

	unsafe public class WVPL : List<WAVE>
	{
		public WVPL() {}

		public WVPL(byte* ptr, UInt32 endAddr)
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
			case ListID.WAVE:
				Add(new WAVE(ptr, endAddr));
				break;
			default:
				throw new Exception();
			}
		}
	}
}
