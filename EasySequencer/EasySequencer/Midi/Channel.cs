namespace MIDI {
	public class Channel {
		public readonly int No;
		public readonly double DeltaTime;

		public InstID InstID;
		public bool Enable;
		public bool[] Keyboard;

		public double Wave;

		private double mVolD;
		private byte mVolB;
		public byte Vol { get { return mVolB; } }

		private double mExpD;
		private byte mExpB;
		public byte Exp { get { return mExpB; } }

		private double mPanL;
		private double mPanR;
		private byte mPanB;
		public byte Pan { get { return mPanB; } }

		private double mRevD;
		private byte mRevB;
		public byte Rev { get { return mRevB; } }

		private double mDelD;
		private byte mDelB;
		public byte Del { get { return mDelB; } }

		private double mChoD;
		private byte mChoB;
		public byte Cho { get { return mChoB; } }

		private double mHldD;
		public double Hld { get { return mHldD; } }

		private double mFcD;
		private byte mFcB;
		public double Fc { get { return mFcD; } }

		private double mFqD;
		private byte mFqB;
		public double Fq { get { return mFqD; } }

		private byte mRPN_MSB;
		private byte mRPN_LSB;

		private short mPitchS;
		private double mPitch;
		public byte PitchRange;
		public short Pitch {
			get { return mPitchS; }
			set {
				mPitchS = value;
				var temp = mPitchS * PitchRange;
				if (temp < 0) {
					temp = -temp;
					mPitch = 1.0 / (Const.SemiTone[temp >> 13] * Const.PitchMSB[(temp >> 7) % 64] * Const.PitchLSB[temp % 128]);
				}
				else {
					mPitch = Const.SemiTone[temp >> 13] * Const.PitchMSB[(temp >> 7) % 64] * Const.PitchLSB[temp % 128];
				}
			}
		}
		public double PitchD { get { return mPitch; } }

		public Envelope EnvAmp;
		public Envelope EnvCutoff;

		private InstTable mInstTable;
		public WaveInfo[] WaveList;

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

		public Channel(int no, int sampleRate, InstTable instTable) {
			No = no;
			DeltaTime = 1.0 / sampleRate;

			Enable = true;
			InstID = new InstID();
			Keyboard = new bool[128];

			mInstTable = instTable;

			mDelayTapL = new double[sampleRate];
			mDelayTapR = new double[sampleRate];

			mChoLfoK = 0.25 * 6.283185307 * DeltaTime;
			mChoLfo1Re = 1.0;
			mChoLfo1Im = 0.0;
			mChoLfo2Re = System.Math.Cos(3 * 2 * System.Math.PI / 16.0);
			mChoLfo2Im = System.Math.Sin(3 * 2 * System.Math.PI / 16.0);
			mChoLfo3Re = System.Math.Cos(6 * 2 * System.Math.PI / 16.0);
			mChoLfo3Im = System.Math.Sin(6 * 2 * System.Math.PI / 16.0);

			AllReset();
		}

		public void AllReset() {
			InstID.ProgramNo = 0;
			InstID.BankMSB = 0;
			InstID.BankLSB = 0;
			InstID.IsDrum = (9 == No);
			WaveList = mInstTable.InstList[InstID].WaveInfo;

			Const.ChgInst(this);

			PrgmChg(InstID.ProgramNo);

			mVolB = 100;
			mExpB = 100;
			mPanB = 64;
			mVolD = Const.Amp[mVolB];
			mExpD = Const.Amp[mExpB];
			mPanL = Const.Cos[mPanB];
			mPanR = Const.Sin[mPanB];

			mRevB = 0;
			mChoB = 0;
			mDelB = 0;
			mRevD = 0.0;
			mChoD = 0.0;
			mDelD = 0.0;

			mHldD = 1.0;

			mFcB = 64;
			mFqB = 64;
			mFcD = mFcB / 64.0;
			mFqD = mFqB / 254.0;

			mRPN_MSB = 255;
			mRPN_LSB = 255;

			Pitch = 0;
			PitchRange = 2;

			mDelaySteps = (int)(0.125 / DeltaTime);
		}

		public void Step(ref double left, ref double right) {
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
				var index1 = mWriteIndex - 600 * (0.5 - 0.49 * mChoLfo1Re);
				var index2 = mWriteIndex - 600 * (0.5 - 0.49 * mChoLfo2Re);
				var index3 = mWriteIndex - 600 * (0.5 - 0.49 * mChoLfo3Re);

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
				if (indexCur2 < 0) {
					indexCur2 += mDelayTapL.Length;
				}
				if (indexCur3 < 0) {
					indexCur3 += mDelayTapL.Length;
				}

				if (indexPre1 < 0) {
					indexPre1 += mDelayTapL.Length;
				}
				if (indexPre2 < 0) {
					indexPre2 += mDelayTapL.Length;
				}
				if (indexPre3 < 0) {
					indexPre3 += mDelayTapL.Length;
				}

				var chorusL1 = mDelayTapL[indexCur1] * dt1 + mDelayTapL[indexPre1] * (1.0 - dt1);
				var chorusL2 = mDelayTapL[indexCur2] * dt2 + mDelayTapL[indexPre2] * (1.0 - dt2);
				var chorusR1 = mDelayTapR[indexCur1] * dt1 + mDelayTapR[indexPre1] * (1.0 - dt1);
				var chorusR2 = mDelayTapR[indexCur3] * dt3 + mDelayTapR[indexPre3] * (1.0 - dt3);

				waveL += mChoD * (chorusL1 + chorusL2);
				waveR += mChoD * (chorusR1 + chorusR2);
				waveL *= (1.0 - 0.3 * mChoD);
				waveR *= (1.0 - 0.3 * mChoD);

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

		public void PrgmChg(byte value) {
			InstID.ProgramNo = value;
			InstInfo instInfo;

			if (mInstTable.InstList.ContainsKey(InstID)) {
				instInfo = mInstTable.InstList[InstID];
				Const.ChgInst(this);
			}
			else {
				if (InstID.IsDrum) {
					if (mInstTable.InstList.ContainsKey(new InstID(InstID.ProgramNo, 0, 0, true))) {
						instInfo = mInstTable.InstList[new InstID(InstID.ProgramNo, 0, 0, true)];
					}
					else {
						instInfo = mInstTable.InstList[new InstID(0, 0, 0, true)];
					}
				}
				else {
					instInfo = mInstTable.InstList[new InstID(InstID.ProgramNo, 0, 0)];
					Const.ChgInst(this);
				}
			}

			WaveList = instInfo.WaveInfo;
			EnvAmp = instInfo.EnvAmp;
			EnvCutoff = instInfo.EnvFilter;
		}

		public void CtrlChg(byte type, byte value) {
			switch ((CTRL_TYPE)type) {
			case CTRL_TYPE.BANK_MSB:
				InstID.BankMSB = value;
				break;
			case CTRL_TYPE.BANK_LSB:
				InstID.BankLSB = value;
				break;

			case CTRL_TYPE.VOLUME:
				mVolB = value;
				mVolD = Const.Amp[value];
				break;
			case CTRL_TYPE.EXPRESSION:
				mExpB = value;
				mExpD = Const.Amp[value];
				break;
			case CTRL_TYPE.PAN:
				mPanB = value;
				mPanL = Const.Cos[value];
				mPanR = Const.Sin[value];
				break;

			case CTRL_TYPE.REVERB:
				mRevB = value;
				mRevD = value / 127.0;
				break;
			case CTRL_TYPE.CHORUS:
				mChoB = value;
				mChoD = Const.FeedBack[value];
				break;
			case CTRL_TYPE.DELAY:
				mDelB = value;
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
				break;

			case CTRL_TYPE.HOLD:
				mHldD = (value < 64) ? 1.0 : 0.1;
				break;

			case CTRL_TYPE.CUTOFF:
				mFcB = value;
				mFcD = value / 64.0;
				break;
			case CTRL_TYPE.RESONANCE:
				mFqB = value;
				mFqD = value / 254.0;
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
	}
}