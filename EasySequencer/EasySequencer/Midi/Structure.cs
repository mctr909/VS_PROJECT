namespace MIDI {
	public struct InstID {
		public byte ProgramNo;
		public byte BankMSB;
		public byte BankLSB;
		public bool IsDrum;

		public InstID(byte programNo, byte bankMSB, byte bankLSB, bool isDrum = false) {
			ProgramNo = programNo;
			BankMSB = bankMSB;
			BankLSB = bankLSB;
			IsDrum = isDrum;
		}
	}

	public struct Envelope {
		public double LevelA;
		public double LevelH;
		public double LevelS;
		public double LevelR;
		public double TimeA;
		public double TimeH;
		public double DeltaA;
		public double DeltaD;
		public double DeltaR;

		public double AttackTime {
			set {
				if (value < 0.001) {
					value = 0.001;
				}
				TimeA = value;
				DeltaA = 12.0 / value;
			}
		}

		public double DecayTime {
			set {
				if (value < 0.001) {
					value = 0.001;
				}
				DeltaD = 12.0 / value;
			}
		}

		public double ReleaseTime {
			set {
				if (value < 0.001) {
					value = 0.001;
				}
				DeltaR = 12.0 / value;
			}
		}
	}

	public struct WaveInfo {
		public Envelope EnvAmp;
		public Envelope EnvCutoff;
		public uint LoopBegin;
		public uint LoopEnd;
		public bool LoopEnable;
		public byte BaseNoteNo;
		public double Delta;
		public double Gain;
		public short[] Buff;
	}

	public struct Filter {
		public double Cutoff;
		public double Resonance;
		private double[,] mPole;

		public Filter(double cutoff, double resonance) {
			Cutoff = cutoff;
			Resonance = resonance;
			mPole = new double[2, 4];
		}

		public void Clear() {
			mPole[0, 0] = 0.0;
			mPole[0, 1] = 0.0;
			mPole[0, 2] = 0.0;
			mPole[0, 3] = 0.0;
			mPole[1, 0] = 0.0;
			mPole[1, 1] = 0.0;
			mPole[1, 2] = 0.0;
			mPole[1, 3] = 0.0;
		}

		public void Step(double input, ref double output) {
			var fk = Cutoff * 1.16;
			var fki = 1.0 - fk;

			input -= mPole[0, 3] * (1.0 - fk * fk * 0.15) * 4.0 * Resonance;
			input *= (fk * fk) * (fk * fk) * 0.35013;

			mPole[0, 0] = input + 0.3 * mPole[1, 0] + fki * mPole[0, 0];
			mPole[0, 1] = mPole[0, 0] + 0.3 * mPole[1, 1] + fki * mPole[0, 1];
			mPole[0, 2] = mPole[0, 1] + 0.3 * mPole[1, 2] + fki * mPole[0, 2];
			mPole[0, 3] = mPole[0, 2] + 0.3 * mPole[1, 3] + fki * mPole[0, 3];

			mPole[1, 0] = input;
			mPole[1, 1] = mPole[0, 0];
			mPole[1, 2] = mPole[0, 1];
			mPole[1, 3] = mPole[0, 2];

			output = mPole[0, 3];
		}
	}

	public struct Time {
		private uint mValue;
		private uint mIndex;

		public uint Value {
			get { return mValue; }
		}

		public uint Index {
			get { return mIndex; }
		}

		public Time(uint value, uint index) {
			mValue = value;
			mIndex = index;
		}

		public void Step(uint delta) {
			mValue += delta;
			mIndex = (delta == 0 ? (mIndex + 1) : 0);
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
		public readonly uint Index;
		public readonly Message Message;

		public Event(Time time, Message message) {
			Time = time.Value;
			Index = time.Index;
			Message = message;
		}
	}

	public struct DrawPosition {
		public int X;
		public int Y;
		public int Width;
		public int Height;
		public DrawPosition(int x, int y, int width, int height) {
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}
	}
}