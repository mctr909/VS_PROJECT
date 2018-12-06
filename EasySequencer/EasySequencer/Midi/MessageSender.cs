using System;
using System.Runtime.InteropServices;

namespace MIDI {
	public enum KEY_STATUS : byte {
		OFF,
		ON,
		HOLD
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CONTROL {
		public byte vol;
		public byte exp;
		public byte pan;
		public byte reserve1;

		public byte rev;
		public byte cho;
		public byte del;
		public byte reserve2;

		public byte res;
		public byte cut;
		public byte atk;
		public byte rel;

		public byte vibRate;
		public byte vibDepth;
		public byte vibDelay;
		public byte reserve3;

		public byte bendRange;
		public byte hold;
		public byte reserve4;
		public byte reserve5;

		public byte nrpnMSB;
		public byte nrpnLSB;
		public byte rpnMSB;
		public byte rpnLSB;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	unsafe public struct CHANNEL {
		public double wave;
		public double pitch;
		public double hold;
		public double delayDepth;
		public double delayRate;
		public double chorusDepth;
		public double chorusRate;
		public double tarAmp;
		public double curAmp;
		public double panLeft;
		public double panRight;
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

		public double tarAmp;

		public double envAmp;
		public double envAmpDeltaA;
		public double envAmpDeltaD;
		public double envAmpDeltaR;
		public double envAmpLevel;
		public double envAmpHold;

		public double gain;
		public double delta;

		public double index;
		public double time;
	};

	unsafe public class MessageSender {
		
		private SAMPLER** mppSampler = null;
		private DLS.Instruments mInst = null;

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr WaveOutList();

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern bool WaveOutOpen(uint deviceId, uint sampleRate, uint bufferLength);

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern void WaveOutClose();

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		unsafe public static extern IntPtr LoadDLS(IntPtr filePath, out uint size);

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern CHANNEL** GetChannelPtr();

		[DllImport("WaveOut.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern SAMPLER** GetSamplerPtr();

		private const int CHANNEL_COUNT = 16;
		private const int SAMPLER_COUNT = 128;

		public Channel[] Channel { get; }

		public MessageSender(string dlsPath) {
			var ppChannel = GetChannelPtr();
			mppSampler = GetSamplerPtr();

			uint dlsSize;
			var dlsPtr = LoadDLS(Marshal.StringToHGlobalAuto(dlsPath), out dlsSize);
			mInst = new DLS.Instruments(dlsPtr, dlsSize, Const.SampleRate);

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

					pSmpl->envAmp = 0.0;
					pSmpl->envAmpDeltaA = 256 / wave.envAmp.attackTime;
					pSmpl->envAmpDeltaD = 96 / wave.envAmp.decayTime;
					pSmpl->envAmpDeltaR = 96 / wave.envAmp.releaceTime;
					pSmpl->envAmpLevel = wave.envAmp.sustainLevel;
					pSmpl->envAmpHold = wave.envAmp.holdTime;

					if (Const.SampleRate < pSmpl->envAmpDeltaA) {
						pSmpl->envAmpDeltaA = Const.SampleRate;
					}
					if (Const.SampleRate < pSmpl->envAmpDeltaD) {
						pSmpl->envAmpDeltaD = Const.SampleRate;
					}
					if (Const.SampleRate < pSmpl->envAmpDeltaR) {
						pSmpl->envAmpDeltaR = Const.SampleRate;
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

					pSmpl->onKey = true;
					pSmpl->isActive = true;
				}

				ch.KeyBoard[noteNo] = KEY_STATUS.ON;

				return;
			}
		}
	}
}