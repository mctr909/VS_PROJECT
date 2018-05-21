using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace EasySequencer
{
	public partial class Form1 : Form
	{
		MIDI.MessageSender mNoteOut;
		private bool mIsSeek;
		private bool mIsParamChg;
		private Bitmap mBmp;
		private MIDI.SMF mSMF;
		public MIDI.Player mPlayer;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			mBmp = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);
			Graphics g = Graphics.FromImage(mBmp);
			g.DrawImage(pictureBox1.Image, 0, 0);

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
				btnPalyStop.Text = "再生";
			}
			else {
				mPlayer.Play();
				btnPalyStop.Text = "停止";
			}
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

		private void timer1_Tick(object sender, EventArgs e)
		{
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

			if (!mIsParamChg) {
				var curCh = mPlayer.Recv((byte)(numChannel.Value - 1));
				trkVol.Value = curCh.Vol;
				trkPan.Value = curCh.Pan;
				trkCho.Value = curCh.Cho;
				trkDel.Value = curCh.Del;
			}

			Bitmap bmp = new Bitmap(mBmp.Width, mBmp.Height);
			Graphics g = Graphics.FromImage(bmp);
			g.DrawImage(mBmp, 0, 0);

			var whiteWidth = 10;
			var blackWidth = 7;
			var blackHeight = 26;
			var whiteMarkWidth = whiteWidth - 1;
			var whiteMarkHeight = bmp.Height - blackHeight - 1;

			for (int ch = 0; ch < mPlayer.Channel.Length; ++ch) {
				if (ch == 9) continue;

				var brush = Brushes.Red;
				switch (ch % 4) {
				case 0:
					brush = (new Pen(Color.FromArgb(255, 255, 0, 0), 1.0f)).Brush;
					break;
				case 1:
					brush = (new Pen(Color.FromArgb(255, 255, 192, 0), 1.0f)).Brush;
					break;
				case 2:
					brush = (new Pen(Color.FromArgb(255, 0, 255, 0), 1.0f)).Brush;
					break;
				case 3:
					brush = (new Pen(Color.FromArgb(255, 0, 0, 255), 1.0f)).Brush;
					break;
				}

				for (int k = 0; k < 127; ++k) {
					if (mPlayer.Channel[ch].Keyboard[k]) {
						var oct = 7 * whiteWidth * (k / 12 - 1);
						var key = k % 12;

						switch (key) {
						case 0:
							g.FillRectangle(brush, oct + 1, blackHeight, whiteMarkWidth, whiteMarkHeight);
							break;
						case 1:
							g.FillRectangle(brush, oct + 7, 0, blackWidth, blackHeight);
							break;
						case 2:
							g.FillRectangle(brush, oct + 1 + whiteWidth, blackHeight, whiteMarkWidth, whiteMarkHeight);
							break;
						case 3:
							g.FillRectangle(brush, oct + 17, 0, blackWidth, blackHeight);
							break;
						case 4:
							g.FillRectangle(brush, oct + 1 + whiteWidth * 2, blackHeight, whiteMarkWidth, whiteMarkHeight);
							break;
						case 5:
							g.FillRectangle(brush, oct + 1 + whiteWidth * 3, blackHeight, whiteMarkWidth, whiteMarkHeight);
							break;
						case 6:
							g.FillRectangle(brush, oct + 37, 0, blackWidth, blackHeight);
							break;
						case 7:
							g.FillRectangle(brush, oct + 1 + whiteWidth * 4, blackHeight, whiteMarkWidth, whiteMarkHeight);
							break;
						case 8:
							g.FillRectangle(brush, oct + 47, 0, blackWidth, blackHeight);
							break;
						case 9:
							g.FillRectangle(brush, oct + 1 + whiteWidth * 5, blackHeight, whiteMarkWidth, whiteMarkHeight);
							break;
						case 10:
							g.FillRectangle(brush, oct + 57, 0, blackWidth, blackHeight);
							break;
						case 11:
							g.FillRectangle(brush, oct + 1 + whiteWidth * 6, blackHeight, whiteMarkWidth, whiteMarkHeight);
							break;
						}
					}
				}
			}

			pictureBox1.Image = bmp;
		}

		#region channel Enable
		private void chkSolo_CheckedChanged(object sender, EventArgs e)
		{
			if (chkSolo.Checked) {
				mPlayer.SoloChannel = (int)(numChannel.Value - 1);
			}
			else {
				mPlayer.SoloChannel = -1;
			}
		}

		private void numChannel_ValueChanged(object sender, EventArgs e)
		{
			if (chkSolo.Checked) {
				mPlayer.SoloChannel = (int)(numChannel.Value - 1);
			}
			else {
				mPlayer.SoloChannel = -1;
			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[0].Enable = checkBox1.Checked;
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[1].Enable = checkBox2.Checked;
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[2].Enable = checkBox3.Checked;
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[3].Enable = checkBox4.Checked;
		}

		private void checkBox5_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[4].Enable = checkBox5.Checked;
		}

		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[5].Enable = checkBox6.Checked;
		}

		private void checkBox7_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[6].Enable = checkBox7.Checked;
		}

		private void checkBox8_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[7].Enable = checkBox8.Checked;
		}

		private void checkBox9_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[8].Enable = checkBox9.Checked;
		}

		private void checkBox10_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[9].Enable = checkBox10.Checked;
		}

		private void checkBox11_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[10].Enable = checkBox11.Checked;
		}

		private void checkBox12_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[11].Enable = checkBox12.Checked;
		}

		private void checkBox13_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[12].Enable = checkBox13.Checked;
		}

		private void checkBox14_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[13].Enable = checkBox14.Checked;
		}

		private void checkBox15_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[14].Enable = checkBox15.Checked;
		}

		private void checkBox16_CheckedChanged(object sender, EventArgs e)
		{
			mPlayer.Channel[15].Enable = checkBox16.Checked;
		}
		#endregion

		#region Param
		private void trkVol_Scroll(object sender, EventArgs e)
		{
			mIsParamChg = true;
		}

		private void trkVol_ValueChanged(object sender, EventArgs e)
		{
			mPlayer.Send(new MIDI.Message(
				MIDI.CTRL_TYPE.VOLUME,
				(byte)(numChannel.Value - 1),
				(byte)trkVol.Value
			));
		}

		private void trkVol_MouseLeave(object sender, EventArgs e)
		{
			mIsParamChg = false;
		}

		private void trkPan_Scroll(object sender, EventArgs e)
		{
			mIsParamChg = true;
		}

		private void trkPan_ValueChanged(object sender, EventArgs e)
		{
			mPlayer.Send(new MIDI.Message(
				MIDI.CTRL_TYPE.PAN,
				(byte)(numChannel.Value - 1),
				(byte)trkPan.Value
			));
		}

		private void trkPan_MouseLeave(object sender, EventArgs e)
		{
			mIsParamChg = false;
		}

		private void trkCho_Scroll(object sender, EventArgs e)
		{
			mIsParamChg = true;
		}

		private void trkCho_ValueChanged(object sender, EventArgs e)
		{
			mPlayer.Send(new MIDI.Message(
				MIDI.CTRL_TYPE.CHORUS,
				(byte)(numChannel.Value - 1),
				(byte)trkCho.Value
			));
		}

		private void trkCho_MouseLeave(object sender, EventArgs e)
		{
			mIsParamChg = false;
		}

		private void trkDel_Scroll(object sender, EventArgs e)
		{
			mIsParamChg = true;
		}

		private void trkDel_ValueChanged(object sender, EventArgs e)
		{
			mPlayer.Send(new MIDI.Message(
				MIDI.CTRL_TYPE.DELAY,
				(byte)(numChannel.Value - 1),
				(byte)trkDel.Value
			));
		}

		private void trkDel_MouseLeave(object sender, EventArgs e)
		{
			mIsParamChg = false;
		}
		#endregion
	}
}
