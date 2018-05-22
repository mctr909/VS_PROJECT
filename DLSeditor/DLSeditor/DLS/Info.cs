using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace DLS
{
	unsafe public class CINFO
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

		public CINFO() { }

		public CINFO(byte* buff, UInt32 termAddr)
		{
			ReadText(buff, termAddr);
		}

		private void ReadText(byte* buff, UInt32 termAddr)
		{
			while ((UInt32)buff < termAddr)
			{
				var infoType = *(INFO_TYPE*)buff; buff += 4;
				var textSize = *(UInt32*)buff; buff += 4;
				var pad = (2 - (textSize % 2)) % 2;

				if (!Enum.IsDefined(typeof(INFO_TYPE), infoType)) break;

				var temp = new byte[textSize];
				Marshal.Copy((IntPtr)buff, temp, 0, temp.Length);
				var text = mEnc.GetString(temp).Replace("\0", "");

				buff += textSize + pad;

				switch (infoType)
				{
					case INFO_TYPE.IARL:
						ArchivalLocation = text;
						break;
					case INFO_TYPE.IART:
						Artists = text;
						break;
					case INFO_TYPE.ICMS:
						Commissioned = text;
						break;
					case INFO_TYPE.ICMT:
						Comments = text;
						break;
					case INFO_TYPE.ICOP:
						Copyright = text;
						break;
					case INFO_TYPE.ICRD:
						CreationDate = text;
						break;
					case INFO_TYPE.IENG:
						Engineer = text;
						break;
					case INFO_TYPE.IGNR:
						Genre = text;
						break;
					case INFO_TYPE.IKEY:
						Keywords = text;
						break;
					case INFO_TYPE.IMED:
						Medium = text;
						break;
					case INFO_TYPE.INAM:
						Name = text;
						break;
					case INFO_TYPE.IPRD:
						Product = text;
						break;
					case INFO_TYPE.ISFT:
						Software = text;
						break;
					case INFO_TYPE.ISRC:
						Source = text;
						break;
					case INFO_TYPE.ISRF:
						SourceForm = text;
						break;
					case INFO_TYPE.ISBJ:
						Subject = text;
						break;
					case INFO_TYPE.ITCH:
						Technician = text;
						break;
				}
			}
		}

		private void WriteText(BinaryWriter bw, INFO_TYPE infoType, string text)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				var temp = mEnc.GetBytes(text);
				var pad = (2 - (temp.Length % 2)) % 2;
				var buff = mEnc.GetBytes(text + "".PadRight(pad, '\0'));
				bw.Write((UInt32)infoType);
				bw.Write((UInt32)buff.Length);
				bw.Write(buff);
			}
		}

		public byte[] Bytes
		{
			get
			{
				var ms = new MemoryStream();
				var bw = new BinaryWriter(ms);
				WriteText(bw, INFO_TYPE.IARL, ArchivalLocation);
				WriteText(bw, INFO_TYPE.IART, Artists);
				WriteText(bw, INFO_TYPE.ICMS, Commissioned);
				WriteText(bw, INFO_TYPE.ICMT, Comments);
				WriteText(bw, INFO_TYPE.ICOP, Copyright);
				WriteText(bw, INFO_TYPE.ICRD, CreationDate);
				WriteText(bw, INFO_TYPE.IENG, Engineer);
				WriteText(bw, INFO_TYPE.IGNR, Genre);
				WriteText(bw, INFO_TYPE.IKEY, Keywords);
				WriteText(bw, INFO_TYPE.IMED, Medium);
				WriteText(bw, INFO_TYPE.INAM, Name);
				WriteText(bw, INFO_TYPE.IPRD, Product);
				WriteText(bw, INFO_TYPE.ISFT, Software);
				WriteText(bw, INFO_TYPE.ISRC, Source);
				WriteText(bw, INFO_TYPE.ISRF, SourceForm);
				WriteText(bw, INFO_TYPE.ISBJ, Subject);
				WriteText(bw, INFO_TYPE.ITCH, Technician);
				return ms.ToArray();
			}
		}
	}
}
