using System;
using System.Text;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class INFO {
		private Encoding mEnc = Encoding.GetEncoding("shift-jis");
		private CK_INFO mInfo;

		public string ArchivalLocation;
		public string Artists;
		public string Commissioned;
		public string Comments;
		public string Copyright;
		public string CreationDate;
		public string Engineer;
		public string Genre;
		public string Keywords;
		public string Medium;
		public string Name;
		public string Product;
		public string Software;
		public string Source;
		public string SourceForm;
		public string Subject;
		public string Technician;

		public INFO() { }

		public INFO(byte* ptr, UInt32 endAddr) {
			while ((UInt32)ptr < endAddr) {
				mInfo = (CK_INFO)Marshal.PtrToStructure((IntPtr)ptr, typeof(CK_INFO));
				ptr += sizeof(CK_INFO);

				if (!Enum.IsDefined(typeof(CK_INFO.TYPE), mInfo.Type)) {
					break;
				}

				var temp = new byte[mInfo.Size];
				Marshal.Copy((IntPtr)ptr, temp, 0, temp.Length);
				var text = mEnc.GetString(temp).Replace("\0", "");

				ptr += mInfo.Size + (2 - (mInfo.Size % 2)) % 2;

				switch (mInfo.Type) {
				case CK_INFO.TYPE.IARL:
					ArchivalLocation = text;
					break;
				case CK_INFO.TYPE.IART:
					Artists = text;
					break;
				case CK_INFO.TYPE.ICMS:
					Commissioned = text;
					break;
				case CK_INFO.TYPE.ICMT:
					Comments = text;
					break;
				case CK_INFO.TYPE.ICOP:
					Copyright = text;
					break;
				case CK_INFO.TYPE.ICRD:
					CreationDate = text;
					break;
				case CK_INFO.TYPE.IENG:
					Engineer = text;
					break;
				case CK_INFO.TYPE.IGNR:
					Genre = text;
					break;
				case CK_INFO.TYPE.IKEY:
					Keywords = text;
					break;
				case CK_INFO.TYPE.IMED:
					Medium = text;
					break;
				case CK_INFO.TYPE.INAM:
					Name = text;
					break;
				case CK_INFO.TYPE.IPRD:
					Product = text;
					break;
				case CK_INFO.TYPE.ISFT:
					Software = text;
					break;
				case CK_INFO.TYPE.ISRC:
					Source = text;
					break;
				case CK_INFO.TYPE.ISRF:
					SourceForm = text;
					break;
				case CK_INFO.TYPE.ISBJ:
					Subject = text;
					break;
				case CK_INFO.TYPE.ITCH:
					Technician = text;
					break;
				}
			}
		}
	}
}
