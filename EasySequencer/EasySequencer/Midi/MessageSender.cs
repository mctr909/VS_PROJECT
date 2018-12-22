using System;
using System.Runtime.InteropServices;

namespace MIDI {
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FILTER {
		public double cutoff;
		public double resonance;
		public double pole00;
		public double pole01;
		public double pole02;
		public double pole03;
		public double pole10;
		public double pole11;
		public double pole12;
		public double pole13;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DELAY {
		public double depth;
		public double rate;
		private IntPtr pTapL;
		private IntPtr pTapR;
		private Int32 writeIndex;
		private Int32 readIndex;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CHORUS {
		public double depth;
		public double rate;
		private double lfoK;
		private IntPtr pPanL;
		private IntPtr pPanR;
		private IntPtr pLfoRe;
		private IntPtr pLfoIm;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ENVELOPE {
		public double levelA;
		public double levelD;
		public double levelS;
		public double levelR;
		public double deltaA;
		public double deltaD;
		public double deltaR;
		public double hold;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	unsafe public struct CHANNEL {
		public double wave;
		public double waveL;
		public double waveR;

		public double pitch;
		public double hold;

		public double panLeft;
		public double panRight;

		public double tarCutoff;
		public double tarAmp;
		public double curAmp;

		public FILTER eq;
		public DELAY delay;
		public CHORUS chorus;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	unsafe public struct SAMPLER {
		public ushort channelNo;
		public ushort noteNo;

		public bool onKey;
		public bool isActive;

		public uint pcmAddr;
		public uint pcmLength;

		public bool loopEnable;
		public uint loopBegin;
		public uint loopLength;

		public double gain;
		public double delta;

		public double index;
		public double time;

		public double tarAmp;
		public double curAmp;

		public ENVELOPE envAmp;
		public ENVELOPE envEq;

		public FILTER eq;
	};

	unsafe public class MessageSender {

		private SAMPLER** mppSampler = null;
		private Instruments mInst = null;

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr WaveOutList();

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern bool WaveOutOpen(uint deviceId, uint sampleRate, uint bufferLength);

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern void WaveOutClose();

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		unsafe public static extern IntPtr LoadDLS(IntPtr filePath, out uint size, int sampleRate);

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern CHANNEL** GetChannelPtr();

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern SAMPLER** GetSamplerPtr();

		private const int CHANNEL_COUNT = 16;
		private const int SAMPLER_COUNT = 256;

		public Channel[] Channel { get; }

		public MessageSender(string dlsPath) {
			var ppChannel = GetChannelPtr();
			mppSampler = GetSamplerPtr();

			uint dlsSize;
			var dlsPtr = LoadDLS(Marshal.StringToHGlobalAuto(dlsPath), out dlsSize, Const.SampleRate);
			mInst = new Instruments(dlsPtr, dlsSize, Const.SampleRate);

			Channel = new Channel[CHANNEL_COUNT];
			for (int i = 0; i < CHANNEL_COUNT; ++i) {
				Channel[i] = new Channel(mInst, ppChannel[i], i);
			}

			WaveOutOpen(0xFFFF, (uint)Const.SampleRate, 256);
		}

		public void Send(Message msg) {
			switch (msg.Type) {
			case EVENT_TYPE.NOTE_OFF:
				noteOff(Channel[msg.Channel], msg.Byte1);
				break;

			case EVENT_TYPE.NOTE_ON:
				noteOn(Channel[msg.Channel], msg.Byte1, msg.Byte2);
				break;

			case EVENT_TYPE.CTRL_CHG:
				Channel[msg.Channel].CtrlChange(msg.Byte1, msg.Byte2);
				break;

			case EVENT_TYPE.PRGM_CHG:
				Channel[msg.Channel].ProgramChange(msg.Byte1);
				break;

			case EVENT_TYPE.PITCH:
				Channel[msg.Channel].PitchBend(msg.Byte1, msg.Byte2);
				break;

			default:
				break;
			}
		}

		private void noteOff(Channel ch, byte noteNo) {
			for (var i = 0; i < SAMPLER_COUNT; ++i) {
				if (mppSampler[i]->channelNo == ch.No && mppSampler[i]->noteNo == noteNo) {
					if (ch.Hld < 64) {
						ch.KeyBoard[noteNo] = KEY_STATUS.OFF;
					}
					else {
						ch.KeyBoard[noteNo] = KEY_STATUS.HOLD;
					}
					mppSampler[i]->onKey = false;
				}
			}
		}

		private void noteOn(Channel ch, byte noteNo, byte velocity) {
			if (0 == velocity) {
				noteOff(ch, noteNo);
				return;
			}

			for (var i = 0; i < SAMPLER_COUNT; ++i) {
				var pSmpl = mppSampler[i];
				if (pSmpl->isActive) {
					continue;
				}

				var wave = mInst.List[ch.InstId][noteNo];
				{
					pSmpl->channelNo = ch.No;
					pSmpl->noteNo = noteNo;

					pSmpl->pcmAddr = wave.pcmAddr;
					pSmpl->pcmLength = wave.pcmLength;

					pSmpl->loopEnable = wave.loopEnable;
					pSmpl->loopBegin = wave.loop.start;
					pSmpl->loopLength = wave.loop.length;

					pSmpl->tarAmp = velocity / 127.0;
					pSmpl->curAmp = 0.0;

					pSmpl->envAmp.levelA = 0.0;
					pSmpl->envAmp.levelD = 1.0;
					pSmpl->envAmp.levelS = wave.envAmp.sustainLevel;
					pSmpl->envAmp.levelR = 0.0;
					pSmpl->envAmp.deltaA = 112 / wave.envAmp.attackTime;
					pSmpl->envAmp.deltaD = 16 / wave.envAmp.decayTime;
					pSmpl->envAmp.deltaR = 16 / wave.envAmp.releaceTime;
					pSmpl->envAmp.hold = wave.envAmp.holdTime;

					if (Const.SampleRate < pSmpl->envAmp.deltaA) {
						pSmpl->envAmp.deltaA = Const.SampleRate;
					}
					if (Const.SampleRate < pSmpl->envAmp.deltaD) {
						pSmpl->envAmp.deltaD = Const.SampleRate;
					}
					if (Const.SampleRate < pSmpl->envAmp.deltaR) {
						pSmpl->envAmp.deltaR = Const.SampleRate;
					}

					pSmpl->gain = wave.gain;

					var diffNote = (int)noteNo - wave.unityNote;
					if (diffNote < 0) {
						pSmpl->delta = wave.delta / Const.SemiTone[-diffNote];
					}
					else {
						pSmpl->delta = wave.delta * Const.SemiTone[diffNote];
					}

					pSmpl->index = 0.0;
					pSmpl->time = 0.0;

					//if (ch.No == 9 || ch.InstId.programNo < 33 || 40 < ch.InstId.programNo) {
					pSmpl->envEq.levelA = 1.0;
					pSmpl->envEq.levelD = 1.0;
					pSmpl->envEq.levelS = 1.0;
					pSmpl->envEq.levelR = 1.0;
					pSmpl->envEq.deltaA = 1000;
					pSmpl->envEq.deltaD = 1000;
					pSmpl->envEq.deltaR = 1000;
					pSmpl->envEq.hold = 0.0;
					pSmpl->eq.resonance = 0.0;
					//}
					//else {
					//	pSmpl->envEq.levelA = 0.5;
					//	pSmpl->envEq.levelD = 0.25;
					//	pSmpl->envEq.levelS = 0.01;
					//	pSmpl->envEq.levelR = 0.01;
					//	pSmpl->envEq.deltaA = 100;
					//	pSmpl->envEq.deltaD = 20;
					//	pSmpl->envEq.deltaR = 20;
					//	pSmpl->envEq.hold = 0.03;
					//	pSmpl->eq.resonance = 0.25;
					//}

					pSmpl->eq.cutoff = pSmpl->envEq.levelA;
					pSmpl->eq.pole00 = 0.0;
					pSmpl->eq.pole01 = 0.0;
					pSmpl->eq.pole02 = 0.0;
					pSmpl->eq.pole03 = 0.0;
					pSmpl->eq.pole10 = 0.0;
					pSmpl->eq.pole11 = 0.0;
					pSmpl->eq.pole12 = 0.0;
					pSmpl->eq.pole13 = 0.0;

					pSmpl->onKey = true;
					pSmpl->isActive = true;
				}

				ch.KeyBoard[noteNo] = KEY_STATUS.ON;

				return;
			}
		}
	}
}