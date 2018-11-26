using System;
using System.Runtime.InteropServices;

namespace MIDI {
	public class MessageSender {

		[DllImport("MidiToWave.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr WaveOutList();

		[DllImport("MidiToWave.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern bool WaveOutOpen(uint deviceId, uint sampleRate, uint bufferLength);

		[DllImport("MidiToWave.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern void WaveOutClose();

		[DllImport("MidiToWave.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern void DlsLoad(IntPtr filePath);

		[DllImport("MidiToWave.dll")]
		unsafe public static extern void SendMidi(UInt32 message);

		private const int CHANNEL_COUNT = 16;

		public Channel[] Channel { get; }

		public MessageSender(string dlsPath) {
			Channel = new Channel[CHANNEL_COUNT];

			for (int i = 0; i < CHANNEL_COUNT; ++i) {
				Channel[i] = new Channel(i);
			}

			WaveOutOpen(0xFFFF, 44100, 4096);
			DlsLoad(Marshal.StringToCoTaskMemAuto(dlsPath));
		}

		unsafe public void Send(Message msg) {
			switch (msg.Type) {
			case EVENT_TYPE.NOTE_OFF:
				Channel[msg.Channel].Keyboard[msg.Byte1] = false;
				break;

			case EVENT_TYPE.NOTE_ON:
				Channel[msg.Channel].Keyboard[msg.Byte1] = (msg.Byte2 == 0) ? false : true;
				break;

			case EVENT_TYPE.CTRL_CHG:
				Channel[msg.Channel].ControlChange(msg.Byte1, msg.Byte2);
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

			SendMidi(msg.Bytes);
		}
	}
}