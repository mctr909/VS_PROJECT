using System;
using System.Windows.Forms;

namespace DLSeditor {
	public partial class AmpEnvelope : UserControl {
		public AmpEnvelope() {
			InitializeComponent();

			picAmpAttack.Width = picAmpAttack.Image.Width;
			picAmpAttack.Height = picAmpAttack.Image.Height;
			picAmpHold.Width = picAmpHold.Image.Width;
			picAmpHold.Height = picAmpHold.Image.Height;
			picAmpDecay.Width = picAmpDecay.Image.Width;
			picAmpDecay.Height = picAmpDecay.Image.Height;
			picAmpSustain.Width = picAmpSustain.Image.Width;
			picAmpSustain.Height = picAmpSustain.Image.Height;
			picAmpReleace.Width = picAmpReleace.Image.Width;
			picAmpReleace.Height = picAmpReleace.Image.Height;

			picAmpHold.Top = picAmpAttack.Top + picAmpAttack.Height + 4;
			picAmpDecay.Top = picAmpHold.Top + picAmpHold.Height + 4;
			picAmpSustain.Top = picAmpDecay.Top + picAmpDecay.Height + 4;
			picAmpReleace.Top = picAmpSustain.Top + picAmpSustain.Height + 4;


			hsbAmpAttack.Left = picAmpAttack.Right + 4;
			hsbAmpAttack.Top = picAmpAttack.Top;
			hsbAmpAttack.Width = 400;
			hsbAmpAttack.Height = picAmpAttack.Height;

			hsbAmpHold.Left = picAmpHold.Right + 4;
			hsbAmpHold.Top = picAmpHold.Top;
			hsbAmpHold.Width = 400;
			hsbAmpHold.Height = picAmpHold.Height;

			hsbAmpDecay.Left = picAmpDecay.Right + 4;
			hsbAmpDecay.Top = picAmpDecay.Top;
			hsbAmpDecay.Width = 400;
			hsbAmpDecay.Height = picAmpDecay.Height;

			hsbAmpSustain.Left = picAmpSustain.Right + 4;
			hsbAmpSustain.Top = picAmpSustain.Top;
			hsbAmpSustain.Width = 400;
			hsbAmpSustain.Height = picAmpSustain.Height;

			hsbAmpReleace.Left = picAmpReleace.Right + 4;
			hsbAmpReleace.Top = picAmpReleace.Top;
			hsbAmpReleace.Width = 400;
			hsbAmpReleace.Height = picAmpReleace.Height;


			lblAmpAttack.Left = hsbAmpAttack.Right + 4;
			lblAmpAttack.Top = hsbAmpAttack.Top + (hsbAmpAttack.Height - lblAmpAttack.Height) / 2;

			lblAmpHold.Left = hsbAmpHold.Right + 4;
			lblAmpHold.Top = hsbAmpHold.Top + (hsbAmpHold.Height - lblAmpHold.Height) / 2;

			lblAmpDecay.Left = hsbAmpDecay.Right + 4;
			lblAmpDecay.Top = hsbAmpDecay.Top + (hsbAmpDecay.Height - lblAmpDecay.Height) / 2;

			lblAmpSustain.Left = hsbAmpSustain.Right + 4;
			lblAmpSustain.Top = hsbAmpSustain.Top + (hsbAmpSustain.Height - lblAmpSustain.Height) / 2;

			lblAmpReleace.Left = hsbAmpReleace.Right + 4;
			lblAmpReleace.Top = hsbAmpReleace.Top + (hsbAmpReleace.Height - lblAmpReleace.Height) / 2;

			Width = lblAmpReleace.Right + 4;
		}

		public double Attack {
			get { return hsbToValue(hsbAmpAttack.Value); }
			set {
				if (value <= 0) {
					hsbAmpAttack.Value = 1;
					hsbAmpAttack.Enabled = false;
					lblAmpAttack.Text = "----";
				}
				else if (39 < value) {
					hsbAmpAttack.Value = valueToHsb(39);
					hsbAmpAttack.Enabled = true;
				}
				else {
					hsbAmpAttack.Value = valueToHsb(value);
					hsbAmpAttack.Enabled = true;
				}
			}
		}

		public double Hold {
			get { return hsbToValue(hsbAmpHold.Value); }
			set {
				if (value <= 0) {
					hsbAmpHold.Value = 1;
					hsbAmpHold.Enabled = false;
					lblAmpHold.Text = "----";
				}
				else if (39 < value) {
					hsbAmpHold.Value = valueToHsb(39);
					hsbAmpHold.Enabled = true;
				}
				else {
					hsbAmpHold.Value = valueToHsb(value);
					hsbAmpHold.Enabled = true;
				}
			}
		}

		public double Decay {
			get { return hsbToValue(hsbAmpDecay.Value); }
			set {
				if (value <= 0) {
					hsbAmpDecay.Value = 1;
					hsbAmpDecay.Enabled = false;
					lblAmpDecay.Text = "----";
				}
				else if (39 < value) {
					hsbAmpDecay.Value = valueToHsb(39);
					hsbAmpDecay.Enabled = true;
				}
				else {
					hsbAmpDecay.Value = valueToHsb(value);
					hsbAmpDecay.Enabled = true;
				}
			}
		}

		public double Sustain {
			get { return hsbAmpSustain.Value * 0.1; }
			set { hsbAmpSustain.Value = (int)(value * 10); }
		}

		public double Releace {
			get { return hsbToValue(hsbAmpReleace.Value); }
			set {
				if (value <= 0) {
					hsbAmpReleace.Value = 1;
					hsbAmpReleace.Enabled = false;
					lblAmpReleace.Text = "----";
				}
				else if (39 < value) {
					hsbAmpReleace.Value = valueToHsb(39);
					hsbAmpReleace.Enabled = true;
				}
				else {
					hsbAmpReleace.Value = valueToHsb(value);
					hsbAmpReleace.Enabled = true;
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
			lblAmpAttack.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbAmpAttack.Value)));
		}

		private void hsbAmpHold_ValueChanged(object sender, System.EventArgs e) {
			lblAmpHold.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbAmpHold.Value)));
		}

		private void hsbAmpDecay_ValueChanged(object sender, System.EventArgs e) {
			lblAmpDecay.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbAmpDecay.Value)));
		}

		private void hsbAmpSustain_ValueChanged(object sender, System.EventArgs e) {
			lblAmpSustain.Text = string.Format("{0}%", hsbAmpSustain.Value * 0.1);
		}

		private void hsbAmpReleace_ValueChanged(object sender, System.EventArgs e) {
			lblAmpReleace.Text = string.Format("{0}ms", (int)(1000 * hsbToValue(hsbAmpReleace.Value)));
		}
	}
}
