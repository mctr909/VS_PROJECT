using System;
using System.Windows.Forms;

namespace DLSeditor {
	public partial class AmpEnvelope : UserControl {
		private DLS.ART mArt;

		public AmpEnvelope() {
			InitializeComponent();

			picAttack.Width = picAttack.Image.Width;
			picAttack.Height = picAttack.Image.Height;
			picHold.Width = picHold.Image.Width;
			picHold.Height = picHold.Image.Height;
			picDecay.Width = picDecay.Image.Width;
			picDecay.Height = picDecay.Image.Height;
			picSustain.Width = picSustain.Image.Width;
			picSustain.Height = picSustain.Image.Height;
			picReleace.Width = picReleace.Image.Width;
			picReleace.Height = picReleace.Image.Height;

			picHold.Top = picAttack.Top + picAttack.Height + 4;
			picDecay.Top = picHold.Top + picHold.Height + 4;
			picSustain.Top = picDecay.Top + picDecay.Height + 4;
			picReleace.Top = picSustain.Top + picSustain.Height + 4;


			hsbAttack.Left = picAttack.Right + 4;
			hsbAttack.Top = picAttack.Top;
			hsbAttack.Width = 300;
			hsbAttack.Height = picAttack.Height;

			hsbHold.Left = picHold.Right + 4;
			hsbHold.Top = picHold.Top;
			hsbHold.Width = 300;
			hsbHold.Height = picHold.Height;

			hsbDecay.Left = picDecay.Right + 4;
			hsbDecay.Top = picDecay.Top;
			hsbDecay.Width = 300;
			hsbDecay.Height = picDecay.Height;

			hsbSustain.Left = picSustain.Right + 4;
			hsbSustain.Top = picSustain.Top;
			hsbSustain.Width = 300;
			hsbSustain.Height = picSustain.Height;

			hsbReleace.Left = picReleace.Right + 4;
			hsbReleace.Top = picReleace.Top;
			hsbReleace.Width = 300;
			hsbReleace.Height = picReleace.Height;


			chkAttack.Left = hsbAttack.Right + 4;
			chkAttack.Top = hsbAttack.Top + (hsbAttack.Height - chkAttack.Height) / 2;

			chkHold.Left = hsbHold.Right + 4;
			chkHold.Top = hsbHold.Top + (hsbHold.Height - chkHold.Height) / 2;

			chkDecay.Left = hsbDecay.Right + 4;
			chkDecay.Top = hsbDecay.Top + (hsbDecay.Height - chkDecay.Height) / 2;

			chkSustain.Left = hsbSustain.Right + 4;
			chkSustain.Top = hsbSustain.Top + (hsbSustain.Height - chkSustain.Height) / 2;

			chkReleace.Left = hsbReleace.Right + 4;
			chkReleace.Top = hsbReleace.Top + (hsbReleace.Height - chkReleace.Height) / 2;


			lblAttack.Left = chkAttack.Right + 4;
			lblAttack.Top = hsbAttack.Top + (hsbAttack.Height - lblAttack.Height) / 2;

			lblHold.Left = chkHold.Right + 4;
			lblHold.Top = hsbHold.Top + (hsbHold.Height - lblHold.Height) / 2;

			lblDecay.Left = chkDecay.Right + 4;
			lblDecay.Top = hsbDecay.Top + (hsbDecay.Height - lblDecay.Height) / 2;

			lblSustain.Left = chkSustain.Right + 4;
			lblSustain.Top = hsbSustain.Top + (hsbSustain.Height - lblSustain.Height) / 2;

			lblReleace.Left = chkReleace.Right + 4;
			lblReleace.Top = hsbReleace.Top + (hsbReleace.Height - lblReleace.Height) / 2;

			Width = lblReleace.Right + 4;

			disp();
		}

		public DLS.ART Art {
			get { return mArt; }
			set {
				mArt = value;
				disp();
			}
		}

		private double attack {
			get { return hsbToValue(hsbAttack.Value); }
			set {
				if (value <= 0) {
					hsbAttack.Value = 1;
					hsbAttack.Enabled = false;
					chkAttack.Checked = false;
					lblAttack.Text = "----";
				}
				else if (39 < value) {
					hsbAttack.Value = valueToHsb(39);
					hsbAttack.Enabled = true;
					chkAttack.Checked = true;
				}
				else {
					hsbAttack.Value = valueToHsb(value);
					hsbAttack.Enabled = true;
					chkAttack.Checked = true;
				}
			}
		}

		private double hold {
			get { return hsbToValue(hsbHold.Value); }
			set {
				if (value <= 0) {
					hsbHold.Value = 1;
					hsbHold.Enabled = false;
					chkHold.Checked = false;
					lblHold.Text = "----";
				}
				else if (39 < value) {
					hsbHold.Value = valueToHsb(39);
					hsbHold.Enabled = true;
					chkHold.Checked = true;
				}
				else {
					hsbHold.Value = valueToHsb(value);
					hsbHold.Enabled = true;
					chkHold.Checked = true;
				}
			}
		}

