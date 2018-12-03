using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class WVPL : Chunk {
		public Dictionary<int, WAVE> List = new Dictionary<int, WAVE>();

		public WVPL() { }

		public WVPL(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

		protected override void LoadList(byte* ptr, byte* endPtr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.WAVE:
				List.Add(List.Count, new WAVE(ptr, endPtr));
				break;
			default:
				throw new Exception();
			}
		}

		public new byte[] Bytes {
			get {
				var msPtbl = new MemoryStream();
				var bwPtbl = new BinaryWriter(msPtbl);
				bwPtbl.Write((uint)CK_CHUNK.TYPE.PTBL);
				bwPtbl.Write((uint)(sizeof(CK_PTBL) + List.Count * sizeof(uint)));
				bwPtbl.Write((uint)8);
				bwPtbl.Write((uint)List.Count);

				var msWave = new MemoryStream();
				var bwWave = new BinaryWriter(msWave);
				foreach (var wav in List) {
					bwPtbl.Write((uint)msWave.Position);
					bwWave.Write(wav.Value.Bytes);
				}

				var ms = new MemoryStream();
				var bw = new BinaryWriter(ms);
				if (0 < msWave.Length) {
					bw.Write(msPtbl.ToArray());
					bw.Write((uint)CK_CHUNK.TYPE.LIST);
					bw.Write((uint)(msWave.Length + 4));
					bw.Write((uint)CK_LIST.TYPE.WVPL);
					bw.Write(msWave.ToArray());
				}

				return ms.ToArray();
			}
		}
	}

	unsafe public class WAVE : Chunk {
		public CK_FMT Format;
		public CK_WSMP Sampler;
		public Dictionary<int, WaveLoop> Loops = new Dictionary<int, WaveLoop>();
		public byte[] Data;
		public INFO Info = new INFO();

		public WAVE(string filePath) {
			FileStream fs = new FileStream(filePath, FileMode.Open);
			BinaryReader br = new BinaryReader(fs);

			var riff = br.ReadUInt32();
			var riffSize = br.ReadUInt32();
			var riffType = br.ReadUInt32();

			while (fs.Position < fs.Length) {
				var chunkType = (CK_CHUNK.TYPE)br.ReadUInt32();
				var chunkSize = br.ReadUInt32();
				var chunkData = br.ReadBytes((int)chunkSize);

				switch (chunkType) {
				case CK_CHUNK.TYPE.FMT_:
					fixed (byte* ptr = &chunkData[0]) {
						Format = (CK_FMT)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_FMT));
					}
					break;
				case CK_CHUNK.TYPE.DATA:
					Data = chunkData;
					break;
				case CK_CHUNK.TYPE.WSMP:
					fixed (byte* ptr = &chunkData[0]) {
						Sampler = (CK_WSMP)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_WSMP));
						var pLoop = ptr + sizeof(CK_WSMP);
						for (var i = 0; i < Sampler.LoopCount; ++i) {
							Loops.Add(Loops.Count, (WaveLoop)Marshal.PtrToStructure((IntPtr)pLoop, typeof(WaveLoop)));
							pLoop += sizeof(WaveLoop);
						}
					}
					break;
				case CK_CHUNK.TYPE.LIST:
					fixed (byte* ptr = &chunkData[0]) {
						var list = (CK_LIST)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_LIST));
						switch (list.Type) {
						case CK_LIST.TYPE.INFO:
							Info = new INFO(ptr + sizeof(CK_LIST), ptr + chunkSize);
							break;
						}
					}
					break;
				}
			}

			br.Dispose();
			fs.Close();
			fs.Dispose();
		}

		public WAVE(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

		protected override void LoadChunk(byte* ptr) {
			switch (mChunk.Type) {
			case CK_CHUNK.TYPE.DLID:
			case CK_CHUNK.TYPE.GUID:
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

		protected override void LoadList(byte* ptr, byte* endPtr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.INFO:
				Info = new INFO(ptr, endPtr);
				break;
			}
		}

		protected override void WriteChunk(BinaryWriter bw) {
			mList.Type = CK_LIST.TYPE.WAVE;

			var data = Sampler.Bytes;
			bw.Write((uint)CK_CHUNK.TYPE.WSMP);
			bw.Write((uint)(data.Length + Sampler.LoopCount * sizeof(WaveLoop)));
			bw.Write(data);
			foreach (var loop in Loops.Values) {
				bw.Write(loop.Bytes);
			}

			data = Format.Bytes;
			bw.Write((uint)CK_CHUNK.TYPE.FMT_);
			bw.Write(data.Length);
			bw.Write(data);

			bw.Write((uint)CK_CHUNK.TYPE.DATA);
			bw.Write(Data.Length);
			bw.Write(Data);
		}

		protected override void WriteList(BinaryWriter bw) {
			bw.Write(Info.Bytes);
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

			while (msr.Position < msr.Length) {
				bmw.Write(bmr.ReadInt16());
			}

			// RIFF
			bw.Write((uint)0x46464952);
			bw.Write((uint)0);
			bw.Write((uint)0x45564157);

			// fmt
			bw.Write((uint)CK_CHUNK.TYPE.FMT_);
			bw.Write((uint)16);
			bw.Write(Format.Tag);
			bw.Write(Format.Channels);
			bw.Write(Format.SampleRate);
			bw.Write(Format.BytesPerSec);
			bw.Write(Format.BlockAlign);
			bw.Write(Format.Bits);

			// data
			bw.Write((uint)CK_CHUNK.TYPE.DATA);
			bw.Write((uint)msw.Length);
			bw.Write(msw.ToArray());

			// sampler
			var data = Sampler.Bytes;
			bw.Write((uint)CK_CHUNK.TYPE.WSMP);
			bw.Write((uint)(data.Length + Sampler.LoopCount * sizeof(WaveLoop)));
			bw.Write(data);
			foreach (var loop in Loops.Values) {
				bw.Write(loop.Bytes);
			}

			// info
			bw.Write(Info.Bytes);

			fs.Seek(4, SeekOrigin.Begin);
			bw.Write((uint)(fs.Length - 8));

			bw.Dispose();
			fs.Close();
			fs.Dispose();
		}
	}
}
