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
			this.hsbAmpAttack = new System.Windows.Forms.HScrollBar();
			this.hsbAmpHold = new System.Windows.Forms.HScrollBar();
			this.hsbAmpDecay = new System.Windows.Forms.HScrollBar();
			this.hsbAmpSustain = new System.Windows.Forms.HScrollBar();
			this.hsbAmpReleace = new System.Windows.Forms.HScrollBar();
			this.lblAmpAttack = new System.Windows.Forms.Label();
			this.lblAmpHold = new System.Windows.Forms.Label();
			this.lblAmpDecay = new System.Windows.Forms.Label();
			this.lblAmpSustain = new System.Windows.Forms.Label();
			this.lblAmpReleace = new System.Windows.Forms.Label();
			this.picAmpReleace = new System.Windows.Forms.PictureBox();
			this.picAmpSustain = new System.Windows.Forms.PictureBox();
			this.picAmpDecay = new System.Windows.Forms.PictureBox();
			this.picAmpHold = new System.Windows.Forms.PictureBox();
			this.picAmpAttack = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picAmpReleace)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picAmpSustain)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picAmpDecay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picAmpHold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picAmpAttack)).BeginInit();
			this.SuspendLayout();
			// 
			// hsbAmpAttack
			// 
			this.hsbAmpAttack.Location = new System.Drawing.Point(58, 4);
			this.hsbAmpAttack.Maximum = 2048;
			this.hsbAmpAttack.Minimum = 1;
			this.hsbAmpAttack.Name = "hsbAmpAttack";
			this.hsbAmpAttack.Size = new System.Drawing.Size(478, 38);
			this.hsbAmpAttack.TabIndex = 5;
			this.hsbAmpAttack.Value = 1;
			this.hsbAmpAttack.ValueChanged += new System.EventHandler(this.hsbAmpAttack_ValueChanged);
			// 
			// hsbAmpHold
			// 
			this.hsbAmpHold.Location = new System.Drawing.Point(58, 42);
			this.hsbAmpHold.Maximum = 2048;
			this.hsbAmpHold.Minimum = 1;
			this.hsbAmpHold.Name = "hsbAmpHold";
			this.hsbAmpHold.Size = new System.Drawing.Size(478, 38);
			this.hsbAmpHold.TabIndex = 6;
			this.hsbAmpHold.Value = 1;
			this.hsbAmpHold.ValueChanged += new System.EventHandler(this.hsbAmpHold_ValueChanged);
			// 
			// hsbAmpDecay
			// 
			this.hsbAmpDecay.Location = new System.Drawing.Point(58, 80);
			this.hsbAmpDecay.Maximum = 2048;
			this.hsbAmpDecay.Minimum = 1;
			this.hsbAmpDecay.Name = "hsbAmpDecay";
			this.hsbAmpDecay.Size = new System.Drawing.Size(478, 38);
			this.hsbAmpDecay.TabIndex = 7;
			this.hsbAmpDecay.Value = 1;
			this.hsbAmpDecay.ValueChanged += new System.EventHandler(this.hsbAmpDecay_ValueChanged);
			// 
			// hsbAmpSustain
			// 
			this.hsbAmpSustain.Location = new System.Drawing.Point(58, 117);
			this.hsbAmpSustain.Maximum = 1000;
			this.hsbAmpSustain.Name = "hsbAmpSustain";
			this.hsbAmpSustain.Size = new System.Drawing.Size(478, 38);
			this.hsbAmpSustain.TabIndex = 8;
			this.hsbAmpSustain.ValueChanged += new System.EventHandler(this.hsbAmpSustain_ValueChanged);
			// 
			// hsbAmpReleace
			// 
			this.hsbAmpReleace.Location = new System.Drawing.Point(58, 155);
			this.hsbAmpReleace.Maximum = 2048;
			this.hsbAmpReleace.Minimum = 1;
			this.hsbAmpReleace.Name = "hsbAmpReleace";
			this.hsbAmpReleace.Size = new System.Drawing.Size(478, 38);
			this.hsbAmpReleace.TabIndex = 9;
			this.hsbAmpReleace.Value = 1;
			this.hsbAmpReleace.ValueChanged += new System.EventHandler(this.hsbAmpReleace_ValueChanged);
			// 
			// lblAmpAttack
			// 
			this.lblAmpAttack.AutoSize = true;
			this.lblAmpAttack.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblAmpAttack.Location = new System.Drawing.Point(540, 17);
			this.lblAmpAttack.Name = "lblAmpAttack";
			this.lblAmpAttack.Size = new System.Drawing.Size(82, 24);
			this.lblAmpAttack.TabIndex = 10;
			this.lblAmpAttack.Text = "label1";
			// 
			// lblAmpHold
			// 
			this.lblAmpHold.AutoSize = true;
			this.lblAmpHold.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblAmpHold.Location = new System.Drawing.Point(539, 49);
			this.lblAmpHold.Name = "lblAmpHold";
			this.lblAmpHold.Size = new System.Drawing.Size(82, 24);
			this.lblAmpHold.TabIndex = 11;
			this.lblAmpHold.Text = "label2";
			// 
			// lblAmpDecay
			// 
			this.lblAmpDecay.AutoSize = true;
			this.lblAmpDecay.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblAmpDecay.Location = new System.Drawing.Point(540, 87);
			this.lblAmpDecay.Name = "lblAmpDecay";
			this.lblAmpDecay.Size = new System.Drawing.Size(82, 24);
			this.lblAmpDecay.TabIndex = 12;
			this.lblAmpDecay.Text = "label3";
			// 
			// lblAmpSustain
			// 
			this.lblAmpSustain.AutoSize = true;
			this.lblAmpSustain.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblAmpSustain.Location = new System.Drawing.Point(539, 125);
			this.lblAmpSustain.Name = "lblAmpSustain";
			this.lblAmpSustain.Size = new System.Drawing.Size(82, 24);
			this.lblAmpSustain.TabIndex = 13;
			this.lblAmpSustain.Text = "label4";
			// 
			// lblAmpReleace
			// 
			this.lblAmpReleace.AutoSize = true;
			this.lblAmpReleace.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblAmpReleace.Location = new System.Drawing.Point(539, 155);
			this.lblAmpReleace.Name = "lblAmpReleace";
			this.lblAmpReleace.Size = new System.Drawing.Size(82, 24);
			this.lblAmpReleace.TabIndex = 14;
			this.lblAmpReleace.Text = "label5";
			// 
			// picAmpReleace
			// 
			this.picAmpReleace.Image = global::DLSeditor.Properties.Resources.EnvReleace;
			this.picAmpReleace.Location = new System.Drawing.Point(3, 155);
			this.picAmpReleace.Name = "picAmpReleace";
			this.picAmpReleace.Size = new System.Drawing.Size(48, 32);
			this.picAmpReleace.TabIndex = 4;
			this.picAmpReleace.TabStop = false;
			// 
			// picAmpSustain
			// 
			this.picAmpSustain.Image = global::DLSeditor.Properties.Resources.EnvSustain;
			this.picAmpSustain.Location = new System.Drawing.Point(3, 117);
			this.picAmpSustain.Name = "picAmpSustain";
			this.picAmpSustain.Size = new System.Drawing.Size(48, 32);
			this.picAmpSustain.TabIndex = 3;
			this.picAmpSustain.TabStop = false;
			// 
			// picAmpDecay
			// 
			this.picAmpDecay.Image = global::DLSeditor.Properties.Resources.EnvDecay;
			this.picAmpDecay.Location = new System.Drawing.Point(3, 79);
			this.picAmpDecay.Name = "picAmpDecay";
			this.picAmpDecay.Size = new System.Drawing.Size(48, 32);
			this.picAmpDecay.TabIndex = 2;
			this.picAmpDecay.TabStop = false;
			// 
			// picAmpHold
			// 
			this.picAmpHold.Image = global::DLSeditor.Properties.Resources.EnvHold;
			this.picAmpHold.Location = new System.Drawing.Point(3, 41);
			this.picAmpHold.Name = "picAmpHold";
			this.picAmpHold.Size = new System.Drawing.Size(48, 32);
			this.picAmpHold.TabIndex = 1;
			this.picAmpHold.TabStop = false;
			// 
			// picAmpAttack
			// 
			this.picAmpAttack.Image = global::DLSeditor.Properties.Resources.EnvAttack;
			this.picAmpAttack.Location = new System.Drawing.Point(3, 3);
			this.picAmpAttack.Name = "picAmpAttack";
			this.picAmpAttack.Size = new System.Drawing.Size(48, 32);
			this.picAmpAttack.TabIndex = 0;
			this.picAmpAttack.TabStop = false;
			// 
			// AmpEnvelope
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblAmpReleace);
			this.Controls.Add(this.lblAmpSustain);
			this.Controls.Add(this.lblAmpDecay);
			this.Controls.Add(this.lblAmpHold);
			this.Controls.Add(this.lblAmpAttack);
			this.Controls.Add(this.hsbAmpReleace);
			this.Controls.Add(this.hsbAmpSustain);
			this.Controls.Add(this.hsbAmpDecay);
			this.Controls.Add(this.hsbAmpHold);
			this.Controls.Add(this.hsbAmpAttack);
			this.Controls.Add(this.picAmpReleace);
			this.Controls.Add(this.picAmpSustain);
			this.Controls.Add(this.picAmpDecay);
			this.Controls.Add(this.picAmpHold);
			this.Controls.Add(this.picAmpAttack);
			this.Name = "AmpEnvelope";
			this.Size = new System.Drawing.Size(844, 195);
			((System.ComponentModel.ISupportInitialize)(this.picAmpReleace)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picAmpSustain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picAmpDecay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picAmpHold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picAmpAttack)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picAmpAttack;
		private System.Windows.Forms.PictureBox picAmpHold;
		private System.Windows.Forms.PictureBox picAmpDecay;
		private System.Windows.Forms.PictureBox picAmpSustain;
		private System.Windows.Forms.PictureBox picAmpReleace;
		private System.Windows.Forms.HScrollBar hsbAmpAttack;
		private System.Windows.Forms.HScrollBar hsbAmpHold;
		private System.Windows.Forms.HScrollBar hsbAmpDecay;
		private System.Windows.Forms.HScrollBar hsbAmpSustain;
		private System.Windows.Forms.HScrollBar hsbAmpReleace;
		private System.Windows.Forms.Label lblAmpAttack;
		private System.Windows.Forms.Label lblAmpHold;
		private System.Windows.Forms.Label lblAmpDecay;
		private System.Windows.Forms.Label lblAmpSustain;
		private System.Windows.Forms.Label lblAmpReleace;
	}
}
