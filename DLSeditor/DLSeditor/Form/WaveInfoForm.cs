using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace DLSeditor {
	unsafe public partial class WaveInfoForm : Form {
		private DLS.DLS mFile;
		private int mIndex;
		private WavePlayback mWaveOut;

		private byte[][] mSpectrogram;
		private uint[] mColors;
		private short[] mWave;
		private double mScale;
		private double mScaleLoop;
		private double mTimeDiv;
		private double mDelta;
		private Point mCursolPos;
		private bool mOnWaveDisp;
		private bool onDragWave;
		private bool onDragLoopBegin;
		private bool onDragLoopEnd;
		private DLS.WaveLoop mLoop;
		private int mDetectNote;
		private int mDetectTune;

		private readonly string[] NoteName = new string[] {
			"C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
		};

		public WaveInfoForm(DLS.DLS dls, int index) {
			InitializeComponent();
			StartPosition = FormStartPosition.CenterParent;

			mFile = dls;
			mIndex = index;
			mWaveOut = new WavePlayback();

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

			lblPitch.Text = "";
			lblPitchCent.Text = "";
			mDetectNote = -1;
			mDetectTune = 0;

			mWaveOut.SetValue(mFile.WavePool.List[mIndex]);

			timer1.Interval = 100;
			timer1.Enabled = true;
			timer1.Start();
		}

		private void WaveInfoForm_Load(object sender, EventArgs e) {
			SetPosition();
			SizeChange();

			InitWave();
			mScale = Math.Pow(2.0, ((double)numScale.Value - 32.0) / 4.0);
			mScaleLoop = Math.Pow(2.0, ((double)numScaleLoop.Value - 32.0) / 4.0);
		}

		private void WaveInfoForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (null != mWaveOut) {
				mWaveOut.Stop();
			}
		}

		private void WaveInfoForm_SizeChanged(object sender, EventArgs e) {
			SizeChange();
		}

		#region クリックイベント
		private void btnPlay_Click(object sender, EventArgs e) {
			if ("再生" == btnPlay.Text) {
				if (0 < mFile.WavePool.List[mIndex].Loops.Count) {
					mWaveOut.mLoopBegin = (int)mLoop.Start;
					mWaveOut.mLoopEnd = mWaveOut.mLoopBegin + (int)mLoop.Length;
				}
				else {
					mWaveOut.mLoopBegin = 0;
					mWaveOut.mLoopEnd = mWave.Length;
				}
				mWaveOut.Play();
				btnPlay.Text = "停止";
			}
			else {
				mWaveOut.Stop();
				btnPlay.Text = "再生";
			}
		}

		private void btnUpdate_Click(object sender, EventArgs e) {
			if (0 < mFile.WavePool.List[mIndex].Sampler.LoopCount) {
				mFile.WavePool.List[mIndex].Loops[0] = mLoop;
			}
		}

		private void btnUpdateAutoTune_Click(object sender, EventArgs e) {
			if (0 <= mDetectNote) {
				numUnityNote.Value = mDetectNote;
				numFineTune.Value = mDetectTune;
				mFile.WavePool.List[mIndex].Sampler.UnityNote = (ushort)mDetectNote;
				mFile.WavePool.List[mIndex].Sampler.FineTune = (short)mDetectTune;
			}
		}

		private void btnLoopCreate_Click(object sender, EventArgs e) {
			if(0 < mFile.WavePool.List[mIndex].Sampler.LoopCount) {
				mWaveOut.mLoopBegin = 0;
				mWaveOut.mLoopEnd = mWave.Length;
				mFile.WavePool.List[mIndex].Loops.Clear();
				mFile.WavePool.List[mIndex].Sampler.LoopCount = 0;
				btnLoopCreate.Text = "ループ作成";
			}
			else {
				mLoop.Start = (uint)hsbTime.Value;
				mLoop.Length = 32;
				mWaveOut.mLoopBegin = (int)mLoop.Start;
				mWaveOut.mLoopEnd = (int)mLoop.Start + (int)mLoop.Length;
				mFile.WavePool.List[mIndex].Loops.Add(0, mLoop);
				mFile.WavePool.List[mIndex].Sampler.LoopCount = 1;
				btnLoopCreate.Text = "ループ削除";
			}
		}
		#endregion

		#region チェンジイベント
		private void txtName_TextChanged(object sender, EventArgs e) {
			mFile.WavePool.List[mIndex].Info.Name = txtName.Text;
		}

		private void numScale_ValueChanged(object sender, EventArgs e) {
			mScale = Math.Pow(2.0, ((double)numScale.Value - 32.0) / 4.0);
		}

		private void numScaleLoop_ValueChanged(object sender, EventArgs e) {
			mScaleLoop = Math.Pow(2.0, ((double)numScaleLoop.Value - 32.0) / 4.0);
		}

		private void numVolume_ValueChanged(object sender, EventArgs e) {
			mFile.WavePool.List[mIndex].Sampler.Gain = (double)numVolume.Value / 100.0;
			mWaveOut.mVolume = mFile.WavePool.List[mIndex].Sampler.Gain;
		}

		private void numUnityNote_ValueChanged(object sender, EventArgs e) {
			var oct = (int)numUnityNote.Value / 12 - 2;
			var note = (int)numUnityNote.Value % 12;
			lblUnityNote.Text = string.Format("{0}{1}", NoteName[note], oct);
			mFile.WavePool.List[mIndex].Sampler.UnityNote = (ushort)numUnityNote.Value;
		}

		private void numFineTune_ValueChanged(object sender, EventArgs e) {
			mFile.WavePool.List[mIndex].Sampler.FineTune = (short)numFineTune.Value;
		}
		#endregion

		#region ループ範囲選択
		private void picWave_MouseDown(object sender, MouseEventArgs e) {
			onDragWave = true;
		}

		private void picWave_MouseUp(object sender, MouseEventArgs e) {
			onDragWave = false;
			onDragLoopBegin = false;
			onDragLoopEnd = false;
		}

		private void picWave_MouseMove(object sender, MouseEventArgs e) {
			mCursolPos = picWave.PointToClient(Cursor.Position);
			var pos = hsbTime.Value + mCursolPos.X / mScale;
			if(pos < 0) {
				pos = 0;
			}

			if (onDragLoopBegin) {
				mLoop.Start = (uint)pos;
			}

			if (onDragLoopEnd) {
				if ((pos - 16) < mLoop.Start) {
					mLoop.Length = 16;
				}
				else {
					mLoop.Length = (uint)pos - mLoop.Start;
				}
			}

			mWaveOut.mLoopBegin = (int)mLoop.Start;
			mWaveOut.mLoopEnd = (int)mLoop.Start + (int)mLoop.Length;
		}

		private void picWave_MouseEnter(object sender, EventArgs e) {
			mOnWaveDisp = true;
		}

		private void picWave_MouseLeave(object sender, EventArgs e) {
			mOnWaveDisp = false;
		}
		#endregion

		private void timer1_Tick(object sender, EventArgs e) {
			if (0.0 < mWaveOut.mPitch) {
				var x = 12.0 * Math.Log(mWaveOut.mPitch / 8.1757989, 2.0);
				mDetectNote = (int)(x + 0.5);
				mDetectTune = (int)((mDetectNote - x) * 100);

				var oct = mDetectNote / 12;
				var note = mDetectNote % 12;
				if (note < 0) {
					note += -(note / 12 - 1) * 12;
				}
				lblPitch.Text = string.Format("{0}{1}", NoteName[note], oct - 2);
				lblPitchCent.Text = string.Format("{0}cent", mDetectTune);
			}

			DrawSpec();
			DrawWave();
			DrawLoop();
		}

		private void SetPosition() {
			//
			picWave.Height = 96;
			numScale.Top = 0;
			picSpectrum.Top = numScale.Top + numScale.Height + 4;
			picWave.Top = picSpectrum.Top + picSpectrum.Height + 4;
			hsbTime.Top = picWave.Top + picWave.Height + 4;

			// 
			grbMain.Top = btnPlay.Top + btnPlay.Height + 6;
			grbMain.Height
				= numScale.Height + 4
				+ picSpectrum.Height + 4
				+ picWave.Height + 4
				+ hsbTime.Height + 6
			;

			//
			picLoop.Height = 192;
			numScaleLoop.Top = 0;
			picLoop.Top = numScaleLoop.Top + numScaleLoop.Height + 4;

			//
			grbLoop.Top = grbMain.Top + grbMain.Height + 6;
			grbLoop.Height
				= numScaleLoop.Height + 4
				+ picLoop.Height + 6
			;

			Height
				= btnPlay.Height + 6
				+ grbMain.Height + 6
				+ grbLoop.Height + 48;
			Width = btnUpdateAutoTune.Right + 22;
		}

		private void SizeChange() {
			grbMain.Width = Width - grbMain.Left - 22;
			picSpectrum.Width = Width - (picSpectrum.Left + grbMain.Left) * 2 - 16;
			picWave.Width = Width - (picWave.Left + grbMain.Left) * 2 - 16;
			hsbTime.Width = Width - (hsbTime.Left + grbMain.Left) * 2 - 16;

			grbLoop.Width = Width - grbLoop.Left - 22;
			picLoop.Width = Width - (picLoop.Left + grbLoop.Left) * 2 - 16;
		}

		private void InitWave() {
			var wave = mFile.WavePool.List[mIndex];
			if (null != wave.Info && !string.IsNullOrWhiteSpace(wave.Info.Name)) {
				Text = wave.Info.Name;
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

			if (0 < wave.Loops.Count) {
				var loop = wave.Loops[0];
				mLoop.Start = loop.Start;
				mLoop.Length = loop.Length;
				btnLoopCreate.Text = "ループ削除";
			}
			else {
				btnLoopCreate.Text = "ループ作成";
			}

			numVolume.Value = (decimal)((int)(wave.Sampler.Gain * 1000) / 10.0);
			numUnityNote.Value = wave.Sampler.UnityNote;
			numFineTune.Value = wave.Sampler.FineTune;
			txtName.Text = wave.Info.Name;
		}

		private void DrawWave() {
			var bmp = new Bitmap(picWave.Width, picWave.Height, PixelFormat.Format16bppRgb555);
			var graph = Graphics.FromImage(bmp);

			var amp = bmp.Height - 1;

			var green = new Pen(Color.FromArgb(0, 168, 0), 1.0f);

			var begin = hsbTime.Value;
			var end = hsbTime.Value + bmp.Width / mScale + 1;

			var wave = mFile.WavePool.List[mIndex];

			//
			if (0 < wave.Sampler.LoopCount) {
				var loopBegin = (float)(mScale * (mLoop.Start - begin));
				var loopLength = (float)(mScale * mLoop.Length);
				var loopEnd = loopBegin + loopLength;
				graph.FillRectangle(Brushes.WhiteSmoke, loopBegin, 0, loopLength, bmp.Height);

				if (mOnWaveDisp && Math.Abs(mCursolPos.X - loopBegin) <= 7) {
					Cursor = Cursors.SizeWE;
					if(onDragWave && !onDragLoopEnd) {
						onDragLoopBegin = true;
					}
				}
				else if (mOnWaveDisp && Math.Abs(mCursolPos.X - loopEnd) <= 7) {
					Cursor = Cursors.SizeWE;
					if (onDragWave && !onDragLoopBegin) {
						onDragLoopEnd = true;
					}
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
				graph.DrawLine(green, x1, y1, x2, y2);
			}

			//
			graph.DrawLine(Pens.Red, 0, picWave.Height / 2.0f - 1, picWave.Width - 1, picWave.Height / 2.0f - 1);

			if (null != picWave.Image) {
				picWave.Image.Dispose();
				picWave.Image = null;
			}
			picWave.BackColor = Color.Black;
			picWave.Image = bmp;
			graph.Dispose();
		}

		private void DrawSpec() {
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

		private void DrawLoop() {
			var bmp = new Bitmap(picLoop.Width, picLoop.Height, PixelFormat.Format16bppRgb555);
			var g = Graphics.FromImage(bmp);

			var amp = bmp.Height - 1;
			var halfWidth = (int)(picLoop.Width / 2.0f - 1);

			var wave = mFile.WavePool.List[mIndex];

			var loopBegin = (int)mLoop.Start;
			var loopEnd = (int)mLoop.Start + (int)mLoop.Length;
			if (mWave.Length <= loopEnd) {
				loopEnd = mWave.Length - 1;
			}

			var green = new Pen(Color.FromArgb(0, 168, 0), 1.0f);

			g.DrawLine(Pens.Red, 0, picLoop.Height / 2.0f - 1, picLoop.Width - 1, picLoop.Height / 2.0f - 1);
			g.DrawLine(Pens.Red, halfWidth, 0, halfWidth, picLoop.Height);

			//
			for (int t1 = loopBegin, t2 = loopBegin + 1, px1 = 0, px2 = 1; t2 <= loopEnd; ++t1, ++t2, ++px1, ++px2) {
				var y1 = (float)(amp * (0.5 - 0.5 * mWave[t1] / 32768.0));
				var y2 = (float)(amp * (0.5 - 0.5 * mWave[t2] / 32768.0));
				var x1 = (float)(halfWidth + mScaleLoop * px1);
				var x2 = (float)(halfWidth + mScaleLoop * px2);
				g.DrawLine(green, x1, y1, x2, y2);
			}

			//
			for (int t1 = loopEnd, t2 = loopEnd - 1, px1 = 0, px2 = 1; loopBegin <= t2; --t1, --t2, ++px1, ++px2) {
				var y1 = (float)(amp * (0.5 - 0.5 * mWave[t1] / 32768.0));
				var y2 = (float)(amp * (0.5 - 0.5 * mWave[t2] / 32768.0));
				var x1 = (float)(halfWidth - mScaleLoop * px1);
				var x2 = (float)(halfWidth - mScaleLoop * px2);
				g.DrawLine(green, x1, y1, x2, y2);
			}

			if (null != picLoop.Image) {
				picLoop.Image.Dispose();
				picLoop.Image = null;
			}
			picLoop.BackColor = Color.Black;
			picLoop.Image = bmp;
			g.Dispose();
		}
	}
}