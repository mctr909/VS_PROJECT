namespace MIDI {
	public class Channel {
		public readonly int No;

		public InstID InstID;
		public bool Enable;
		public bool[] Keyboard;

		public double Wave;
		public WaveInfo[] WaveList;

		private Instruments mInstruments;
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

		private int mWriteIndex;
		private int mDelayIndex;
		private int mDelaySteps;
		private double[] mDelayTapL;
		private double[] mDelayTapR;

		private double mChoLfoK;
		private double mChoLfo1Re;
		private double mChoLfo1Im;
		private double mChoLfo2Re;
		private double mChoLfo2Im;
		private double mChoLfo3Re;
		private double mChoLfo3Im;

		public Channel(int no, Instruments instruments) {
			No = no;
			
			Enable = true;
			InstID = new InstID();
			Keyboard = new bool[128];

			mInstruments = instruments;
			mDelayTapL = new double[Const.SampleRate];
			mDelayTapR = new double[Const.SampleRate];

			AllReset();
		}

		public void Step(ref double left, ref double right) {
			// Filter
			{
				mCurCuttoff += 10.0 * (FcD - mCurCuttoff) * Const.DeltaTime;
				mCurResonce += 10.0 * (FqD - mCurResonce) * Const.DeltaTime;
				if(1.0 < mCurCuttoff) {
					mCurCuttoff = 1.0;
				}
				if(1.0 < mCurResonce) {
					mCurResonce = 1.0;
				}
				mFilter.Cutoff = mCurCuttoff;
				mFilter.Resonance = mCurResonce;
				mFilter.Step(Wave, ref Wave);
			}

			var waveC = Wave * mVolD * mExpD;
			var waveL = waveC * mPanL;
			var waveR = waveC * mPanR;

			// Delay
			{
				++mWriteIndex;
				if (mDelayTapL.Length <= mWriteIndex) {
					mWriteIndex = 0;
				}
				mDelayIndex = mWriteIndex - mDelaySteps;
				if (mDelayIndex < 0) {
					mDelayIndex += mDelayTapL.Length;
				}

				var fbL = mDelD * mDelayTapL[mDelayIndex];
				var fbR = mDelD * mDelayTapR[mDelayIndex];
				waveL += 0.5 * (fbL + fbR);
				waveR += 0.5 * (fbR + fbL);
				mDelayTapL[mWriteIndex] = waveL;
				mDelayTapR[mWriteIndex] = waveR;
			}

			// Chorus
			{
				var index1 = mWriteIndex - 2000 * (0.5 - 0.48 * mChoLfo1Re);
				var index2 = mWriteIndex - 2000 * (0.5 - 0.48 * mChoLfo2Re);
				var index3 = mWriteIndex - 2000 * (0.5 - 0.48 * mChoLfo3Re);

				var indexCur1 = (int)index1;
				var indexCur2 = (int)index2;
				var indexCur3 = (int)index3;
				var indexPre1 = indexCur1 - 1;
				var indexPre2 = indexCur2 - 1;
				var indexPre3 = indexCur3 - 1;

				var dt1 = index1 - indexCur1;
				var dt2 = index2 - indexCur2;
				var dt3 = index3 - indexCur3;

				if (indexCur1 < 0) {
					indexCur1 += mDelayTapL.Length;
				}
				if (mDelayTapL.Length <= indexCur1) {
					indexCur1 -= mDelayTapL.Length;
				}
				if (indexCur2 < 0) {
					indexCur2 += mDelayTapL.Length;
				}
				if (mDelayTapL.Length <= indexCur2) {
					indexCur2 -= mDelayTapL.Length;
				}
				if (indexCur3 < 0) {
					indexCur3 += mDelayTapL.Length;
				}
				if (mDelayTapL.Length <= indexCur3) {
					indexCur3 -= mDelayTapL.Length;
				}

				if (indexPre1 < 0) {
					indexPre1 += mDelayTapL.Length;
				}
				if (mDelayTapL.Length <= indexPre1) {
					indexPre1 -= mDelayTapL.Length;
				}
				if (indexPre2 < 0) {
					indexPre2 += mDelayTapL.Length;
				}
				if (mDelayTapL.Length <= indexPre2) {
					indexPre2 -= mDelayTapL.Length;
				}
				if (indexPre3 < 0) {
					indexPre3 += mDelayTapL.Length;
				}
				if (mDelayTapL.Length <= indexPre3) {
					indexPre3 -= mDelayTapL.Length;
				}

				var chorusL1 = mDelayTapL[indexCur1] * dt1 + mDelayTapL[indexPre1] * (1.0 - dt1);
				var chorusL2 = mDelayTapL[indexCur2] * dt2 + mDelayTapL[indexPre2] * (1.0 - dt2);
				var chorusR1 = mDelayTapR[indexCur1] * dt1 + mDelayTapR[indexPre1] * (1.0 - dt1);
				var chorusR2 = mDelayTapR[indexCur3] * dt3 + mDelayTapR[indexPre3] * (1.0 - dt3);

				waveL += mChoD * (chorusL1 + chorusL2);
				waveR += mChoD * (chorusR1 + chorusR2);
				waveL *= (1.0 - 0.375 * mChoD);
				waveR *= (1.0 - 0.375 * mChoD);

				mChoLfo1Re -= mChoLfoK * mChoLfo1Im;
				mChoLfo1Im += mChoLfoK * mChoLfo1Re;
				mChoLfo2Re -= mChoLfoK * mChoLfo2Im;
				mChoLfo2Im += mChoLfoK * mChoLfo2Re;
				mChoLfo3Re -= mChoLfoK * mChoLfo3Im;
				mChoLfo3Im += mChoLfoK * mChoLfo3Re;
			}

			left += waveL;
			right += waveR;

			Wave = 0.0;
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

			if (mInstruments.List.ContainsKey(InstID)) {
				WaveList = mInstruments.List[InstID];
			}
			else {
				if (InstID.IsDrum) {
					if (mInstruments.List.ContainsKey(new InstID(InstID.ProgramNo, 0, 0, true))) {
						WaveList = mInstruments.List[new InstID(InstID.ProgramNo, 0, 0, true)];
					}
					else {
						WaveList = mInstruments.List[new InstID(0, 0, 0, true)];
					}
				}
				else {
					if (mInstruments.List.ContainsKey(new InstID(InstID.ProgramNo, 0, 0))) {
						WaveList = mInstruments.List[new InstID(InstID.ProgramNo, 0, 0)];
					}
					else {
						WaveList = mInstruments.List[new InstID(0, 0, 0)];
					}
				}
			}
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
			WaveList = mInstruments.List[InstID];

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

			mDelaySteps = (int)(0.1 * Const.SampleRate);

			// ChorusLFO
			mChoLfoK = 0.05 * 6.283185307 * Const.DeltaTime;
			mChoLfo1Re = 1.0;
			mChoLfo1Im = 0.0;
			mChoLfo2Re = System.Math.Cos(3 * 2 * System.Math.PI / 16.0);
			mChoLfo2Im = System.Math.Sin(3 * 2 * System.Math.PI / 16.0);
			mChoLfo3Re = System.Math.Cos(6 * 2 * System.Math.PI / 16.0);
			mChoLfo3Im = System.Math.Sin(6 * 2 * System.Math.PI / 16.0);
		}
	}
}