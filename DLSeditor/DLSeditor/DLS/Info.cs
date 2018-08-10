using System;
using System.Text;
using System.Runtime.InteropServices;

namespace DLS {
	unsafe public class INFO {
		private enum INFO_ID : UInt32 {
			IARL = 0x4C524149, // ArchivalLocation
			IART = 0x54524149, // Artists
			ICMS = 0x534D4349, // Commissioned
			ICMT = 0x544D4349, // Comments
			ICOP = 0x504F4349, // Copyright
			ICRD = 0x44524349, // CreationDate
			IENG = 0x474E4549, // Engineer
			IGNR = 0x524E4749, // Genre
			IKEY = 0x59454B49, // Keywords
			IMED = 0x44454D49, // Medium
			INAM = 0x4D414E49, // Name
			IPRD = 0x44525049, // Product
			ISFT = 0x54465349, // Software
			ISRC = 0x43525349, // Source
			ISRF = 0x46525349, // SourceForm
			ISBJ = 0x4A425349, // Subject
			ITCH = 0x48435449  // Technician
		}

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

		public INFO() { }

		public INFO(byte* buff, UInt32 endAddr) {
			while ((UInt32)buff < endAddr) {
				var infoType = *(INFO_ID*)buff;
				buff += 4;
				var textSize = *(UInt32*)buff;
				buff += 4;

				if (!Enum.IsDefined(typeof(INFO_ID), infoType)) {
					break;
				}

				var temp = new byte[textSize];
				Marshal.Copy((IntPtr)buff, temp, 0, temp.Length);
				var text = mEnc.GetString(temp).Replace("\0", "");

				var pad = (2 - (textSize % 2)) % 2;
				buff += textSize + pad;

				switch (infoType) {
				case INFO_ID.IARL:
					ArchivalLocation = text;
					break;
				case INFO_ID.IART:
					Artists = text;
					break;
				case INFO_ID.ICMS:
					Commissioned = text;
					break;
				case INFO_ID.ICMT:
					Comments = text;
					break;
				case INFO_ID.ICOP:
					Copyright = text;
					break;
				case INFO_ID.ICRD:
					CreationDate = text;
					break;
				case INFO_ID.IENG:
					Engineer = text;
					break;
				case INFO_ID.IGNR:
					Genre = text;
					break;
				case INFO_ID.IKEY:
					Keywords = text;
					break;
				case INFO_ID.IMED:
					Medium = text;
					break;
				case INFO_ID.INAM:
					Name = text;
					break;
				case INFO_ID.IPRD:
					Product = text;
					break;
				case INFO_ID.ISFT:
					Software = text;
					break;
				case INFO_ID.ISRC:
					Source = text;
					break;
				case INFO_ID.ISRF:
					SourceForm = text;
					break;
				case INFO_ID.ISBJ:
					Subject = text;
					break;
				case INFO_ID.ITCH:
					Technician = text;
					break;
				}
			}
		}
	}
}
