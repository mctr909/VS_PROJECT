namespace DLSeditor {
	partial class AmpEnvelope {
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
			((System.ComponentModel.ISupportInitialize)(this.picReleace)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picSustain)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picDecay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picHold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picAttack)).BeginInit();
			this.SuspendLayout();
			// 
			// hsbAttack
			// 
			this.hsbAttack.Location = new System.Drawing.Point(58, 4);
			this.hsbAttack.Maximum = 2048;
			this.hsbAttack.Minimum = 1;
			this.hsbAttack.Name = "hsbAttack";
			this.hsbAttack.Size = new System.Drawing.Size(478, 38);
			this.hsbAttack.TabIndex = 5;
			this.hsbAttack.Value = 1;
			this.hsbAttack.ValueChanged += new System.EventHandler(this.hsbAmpAttack_ValueChanged);
			// 
			// hsbHold
			// 
			this.hsbHold.Location = new System.Drawing.Point(58, 42);
			this.hsbHold.Maximum = 2048;
			this.hsbHold.Minimum = 1;
			this.hsbHold.Name = "hsbHold";
			this.hsbHold.Size = new System.Drawing.Size(478, 38);
			this.hsbHold.TabIndex = 6;
			this.hsbHold.Value = 1;
			this.hsbHold.ValueChanged += new System.EventHandler(this.hsbAmpHold_ValueChanged);
			// 
			// hsbDecay
			// 
			this.hsbDecay.Location = new System.Drawing.Point(58, 80);
			this.hsbDecay.Maximum = 2048;
			this.hsbDecay.Minimum = 1;
			this.hsbDecay.Name = "hsbDecay";
			this.hsbDecay.Size = new System.Drawing.Size(478, 38);
			this.hsbDecay.TabIndex = 7;
			this.hsbDecay.Value = 1;
			this.hsbDecay.ValueChanged += new System.EventHandler(this.hsbAmpDecay_ValueChanged);
			// 
			// hsbSustain
			// 
			this.hsbSustain.Location = new System.Drawing.Point(58, 117);
			this.hsbSustain.Maximum = 1000;
			this.hsbSustain.Name = "hsbSustain";
			this.hsbSustain.Size = new System.Drawing.Size(478, 38);
			this.hsbSustain.TabIndex = 8;
			this.hsbSustain.ValueChanged += new System.EventHandler(this.hsbAmpSustain_ValueChanged);
			// 
			// hsbReleace
			// 
			this.hsbReleace.Location = new System.Drawing.Point(58, 155);
			this.hsbReleace.Maximum = 2048;
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
			this.lblAttack.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblAttack.Location = new System.Drawing.Point(584, 18);
			this.lblAttack.Name = "lblAttack";
			this.lblAttack.Size = new System.Drawing.Size(82, 24);
			this.lblAttack.TabIndex = 10;
			this.lblAttack.Text = "label1";
			// 
			// lblHold
			// 
			this.lblHold.AutoSize = true;
			this.lblHold.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblHold.Location = new System.Drawing.Point(584, 49);
			this.lblHold.Name = "lblHold";
			this.lblHold.Size = new System.Drawing.Size(82, 24);
			this.lblHold.TabIndex = 11;
			this.lblHold.Text = "label2";
			// 
			// lblDecay
			// 
			this.lblDecay.AutoSize = true;
			this.lblDecay.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblDecay.Location = new System.Drawing.Point(584, 87);
			this.lblDecay.Name = "lblDecay";
			this.lblDecay.Size = new System.Drawing.Size(82, 24);
			this.lblDecay.TabIndex = 12;
			this.lblDecay.Text = "label3";
			// 
			// lblSustain
			// 
			this.lblSustain.AutoSize = true;
			this.lblSustain.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblSustain.Location = new System.Drawing.Point(584, 125);
			this.lblSustain.Name = "lblSustain";
			this.lblSustain.Size = new System.Drawing.Size(82, 24);
			this.lblSustain.TabIndex = 13;
			this.lblSustain.Text = "label4";
			// 
			// lblReleace
			// 
			this.lblReleace.AutoSize = true;
			this.lblReleace.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblReleace.Location = new System.Drawing.Point(584, 163);
			this.lblReleace.Name = "lblReleace";
			this.lblReleace.Size = new System.Drawing.Size(82, 24);
			this.lblReleace.TabIndex = 14;
			this.lblReleace.Text = "label5";
			// 
			// picReleace
			// 
			this.picReleace.Image = global::DLSeditor.Properties.Resources.EnvReleace;
			this.picReleace.Location = new System.Drawing.Point(3, 155);
			this.picReleace.Name = "picReleace";
			this.picReleace.Size = new System.Drawing.Size(48, 32);
			this.picReleace.TabIndex = 4;
			this.picReleace.TabStop = false;
			// 
			// picSustain
			// 
			this.picSustain.Image = global::DLSeditor.Properties.Resources.EnvSustain;
			this.picSustain.Location = new System.Drawing.Point(3, 117);
			this.picSustain.Name = "picSustain";
			this.picSustain.Size = new System.Drawing.Size(48, 32);
			this.picSustain.TabIndex = 3;
			this.picSustain.TabStop = false;
			// 
			// picDecay
			// 
			this.picDecay.Image = global::DLSeditor.Properties.Resources.EnvDecay;
			this.picDecay.Location = new System.Drawing.Point(3, 79);
			this.picDecay.Name = "picDecay";
			this.picDecay.Size = new System.Drawing.Size(48, 32);
			this.picDecay.TabIndex = 2;
			this.picDecay.TabStop = false;
			// 
			// picHold
			// 
			this.picHold.Image = global::DLSeditor.Properties.Resources.EnvHold;
			this.picHold.Location = new System.Drawing.Point(3, 41);
			this.picHold.Name = "picHold";
			this.picHold.Size = new System.Drawing.Size(48, 32);
			this.picHold.TabIndex = 1;
			this.picHold.TabStop = false;
			// 
			// picAttack
			// 
			this.picAttack.Image = global::DLSeditor.Properties.Resources.EnvAttack;
			this.picAttack.Location = new System.Drawing.Point(3, 3);
			this.picAttack.Name = "picAttack";
			this.picAttack.Size = new System.Drawing.Size(48, 32);
			this.picAttack.TabIndex = 0;
			this.picAttack.TabStop = false;
			// 
			// chkAttack
			// 
			this.chkAttack.AutoSize = true;
			this.chkAttack.Location = new System.Drawing.Point(540, 13);
			this.chkAttack.Name = "chkAttack";
			this.chkAttack.Size = new System.Drawing.Size(28, 27);
			this.chkAttack.TabIndex = 15;
			this.chkAttack.UseVisualStyleBackColor = true;
			this.chkAttack.CheckedChanged += new System.EventHandler(this.chkAttack_CheckedChanged);
			// 
			// chkHold
			// 
			this.chkHold.AutoSize = true;
			this.chkHold.Location = new System.Drawing.Point(540, 49);
			this.chkHold.Name = "chkHold";
			this.chkHold.Size = new System.Drawing.Size(28, 27);
			this.chkHold.TabIndex = 16;
			this.chkHold.UseVisualStyleBackColor = true;
			this.chkHold.CheckedChanged += new System.EventHandler(this.chkHold_CheckedChanged);
			// 
			// chkDecay
			// 
			this.chkDecay.AutoSize = true;
			this.chkDecay.Location = new System.Drawing.Point(539, 80);
			this.chkDecay.Name = "chkDecay";
			this.chkDecay.Size = new System.Drawing.Size(28, 27);
			this.chkDecay.TabIndex = 17;
			this.chkDecay.UseVisualStyleBackColor = true;
			this.chkDecay.CheckedChanged += new System.EventHandler(this.chkDecay_CheckedChanged);
			// 
			// chkSustain
			// 
			this.chkSustain.AutoSize = true;
			this.chkSustain.Location = new System.Drawing.Point(539, 125);
			this.chkSustain.Name = "chkSustain";
			this.chkSustain.Size = new System.Drawing.Size(28, 27);
			this.chkSustain.TabIndex = 18;
			this.chkSustain.UseVisualStyleBackColor = true;
			this.chkSustain.CheckedChanged += new System.EventHandler(this.chkSustain_CheckedChanged);
			// 
			// chkReleace
			// 
			this.chkReleace.AutoSize = true;
			this.chkReleace.Location = new System.Drawing.Point(540, 163);
			this.chkReleace.Name = "chkReleace";
			this.chkReleace.Size = new System.Drawing.Size(28, 27);
			this.chkReleace.TabIndex = 19;
			this.chkReleace.UseVisualStyleBackColor = true;
			this.chkReleace.CheckedChanged += new System.EventHandler(this.chkReleace_CheckedChanged);
			// 
			// AmpEnvelope
			// 
			this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkReleace);
			this.Controls.Add(this.chkSustain);
			this.Controls.Add(this.chkDecay);
			this.Controls.Add(this.chkHold);
			this.Controls.Add(this.chkAttack);
			this.Controls.Add(this.lblReleace);
			this.Controls.Add(this.lblSustain);
			this.Controls.Add(this.lblDecay);
			this.Controls.Add(this.lblHold);
			this.Controls.Add(this.lblAttack);
			this.Controls.Add(this.hsbReleace);
			this.Controls.Add(this.hsbSustain);
			this.Controls.Add(this.hsbDecay);
			this.Controls.Add(this.hsbHold);
			this.Controls.Add(this.hsbAttack);
			this.Controls.Add(this.picReleace);
			this.Controls.Add(this.picSustain);
			this.Controls.Add(this.picDecay);
			this.Controls.Add(this.picHold);
			this.Controls.Add(this.picAttack);
			this.Name = "AmpEnvelope";
			this.Size = new System.Drawing.Size(706, 195);
			((System.ComponentModel.ISupportInitialize)(this.picReleace)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picSustain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picDecay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picHold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picAttack)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

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
	}
}
