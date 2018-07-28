using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace EasySequencer
{
	public partial class Form1 : Form
	{
		private static readonly Font mFont = new Font("ＭＳ ゴシック", 9.0f, FontStyle.Regular, GraphicsUnit.Point);

		MIDI.MessageSender mNoteOut;
		private bool mIsSeek = false;
		private bool mIsParamChg = false;
		private int mKnobX = 0;
		private int mKnobY = 0;
		private int mChangeValue = 0;

		private MIDI.SMF mSMF;
		public MIDI.Player mPlayer;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			mNoteOut = new MIDI.MessageSender(new MIDI.InstTable("C:\\Users\\owner\\Desktop\\gm2.dls"));
			mPlayer = new MIDI.Player(mNoteOut);

			timer1.Interval = 10;
			timer1.Enabled = true;
			timer1.Start();
		}

		private void 開くOToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog1.Filter = "MIDIファイル(*.mid)|*.mid";
			openFileDialog1.ShowDialog();
			var filePath = openFileDialog1.FileName;
			if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath)) {
				return;
			}

			if (mPlayer.IsPlay) {
				mPlayer.Stop();
				btnPalyStop.Text = "再生";
			}

			mSMF = new MIDI.SMF(filePath);
			mPlayer.SetEventList(mSMF.EventList, mSMF.Ticks);
			hsbSeek.Maximum = mPlayer.MaxTime;
		}

		private void btnPalyStop_Click(object sender, EventArgs e)
		{
			if (mPlayer.IsPlay) {
				mPlayer.Stop();
			}
			else {
				mPlayer.Play();
			}

			btnPalyStop.Text = mPlayer.IsPlay ? "停止" : "再生";
		}

		private void hsbSeek_MouseLeave(object sender, EventArgs e)
		{
			if (mIsSeek) {
				mIsSeek = false;
				mPlayer.SeekTime = hsbSeek.Value;
				btnPalyStop.Text = "停止";
			}
		}

		private void hsbSeek_Scroll(object sender, ScrollEventArgs e)
		{
			mIsSeek = true;
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			mIsSeek = true;
			mPlayer.Transpose = (int)numericUpDown1.Value;
			mPlayer.SeekTime = hsbSeek.Value;
			mIsSeek = false;
		}

		private void timer1_Tick(object sender, EventArgs e) {
			if (null == mPlayer) {
				return;
			}

			lblPosition.Text = mPlayer.TimeText;
			lblTempo.Text = mPlayer.TempoText;

			if (!mIsSeek) {
				if (mPlayer.CurrentTime <= hsbSeek.Maximum) {
					hsbSeek.Value = mPlayer.CurrentTime;
				}
				else {
					hsbSeek.Value = 0;
					mPlayer.SeekTime = hsbSeek.Value;
				}
			}

			if (mIsParamChg) {
				switch (mKnobX) {
				case 0:
					mPlayer.Send(new MIDI.Message(
						MIDI.CTRL_TYPE.VOLUME,
						(byte)mKnobY,
						(byte)mChangeValue
					));
					break;

				case 1:
					mPlayer.Send(new MIDI.Message(
						MIDI.CTRL_TYPE.EXPRESSION,
						(byte)mKnobY,
						(byte)mChangeValue
					));
					break;

				case 2:
					mPlayer.Send(new MIDI.Message(
						MIDI.CTRL_TYPE.PAN,
						(byte)mKnobY,
						(byte)mChangeValue
					));
					break;

				case 3:
					mPlayer.Send(new MIDI.Message(
						MIDI.CTRL_TYPE.REVERB,
						(byte)mKnobY,
						(byte)mChangeValue
					));
					break;

				case 4:
					mPlayer.Send(new MIDI.Message(
						MIDI.CTRL_TYPE.CHORUS,
						(byte)mKnobY,
						(byte)mChangeValue
					));
					break;

				case 5:
					mPlayer.Send(new MIDI.Message(
						MIDI.CTRL_TYPE.DELAY,
						(byte)mKnobY,
						(byte)mChangeValue
					));
					break;

				case 6:
					mPlayer.Send(new MIDI.Message(
						MIDI.CTRL_TYPE.CUTOFF,
						(byte)mKnobY,
						(byte)mChangeValue
					));
					break;

				case 7:
					mPlayer.Send(new MIDI.Message(
						MIDI.CTRL_TYPE.RESONANCE,
						(byte)mKnobY,
						(byte)mChangeValue
					));
					break;
				}
			}

			Bitmap bmp = new Bitmap(picKeyboard.Width, picKeyboard.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			bmp.MakeTransparent(Color.Black);
			Graphics g = Graphics.FromImage(bmp);

			var whiteWidth = MIDI.Const.KeyboardPos[0].Width + 1;

			for (int ch = 0; ch < mPlayer.Channel.Length; ++ch) {
				var y_ch = 40 * ch;

				for (int k = 0; k < 127; ++k) {
					if (mPlayer.Channel[ch].Keyboard[k]) {
						var x_oct = 7 * whiteWidth * (k / 12 - 1);
						var key = MIDI.Const.KeyboardPos[k % 12];
						g.FillRectangle(Brushes.Red, key.X + x_oct, key.Y + y_ch, key.Width, key.Height);
					}
				}

				// Vol
				g.FillRectangle(
					Brushes.White,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Vol][0]) + MIDI.Const.KnobPos[0].X,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Vol][1]) + MIDI.Const.KnobPos[0].Y + y_ch,
					3, 3
				);
				g.DrawString(
					mPlayer.Channel[ch].Vol.ToString("000"),
					mFont, Brushes.Black,
					MIDI.Const.KnobValPos[0].X, MIDI.Const.KnobValPos[0].Y + y_ch
				);

				// Exp
				g.FillRectangle(
					Brushes.White,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Exp][0]) + MIDI.Const.KnobPos[1].X,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Exp][1]) + MIDI.Const.KnobPos[1].Y + y_ch,
					3, 3
				);
				g.DrawString(
					mPlayer.Channel[ch].Exp.ToString("000"),
					mFont, Brushes.Black,
					MIDI.Const.KnobValPos[1].X, MIDI.Const.KnobValPos[1].Y + y_ch
				);

				// Pan
				g.FillRectangle(
					Brushes.White,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Pan][0]) + MIDI.Const.KnobPos[2].X,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Pan][1]) + MIDI.Const.KnobPos[2].Y + y_ch,
					3, 3
				);
				var exp = mPlayer.Channel[ch].Pan - 64;
				if (0 == exp) {
					g.DrawString(
						" C ",
						mFont, Brushes.Black,
						MIDI.Const.KnobValPos[2].X, MIDI.Const.KnobValPos[2].Y + y_ch
					);
				}
				else if (exp < 0) {
					g.DrawString(
						"L" + (-exp).ToString("00"),
						mFont, Brushes.Black,
						MIDI.Const.KnobValPos[2].X, MIDI.Const.KnobValPos[2].Y + y_ch
					);
				}
				else {
					g.DrawString(
						"R" + exp.ToString("00"),
						mFont, Brushes.Black,
						MIDI.Const.KnobValPos[2].X, MIDI.Const.KnobValPos[2].Y + y_ch
					);
				}

				// Rev
				g.FillRectangle(
					Brushes.White,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Rev][0]) + MIDI.Const.KnobPos[3].X,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Rev][1]) + MIDI.Const.KnobPos[3].Y + y_ch,
					3, 3
				);
				g.DrawString(
					mPlayer.Channel[ch].Rev.ToString("000"),
					mFont, Brushes.Black,
					MIDI.Const.KnobValPos[3].X, MIDI.Const.KnobValPos[3].Y + y_ch
				);

				// Cho
				g.FillRectangle(
					Brushes.White,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Cho][0]) + MIDI.Const.KnobPos[4].X,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Cho][1]) + MIDI.Const.KnobPos[4].Y + y_ch,
					3, 3
				);
				g.DrawString(
					mPlayer.Channel[ch].Cho.ToString("000"),
					mFont, Brushes.Black,
					MIDI.Const.KnobValPos[4].X, MIDI.Const.KnobValPos[4].Y + y_ch
				);

				// Del
				g.FillRectangle(
					Brushes.White,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Del][0]) + MIDI.Const.KnobPos[5].X,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Del][1]) + MIDI.Const.KnobPos[5].Y + y_ch,
					3, 3
				);
				g.DrawString(
					mPlayer.Channel[ch].Del.ToString("000"),
					mFont, Brushes.Black,
					MIDI.Const.KnobValPos[5].X, MIDI.Const.KnobValPos[5].Y + y_ch
				);

				// Fc
				g.FillRectangle(
					Brushes.White,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Fc][0]) + MIDI.Const.KnobPos[6].X,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Fc][1]) + MIDI.Const.KnobPos[6].Y + y_ch,
					3, 3
				);
				g.DrawString(
					mPlayer.Channel[ch].Fc.ToString("000"),
					mFont, Brushes.Black,
					MIDI.Const.KnobValPos[6].X, MIDI.Const.KnobValPos[6].Y + y_ch
				);

				// Fq
				g.FillRectangle(
					Brushes.White,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Fq][0]) + MIDI.Const.KnobPos[7].X,
					(int)(7 * MIDI.Const.Knob[mPlayer.Channel[ch].Fq][1]) + MIDI.Const.KnobPos[7].Y + y_ch,
					3, 3
				);
				g.DrawString(
					mPlayer.Channel[ch].Fq.ToString("000"),
					mFont, Brushes.Black,
					MIDI.Const.KnobValPos[7].X, MIDI.Const.KnobValPos[7].Y + y_ch
				);

				if (!mPlayer.Channel[ch].Enable) {
					g.FillRectangle(Brushes.Red, 722, 4 + y_ch, 13, 18);
				}
			}

			if (null != g) {
				g.Dispose();
				g = null;
			}

			if (null != picKeyboard.Image) {
				picKeyboard.Image.Dispose();
				picKeyboard.Image = null;
			}

			picKeyboard.Image = bmp;
		}

		private void picKeyboard_MouseDown(Object sender, MouseEventArgs e) {
			var pos = picKeyboard.PointToClient(Cursor.Position);
			var knobX = (pos.X - 525) / 24;
			var knobY = pos.Y / 40;

			if (0 <= knobY && knobY <= 15) {
				if (knobX == 8) {
					if (e.Button == MouseButtons.Right) {
						if (mPlayer.Channel[knobY].Enable) {
							for (int i = 0; i < mPlayer.Channel.Length; ++i) {
								if (knobY == i) {
									mPlayer.Channel[i].Enable = false;
								}
								else {
									mPlayer.Channel[i].Enable = true;
								}
							}
						}
						else {
							for (int i = 0; i < mPlayer.Channel.Length; ++i) {
								if (knobY == i) {
									mPlayer.Channel[i].Enable = true;
								}
								else {
									mPlayer.Channel[i].Enable = false;
								}
							}
						}
					}
					else {
						mPlayer.Channel[knobY].Enable = !mPlayer.Channel[knobY].Enable;
					}
				}

				if (0 <= knobX && knobX <= 7) {
					mIsParamChg = true;
					mKnobX = knobX;
					mKnobY = knobY;
				}
			}
		}

		private void picKeyboard_MouseUp(Object sender, MouseEventArgs e) {
			if (mIsParamChg) {
				mIsParamChg = false;
				mKnobX = 0;
				mKnobY = 0;
			}
		}

		private void picKeyboard_MouseMove(Object sender, MouseEventArgs e) {
			if (mIsParamChg) {
				var pos = picKeyboard.PointToClient(Cursor.Position);
				var knobCenter = MIDI.Const.KnobPos[mKnobX];
				knobCenter.Y += mKnobY * 40;

				var sx = pos.X - knobCenter.X;
				var sy = pos.Y - knobCenter.Y;
				var th = 0.625 * Math.Atan2(sx, -sy) / Math.PI;
				if (th < -0.5) {
					th = -0.5;
				}
				if (0.5 < th) {
					th = 0.5;
				}

				mChangeValue = (int)((th + 0.5) * 127.0);
				if (mChangeValue < 0) {
					mChangeValue = 0;
				}
				if (127 < mChangeValue) {
					mChangeValue = 127;
				}
			}
		}

		private void Form1_SizeChanged(object sender, EventArgs e)
		{
			tabControl1.Width = Width - tabControl1.Location.X - 20;
			tabControl1.Height = Height - tabControl1.Location.Y - 48;
			pnlKeyboard.Width = tabControl1.Width - 16;
			pnlKeyboard.Height = tabControl1.Height - 38;
		}
	}
}
