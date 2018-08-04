namespace MIDI {
	public class MessageSender {
		private const int CHANNEL_COUNT = 16;
		private const int SAMPLER_COUNT = 128;

		private Instruments mInstruments;
		private Channel[] mChannel;
		private Sampler[] mSampler;

		public Channel[] Channel {
			get { return mChannel; }
		}

		public MessageSender(Instruments instruments) {
			mInstruments = instruments;
			mChannel = new Channel[CHANNEL_COUNT];
			mSampler = new Sampler[SAMPLER_COUNT];

			for (int i = 0; i < CHANNEL_COUNT; ++i) {
				mChannel[i] = new Channel(i, mInstruments);
			}

			for (int i = 0; i < SAMPLER_COUNT; ++i) {
				mSampler[i] = new Sampler();
			}
		}

		public void SetWave(ref short[] waveBuffer) {
			int i, s, ch;
			double sumL = 0.0;
			double sumR = 0.0;

			for (i = 0; i < waveBuffer.Length; i += 2) {
				for (s = 0; s < SAMPLER_COUNT; ++s) {
					if (null == mSampler[s]) {
						continue;
					}
					if (mSampler[s].IsActive) {
						mSampler[s].Output();
					}
				}

				sumL = 0.0;
				sumR = 0.0;
				for (ch = 0; ch < CHANNEL_COUNT; ++ch) {
					mChannel[ch].Step(ref sumL, ref sumR);
				}

				if (sumL < -1.0) {
					sumL = -1.0;
				}
				else if (1.0 < sumL) {
					sumL = 1.0;
				}

				if (sumR < -1.0) {
					sumR = -1.0;
				}
				else if (1.0 < sumR) {
					sumR = 1.0;
				}

				waveBuffer[i] = (short)(32767 * sumL);
				waveBuffer[i + 1] = (short)(32767 * sumR);
			}
		}

		public void Send(Message msg) {
			switch (msg.Type) {
			case EVENT_TYPE.NOTE_OFF:
				NoteOff(mChannel[msg.Channel], msg.Byte1);
				mChannel[msg.Channel].Keyboard[msg.Byte1] = false;
				break;

			case EVENT_TYPE.NOTE_ON:
				NoteOn(mChannel[msg.Channel], msg.Byte1, msg.Byte2);
				mChannel[msg.Channel].Keyboard[msg.Byte1] = (msg.Byte2 == 0) ? false : true;
				break;

			case EVENT_TYPE.CTRL_CHG:
				mChannel[msg.Channel].ControlChange(msg.Byte1, msg.Byte2);
				break;

			case EVENT_TYPE.PRGM_CHG:
				mChannel[msg.Channel].ProgramChange(msg.Byte1);
				break;

			case EVENT_TYPE.PITCH:
				mChannel[msg.Channel].PitchBend(msg.Byte1, msg.Byte2);
				break;

			default:
				break;
			}
		}

		private void NoteOff(Channel channel, byte note) {
			for (int i = 0; i < SAMPLER_COUNT; ++i) {
				if (channel.No == mSampler[i].ChannelNo && note == mSampler[i].NoteNo) {
					mSampler[i].NoteOff();
					return;
				}
			}
		}

		private void NoteOn(Channel channel, byte note, byte velocity) {
			if (0 == velocity) {
				NoteOff(channel, note);
				return;
			}

			if (channel.InstID.IsDrum) {
				switch (note) {
				case 42:
					NoteOff(channel, 44);
					NoteOff(channel, 46);
					break;
				case 44:
					NoteOff(channel, 46);
					break;
				}
			}

			for (int i = 0; i < SAMPLER_COUNT; ++i) {
				if (channel.No == mSampler[i].ChannelNo && note == mSampler[i].NoteNo) {
					mSampler[i].NoteOn(channel, note, velocity);
					return;
				}
			}

			for (int i = 0; i < SAMPLER_COUNT; ++i) {
				if (!mSampler[i].IsActive) {
					mSampler[i].NoteOn(channel, note, velocity);
					return;
				}
			}
		}
	}
}