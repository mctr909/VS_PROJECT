namespace DLSeditor {
    partial class Envelope {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.hsbAttack = new System.Windows.Forms.HScrollBar();
            this.hsbHold = new System.Windows.Forms.HScrollBar();
            this.hsbDecay = new System.Windows.Forms.HScrollBar();
            this.hsbSustain = new System.Windows.Forms.HScrollBar();
            this.hsbReleace = new System.Windows.Forms.HScrollBar();
            this.lblAttack = new System.Windows.Forms.Label();
            this.lblHold = new System.Windows.Forms.Label();
            this.lblDecay = new System.Windows.Forms.Label();
            this.lblSustain = new System.Windows.Forms.Label();
            this.lblReleace = new System.Windows.Forms.Label();
            this.picReleace = new System.Windows.Forms.PictureBox();
            this.picSustain = new System.Windows.Forms.PictureBox();
            this.picDecay = new System.Windows.Forms.PictureBox();
            this.picHold = new System.Windows.Forms.PictureBox();
            this.picAttack = new System.Windows.Forms.PictureBox();
            this.chkAttack = new System.Windows.Forms.CheckBox();
            this.chkHold = new System.Windows.Forms.CheckBox();
            this.chkDecay = new System.Windows.Forms.CheckBox();
            this.chkSustain = new System.Windows.Forms.CheckBox();
            this.chkReleace = new System.Windows.Forms.CheckBox();
            this.grpAmp = new System.Windows.Forms.GroupBox();
            this.grpEq = new System.Windows.Forms.GroupBox();
            this.hsbEqAttack = new System.Windows.Forms.HScrollBar();
            this.chkEqReleace = new System.Windows.Forms.CheckBox();
            this.lblEqAttack = new System.Windows.Forms.Label();
            this.picEqAttack = new System.Windows.Forms.PictureBox();
            this.hsbEqReleace = new System.Windows.Forms.HScrollBar();
            this.chkEqSustain = new System.Windows.Forms.CheckBox();
            this.lblEqHold = new System.Windows.Forms.Label();
            this.picEqHold = new System.Windows.Forms.PictureBox();
            this.hsbEqSustain = new System.Windows.Forms.HScrollBar();
            this.chkEqDecay = new System.Windows.Forms.CheckBox();
            this.lblEqDecay = new System.Windows.Forms.Label();
            this.picEqDecay = new System.Windows.Forms.PictureBox();
            this.hsbEqDecay = new System.Windows.Forms.HScrollBar();
            this.chkEqHold = new System.Windows.Forms.CheckBox();
            this.lblEqSustain = new System.Windows.Forms.Label();
            this.picEqSustain = new System.Windows.Forms.PictureBox();
            this.hsbEqHold = new System.Windows.Forms.HScrollBar();
            this.chkEqAttack = new System.Windows.Forms.CheckBox();
            this.lblEqReleace = new System.Windows.Forms.Label();
            this.picEqReleace = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picReleace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSustain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDecay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAttack)).BeginInit();
            this.grpAmp.SuspendLayout();
            this.grpEq.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEqAttack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEqHold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEqDecay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEqSustain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEqReleace)).BeginInit();
            this.SuspendLayout();
            // 
            // hsbAttack
            // 
            this.hsbAttack.Location = new System.Drawing.Point(70, 37);
            this.hsbAttack.Maximum = 4096;
            this.hsbAttack.Minimum = 1;
            this.hsbAttack.Name = "hsbAttack";
            this.hsbAttack.Size = new System.Drawing.Size(478, 38);
            this.hsbAttack.TabIndex = 5;
            this.hsbAttack.Value = 1;
            this.hsbAttack.ValueChanged += new System.EventHandler(this.hsbAmpAttack_ValueChanged);
            // 
            // hsbHold
            // 
            this.hsbHold.Location = new System.Drawing.Point(70, 75);
            this.hsbHold.Maximum = 4096;
            this.hsbHold.Minimum = 1;
            this.hsbHold.Name = "hsbHold";
            this.hsbHold.Size = new System.Drawing.Size(478, 38);
            this.hsbHold.TabIndex = 6;
            this.hsbHold.Value = 1;
            this.hsbHold.ValueChanged += new System.EventHandler(this.hsbAmpHold_ValueChanged);
            // 
            // hsbDecay
            // 
            this.hsbDecay.Location = new System.Drawing.Point(70, 113);
            this.hsbDecay.Maximum = 4096;
            this.hsbDecay.Minimum = 1;
            this.hsbDecay.Name = "hsbDecay";
            this.hsbDecay.Size = new System.Drawing.Size(478, 38);
            this.hsbDecay.TabIndex = 7;
            this.hsbDecay.Value = 1;
            this.hsbDecay.ValueChanged += new System.EventHandler(this.hsbAmpDecay_ValueChanged);
            // 
            // hsbSustain
            // 
            this.hsbSustain.Location = new System.Drawing.Point(70, 150);
            this.hsbSustain.Maximum = 1000;
            this.hsbSustain.Name = "hsbSustain";
            this.hsbSustain.Size = new System.Drawing.Size(478, 38);
            this.hsbSustain.TabIndex = 8;
            this.hsbSustain.ValueChanged += new System.EventHandler(this.hsbAmpSustain_ValueChanged);
            // 
            // hsbReleace
            // 
            this.hsbReleace.Location = new System.Drawing.Point(70, 188);
            this.hsbReleace.Maximum = 4096;
            this.hsbReleace.Minimum = 1;
            this.hsbReleace.Name = "hsbReleace";
            this.hsbReleace.Size = new System.Drawing.Size(478, 38);
            this.hsbReleace.TabIndex = 9;
            this.hsbReleace.Value = 1;
            this.hsbReleace.ValueChanged += new System.EventHandler(this.hsbAmpReleace_ValueChanged);
            // 
            // lblAttack
            // 
            this.lblAttack.AutoSize = true;
            this.lblAttack.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblAttack.Location = new System.Drawing.Point(596, 51);
            this.lblAttack.Name = "lblAttack";
            this.lblAttack.Size = new System.Drawing.Size(103, 30);
            this.lblAttack.TabIndex = 10;
            this.lblAttack.Text = "label1";
            // 
            // lblHold
            // 
            this.lblHold.AutoSize = true;
            this.lblHold.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblHold.Location = new System.Drawing.Point(596, 82);
            this.lblHold.Name = "lblHold";
            this.lblHold.Size = new System.Drawing.Size(103, 30);
            this.lblHold.TabIndex = 11;
            this.lblHold.Text = "label2";
            // 
            // lblDecay
            // 
            this.lblDecay.AutoSize = true;
            this.lblDecay.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblDecay.Location = new System.Drawing.Point(596, 120);
            this.lblDecay.Name = "lblDecay";
            this.lblDecay.Size = new System.Drawing.Size(103, 30);
            this.lblDecay.TabIndex = 12;
            this.lblDecay.Text = "label3";
            // 
            // lblSustain
            // 
            this.lblSustain.AutoSize = true;
            this.lblSustain.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblSustain.Location = new System.Drawing.Point(596, 158);
            this.lblSustain.Name = "lblSustain";
            this.lblSustain.Size = new System.Drawing.Size(103, 30);
            this.lblSustain.TabIndex = 13;
            this.lblSustain.Text = "label4";
            // 
            // lblReleace
            // 
            this.lblReleace.AutoSize = true;
            this.lblReleace.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblReleace.Location = new System.Drawing.Point(596, 196);
            this.lblReleace.Name = "lblReleace";
            this.lblReleace.Size = new System.Drawing.Size(103, 30);
            this.lblReleace.TabIndex = 14;
            this.lblReleace.Text = "label5";
            // 
            // picReleace
            // 
            this.picReleace.Image = global::DLSeditor.Properties.Resources.EnvReleace;
            this.picReleace.Location = new System.Drawing.Point(15, 188);
            this.picReleace.Name = "picReleace";
            this.picReleace.Size = new System.Drawing.Size(48, 32);
            this.picReleace.TabIndex = 4;
            this.picReleace.TabStop = false;
            // 
            // picSustain
            // 
            this.picSustain.Image = global::DLSeditor.Properties.Resources.EnvSustain;
            this.picSustain.Location = new System.Drawing.Point(15, 150);
            this.picSustain.Name = "picSustain";
            this.picSustain.Size = new System.Drawing.Size(48, 32);
            this.picSustain.TabIndex = 3;
            this.picSustain.TabStop = false;
            // 
            // picDecay
            // 
            this.picDecay.Image = global::DLSeditor.Properties.Resources.EnvDecay;
            this.picDecay.Location = new System.Drawing.Point(15, 112);
            this.picDecay.Name = "picDecay";
            this.picDecay.Size = new System.Drawing.Size(48, 32);
            this.picDecay.TabIndex = 2;
            this.picDecay.TabStop = false;
            // 
            // picHold
            // 
            this.picHold.Image = global::DLSeditor.Properties.Resources.EnvHold;
            this.picHold.Location = new System.Drawing.Point(15, 74);
            this.picHold.Name = "picHold";
            this.picHold.Size = new System.Drawing.Size(48, 32);
            this.picHold.TabIndex = 1;
            this.picHold.TabStop = false;
            // 
            // picAttack
            // 
            this.picAttack.Image = global::DLSeditor.Properties.Resources.EnvAttack;
            this.picAttack.Location = new System.Drawing.Point(15, 36);
            this.picAttack.Name = "picAttack";
            this.picAttack.Size = new System.Drawing.Size(48, 32);
            this.picAttack.TabIndex = 0;
            this.picAttack.TabStop = false;
            // 
            // chkAttack
            // 
            this.chkAttack.AutoSize = true;
            this.chkAttack.Location = new System.Drawing.Point(552, 46);
            this.chkAttack.Name = "chkAttack";
            this.chkAttack.Size = new System.Drawing.Size(28, 27);
            this.chkAttack.TabIndex = 15;
            this.chkAttack.UseVisualStyleBackColor = true;
            this.chkAttack.CheckedChanged += new System.EventHandler(this.chkAttack_CheckedChanged);
            // 
            // chkHold
            // 
            this.chkHold.AutoSize = true;
            this.chkHold.Location = new System.Drawing.Point(552, 82);
            this.chkHold.Name = "chkHold";
            this.chkHold.Size = new System.Drawing.Size(28, 27);
            this.chkHold.TabIndex = 16;
            this.chkHold.UseVisualStyleBackColor = true;
            this.chkHold.CheckedChanged += new System.EventHandler(this.chkHold_CheckedChanged);
            // 
            // chkDecay
            // 
            this.chkDecay.AutoSize = true;
            this.chkDecay.Location = new System.Drawing.Point(551, 113);
            this.chkDecay.Name = "chkDecay";
            this.chkDecay.Size = new System.Drawing.Size(28, 27);
            this.chkDecay.TabIndex = 17;
            this.chkDecay.UseVisualStyleBackColor = true;
            this.chkDecay.CheckedChanged += new System.EventHandler(this.chkDecay_CheckedChanged);
            // 
            // chkSustain
            // 
            this.chkSustain.AutoSize = true;
            this.chkSustain.Location = new System.Drawing.Point(551, 158);
            this.chkSustain.Name = "chkSustain";
            this.chkSustain.Size = new System.Drawing.Size(28, 27);
            this.chkSustain.TabIndex = 18;
            this.chkSustain.UseVisualStyleBackColor = true;
            this.chkSustain.CheckedChanged += new System.EventHandler(this.chkSustain_CheckedChanged);
            // 
            // chkReleace
            // 
            this.chkReleace.AutoSize = true;
            this.chkReleace.Location = new System.Drawing.Point(552, 196);
            this.chkReleace.Name = "chkReleace";
            this.chkReleace.Size = new System.Drawing.Size(28, 27);
            this.chkReleace.TabIndex = 19;
            this.chkReleace.UseVisualStyleBackColor = true;
            this.chkReleace.CheckedChanged += new System.EventHandler(this.chkReleace_CheckedChanged);
            // 
            // grpAmp
            // 
            this.grpAmp.Controls.Add(this.hsbAttack);
            this.grpAmp.Controls.Add(this.chkReleace);
            this.grpAmp.Controls.Add(this.picAttack);
            this.grpAmp.Controls.Add(this.chkSustain);
            this.grpAmp.Controls.Add(this.picHold);
            this.grpAmp.Controls.Add(this.chkDecay);
            this.grpAmp.Controls.Add(this.picDecay);
            this.grpAmp.Controls.Add(this.chkHold);
            this.grpAmp.Controls.Add(this.picSustain);
            this.grpAmp.Controls.Add(this.chkAttack);
            this.grpAmp.Controls.Add(this.picReleace);
            this.grpAmp.Controls.Add(this.lblReleace);
            this.grpAmp.Controls.Add(this.hsbHold);
            this.grpAmp.Controls.Add(this.lblSustain);
            this.grpAmp.Controls.Add(this.hsbDecay);
            this.grpAmp.Controls.Add(this.lblDecay);
            this.grpAmp.Controls.Add(this.hsbSustain);
            this.grpAmp.Controls.Add(this.lblHold);
            this.grpAmp.Controls.Add(this.hsbReleace);
            this.grpAmp.Controls.Add(this.lblAttack);
            this.grpAmp.Location = new System.Drawing.Point(3, 3);
            this.grpAmp.Name = "grpAmp";
            this.grpAmp.Size = new System.Drawing.Size(793, 280);
            this.grpAmp.TabIndex = 20;
            this.grpAmp.TabStop = false;
            this.grpAmp.Text = "振幅エンベロープ";
            // 
            // grpEq
            // 
            this.grpEq.Controls.Add(this.hsbEqAttack);
            this.grpEq.Controls.Add(this.chkEqReleace);
            this.grpEq.Controls.Add(this.lblEqAttack);
            this.grpEq.Controls.Add(this.picEqAttack);
            this.grpEq.Controls.Add(this.hsbEqReleace);
            this.grpEq.Controls.Add(this.chkEqSustain);
            this.grpEq.Controls.Add(this.lblEqHold);
            this.grpEq.Controls.Add(this.picEqHold);
            this.grpEq.Controls.Add(this.hsbEqSustain);
            this.grpEq.Controls.Add(this.chkEqDecay);
            this.grpEq.Controls.Add(this.lblEqDecay);
            this.grpEq.Controls.Add(this.picEqDecay);
            this.grpEq.Controls.Add(this.hsbEqDecay);
            this.grpEq.Controls.Add(this.chkEqHold);
            this.grpEq.Controls.Add(this.lblEqSustain);
            this.grpEq.Controls.Add(this.picEqSustain);
            this.grpEq.Controls.Add(this.hsbEqHold);
            this.grpEq.Controls.Add(this.chkEqAttack);
            this.grpEq.Controls.Add(this.lblEqReleace);
            this.grpEq.Controls.Add(this.picEqReleace);
            this.grpEq.Location = new System.Drawing.Point(3, 289);
            this.grpEq.Name = "grpEq";
            this.grpEq.Size = new System.Drawing.Size(793, 280);
            this.grpEq.TabIndex = 21;
            this.grpEq.TabStop = false;
            this.grpEq.Text = "フィルターエンベロープ";
            // 
            // hsbEqAttack
            // 
            this.hsbEqAttack.Location = new System.Drawing.Point(87, 30);
            this.hsbEqAttack.Maximum = 4096;
            this.hsbEqAttack.Minimum = 1;
            this.hsbEqAttack.Name = "hsbEqAttack";
            this.hsbEqAttack.Size = new System.Drawing.Size(478, 38);
            this.hsbEqAttack.TabIndex = 25;
            this.hsbEqAttack.Value = 1;
            this.hsbEqAttack.ValueChanged += new System.EventHandler(this.hsbEqAttack_ValueChanged);
            // 
            // chkEqReleace
            // 
            this.chkEqReleace.AutoSize = true;
            this.chkEqReleace.Location = new System.Drawing.Point(569, 189);
            this.chkEqReleace.Name = "chkEqReleace";
            this.chkEqReleace.Size = new System.Drawing.Size(28, 27);
            this.chkEqReleace.TabIndex = 39;
            this.chkEqReleace.UseVisualStyleBackColor = true;
            this.chkEqReleace.CheckedChanged += new System.EventHandler(this.chkEqReleace_CheckedChanged);
            // 
            // lblEqAttack
            // 
            this.lblEqAttack.AutoSize = true;
            this.lblEqAttack.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblEqAttack.Location = new System.Drawing.Point(613, 44);
            this.lblEqAttack.Name = "lblEqAttack";
            this.lblEqAttack.Size = new System.Drawing.Size(103, 30);
            this.lblEqAttack.TabIndex = 30;
            this.lblEqAttack.Text = "label1";
            // 
            // picEqAttack
            // 
            this.picEqAttack.Image = global::DLSeditor.Properties.Resources.EnvAttack;
            this.picEqAttack.Location = new System.Drawing.Point(15, 30);
            this.picEqAttack.Name = "picEqAttack";
            this.picEqAttack.Size = new System.Drawing.Size(48, 32);
            this.picEqAttack.TabIndex = 20;
            this.picEqAttack.TabStop = false;
            // 
            // hsbEqReleace
            // 
            this.hsbEqReleace.Location = new System.Drawing.Point(87, 181);
            this.hsbEqReleace.Maximum = 4096;
            this.hsbEqReleace.Minimum = 1;
            this.hsbEqReleace.Name = "hsbEqReleace";
            this.hsbEqReleace.Size = new System.Drawing.Size(478, 38);
            this.hsbEqReleace.TabIndex = 29;
            this.hsbEqReleace.Value = 1;
            this.hsbEqReleace.ValueChanged += new System.EventHandler(this.hsbEqReleace_ValueChanged);
            // 
            // chkEqSustain
            // 
            this.chkEqSustain.AutoSize = true;
            this.chkEqSustain.Location = new System.Drawing.Point(568, 151);
            this.chkEqSustain.Name = "chkEqSustain";
            this.chkEqSustain.Size = new System.Drawing.Size(28, 27);
            this.chkEqSustain.TabIndex = 38;
            this.chkEqSustain.UseVisualStyleBackColor = true;
            this.chkEqSustain.CheckedChanged += new System.EventHandler(this.chkEqSustain_CheckedChanged);
            // 
            // lblEqHold
            // 
            this.lblEqHold.AutoSize = true;
            this.lblEqHold.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblEqHold.Location = new System.Drawing.Point(613, 75);
            this.lblEqHold.Name = "lblEqHold";
            this.lblEqHold.Size = new System.Drawing.Size(103, 30);
            this.lblEqHold.TabIndex = 31;
            this.lblEqHold.Text = "label2";
            // 
            // picEqHold
            // 
            this.picEqHold.Image = global::DLSeditor.Properties.Resources.EnvHold;
            this.picEqHold.Location = new System.Drawing.Point(15, 68);
            this.picEqHold.Name = "picEqHold";
            this.picEqHold.Size = new System.Drawing.Size(48, 32);
            this.picEqHold.TabIndex = 21;
            this.picEqHold.TabStop = false;
            // 
            // hsbEqSustain
            // 
            this.hsbEqSustain.Location = new System.Drawing.Point(87, 143);
            this.hsbEqSustain.Maximum = 1000;
            this.hsbEqSustain.Name = "hsbEqSustain";
            this.hsbEqSustain.Size = new System.Drawing.Size(478, 38);
            this.hsbEqSustain.TabIndex = 28;
            this.hsbEqSustain.ValueChanged += new System.EventHandler(this.hsbEqSustain_ValueChanged);
            // 
            // chkEqDecay
            // 
            this.chkEqDecay.AutoSize = true;
            this.chkEqDecay.Location = new System.Drawing.Point(568, 106);
            this.chkEqDecay.Name = "chkEqDecay";
            this.chkEqDecay.Size = new System.Drawing.Size(28, 27);
            this.chkEqDecay.TabIndex = 37;
            this.chkEqDecay.UseVisualStyleBackColor = true;
            this.chkEqDecay.CheckedChanged += new System.EventHandler(this.chkEqDecay_CheckedChanged);
            // 
            // lblEqDecay
            // 
            this.lblEqDecay.AutoSize = true;
            this.lblEqDecay.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblEqDecay.Location = new System.Drawing.Point(613, 113);
            this.lblEqDecay.Name = "lblEqDecay";
            this.lblEqDecay.Size = new System.Drawing.Size(103, 30);
            this.lblEqDecay.TabIndex = 32;
            this.lblEqDecay.Text = "label3";
            // 
            // picEqDecay
            // 
            this.picEqDecay.Image = global::DLSeditor.Properties.Resources.EnvDecay;
            this.picEqDecay.Location = new System.Drawing.Point(15, 106);
            this.picEqDecay.Name = "picEqDecay";
            this.picEqDecay.Size = new System.Drawing.Size(48, 32);
            this.picEqDecay.TabIndex = 22;
            this.picEqDecay.TabStop = false;
            // 
            // hsbEqDecay
            // 
            this.hsbEqDecay.Location = new System.Drawing.Point(87, 106);
            this.hsbEqDecay.Maximum = 4096;
            this.hsbEqDecay.Minimum = 1;
            this.hsbEqDecay.Name = "hsbEqDecay";
            this.hsbEqDecay.Size = new System.Drawing.Size(478, 38);
            this.hsbEqDecay.TabIndex = 27;
            this.hsbEqDecay.Value = 1;
            this.hsbEqDecay.ValueChanged += new System.EventHandler(this.hsbEqDecay_ValueChanged);
            // 
            // chkEqHold
            // 
            this.chkEqHold.AutoSize = true;
            this.chkEqHold.Location = new System.Drawing.Point(569, 75);
            this.chkEqHold.Name = "chkEqHold";
            this.chkEqHold.Size = new System.Drawing.Size(28, 27);
            this.chkEqHold.TabIndex = 36;
            this.chkEqHold.UseVisualStyleBackColor = true;
            this.chkEqHold.CheckedChanged += new System.EventHandler(this.chkEqHold_CheckedChanged);
            // 
            // lblEqSustain
            // 
            this.lblEqSustain.AutoSize = true;
            this.lblEqSustain.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblEqSustain.Location = new System.Drawing.Point(613, 151);
            this.lblEqSustain.Name = "lblEqSustain";
            this.lblEqSustain.Size = new System.Drawing.Size(103, 30);
            this.lblEqSustain.TabIndex = 33;
            this.lblEqSustain.Text = "label4";
            // 
            // picEqSustain
            // 
            this.picEqSustain.Image = global::DLSeditor.Properties.Resources.EnvSustain;
            this.picEqSustain.Location = new System.Drawing.Point(15, 144);
            this.picEqSustain.Name = "picEqSustain";
            this.picEqSustain.Size = new System.Drawing.Size(48, 32);
            this.picEqSustain.TabIndex = 23;
            this.picEqSustain.TabStop = false;
            // 
            // hsbEqHold
            // 
            this.hsbEqHold.Location = new System.Drawing.Point(87, 68);
            this.hsbEqHold.Maximum = 4096;
            this.hsbEqHold.Minimum = 1;
            this.hsbEqHold.Name = "hsbEqHold";
            this.hsbEqHold.Size = new System.Drawing.Size(478, 38);
            this.hsbEqHold.TabIndex = 26;
            this.hsbEqHold.Value = 1;
            this.hsbEqHold.ValueChanged += new System.EventHandler(this.hsbEqHold_ValueChanged);
            // 
            // chkEqAttack
            // 
            this.chkEqAttack.AutoSize = true;
            this.chkEqAttack.Location = new System.Drawing.Point(569, 39);
            this.chkEqAttack.Name = "chkEqAttack";
            this.chkEqAttack.Size = new System.Drawing.Size(28, 27);
            this.chkEqAttack.TabIndex = 35;
            this.chkEqAttack.UseVisualStyleBackColor = true;
            this.chkEqAttack.CheckedChanged += new System.EventHandler(this.chkEqAttack_CheckedChanged);
            // 
            // lblEqReleace
            // 
            this.lblEqReleace.AutoSize = true;
            this.lblEqReleace.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblEqReleace.Location = new System.Drawing.Point(613, 189);
            this.lblEqReleace.Name = "lblEqReleace";
            this.lblEqReleace.Size = new System.Drawing.Size(103, 30);
            this.lblEqReleace.TabIndex = 34;
            this.lblEqReleace.Text = "label5";
            // 
            // picEqReleace
            // 
            this.picEqReleace.Image = global::DLSeditor.Properties.Resources.EnvReleace;
            this.picEqReleace.Location = new System.Drawing.Point(15, 182);
            this.picEqReleace.Name = "picEqReleace";
            this.picEqReleace.Size = new System.Drawing.Size(48, 32);
            this.picEqReleace.TabIndex = 24;
            this.picEqReleace.TabStop = false;
            // 
            // Envelope
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpEq);
            this.Controls.Add(this.grpAmp);
            this.Name = "Envelope";
            this.Size = new System.Drawing.Size(1559, 1057);
            ((System.ComponentModel.ISupportInitialize)(this.picReleace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSustain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDecay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAttack)).EndInit();
            this.grpAmp.ResumeLayout(false);
            this.grpAmp.PerformLayout();
            this.grpEq.ResumeLayout(false);
            this.grpEq.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEqAttack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEqHold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEqDecay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEqSustain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEqReleace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picAttack;
        private System.Windows.Forms.PictureBox picHold;
        private System.Windows.Forms.PictureBox picDecay;
        private System.Windows.Forms.PictureBox picSustain;
        private System.Windows.Forms.PictureBox picReleace;
        private System.Windows.Forms.HScrollBar hsbAttack;
        private System.Windows.Forms.HScrollBar hsbHold;
        private System.Windows.Forms.HScrollBar hsbDecay;
        private System.Windows.Forms.HScrollBar hsbSustain;
        private System.Windows.Forms.HScrollBar hsbReleace;
        private System.Windows.Forms.Label lblAttack;
        private System.Windows.Forms.Label lblHold;
        private System.Windows.Forms.Label lblDecay;
        private System.Windows.Forms.Label lblSustain;
        private System.Windows.Forms.Label lblReleace;
        private System.Windows.Forms.CheckBox chkAttack;
        private System.Windows.Forms.CheckBox chkHold;
        private System.Windows.Forms.CheckBox chkDecay;
        private System.Windows.Forms.CheckBox chkSustain;
        private System.Windows.Forms.CheckBox chkReleace;
        private System.Windows.Forms.GroupBox grpAmp;
        private System.Windows.Forms.GroupBox grpEq;
        private System.Windows.Forms.HScrollBar hsbEqAttack;
        private System.Windows.Forms.CheckBox chkEqReleace;
        private System.Windows.Forms.Label lblEqAttack;
        private System.Windows.Forms.PictureBox picEqAttack;
        private System.Windows.Forms.HScrollBar hsbEqReleace;
        private System.Windows.Forms.CheckBox chkEqSustain;
        private System.Windows.Forms.Label lblEqHold;
        private System.Windows.Forms.PictureBox picEqHold;
        private System.Windows.Forms.HScrollBar hsbEqSustain;
        private System.Windows.Forms.CheckBox chkEqDecay;
        private System.Windows.Forms.Label lblEqDecay;
        private System.Windows.Forms.PictureBox picEqDecay;
        private System.Windows.Forms.HScrollBar hsbEqDecay;
        private System.Windows.Forms.CheckBox chkEqHold;
        private System.Windows.Forms.Label lblEqSustain;
        private System.Windows.Forms.PictureBox picEqSustain;
        private System.Windows.Forms.HScrollBar hsbEqHold;
        private System.Windows.Forms.CheckBox chkEqAttack;
        private System.Windows.Forms.Label lblEqReleace;
        private System.Windows.Forms.PictureBox picEqReleace;
    }
}
