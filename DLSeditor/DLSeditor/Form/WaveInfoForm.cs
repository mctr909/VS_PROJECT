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
		private DLS.File mDLS;
		private int mIndex;
		private WavePlayback mWaveOut;

		private Task mTaskWave;
		private Task mTaskSpec;
		private byte[][] mSpectrum;
		private byte[][] mColors;
		private short[] mWave;
		private double mScale;
		private double mDelta;
		private double mTimeDiv;

		public WaveInfoForm(WavePlayback waveOut, DLS.File dls, int index)
		{
			InitializeComponent();
			StartPosition = FormStartPosition.CenterParent;
			mWaveOut = waveOut;
			mDLS = dls;
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

			mColors = new byte[256][];
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
				mColors[i] = new byte[] { (byte)r, (byte)g, (byte)b };
				vColor += dColor;
			}

			timer1.Interval = 30;
			timer1.Enabled = true;
			timer1.Start();
		}

		private void WaveInfoForm_Load(object sender, EventArgs e)
		{
			InitWave();
			numScale.Value = 64;
			hsbTime.Value = 0;
			hsbTime.Maximum = (int)(mWave.Length * mTimeDiv * Math.Pow(2.0 , ((double)numScale.Value - 32.0) / 16.0));
			mScale = 1.0;
			mTaskWave = Task.Factory.StartNew(() => DrawWave());
			mTaskSpec = Task.Factory.StartNew(() => DrawSpec());
		}

		private void btnPlay_Click(object sender, EventArgs e)
		{
			if ("再生" == btnPlay.Text) {
				mWaveOut.SetValue(mDLS.WavePool[mIndex]);
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
			mScale = Math.Pow(2.0, ((double)numScale.Value - 32.0) / 16.0);
			hsbTime.Value = 0;
			hsbTime.Maximum = (int)(mWave.Length * mTimeDiv * mScale);
		}

		private void timer1_Tick(object sender, EventArgs e)
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
			var wave = mDLS.WavePool[mIndex];
			if (null != wave.Info && !string.IsNullOrWhiteSpace(wave.Info.Name)) {
				Text = wave.Info.Name;
			}

			var ms = new MemoryStream(wave.Data);
			var br = new BinaryReader(ms);
			var samples = 8 * wave.Data.Length / wave.Format.BitsPerSample;
			var packSize = 128;
			samples += packSize - (samples % packSize);

			mWave = new short[samples];
			switch (wave.Format.BitsPerSample) {
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
			br.Close();
			br.Dispose();

			mDelta = wave.Format.SampleRate / 44100.0;
			mTimeDiv = 1.0 / mDelta / packSize;
			mSpectrum = new byte[(int)(mWave.Length * mTimeDiv)][];

			var sp = new Spectrum(44100, 27.5, 24, 232);
			var time = 0.0;
			for (var s = 0; s < mSpectrum.Length; ++s) {
				for (var i = 0; i < packSize && time < mWave.Length; ++i) {
					var w = mWave[(int)time] / 32768.0;
					for (uint b = 0; b < sp.Banks; ++b) {
						sp.Filtering(b, w);
					}
					time += mDelta;
				}

				sp.SetLevel();
				var level = sp.Level;
				mSpectrum[s] = new byte[sp.Banks];
				for (var b = 0; b < sp.Banks; ++b) {
					var lv = level[b] / sp.Max;
					mSpectrum[s][b] = (byte)(1.0 < lv ? 255 : (255 * lv));
				}
			}
		}

		unsafe private void DrawSpec()
		{
			var bmp = new Bitmap(picSpectrum.Width, picSpectrum.Height, PixelFormat.Format24bppRgb);
			BitmapData bmpData = bmp.LockBits(
				new Rectangle(0, 0, bmp.Width, bmp.Height),
				ImageLockMode.WriteOnly,
				bmp.PixelFormat
			);

			int y, x;
			var pix = (byte*)bmpData.Scan0.ToPointer();
			for (y = bmp.Height - 1; 0 <= y; --y) {
				for (x = 0; x < bmp.Width; ++x) {
					var sx = (int)((x + hsbTime.Value) / mScale);
					if (sx < mSpectrum.Length) {
						*pix = mColors[mSpectrum[sx][y]][2];
						*(pix + 1) = mColors[mSpectrum[sx][y]][1];
						*(pix + 2) = mColors[mSpectrum[sx][y]][0];
					}
					pix += 3;
				}
			}
			bmp.UnlockBits(bmpData);

			if (null != picSpectrum.Image) {
				picSpectrum.Image.Dispose();
				picSpectrum.Image = null;
			}
			picSpectrum.BackColor = Color.Black;
			picSpectrum.Image = bmp;

			Thread.Sleep(50);
			DrawSpec();
		}

		private void DrawWave()
		{
			var bmpW = new Bitmap(picWave.Width, picWave.Height);
			var gw = Graphics.FromImage(bmpW);

			var scale = mScale * mTimeDiv;
			var amp = bmpW.Height - 1;

			var green = new Pen(Color.FromArgb(0, 168, 0), 1.0f);

			for (int t1 = hsbTime.Value, t2 = t1 + 1; t2 < mWave.Length; ++t1, ++t2) {
				var v1 = (0.5 - 0.5 * mWave[t1] / 32768.0);
				var v2 = (0.5 - 0.5 * mWave[t2] / 32768.0);
				if (v1 < 0.0) {
					v1 = 0.0;
				}
				else if (1.0 < v1) {
					v1 = 1.0;
				}
				if (v2 < 0.0) {
					v2 = 0.0;
				}
				else if (1.0 < v2) {
					v2 = 1.0;
				}

				var x1 = (float)(t1 * scale - hsbTime.Value);
				var y1 = (float)(v1 * amp);
				var x2 = (float)(t2 * scale - hsbTime.Value);
				var y2 = (float)(v2 * amp);
				if (bmpW.Width <= x2) {
					break;
				}
				gw.DrawLine(green, x1, y1, x2, y2);
			}

			gw.DrawLine(Pens.Red, 0, picWave.Height / 2.0f - 1, picWave.Width - 1, picWave.Height / 2.0f - 1);
			if (null != picWave.Image) {
				picWave.Image.Dispose();
				picWave.Image = null;
			}
			picWave.BackColor = Color.Black;
			picWave.Image = bmpW;
			gw.Dispose();

			Thread.Sleep(30);
			DrawWave();
		}
	}
}
