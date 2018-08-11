﻿using System;
using System.Runtime.InteropServices;

namespace DLS {
	[StructLayout(LayoutKind.Sequential)]
	public struct DLSID {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Data1;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Data2;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Data3;
		[MarshalAs(UnmanagedType.U8, SizeConst = 8)]
		public UInt64 Data4;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MIDILOCALE {
		[MarshalAs(UnmanagedType.U1, SizeConst = 1)]
		public byte BankLSB;
		[MarshalAs(UnmanagedType.U1, SizeConst = 1)]
		public byte BankMSB;
		[MarshalAs(UnmanagedType.U1, SizeConst = 1)]
		public byte Reserve1;
		[MarshalAs(UnmanagedType.U1, SizeConst = 1)]
		public byte BankFlags;
		[MarshalAs(UnmanagedType.U1, SizeConst = 1)]
		public byte ProgramNo;
		[MarshalAs(UnmanagedType.U1, SizeConst = 1)]
		public byte Reserve2;
		[MarshalAs(UnmanagedType.U1, SizeConst = 1)]
		public byte Reserve3;
		[MarshalAs(UnmanagedType.U1, SizeConst = 1)]
		public byte Reserve4;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RGNRANGE {
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Low;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 High;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Connection {
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Source;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Control;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Destination;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Transform;
		[MarshalAs(UnmanagedType.I4, SizeConst = 4)]
		public Int32 Scale;

		public enum SRC_TYPE : UInt16 {
			// MODULATOR SOURCES
			NONE = 0x0000,
			LFO = 0x0001,
			KEY_ON_VELOCITY = 0x0002,
			KEY_NUMBER = 0x0003,
			EG1 = 0x0004,
			EG2 = 0x0005,
			PITCH_WHEEL = 0x0006,
			POLY_PRESSURE = 0x0007,
			CHANNEL_PRESSURE = 0x0008,
			VIBRATO = 0x0009,

			// MIDI CONTROLLER SOURCES
			CC1 = 0x0081,
			CC7 = 0x0087,
			CC10 = 0x008A,
			CC11 = 0x008B,
			CC91 = 0x00DB,
			CC93 = 0x00DD,

			// REGISTERED PARAMETER NUMBERS
			RPN0 = 0x0100,
			RPN1 = 0x0101,
			RPN2 = 0x0102
		}

		public enum DST_TYPE : UInt16 {
			// GENERIC DESTINATIONS
			NONE = 0x0000,
			ATTENUATION = 0x0001,
			RESERVED = 0x0002,
			PITCH = 0x0003,
			PAN = 0x0004,
			KEY_NUMBER = 0x0005,

			// CHANNEL OUTPUT DESTINATIONS
			LEFT = 0x0010,
			RIGHT = 0x0011,
			CENTER = 0x0012,
			LFET_CHANNEL = 0x0013,
			LEFT_REAR = 0x0014,
			RIGHT_REAR = 0x0015,
			CHORUS = 0x0080,
			REVERB = 0x0081,

			// MODULATOR LFO DESTINATIONS
			LFO_FREQUENCY = 0x0104,
			LFO_START_DELAY = 0x0105,

			// VIBRATO LFO DESTINATIONS
			VIB_FREQUENCY = 0x0114,
			VIB_START_DELAY = 0x0115,

			// EG DESTINATIONS
			EG1_ATTACK_TIME = 0x0206,
			EG1_DECAY_TIME = 0x0207,
			EG1_RESERVED = 0x0208,
			EG1_RELEASE_TIME = 0x0209,
			EG1_SUSTAIN_LEVEL = 0x020A,
			EG1_DELAY_TIME = 0x020B,
			EG1_HOLD_TIME = 0x020C,
			EG1_SHUTDOWN_TIME = 0x020D,
			EG2_ATTACK_TIME = 0x030A,
			EG2_DECAY_TIME = 0x030B,
			EG2_RESERVED = 0x030C,
			EG2_RELEASE_TIME = 0x030D,
			EG2_SUSTAIN_LEVEL = 0x030E,
			EG2_DELAY_TIME = 0x030F,
			EG2_HOLD_TIME = 0x0310,

			// FILTER DESTINATIONS
			FILTER_CUTOFF = 0x0500,
			FILTER_Q = 0x0501
		}

		public enum TRN_TYPE : UInt16 {
			NONE = 0x0000,
			CONCAVE = 0x0001,
			CONVEX = 0x0002,
			SWITCH = 0x0003
		}

		public double Value {
			get {
				switch ((DST_TYPE)Destination) {
				case DST_TYPE.ATTENUATION:
				case DST_TYPE.FILTER_Q:
					return Math.Pow(10.0, Scale / (200 * 65536.0));
				case DST_TYPE.PAN:
					return (Scale / 655360.0) - 0.5;
				case DST_TYPE.LFO_START_DELAY:
				case DST_TYPE.VIB_START_DELAY:
				case DST_TYPE.EG1_ATTACK_TIME:
				case DST_TYPE.EG1_DECAY_TIME:
				case DST_TYPE.EG1_RELEASE_TIME:
				case DST_TYPE.EG1_DELAY_TIME:
				case DST_TYPE.EG1_HOLD_TIME:
				case DST_TYPE.EG1_SHUTDOWN_TIME:
				case DST_TYPE.EG2_ATTACK_TIME:
				case DST_TYPE.EG2_DECAY_TIME:
				case DST_TYPE.EG2_RELEASE_TIME:
				case DST_TYPE.EG2_DELAY_TIME:
				case DST_TYPE.EG2_HOLD_TIME:
					return Math.Pow(2.0, Scale / (1200 * 65536.0));
				case DST_TYPE.EG1_SUSTAIN_LEVEL:
				case DST_TYPE.EG2_SUSTAIN_LEVEL:
					return Scale / 655360.0;
				case DST_TYPE.PITCH:
				case DST_TYPE.LFO_FREQUENCY:
				case DST_TYPE.VIB_FREQUENCY:
				case DST_TYPE.FILTER_CUTOFF:
					return Math.Pow(2.0, (Scale / 65536.0 - 6900) / 1200.0) * 440;
				default:
					return 0.0;
				}
			}

			set {
				switch ((DST_TYPE)Destination) {
				case DST_TYPE.ATTENUATION:
				case DST_TYPE.FILTER_Q:
					Scale = (Int32)(Math.Log10(value) * 200 * 65536);
					break;
				case DST_TYPE.PAN:
					Scale = (Int32)((value + 0.5) * 655360);
					break;
				case DST_TYPE.LFO_START_DELAY:
				case DST_TYPE.VIB_START_DELAY:
				case DST_TYPE.EG1_ATTACK_TIME:
				case DST_TYPE.EG1_DECAY_TIME:
				case DST_TYPE.EG1_RELEASE_TIME:
				case DST_TYPE.EG1_DELAY_TIME:
				case DST_TYPE.EG1_HOLD_TIME:
				case DST_TYPE.EG1_SHUTDOWN_TIME:
				case DST_TYPE.EG2_ATTACK_TIME:
				case DST_TYPE.EG2_DECAY_TIME:
				case DST_TYPE.EG2_RELEASE_TIME:
				case DST_TYPE.EG2_DELAY_TIME:
				case DST_TYPE.EG2_HOLD_TIME:
					Scale = (Int32)(Math.Log(value, 2.0) * 1200 * 65536);
					break;
				case DST_TYPE.EG1_SUSTAIN_LEVEL:
				case DST_TYPE.EG2_SUSTAIN_LEVEL:
					Scale = (Int32)(value * 655360);
					break;
				case DST_TYPE.PITCH:
				case DST_TYPE.LFO_FREQUENCY:
				case DST_TYPE.VIB_FREQUENCY:
				case DST_TYPE.FILTER_CUTOFF:
					Scale = (Int32)(((Math.Log(value / 440, 2.0) * 1200) + 6900) * 65536);
					break;
				default:
					Scale = 0;
					break;
				}
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WaveLoop {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Size;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Type;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Start;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Length;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_CHUNK {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public TYPE Type;

		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Size;

		public enum TYPE : UInt32 {
			COLH = 0x686C6F63,
			VERS = 0x73726576,
			MSYN = 0x6E79736D,
			PTBL = 0x6C627470,
			LIST = 0x5453494C,
			DLID = 0x64696C64,
			GUID = 0x64697567,
			INSH = 0x68736E69,
			RGNH = 0x686E6772,
			WSMP = 0x706D7377,
			WLNK = 0x6B6E6C77,
			ART1 = 0x31747261,
			ART2 = 0x32747261,
			FMT_ = 0x20746D66,
			DATA = 0x61746164
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_LIST {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public TYPE Type;

		public enum TYPE : UInt32 {
			LINS = 0x736E696C,
			WVPL = 0x6C707677,
			INFO = 0x4F464E49,
			INS_ = 0x20736E69,
			WAVE = 0x65766177,
			LRGN = 0x6E67726C,
			LART = 0x7472616C,
			LAR2 = 0x3272616C,
			RGN_ = 0x206E6772,
			RGN2 = 0x326E6772
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_VERS {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 MSB;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 LSB;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_DLID {
		DLSID DlsId;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_COLH {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Instruments;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_INSH {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Regions;
		public MIDILOCALE Locale;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_RGNH {
		public RGNRANGE Key;
		public RGNRANGE Velocity;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Options;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 KeyGroup;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Layer;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_ART1 {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Size;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Count;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_WLNK {
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Options;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 PhaseGroup;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Channel;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 TableIndex;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_WSMP {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Size;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 UnityNote;
		[MarshalAs(UnmanagedType.I2, SizeConst = 2)]
		public Int16 FineTune;
		[MarshalAs(UnmanagedType.I4, SizeConst = 4)]
		public Int32 GainInt;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Options;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 LoopCount;

		public double Gain {
			get {
				return Math.Pow(10.0, GainInt / (200 * 65536.0));
			}
			set {
				GainInt = (Int32)(Math.Log10(value) * 200 * 65536);
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_PTBL {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Size;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Count;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_FMT {
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Tag;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Channels;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 SampleRate;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 BytesPerSec;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 BlockAlign;
		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Bits;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CK_INFO {
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public TYPE Type;

		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Size;

		public enum TYPE : UInt32 {
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
	}
}
