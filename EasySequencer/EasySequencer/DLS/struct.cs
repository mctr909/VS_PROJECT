using System.Runtime.InteropServices;

namespace DLS {
	public enum SRC_TYPE : ushort {
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
	};

	public enum DST_TYPE : ushort {
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

		// EG1 DESTINATIONS
		EG1_ATTACK_TIME = 0x0206,
		EG1_DECAY_TIME = 0x0207,
		EG1_RESERVED = 0x0208,
		EG1_RELEASE_TIME = 0x0209,
		EG1_SUSTAIN_LEVEL = 0x020A,
		EG1_DELAY_TIME = 0x020B,
		EG1_HOLD_TIME = 0x020C,
		EG1_SHUTDOWN_TIME = 0x020D,

		// EG2 DESTINATIONS
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
	};

	public enum TRN_TYPE : ushort {
		NONE = 0x0000,
		CONCAVE = 0x0001,
		CONVEX = 0x0002,
		SWITCH = 0x0003
	};

	public enum CHUNK_TYPE : uint {
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
	};

	public enum LIST_TYPE : uint {
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
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MidiLocale {
		public	byte bankLSB;
		public	byte bankMSB;
		private	byte reserve1;
		public	byte bankFlags;
		public	byte programNo;
		private	byte reserve2;
		private	byte reserve3;
		private	byte reserve4;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Range {
		public ushort low;
		public ushort high;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Connection {
		public SRC_TYPE	source;
		public SRC_TYPE	control;
		public DST_TYPE	destination;
		public TRN_TYPE	transform;
		public int		scale;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct WaveLoop {
		public uint size;
		public uint type;
		public uint start;
		public uint length;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CK_CHUNK {
		public CHUNK_TYPE	type;
		public uint			size;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CK_LIST {
		public LIST_TYPE type;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CK_VERS {
		public uint msb;
		public uint lsb;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CK_COLH {
		public uint instruments;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CK_INSH {
		public uint			regions;
		public MidiLocale	locale;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct CK_RGNH {
		public Range key;
		public Range velocity;
		public ushort options;
		public ushort keyGroup;
		//public ushort layer;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CK_ART1 {
		public uint size;
		public uint count;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CK_WLNK {
		public ushort	options;
		public ushort	phaseGroup;
		public uint		channel;
		public uint		tableIndex;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CK_WSMP {
		public uint		size;
		public ushort	unityNote;
		public short	fineTune;
		public int		gainInt;
		public uint		options;
		public uint		loopCount;

		public double Gain {
			get { return System.Math.Pow(10.0, gainInt / (200 * 65536.0)); }
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CK_PTBL {
		public uint size;
		public uint count;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CK_FMT {
		public ushort	tag;
		public ushort	channels;
		public uint		sampleRate;
		public uint		bytesPerSec;
		public ushort	blockAlign;
		public ushort	bits;
	};
}
