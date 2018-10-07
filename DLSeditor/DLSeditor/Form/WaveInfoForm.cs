using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace DLSeditor
{
	public partial class WaveInfoForm : Form
	{
		private DLS.DLS mFile;
		private int mIndex;
		private WavePlayback mWaveOut;

		private byte[][] mSpectrogram;
		private uint[] mColors;
		private short[] mWave;
		private double mScale;
		private double mTimeDiv;
		private double mDelta;
		private Point mCursolPos;
		private bool onLoopDrag;
		private int mLoopBegin;
		private int mLoopEnd;

		public WaveInfoForm(WavePlayback waveOut, DLS.DLS dls, int index)
		{
			InitializeComponent();
			StartPosition = FormStartPosition.CenterParent;
			mWaveOut = waveOut;
			mFile = dls;
			mIndex = index;

			mColors = new uint[256];
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
				mColors[i] = (uint)((r << 16) | (g << 8) | b);
				vColor += dColor;
			}

			timer1.Interval = 20;
			timer1.Enabled = true;
			timer1.Start();
		}

		private void WaveInfoForm_Load(object sender, EventArgs e)
		{
			InitWave();
			mScale = Math.Pow(2.0, ((double)numScale.Value - 32.0) / 4.0);
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

		private void picWave_MouseUp(object sender, MouseEventArgs e) {
			if (onLoopDrag) {
				onLoopDrag = false;
			}
		}

		private void picWave_MouseMove(object sender, MouseEventArgs e) {
			mCursolPos = picWave.PointToClient(Cursor.Position);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			hsbTime.Width = Width - hsbTime.Left - 24;
			picWave.Width = Width - picWave.Left - 24;
			picSpectrum.Width = Width - picSpectrum.Left - 24;
			DrawWave();
			DrawSpec();
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
			var packSize = 32;
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

			var sp = new Spectrum(wave.Format.SampleRate, 27.5, 12, 112);
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

			var begin = hsbTime.Value;
			var end = hsbTime.Value + bmp.Width / mScale + 1;

			var wave = mFile.WavePool.List[mIndex];

			//
			if (0 < wave.Sampler.LoopCount) {
				var loopBegin = (float)(mScale * (wave.Loops[0].Start - begin));
				var loopLength = (float)(mScale * wave.Loops[0].Length);
				var loopEnd = loopBegin + loopLength - 1;
				gM.FillRectangle(Brushes.WhiteSmoke, loopBegin, 0, loopLength, bmp.Height);

				if (!onLoopDrag && Math.Abs(mCursolPos.X - loopBegin) <= 4) {
					Cursor = Cursors.SizeWE;
				}
				else if (!onLoopDrag && Math.Abs(mCursolPos.X - loopEnd) <= 4) {
					Cursor = Cursors.SizeWE;
				}
				else {
					Cursor = Cursors.Default;
				}
			}

			//
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
				gM.DrawLine(green, x1, y1, x2, y2);
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
			var pix = (uint*)bmpData.Scan0.ToPointer();

			int y, x;
			double begin = hsbTime.Value * mTimeDiv;
			double scale = mTimeDiv / mScale;
			for (y = bmp.Height - 1; 0 <= y; --y) {
				for (x = 0; x < bmp.Width; ++x) {
					var sx = (int)(begin + scale * x);
					if (sx < mSpectrogram.Length && y < mSpectrogram[sx].Length) {
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
