using System;
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
	public struct ConnectionBlock {
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

		public double Value {
			get {
				switch ((CONN_DST_TYPE)Destination) {
				case CONN_DST_TYPE.ATTENUATION:
				case CONN_DST_TYPE.FILTER_Q:
					return Math.Pow(10.0, Scale / (200 * 65536.0));
				case CONN_DST_TYPE.PAN:
					return (Scale / 655360.0) - 0.5;
				case CONN_DST_TYPE.LFO_START_DELAY:
				case CONN_DST_TYPE.VIB_START_DELAY:
				case CONN_DST_TYPE.EG1_ATTACK_TIME:
				case CONN_DST_TYPE.EG1_DECAY_TIME:
				case CONN_DST_TYPE.EG1_RELEASE_TIME:
				case CONN_DST_TYPE.EG1_DELAY_TIME:
				case CONN_DST_TYPE.EG1_HOLD_TIME:
				case CONN_DST_TYPE.EG1_SHUTDOWN_TIME:
				case CONN_DST_TYPE.EG2_ATTACK_TIME:
				case CONN_DST_TYPE.EG2_DECAY_TIME:
				case CONN_DST_TYPE.EG2_RELEASE_TIME:
				case CONN_DST_TYPE.EG2_DELAY_TIME:
				case CONN_DST_TYPE.EG2_HOLD_TIME:
					return Math.Pow(2.0, Scale / (1200 * 65536.0));
				case CONN_DST_TYPE.EG1_SUSTAIN_LEVEL:
				case CONN_DST_TYPE.EG2_SUSTAIN_LEVEL:
					return Scale / 655360.0;
				case CONN_DST_TYPE.PITCH:
				case CONN_DST_TYPE.LFO_FREQUENCY:
				case CONN_DST_TYPE.VIB_FREQUENCY:
				case CONN_DST_TYPE.FILTER_CUTOFF:
					return Math.Pow(2.0, (Scale / 65536.0 - 6900) / 1200.0) * 440;
				default:
					return 0.0;
				}
			}

			set {
				switch ((CONN_DST_TYPE)Destination) {
				case CONN_DST_TYPE.ATTENUATION:
				case CONN_DST_TYPE.FILTER_Q:
					Scale = (Int32)(Math.Log10(value) * 200 * 65536);
					break;
				case CONN_DST_TYPE.PAN:
					Scale = (Int32)((value + 0.5) * 655360);
					break;
				case CONN_DST_TYPE.LFO_START_DELAY:
				case CONN_DST_TYPE.VIB_START_DELAY:
				case CONN_DST_TYPE.EG1_ATTACK_TIME:
				case CONN_DST_TYPE.EG1_DECAY_TIME:
				case CONN_DST_TYPE.EG1_RELEASE_TIME:
				case CONN_DST_TYPE.EG1_DELAY_TIME:
				case CONN_DST_TYPE.EG1_HOLD_TIME:
				case CONN_DST_TYPE.EG1_SHUTDOWN_TIME:
				case CONN_DST_TYPE.EG2_ATTACK_TIME:
				case CONN_DST_TYPE.EG2_DECAY_TIME:
				case CONN_DST_TYPE.EG2_RELEASE_TIME:
				case CONN_DST_TYPE.EG2_DELAY_TIME:
				case CONN_DST_TYPE.EG2_HOLD_TIME:
					Scale = (Int32)(Math.Log(value, 2.0) * 1200 * 65536);
					break;
				case CONN_DST_TYPE.EG1_SUSTAIN_LEVEL:
				case CONN_DST_TYPE.EG2_SUSTAIN_LEVEL:
					Scale = (Int32)(value * 655360);
					break;
				case CONN_DST_TYPE.PITCH:
				case CONN_DST_TYPE.LFO_FREQUENCY:
				case CONN_DST_TYPE.VIB_FREQUENCY:
				case CONN_DST_TYPE.FILTER_CUTOFF:
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
	public struct CK_LIST {
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Type;
		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Size;
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
		public UInt32 Blocks;
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
		public UInt32 Cues;
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
}
