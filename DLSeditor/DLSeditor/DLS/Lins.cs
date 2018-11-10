﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DLS {
	public sealed class LINSSort : IComparer<MidiLocale> {
		// IComparerの実装
		public int Compare(MidiLocale x, MidiLocale y) {
			var xKey = ((x.BankFlags & 0x80) << 17) | (x.ProgramNo << 16) | (x.BankMSB << 8) | x.BankLSB;
			var yKey = ((y.BankFlags & 0x80) << 17) | (y.ProgramNo << 16) | (y.BankMSB << 8) | y.BankLSB;
			return xKey - yKey;
		}
	}

	unsafe public class LINS : Chunk {
		public SortedDictionary<MidiLocale, INS> List = new SortedDictionary<MidiLocale, INS>(new LINSSort());

		public LINS() { }

		public LINS(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

		protected override void LoadList(byte* ptr, byte* endPtr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.INS_:
				var inst = new INS(ptr, endPtr);
				List.Add(inst.Header.Locale, inst);
				break;
			default:
				throw new Exception(string.Format("Unknown ListId [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((uint)mList.Type))));
			}
		}

		public new byte[] Bytes {
			get {
				var ms = new MemoryStream();
				var bw = new BinaryWriter(ms);
				foreach (var ins in List) {
					bw.Write(ins.Value.Bytes);
				}

				var ms2 = new MemoryStream();
				var bw2 = new BinaryWriter(ms2);
				if (0 < ms.Length) {
					bw2.Write((uint)CK_CHUNK.TYPE.LIST);
					bw2.Write((uint)(ms.Length + 4));
					bw2.Write((uint)CK_LIST.TYPE.LINS);
					bw2.Write(ms.ToArray());
				}

				return ms2.ToArray();
			}
		}
	}

	unsafe public class INS : Chunk {
		public CK_INSH Header;
		public LRGN Regions = new LRGN();
		public LART Articulations = new LART();
		public INFO Info = new INFO();

		public INS() { }

		public INS(byte programNo, byte bankMSB = 0, byte bankLSB = 0, bool isDrum = false) {
			Header.Locale.BankFlags = (byte)(isDrum ? 0x80 : 0x00);
			Header.Locale.ProgramNo = programNo;
			Header.Locale.BankMSB = bankMSB;
			Header.Locale.BankLSB = bankLSB;
		}

		public INS(byte* ptr, byte* endPtr) : base(ptr, endPtr) { }

		protected override void LoadChunk(byte* ptr) {
			switch (mChunk.Type) {
			case CK_CHUNK.TYPE.INSH:
				Header = (CK_INSH)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_INSH));
				break;
			default:
				throw new Exception(string.Format("Unknown ChunkType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((uint)mChunk.Type))));
			}
		}

		protected override void LoadList(byte* ptr, byte* endPtr) {
			switch (mList.Type) {
			case CK_LIST.TYPE.LRGN:
				Regions = new LRGN(ptr, endPtr);
				break;
			case CK_LIST.TYPE.LART:
			case CK_LIST.TYPE.LAR2:
				Articulations = new LART(ptr, endPtr);
				break;
			case CK_LIST.TYPE.INFO:
				Info = new INFO(ptr, endPtr);
				break;
			default:
				throw new Exception(string.Format("Unknown ListType [{0}]", Encoding.ASCII.GetString(BitConverter.GetBytes((uint)mList.Type))));
			}
		}

		protected override void WriteChunk(BinaryWriter bw) {
			mList.Type = CK_LIST.TYPE.INS_;

			var data = Header.Bytes;
			bw.Write((uint)CK_CHUNK.TYPE.INSH);
			bw.Write(data.Length);
			bw.Write(data);
		}

		protected override void WriteList(BinaryWriter bw) {
			bw.Write(Regions.Bytes);
			bw.Write(Articulations.Bytes);
			bw.Write(Info.Bytes);
		}
	}
}