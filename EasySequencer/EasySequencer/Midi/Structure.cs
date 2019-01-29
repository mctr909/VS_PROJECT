namespace MIDI {
	public struct INST_ID {
		public byte isDrum;
		public byte programNo;
		public byte bankMSB;
		public byte bankLSB;
	};

	public struct WaveLoop {
		public uint size;
		public uint type;
		public uint start;
		public uint length;
	}

	public struct Envelope {
		public double attackTime;
		public double decayTime;
		public double releaceTime;
		public double holdTime;
		public double sustainLevel;
	};

	public struct WaveInfo {
		public uint unityNote;
		public double gain;
		public double delta;
		public Envelope envAmp;
		public WaveLoop loop;
		public bool loopEnable;
		public uint pcmAddr;
		public uint pcmLength;
	}

	public struct Time {
		public uint Value { get; private set; }
		public uint Index { get; private set; }

		public Time(uint value, uint index) {
			Value = value;
			Index = index;
		}

		public void Step(uint delta) {
			Value += delta;
			Index = (delta == 0 ? (Index + 1) : 0);
		}
	}

	public class Meta {
		public readonly META_TYPE Type;
		public readonly byte[] Data;

		public double BPM {
			get { return 60000000.0 / ((Data[0] << 16) | (Data[1] << 8) | Data[2]); }
		}

		public string Text {
			get { return System.Text.Encoding.GetEncoding("shift-jis").GetString(Data); }
		}

		public KEY Key {
			get { return (KEY)((Data[0] << 8) | Data[1]); }
		}

		public Meta(META_TYPE type, params byte[] data) {
			Type = type;
			Data = data;
		}

		public new string ToString() {
			switch (Type) {
			case META_TYPE.SEQ_NO:
				return string.Format("[{0}]\t{1}", Type, (Data[0] << 8) | Data[1]);

			case META_TYPE.TEXT:
			case META_TYPE.COMPOSER:
			case META_TYPE.SEQ_NAME:
			case META_TYPE.INST_NAME:
			case META_TYPE.LYRIC:
			case META_TYPE.MARKER:
			case META_TYPE.PRG_NAME:
				return string.Format("[{0}]\t\"{1}\"", Type, Text);

			case META_TYPE.CH_PREFIX:
			case META_TYPE.PORT:
				return string.Format("[{0}]\t{1}", Type, Data[0]);

			case META_TYPE.TEMPO:
				return string.Format("[{0}]\t{1}", Type, BPM.ToString("0.00"));

			case META_TYPE.MEASURE:
				return string.Format("[{0}]\t{1}/{2} ({3}, {4})", Type, Data[0], (int)System.Math.Pow(2.0, Data[1]), Data[2], Data[3]);

			case META_TYPE.KEY:
				return string.Format("[{0}]\t{1}", Type, Key);

			case META_TYPE.META:
				return string.Format("[{0}]\t{1}", Type, System.BitConverter.ToString(Data));

			default:
				return string.Format("[{0}]", Type);
			}
		}
	}

	public class SystemEx {
		public readonly uint Length;
		public readonly byte[] Data;

		public SystemEx(byte[] data) {
			Length = (uint)data.Length;
			Data = data;
		}
	}

	public struct Message {
		public readonly EVENT_TYPE Type;
		public readonly byte Channel;
		public readonly byte Byte1;
		public readonly byte Byte2;
		public Meta Meta;
		public SystemEx SystemEx;

		public uint Bytes {
			get {
				return (uint)Type | Channel | (uint)(Byte1 << 8) | (uint)(Byte2 << 16);
			}
		}

		public Message(EVENT_TYPE type = EVENT_TYPE.INVALID, byte channel = 0xF0, byte byte1 = 0x00, byte byte2 = 0x00) {
			Type = type;
			Channel = channel;
			Byte1 = byte1;
			Byte2 = byte2;
			Meta = null;
			SystemEx = null;
		}

		public Message(CTRL_TYPE type, byte channel, byte value = 0) {
			Type = EVENT_TYPE.CTRL_CHG;
			Channel = channel;
			Byte1 = (byte)type;
			Byte2 = value;
			Meta = null;
			SystemEx = null;
		}

		public Message(SystemEx systemEx) {
			Type = EVENT_TYPE.SYS_EX;
			Channel = 0xF0;
			Byte1 = 0x80;
			Byte2 = 0x80;
			Meta = null;
			SystemEx = systemEx;
		}

		public Message(Meta meta) {
			Type = EVENT_TYPE.META;
			Channel = 0xF0;
			Byte1 = 0x80;
			Byte2 = 0x80;
			Meta = meta;
			SystemEx = null;
		}
	}

    public struct Event {
        public readonly uint Time;
        public readonly uint Track;
        public readonly uint Index;
        public readonly Message Message;

        public Event(Time time, uint trackNo, Message message) {
            Time = time.Value;
            Track = trackNo;
            Index = time.Index;
            Message = message;
        }
    }
}