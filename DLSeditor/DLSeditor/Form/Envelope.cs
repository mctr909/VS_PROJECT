using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DLSeditor {
    public partial class Envelope : UserControl {
        private DLS.ART mArt;

        public Envelope() {
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
            hsbAttack.Width = 400;
            hsbAttack.Height = picAttack.Height;

            hsbHold.Left = picHold.Right + 4;
            hsbHold.Top = picHold.Top;
            hsbHold.Width = 400;
            hsbHold.Height = picHold.Height;

            hsbDecay.Left = picDecay.Right + 4;
            hsbDecay.Top = picDecay.Top;
            hsbDecay.Width = 400;
            hsbDecay.Height = picDecay.Height;

            hsbSustain.Left = picSustain.Right + 4;
            hsbSustain.Top = picSustain.Top;
            hsbSustain.Width = 400;
            hsbSustain.Height = picSustain.Height;

            hsbReleace.Left = picReleace.Right + 4;
            hsbReleace.Top = picReleace.Top;
            hsbReleace.Width = 400;
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


            grpAmp.Width = lblReleace.Location.X + lblReleace.Width + 8;
            grpAmp.Height = lblReleace.Location.Y + lblReleace.Height + 16;


            //
            grpEq.Top = grpAmp.Bottom + 8;

            picEqAttack.Width = picEqAttack.Image.Width;
            picEqAttack.Height = picEqAttack.Image.Height;
            picEqHold.Width = picEqHold.Image.Width;
            picEqHold.Height = picEqHold.Image.Height;
            picEqDecay.Width = picEqDecay.Image.Width;
            picEqDecay.Height = picEqDecay.Image.Height;
            picEqSustain.Width = picEqSustain.Image.Width;
            picEqSustain.Height = picEqSustain.Image.Height;
            picEqReleace.Width = picEqReleace.Image.Width;
            picEqReleace.Height = picEqReleace.Image.Height;

            picEqHold.Top = picEqAttack.Top + picEqAttack.Height + 4;
            picEqDecay.Top = picEqHold.Top + picEqHold.Height + 4;
            picEqSustain.Top = picEqDecay.Top + picEqDecay.Height + 4;
            picEqReleace.Top = picEqSustain.Top + picEqSustain.Height + 4;


            hsbEqAttack.Left = picEqAttack.Right + 4;
            hsbEqAttack.Top = picEqAttack.Top;
            hsbEqAttack.Width = 400;
            hsbEqAttack.Height = picEqAttack.Height;

            hsbEqHold.Left = picEqHold.Right + 4;
            hsbEqHold.Top = picEqHold.Top;
            hsbEqHold.Width = 400;
            hsbEqHold.Height = picEqHold.Height;

            hsbEqDecay.Left = picEqDecay.Right + 4;
            hsbEqDecay.Top = picEqDecay.Top;
            hsbEqDecay.Width = 400;
            hsbEqDecay.Height = picEqDecay.Height;

            hsbEqSustain.Left = picEqSustain.Right + 4;
            hsbEqSustain.Top = picEqSustain.Top;
            hsbEqSustain.Width = 400;
            hsbEqSustain.Height = picEqSustain.Height;

            hsbEqReleace.Left = picEqReleace.Right + 4;
            hsbEqReleace.Top = picEqReleace.Top;
            hsbEqReleace.Width = 400;
            hsbEqReleace.Height = picEqReleace.Height;


            chkEqAttack.Left = hsbEqAttack.Right + 4;
            chkEqAttack.Top = hsbEqAttack.Top + (hsbEqAttack.Height - chkEqAttack.Height) / 2;

            chkEqHold.Left = hsbEqHold.Right + 4;
            chkEqHold.Top = hsbEqHold.Top + (hsbEqHold.Height - chkEqHold.Height) / 2;

            chkEqDecay.Left = hsbEqDecay.Right + 4;
            chkEqDecay.Top = hsbEqDecay.Top + (hsbEqDecay.Height - chkEqDecay.Height) / 2;

            chkEqSustain.Left = hsbEqSustain.Right + 4;
            chkEqSustain.Top = hsbEqSustain.Top + (hsbEqSustain.Height - chkEqSustain.Height) / 2;

            chkEqReleace.Left = hsbEqReleace.Right + 4;
            chkEqReleace.Top = hsbEqReleace.Top + (hsbEqReleace.Height - chkEqReleace.Height) / 2;


            lblEqAttack.Left = chkEqAttack.Right + 4;
            lblEqAttack.Top = hsbEqAttack.Top + (hsbEqAttack.Height - lblEqAttack.Height) / 2;

            lblEqHold.Left = chkEqHold.Right + 4;
            lblEqHold.Top = hsbEqHold.Top + (hsbEqHold.Height - lblEqHold.Height) / 2;

            lblEqDecay.Left = chkEqDecay.Right + 4;
            lblEqDecay.Top = hsbEqDecay.Top + (hsbEqDecay.Height - lblEqDecay.Height) / 2;

            lblEqSustain.Left = chkEqSustain.Right + 4;
            lblEqSustain.Top = hsbEqSustain.Top + (hsbEqSustain.Height - lblEqSustain.Height) / 2;

            lblEqReleace.Left = chkEqReleace.Right + 4;
            lblEqReleace.Top = hsbEqReleace.Top + (hsbEqReleace.Height - lblEqReleace.Height) / 2;


            grpEq.Width = lblEqReleace.Location.X + lblEqReleace.Width + 8;
            grpEq.Height = lblEqReleace.Location.Y + lblEqReleace.Height + 16;


            Width = grpEq.Width + 4;
            Height = grpEq.Location.Y + grpEq.Height + 8;

            disp();
        }

        public DLS.ART Art {
            get { return mArt; }
            set {
                mArt = value;
                disp();
            }
        }

        public void SetList(Dictionary<int, DLS.Connection> list) {
            if (chkAttack.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG1_ATTACK_TIME;
                v.Value = ampAttack;
                list.Add(list.Count, v);
            }

            if (chkHold.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG1_HOLD_TIME;
                v.Value = ampHold;
                list.Add(list.Count, v);
            }

            if (chkDecay.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG1_DECAY_TIME;
                v.Value = ampDecay;
                list.Add(list.Count, v);
            }

            if (chkSustain.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG1_SUSTAIN_LEVEL;
                v.Value = ampSustain;
                list.Add(list.Count, v);
            }

            if (chkReleace.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG1_RELEASE_TIME;
                v.Value = ampReleace;
                list.Add(list.Count, v);
            }

            if (chkEqAttack.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG2_ATTACK_TIME;
                v.Value = eqAttack;
                list.Add(list.Count, v);
            }

            if (chkEqHold.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG2_HOLD_TIME;
                v.Value = eqHold;
                list.Add(list.Count, v);
            }

            if (chkEqDecay.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG2_DECAY_TIME;
                v.Value = eqDecay;
                list.Add(list.Count, v);
            }

            if (chkEqSustain.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG2_SUSTAIN_LEVEL;
                v.Value = eqSustain;
                list.Add(list.Count, v);
            }

            if (chkEqReleace.Checked) {
                var v = new DLS.Connection();
                v.Source = DLS.Connection.SRC_TYPE.NONE;
                v.Control = DLS.Connection.SRC_TYPE.NONE;
                v.Destination = DLS.Connection.DST_TYPE.EG2_RELEASE_TIME;
                v.Value = eqReleace;
                list.Add(list.Count, v);
            }
        }

        #region Amp
        private double ampAttack {
            get { return hsbToValue(hsbAttack.Value); }
            set { setValue(hsbAttack, chkAttack, lblAttack, value); }
        }

        private double ampHold {
            get { return hsbToValue(hsbHold.Value); }
            set { setValue(hsbHold, chkHold, lblHold, value); }
        }

        private double ampDecay {
            get { return hsbToValue(hsbDecay.Value); }
            set { setValue(hsbDecay, chkDecay, lblDecay, value); }
        }

        private double ampSustain {
            get { return hsbSustain.Value * 0.1; }
            set { hsbSustain.Value = (int)(value * 10); }
        }

        private double ampReleace {
            get { return hsbToValue(hsbReleace.Value); }
            set { setValue(hsbReleace, chkReleace, lblReleace, value); }
        }

        private void hsbAmpAttack_ValueChanged(object sender, System.EventArgs e) {
            lblAttack.Text = hsbToText(hsbAttack.Value);
        }

        private void hsbAmpHold_ValueChanged(object sender, System.EventArgs e) {
            lblHold.Text = hsbToText(hsbHold.Value);
        }

        private void hsbAmpDecay_ValueChanged(object sender, System.EventArgs e) {
            lblDecay.Text = hsbToText(hsbDecay.Value);
        }

        private void hsbAmpSustain_ValueChanged(object sender, System.EventArgs e) {
            lblSustain.Text = string.Format("{0}%", hsbSustain.Value * 0.1);
        }

        private void hsbAmpReleace_ValueChanged(object sender, System.EventArgs e) {
            lblReleace.Text = hsbToText(hsbReleace.Value);
        }

        private void chkAttack_CheckedChanged(object sender, EventArgs e) {
            if (chkAttack.Checked) {
                hsbAttack.Enabled = true;
                lblAttack.Text = hsbToText(hsbAttack.Value);
            }
            else {
                hsbAttack.Enabled = false;
                lblAttack.Text = "----";
            }
        }

        private void chkHold_CheckedChanged(object sender, EventArgs e) {
            if (chkHold.Checked) {
                hsbHold.Enabled = true;
                lblHold.Text = hsbToText(hsbHold.Value);
            }
            else {
                hsbHold.Enabled = false;
                lblHold.Text = "----";
            }
        }

        private void chkDecay_CheckedChanged(object sender, EventArgs e) {
            if (chkDecay.Checked) {
                hsbDecay.Enabled = true;
                lblDecay.Text = hsbToText(hsbDecay.Value);
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
                lblReleace.Text = hsbToText(hsbReleace.Value);
            }
            else {
                hsbReleace.Enabled = false;
                lblReleace.Text = "----";
            }
        }
        #endregion

        #region Eq
        private double eqAttack {
            get { return hsbToValue(hsbEqAttack.Value); }
            set { setValue(hsbEqAttack, chkEqAttack, lblEqAttack, value); }
        }

        private double eqHold {
            get { return hsbToValue(hsbEqHold.Value); }
            set { setValue(hsbEqHold, chkEqHold, lblEqHold, value); }
        }

        private double eqDecay {
            get { return hsbToValue(hsbEqDecay.Value); }
            set { setValue(hsbEqDecay, chkEqDecay, lblEqDecay, value); }
        }

        private double eqSustain {
            get { return hsbEqSustain.Value * 0.1; }
            set { hsbEqSustain.Value = (int)(value * 10); }
        }

        private double eqReleace {
            get { return hsbToValue(hsbEqReleace.Value); }
            set { setValue(hsbEqReleace, chkEqReleace, lblEqReleace, value); }
        }

        private void hsbEqAttack_ValueChanged(object sender, EventArgs e) {
            lblEqAttack.Text = hsbToText(hsbEqAttack.Value);
        }

        private void hsbEqHold_ValueChanged(object sender, EventArgs e) {
            lblEqHold.Text = hsbToText(hsbEqHold.Value);
        }

        private void hsbEqDecay_ValueChanged(object sender, EventArgs e) {
            lblEqDecay.Text = hsbToText(hsbEqDecay.Value);
        }

        private void hsbEqSustain_ValueChanged(object sender, EventArgs e) {
            lblEqSustain.Text = string.Format("{0}%", hsbEqSustain.Value * 0.1);
        }

        private void hsbEqReleace_ValueChanged(object sender, EventArgs e) {
            lblEqReleace.Text = hsbToText(hsbEqReleace.Value);
        }

        private void chkEqAttack_CheckedChanged(object sender, EventArgs e) {
            if (chkEqAttack.Checked) {
                hsbEqAttack.Enabled = true;
                lblEqAttack.Text = hsbToText(hsbEqAttack.Value);
            }
            else {
                hsbEqAttack.Enabled = false;
                lblEqAttack.Text = "----";
            }
        }

        private void chkEqHold_CheckedChanged(object sender, EventArgs e) {
            if (chkEqHold.Checked) {
                hsbEqHold.Enabled = true;
                lblEqHold.Text = hsbToText(hsbEqHold.Value);
            }
            else {
                hsbEqHold.Enabled = false;
                lblEqHold.Text = "----";
            }
        }

        private void chkEqDecay_CheckedChanged(object sender, EventArgs e) {
            if (chkEqDecay.Checked) {
                hsbEqDecay.Enabled = true;
                lblEqDecay.Text = hsbToText(hsbEqDecay.Value);
            }
            else {
                hsbEqDecay.Enabled = false;
                lblEqDecay.Text = "----";
            }
        }

        private void chkEqSustain_CheckedChanged(object sender, EventArgs e) {
            if (chkEqSustain.Checked) {
                hsbEqSustain.Enabled = true;
                lblEqSustain.Text = string.Format("{0}%", hsbEqSustain.Value * 0.1);
            }
            else {
                hsbEqSustain.Enabled = false;
                lblEqSustain.Text = "----";
            }
        }

        private void chkEqReleace_CheckedChanged(object sender, EventArgs e) {
            if (chkEqReleace.Checked) {
                hsbEqReleace.Enabled = true;
                lblEqReleace.Text = hsbToText(hsbEqReleace.Value);
            }
            else {
                hsbEqReleace.Enabled = false;
                lblEqReleace.Text = "----";
            }
        }
        #endregion

        private string hsbToText(int value) {
            var v = hsbToValue(value);

            if (v < 1.0) {
                return string.Format("{0}ms", (int)(1000 * v));
            }
            else if (v < 10.0) {
                return string.Format("{0}s", ((int)(100 * v) / 100.0).ToString("0.00"));
            }
            else {
                return string.Format("{0}s", ((int)(10 * v) / 10.0).ToString("0.0"));
            }
        }

        private double hsbToValue(int value) {
            return 40 * Math.Pow(64, value / 4096.0) / 64 - 0.626;
        }

        private int valueToHsb(double hsb) {
            return (int)(Math.Log((hsb * 1000 + 626) * 64 / 40000, 64) * 4096 + 1);
        }

        private void setValue(HScrollBar hsb, CheckBox chk, Label lbl, double value) {
            if (value <= 0) {
                hsb.Value = 1;
                hsb.Enabled = false;
                chk.Checked = false;
                lbl.Text = "----";
            }
            else if (39 < value) {
                hsb.Value = valueToHsb(39);
                hsb.Enabled = true;
                chk.Checked = true;
            }
            else {
                hsb.Value = valueToHsb(value);
                hsb.Enabled = true;
                chk.Checked = true;
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


            hsbEqAttack.Enabled = false;
            chkEqAttack.Checked = false;
            lblEqAttack.Text = "----";

            hsbEqHold.Enabled = false;
            chkEqHold.Checked = false;
            lblEqHold.Text = "----";

            hsbEqDecay.Enabled = false;
            chkEqDecay.Checked = false;
            lblEqDecay.Text = "----";

            hsbEqSustain.Enabled = false;
            chkEqSustain.Checked = false;
            lblEqSustain.Text = "----";

            hsbEqReleace.Enabled = false;
            chkEqReleace.Checked = false;
            lblEqReleace.Text = "----";


            ampAttack = 0;
            ampHold = 0;
            ampDecay = 0;
            ampSustain = 0;
            ampReleace = 0;

            eqAttack = 0;
            eqHold = 0;
            eqDecay = 0;
            eqSustain = 0;
            eqReleace = 0;

            if (null != mArt) {
                foreach (var art in mArt.List.Values) {
                    if (DLS.Connection.SRC_TYPE.NONE != art.Source) {
                        continue;
                    }

                    if (DLS.Connection.SRC_TYPE.NONE != art.Control) {
                        continue;
                    }

                    switch (art.Destination) {
                        case DLS.Connection.DST_TYPE.EG1_ATTACK_TIME:
                            ampAttack = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG1_HOLD_TIME:
                            ampHold = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG1_DECAY_TIME:
                            ampDecay = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG1_SUSTAIN_LEVEL:
                            ampSustain = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG1_RELEASE_TIME:
                            ampReleace = art.Value;
                            break;

                        case DLS.Connection.DST_TYPE.EG2_ATTACK_TIME:
                            eqAttack = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG2_HOLD_TIME:
                            eqHold = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG2_DECAY_TIME:
                            eqDecay = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG2_SUSTAIN_LEVEL:
                            eqSustain = art.Value;
                            break;
                        case DLS.Connection.DST_TYPE.EG2_RELEASE_TIME:
                            eqReleace = art.Value;
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}