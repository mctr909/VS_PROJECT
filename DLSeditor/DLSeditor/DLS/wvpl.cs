using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class WVPL : Chunk {
		public Dictionary<int, WAVE> List = new Dictionary<int, WAVE>();

		public WVPL() { }

		public WVPL(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override void LoadList(byte* ptr, UInt32 endAddr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.WAVE:
				List.Add(List.Count, new WAVE(ptr, endAddr));
				break;
			default:
				throw new Exception();
			}
		}
	}

	unsafe public class WAVE : Chunk {
		public CK_DLID DlId;
		public CK_FMT Format;
		public CK_WSMP Sampler;
		public Dictionary<int, WaveLoop> Loops = new Dictionary<int, WaveLoop>();
		public byte[] Data;
		public INFO Text = new INFO();

		public WAVE(string filePath) {
			FileStream fs = new FileStream(filePath, FileMode.Open);
			BinaryReader br = new BinaryReader(fs);

			var riff = br.ReadUInt32();
			var riffSize = br.ReadUInt32();
			var riffType = br.ReadUInt32();

			while (fs.CanRead) {
				var chunkType = (CK_CHUNK.TYPE)br.ReadUInt32();
				var chunkSize = br.ReadUInt32();
				var chunkData = br.ReadBytes((int)chunkSize);
				byte* ptr;
				fixed (byte* p = &chunkData[0]) ptr = p;

				switch (chunkType) {
				case CK_CHUNK.TYPE.FMT_:
					Format.Tag			= *(UInt16*)ptr; ptr += 2;
					Format.Channels		= *(UInt16*)ptr; ptr += 2;
					Format.SampleRate	= *(UInt32*)ptr; ptr += 4;
					Format.BytesPerSec	= *(UInt32*)ptr; ptr += 4;
					Format.BlockAlign	= *(UInt16*)ptr; ptr += 2;
					Format.Bits			= *(UInt16*)ptr;
					break;
				case CK_CHUNK.TYPE.DATA:
					Data = chunkData;
					break;
				}
			}

			br.Dispose();
			fs.Close();
			fs.Dispose();
		}

		public WAVE(byte* ptr, UInt32 endAddr) : base(ptr, endAddr) { }

		protected override unsafe void LoadChunk(Byte* ptr) {
			switch (mChunk.Type) {
			case CK_CHUNK.TYPE.DLID:
			case CK_CHUNK.TYPE.GUID:
				DlId = (CK_DLID)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_DLID));
				break;
			case CK_CHUNK.TYPE.FMT_:
				Format = (CK_FMT)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_FMT));
				break;
			case CK_CHUNK.TYPE.DATA:
				Data = new byte[mChunk.Size];
				Marshal.Copy((IntPtr)ptr, Data, 0, Data.Length);
				break;
			case CK_CHUNK.TYPE.WSMP:
				Sampler = (CK_WSMP)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_WSMP));
				var pLoop = ptr + sizeof(CK_WSMP);
				for (var i = 0; i < Sampler.LoopCount; ++i) {
					Loops.Add(Loops.Count, (WaveLoop)Marshal.PtrToStructure((IntPtr)pLoop, typeof(WaveLoop)));
					pLoop += sizeof(WaveLoop);
				}
				break;
			}
		}

		protected override unsafe void LoadList(byte* ptr, UInt32 endAddr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.INFO:
				Text = new INFO(ptr, endAddr);
				break;
			}
		}

		public void ToFile(string filePath) {
			if (16 != Format.Bits) {
				return;
			}

			FileStream fs = new FileStream(filePath, FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);

			var msr = new MemoryStream(Data);
			var bmr = new BinaryReader(msr);
			var msw = new MemoryStream();
			var bmw = new BinaryWriter(msw);

			if (0 < Sampler.LoopCount) {
				for (var i = 0; i < Loops[0].Start; ++i) {
					bmw.Write(bmr.ReadInt16());
				}
				for (int j = 0; j < 100; ++j) {
					msr.Seek(2 * Loops[0].Start, SeekOrigin.Begin);
					for (var i = 0; i < Loops[0].Length; ++i) {
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
			bw.Write((UInt32)0x46464952);
			bw.Write((UInt32)0);
			bw.Write((UInt32)0x45564157);

			// fmt
			bw.Write((UInt32)CK_CHUNK.TYPE.FMT_);
			bw.Write((UInt32)16);
			bw.Write(Format.Tag);
			bw.Write(Format.Channels);
			bw.Write(Format.SampleRate);
			bw.Write(Format.BytesPerSec);
			bw.Write(Format.BlockAlign);
			bw.Write(Format.Bits);

			// data
			bw.Write((UInt32)CK_CHUNK.TYPE.DATA);
			bw.Write((UInt32)msw.Length);
			bw.Write(msw.ToArray());

			fs.Seek(4, SeekOrigin.Begin);
			bw.Write((UInt32)(fs.Length - 8));

			bw.Dispose();
			fs.Close();
			fs.Dispose();
		}
	}
}
