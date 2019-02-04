using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Test {
	public partial class Form1 : Form {

		[DllImport("MidiToWave.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr WaveOutList();

		[DllImport("MidiToWave.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern bool WaveOutOpen(uint deviceId, uint sampleRate, uint bufferLength);

		[DllImport("MidiToWave.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern void WaveOutClose();

		[DllImport("MidiToWave.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern void DlsLoad(IntPtr filePath);

		[DllImport("MidiToWave.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern void SendMidi(IntPtr message);

		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			textBox1.Text = Marshal.PtrToStringAuto(WaveOutList());
		}

		private void button1_Click(object sender, EventArgs e) {
			WaveOutOpen(0xFFFF, 44100, 4096);
		}

		private void button2_Click(object sender, EventArgs e) {
			WaveOutClose();
		}

		private void button3_Click(object sender, EventArgs e) {
			DlsLoad(Marshal.StringToCoTaskMemAuto("C:\\Users\\owner\\Desktop\\gm.dls"));
		}

		unsafe private void button4_Click(object sender, EventArgs e) {
			Task.Run(() => {
				var events1 = new byte[][] {
					new byte[] { 0xB0, 0x00, 0x01 },
					new byte[] { 0xB0, 0x20, 0x00 },
					new byte[] { 0xC0, 0x50 },

					new byte[] { 0x90, 0x40, 0x60 },
					new byte[] { 0x90, 0x43, 0x60 },
					new byte[] { 0x90, 0x47, 0x60 },
					new byte[] { 0x90, 0x4A, 0x60 },
					new byte[] { 0x90, 0x4C, 0x60 }
				};

				var events2 = new byte[][] {
					new byte[] { 0x80, 0x40 },
					new byte[] { 0x80, 0x43 },
					new byte[] { 0x80, 0x47 },
					new byte[] { 0x80, 0x4A },
					new byte[] { 0x80, 0x4C }
				};

				foreach (var msg in events1) {
					fixed (byte* p = &msg[0]) {
						SendMidi((IntPtr)p);
					}
					Thread.Sleep(250);
				}

				Thread.Sleep(1000);

				foreach (var msg in events2) {
					fixed (byte* p = &msg[0]) {
						SendMidi((IntPtr)p);
					}
					Thread.Sleep(100);
				}
			});
		}
	}
}
