using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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
			WaveOutOpen(0, 44100, 4096);
		}

		private void button2_Click(object sender, EventArgs e) {
			WaveOutClose();
		}

		private void button3_Click(object sender, EventArgs e) {
			DlsLoad(Marshal.StringToCoTaskMemAuto("C:\\Users\\owner\\Desktop\\gm.dls"));
		}

		private void button4_Click(object sender, EventArgs e) {

		}
	}
}