		private double decay {
			get { return hsbToValue(hsbDecay.Value); }
			set {
				if (value <= 0) {
					hsbDecay.Value = 1;
					hsbDecay.Enabled = false;
					chkDecay.Checked = false;
					lblDecay.Text = "----";
				}
				else if (39 < value) {
					hsbDecay.Value = valueToHsb(39);
					hsbDecay.Enabled = true;
					chkDecay.Checked = true;
				}
				else {
					hsbDecay.Value = valueToHsb(value);
					hsbDecay.Enabled = true;
					chkDecay.Checked = true;
				}
			}
		}

		private double sustain {
			get { return hsbSustain.Value * 0.1; }
			set { hsbSustain.Value = (int)(value * 10); }
		}

		private double releace {
			get { return hsbToValue(hsbReleace.Value); }
			set {
				if (value <= 0) {
					hsbReleace.Value = 1;
					hsbReleace.Enabled = false;
					chkReleace.Checked = false;
					lblReleace.Text = "----";
				}
				else if (39 < value) {
					hsbReleace.Value = valueToHsb(39);
					hsbReleace.Enabled = true;
					chkReleace.Checked = true;
				}
				else {
					hsbReleace.Value = valueToHsb(value);
					hsbReleace.Enabled = true;
					chkReleace.Checked = true;
				}
			}
		}

		private double hsbToValue(int value) {
			return 40 * Math.Pow(64, value / 2048.0) / 64 - 0.626;
		}

		private int valueToHsb(double hsb) {
			return (int)(Math.Log((hsb * 1000 + 626) * 64 / 40000, 64) * 2048 + 1);
		}

		private void hsbAmpAttack_ValueChanged(object sender, System.EventArgs e) {
			lblAttack.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbAttack.Value)));
		}

		private void hsbAmpHold_ValueChanged(object sender, System.EventArgs e) {
			lblHold.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbHold.Value)));
		}

		private void hsbAmpDecay_ValueChanged(object sender, System.EventArgs e) {
			lblDecay.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbDecay.Value)));
		}

		private void hsbAmpSustain_ValueChanged(object sender, System.EventArgs e) {
			lblSustain.Text = string.Format("{0}%", hsbSustain.Value * 0.1);
		}

		private void hsbAmpReleace_ValueChanged(object sender, System.EventArgs e) {
			lblReleace.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbReleace.Value)));
		}

		private void chkAttack_CheckedChanged(object sender, EventArgs e) {
			if (chkAttack.Checked) {
				hsbAttack.Enabled = true;
				lblAttack.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbAttack.Value)));
			}
			else {
				hsbAttack.Enabled = false;
				lblAttack.Text = "----";
			}
		}

		private void chkHold_CheckedChanged(object sender, EventArgs e) {
			if (chkHold.Checked) {
				hsbHold.Enabled = true;
				lblHold.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbHold.Value)));
			}
			else {
				hsbHold.Enabled = false;
				lblHold.Text = "----";
			}
		}

		private void chkDecay_CheckedChanged(object sender, EventArgs e) {
			if (chkDecay.Checked) {
				hsbDecay.Enabled = true;
				lblDecay.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbDecay.Value)));
			}
			else {
				hsbDecay.Enabled = false;
				lblDecay.Text = "----";
			}
		}

		private void chkSustain_CheckedChanged(object sender, EventArgs e) {
			if (chkSustain.Checked) {
				hsbSustain.Enabled = true;
				lblSustain.Text = string.Format("{0}%", hsbSustain.Value * 0.1);
			}
			else {
				hsbSustain.Enabled = false;
				lblSustain.Text = "----";
			}
		}

		private void chkReleace_CheckedChanged(object sender, EventArgs e) {
			if (chkReleace.Checked) {
				hsbReleace.Enabled = true;
				lblReleace.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbReleace.Value)));
			}
			else {
				hsbReleace.Enabled = false;
				lblReleace.Text = "----";
			}
		}

		private void disp() {
			hsbAttack.Enabled = false;
			chkAttack.Checked = false;
			lblAttack.Text = "----";

			hsbHold.Enabled = false;
			chkHold.Checked = false;
			lblHold.Text = "----";

			hsbDecay.Enabled = false;
			chkDecay.Checked = false;
			lblDecay.Text = "----";

			hsbSustain.Enabled = false;
			chkSustain.Checked = false;
			lblSustain.Text = "----";

			hsbReleace.Enabled = false;
			chkReleace.Checked = false;
			lblReleace.Text = "----";

			attack = 0;
			hold = 0;
			decay = 0;
			sustain = 0;
			releace = 0;

			if (null != mArt) {
				foreach (var art in mArt.List.Values) {
					if (DLS.Connection.SRC_TYPE.NONE == art.Source) {
						switch (art.Destination) {
						case DLS.Connection.DST_TYPE.EG1_ATTACK_TIME:
							attack = art.Value;
							break;
						case DLS.Connection.DST_TYPE.EG1_HOLD_TIME:
							hold = art.Value;
							break;
						case DLS.Connection.DST_TYPE.EG1_DECAY_TIME:
							decay = art.Value;
							break;
						case DLS.Connection.DST_TYPE.EG1_SUSTAIN_LEVEL:
							sustain = art.Value;
							break;
						case DLS.Connection.DST_TYPE.EG1_RELEASE_TIME:
							releace = art.Value;
							break;
						}
					}
				}
			}
		}
	}
}
