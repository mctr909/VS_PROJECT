using System;
using System.Text;
using System.Runtime.InteropServices;

namespace DLS
{
	unsafe public class INFO
	{
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

		private Encoding mEnc = Encoding.GetEncoding("shift-jis");

		public INFO() {}

		public INFO(byte* buff, UInt32 endAddr)
		{
			ReadText(buff, endAddr);
		}

		private void ReadText(byte* buff, UInt32 endAddr)
		{
			while ((UInt32)buff < endAddr) {
				var infoType = *(InfoID*)buff;
				buff += 4;
				var textSize = *(UInt32*)buff;
				buff += 4;

				if (!Enum.IsDefined(typeof(InfoID), infoType)) {
					break;
				}

				var temp = new byte[textSize];
				Marshal.Copy((IntPtr)buff, temp, 0, temp.Length);
				var text = mEnc.GetString(temp).Replace("\0", "");

				var pad = (2 - (textSize % 2)) % 2;
				buff += textSize + pad;

				switch (infoType) {
				case InfoID.IARL:
					ArchivalLocation = text;
					break;
				case InfoID.IART:
					Artists = text;
					break;
				case InfoID.ICMS:
					Commissioned = text;
					break;
				case InfoID.ICMT:
					Comments = text;
					break;
				case InfoID.ICOP:
					Copyright = text;
					break;
				case InfoID.ICRD:
					CreationDate = text;
					break;
				case InfoID.IENG:
					Engineer = text;
					break;
				case InfoID.IGNR:
					Genre = text;
					break;
				case InfoID.IKEY:
					Keywords = text;
					break;
				case InfoID.IMED:
					Medium = text;
					break;
				case InfoID.INAM:
					Name = text;
					break;
				case InfoID.IPRD:
					Product = text;
					break;
				case InfoID.ISFT:
					Software = text;
					break;
				case InfoID.ISRC:
					Source = text;
					break;
				case InfoID.ISRF:
					SourceForm = text;
					break;
				case InfoID.ISBJ:
					Subject = text;
					break;
				case InfoID.ITCH:
					Technician = text;
					break;
				}
			}
		}
	}
}
