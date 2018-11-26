namespace MIDI {
	public class Channel {
		public readonly int No;

		public InstID InstID;
		public bool Enable;
		public bool[] Keyboard;

		private Filter mFilter = new Filter(1.0, 0.0);
		private double mCurCuttoff;
		private double mCurResonce;

		private double mVolD;
		public byte Vol { get; private set; }

		private double mExpD;
		public byte Exp { get; private set; }

		private double mPanL;
		private double mPanR;
		public byte Pan { get; private set; }

		private double mRevD;
		public byte Rev { get; private set; }

		private double mDelD;
		public byte Del { get; private set; }

		private double mChoD;
		public byte Cho { get; private set; }

		public double Hld { get; private set; }

		public byte Fc { get; private set; }
		public double FcD { get; private set; }

		public byte Fq { get; private set; }
		public double FqD { get; private set; }

		public byte PitchRange { get; private set; }

		public short Pitch { get; private set; }

		public double PitchD { get; private set; }

		private byte mRPN_MSB;
		private byte mRPN_LSB;

		public Channel(int no) {
			No = no;

			Enable = true;
			InstID = new InstID();
			Keyboard = new bool[128];

			AllReset();
		}

		public void ControlChange(byte type, byte value) {
			switch ((CTRL_TYPE)type) {
			case CTRL_TYPE.BANK_MSB:
				InstID.BankMSB = value;
				break;
			case CTRL_TYPE.BANK_LSB:
				InstID.BankLSB = value;
				break;

			case CTRL_TYPE.VOLUME:
				Vol = value;
				mVolD = Const.Amp[value];
				break;
			case CTRL_TYPE.EXPRESSION:
				Exp = value;
				mExpD = Const.Amp[value];
				break;
			case CTRL_TYPE.PAN:
				Pan = value;
				mPanL = Const.Cos[value];
				mPanR = Const.Sin[value];
				break;

			case CTRL_TYPE.REVERB:
				Rev = value;
				mRevD = value / 127.0;
				break;
			case CTRL_TYPE.CHORUS:
				Cho = value;
				mChoD = Const.FeedBack[value];
				break;
			case CTRL_TYPE.DELAY:
				Del = value;
				mDelD = 0.75 * Const.FeedBack[value];
				break;

			case CTRL_TYPE.MODULATION:
				//Mod
				break;
			case CTRL_TYPE.PORTAMENTO:
				//Pol
				break;
			case CTRL_TYPE.PORTAMENTO_TIME:
				//PoT
				break;

			case CTRL_TYPE.DATA:
				if (mRPN_MSB == 0 && mRPN_LSB == 0) {
					PitchRange = value;
				}
				mRPN_MSB = 255;
				mRPN_LSB = 255;
				break;

			case CTRL_TYPE.HOLD:
				Hld = (value < 64) ? 1.0 : 0.125;
				break;

			case CTRL_TYPE.CUTOFF:
				Fc = value;
				FcD = (2.0 * Const.Amp[Fc] + 0.001);
				break;
			case CTRL_TYPE.RESONANCE:
				Fq = value;
				FqD = value / 168.0;
				break;

			case CTRL_TYPE.RPN_LSB:
				mRPN_LSB = value;
				break;
			case CTRL_TYPE.RPN_MSB:
				mRPN_MSB = value;
				break;

			case CTRL_TYPE.ALL_RESET:
				AllReset();
				break;
			}
		}

		public void ProgramChange(byte no) {
			InstID.ProgramNo = no;

			//if (mInstruments.List.ContainsKey(InstID)) {
			//	WaveList = mInstruments.List[InstID];
			//}
			//else {
			//	if (InstID.IsDrum) {
			//		if (mInstruments.List.ContainsKey(new InstID(InstID.ProgramNo, 0, 0, true))) {
			//			WaveList = mInstruments.List[new InstID(InstID.ProgramNo, 0, 0, true)];
			//		}
			//		else {
			//			WaveList = mInstruments.List[new InstID(0, 0, 0, true)];
			//		}
			//	}
			//	else {
			//		if (mInstruments.List.ContainsKey(new InstID(InstID.ProgramNo, 0, 0))) {
			//			WaveList = mInstruments.List[new InstID(InstID.ProgramNo, 0, 0)];
			//		}
			//		else {
			//			WaveList = mInstruments.List[new InstID(0, 0, 0)];
			//		}
			//	}
			//}
		}

		public void PitchBend(byte lsb, byte msb) {
			Pitch = (short)((lsb | (msb << 7)) - 8192);
			var temp = Pitch * PitchRange;
			if (temp < 0) {
				temp = -temp;
				PitchD = 1.0 / (Const.SemiTone[temp >> 13] * Const.PitchMSB[(temp >> 7) % 64] * Const.PitchLSB[temp % 128]);
			}
			else {
				PitchD = Const.SemiTone[temp >> 13] * Const.PitchMSB[(temp >> 7) % 64] * Const.PitchLSB[temp % 128];
			}
		}

		private void AllReset() {
			InstID.ProgramNo = 0;
			InstID.BankMSB = 0;
			InstID.BankLSB = 0;
			InstID.IsDrum = (9 == No);

			ProgramChange(InstID.ProgramNo);

			Vol = 100;
			Exp = 100;
			Pan = 64;
			mVolD = Const.Amp[Vol];
			mExpD = Const.Amp[Exp];
			mPanL = Const.Cos[Pan];
			mPanR = Const.Sin[Pan];

			Rev = 0;
			Cho = 0;
			Del = 0;
			mRevD = 0.0;
			mChoD = 0.0;
			mDelD = 0.0;

			Hld = 1.0;

			Fc = 127;
			Fq = 64;
			FcD = (2.0 * Const.Amp[Fc] + 0.001);
			FqD = Fq / 168.0;

			mCurCuttoff = FcD;
			mCurResonce = FqD;
			mFilter.Clear();

			mRPN_MSB = 255;
			mRPN_LSB = 255;

			PitchRange = 2;
			Pitch = 0;
			PitchD = 1.0;
		}
	}
}