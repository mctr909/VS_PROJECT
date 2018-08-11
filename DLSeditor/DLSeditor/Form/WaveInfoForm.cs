using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace DLSeditor
{
	public partial class WaveInfoForm : Form
	{
		private DLS.DLS mFile;
		private int mIndex;
		private WavePlayback mWaveOut;

		private byte[][] mSpectrogram;
		private UInt32[] mColors;
		private short[] mWave;
		private double mScale;
		private double mTimeDiv;
		private double mDelta;

		public WaveInfoForm(WavePlayback waveOut, DLS.DLS dls, int index)
		{
			InitializeComponent();
			StartPosition = FormStartPosition.CenterParent;
			mWaveOut = waveOut;
			mFile = dls;
			mIndex = index;

			chart1.BackColor = Color.Black;
			chart1.ChartAreas[0].BackColor = Color.Black;
			chart1.ChartAreas[0].Axes[0].LabelStyle.Interval = 12;
			chart1.ChartAreas[0].Axes[0].LabelStyle.Enabled = false;
			chart1.ChartAreas[0].Axes[1].LabelStyle.Enabled = false;
			chart1.ChartAreas[0].AxisX.LineColor = Color.FromArgb(255, 0, 0);
			chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(128, 0, 0);
			chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
			chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 12;
			chart1.ChartAreas[0].AxisY.LineColor = Color.FromArgb(255, 0, 0);
			chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(128, 0, 0);
			chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
			chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 0.125;
			chart1.Legends.Clear();

			mColors = new UInt32[256];
			var dColor = 1280.0 / mColors.Length;
			var vColor = 0.0;
			for (int i = 0; i < mColors.Length; ++i) {
				var r = 0;
				var g = 0;
				var b = 0;

				if (vColor < 256.0) {
					b = (int)vColor;
				}
				else if (vColor < 512.0) {
					b = 255;
					g = (int)vColor - 256;
				}
				else if (vColor < 768.0) {
					b = 255 - (int)(vColor - 512);
					g = 255;
				}
				else if (vColor < 1024.0) {
					g = 255;
					r = (int)vColor - 768;
				}
				else {
					g = 255 - (int)(vColor - 1024);
					r = 255;
				}
				mColors[i] = (UInt32)((r << 16) | (g << 8) | b);
				vColor += dColor;
			}

			timer1.Interval = 20;
			timer1.Enabled = true;
			timer1.Start();

			timer2.Interval = 20;
			timer2.Enabled = true;
			timer2.Start();
		}

		private void WaveInfoForm_Load(object sender, EventArgs e)
		{
			InitWave();
			mScale = Math.Pow(2.0, ((double)numScale.Value - 32.0) / 16.0);
		}

		private void btnPlay_Click(object sender, EventArgs e)
		{
			if ("再生" == btnPlay.Text) {
				mWaveOut.SetValue(mFile.WavePool.List[mIndex]);
				btnPlay.Text = "停止";
			}
			else {
				mWaveOut.Stop();
				btnPlay.Text = "再生";
			}
		}

		private void WaveInfoForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (null != mWaveOut) {
				mWaveOut.Stop();
			}
		}

		private void numScale_ValueChanged(object sender, EventArgs e)
		{
			mScale = Math.Pow(2.0, ((double)numScale.Value - 32.0) / 4.0);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			hsbTime.Width = Width - hsbTime.Left - 24;
			picWave.Width = Width - picWave.Left - 24;
			picSpectrum.Width = Width - picSpectrum.Left - 24;
			DrawWave();
			DrawSpec();
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			Series t = new Series();
			t.Label = "";
			t.ChartType = SeriesChartType.Line;
			t.Points.AddXY(mWaveOut.Spectrum.Banks, 0.95);

			Series s = new Series("amp");
			s.Label = "";
			s.ChartType = SeriesChartType.Spline;
			s.Color = Color.FromArgb(0, 255, 0);
			for (int i = 0; i < mWaveOut.Spectrum.Banks; i++) {
				var temp = mWaveOut.Spectrum.Level[i] / mWaveOut.Spectrum.Max;
				s.Points.AddXY(i, temp);
			}

			chart1.Series.Clear();
			chart1.Series.Add(t);
			chart1.Series.Add(s);
		}

		private void InitWave()
		{
			var wave = mFile.WavePool.List[mIndex];
			if (null != wave.Text && !string.IsNullOrWhiteSpace(wave.Text.Name)) {
				Text = wave.Text.Name;
			}

			var ms = new MemoryStream(wave.Data);
			var br = new BinaryReader(ms);
			var samples = 8 * wave.Data.Length / wave.Format.Bits;
			var packSize = 128;
			samples += packSize - (samples % packSize);

			mWave = new short[samples];
			switch (wave.Format.Bits) {
			case 8:
				for (var i = 0; ms.Position < ms.Length; ++i) {
					mWave[i] = (short)(256 * (br.ReadByte() - 128));
				}
				break;
			case 16:
				for (var i = 0; ms.Position < ms.Length; ++i) {
					mWave[i] = br.ReadInt16();
				}
				break;
			}

			hsbTime.Value = 0;
			hsbTime.Maximum = samples;

			br.Close();
			br.Dispose();

			mDelta = wave.Format.SampleRate / 44100.0;
			mTimeDiv = 1.0 / mDelta / packSize;
			mSpectrogram = new byte[(int)(mWave.Length * mTimeDiv)][];

			var sp = new Spectrum(44100, 27.5, 24, 224);
			var time = 0.0;
			for (var s = 0; s < mSpectrogram.Length; ++s) {
				for (var i = 0; i < packSize && time < mWave.Length; ++i) {
					var w = mWave[(int)time] / 32768.0;
					for (uint b = 0; b < sp.Banks; ++b) {
						sp.Filtering(b, w);
					}
					time += mDelta;
				}

				sp.SetLevel();
				var level = sp.Level;
				var amp = mColors.Length - 1;
				mSpectrogram[s] = new byte[sp.Banks];
				for (var b = 0; b < sp.Banks; ++b) {
					var lv = level[b] / sp.Max;
					mSpectrogram[s][b] = (byte)(1.0 < lv ? amp : (amp * lv));
				}
			}
		}

		private void DrawWave()
		{
			var bmp = new Bitmap(picWave.Width, picWave.Height, PixelFormat.Format16bppRgb555);
			var gM = Graphics.FromImage(bmp);

			var amp = bmp.Height - 1;

			var green = new Pen(Color.FromArgb(0, 168, 0), 1.0f);
			var blue = new Pen(Color.FromArgb(0, 0, 255), 1.0f);

			var wave = mFile.WavePool.List[mIndex];
			var loopBegin = -1;
			var loopEnd = -1;
			if (0 < wave.Sampler.LoopCount) {
				loopBegin = (int)wave.Loops[0].Start;
				loopEnd = loopBegin + (int)wave.Loops[0].Length - 1;
			}

			//
			var begin = hsbTime.Value;
			var end = hsbTime.Value + bmp.Width / mScale + 1;
			for (int t1 = begin, t2 = begin + 1; t2 < end; ++t1, ++t2) {
				if (t1 < 0) {
					continue;
				}
				if (mWave.Length <= t2) {
					break;
				}

				var y1 = (float)(amp * (0.5 - 0.5 * mWave[t1] / 32768.0));
				var y2 = (float)(amp * (0.5 - 0.5 * mWave[t2] / 32768.0));
				var x1 = (float)(mScale * (t1 - begin));
				var x2 = (float)(mScale * (t2 - begin));

				if (loopBegin <= t1 && t2 <= loopEnd) {
					gM.DrawLine(blue, x1, y1, x2, y2);
				}
				else {
					gM.DrawLine(green, x1, y1, x2, y2);
				}
			}

			//
			gM.DrawLine(Pens.Red, 0, picWave.Height / 2.0f - 1, picWave.Width - 1, picWave.Height / 2.0f - 1);

			if (null != picWave.Image) {
				picWave.Image.Dispose();
				picWave.Image = null;
			}
			picWave.BackColor = Color.Black;
			picWave.Image = bmp;
			gM.Dispose();
		}

		unsafe private void DrawSpec()
		{
			var bmp = new Bitmap(picSpectrum.Width, picSpectrum.Height, PixelFormat.Format32bppRgb);
			BitmapData bmpData = bmp.LockBits(
				new Rectangle(0, 0, bmp.Width, bmp.Height),
				ImageLockMode.WriteOnly,
				bmp.PixelFormat
			);
			var pix = (UInt32*)bmpData.Scan0.ToPointer();

			int y, x;
			double begin = hsbTime.Value * mTimeDiv;
			double scale = mTimeDiv / mScale;
			for (y = bmp.Height - 1; 0 <= y; --y) {
				for (x = 0; x < bmp.Width; ++x) {
					var sx = (int)(begin + scale * x);
					if (sx < mSpectrogram.Length) {
						*pix = mColors[mSpectrogram[sx][y]];
					}
					++pix;
				}
			}
			bmp.UnlockBits(bmpData);

			if (null != picSpectrum.Image) {
				picSpectrum.Image.Dispose();
				picSpectrum.Image = null;
			}
			picSpectrum.BackColor = Color.Black;
			picSpectrum.Image = bmp;
		}
	}
}
