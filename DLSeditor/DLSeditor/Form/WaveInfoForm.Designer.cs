namespace DLSeditor
{
	partial class WaveInfoForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnPlay = new System.Windows.Forms.Button();
			this.picSpectrum = new System.Windows.Forms.PictureBox();
			this.hsbTime = new System.Windows.Forms.HScrollBar();
			this.picWave = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.numScale = new System.Windows.Forms.NumericUpDown();
			this.picLoop = new System.Windows.Forms.PictureBox();
			this.grbMain = new System.Windows.Forms.GroupBox();
			this.grbLoop = new System.Windows.Forms.GroupBox();
			this.numScaleLoop = new System.Windows.Forms.NumericUpDown();
			this.btnUpdate = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picSpectrum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWave)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picLoop)).BeginInit();
			this.grbMain.SuspendLayout();
			this.grbLoop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numScaleLoop)).BeginInit();
			this.SuspendLayout();
			// 
			// btnPlay
			// 
			this.btnPlay.Location = new System.Drawing.Point(16, 15);
			this.btnPlay.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.btnPlay.Name = "btnPlay";
			this.btnPlay.Size = new System.Drawing.Size(132, 46);
			this.btnPlay.TabIndex = 0;
			this.btnPlay.Text = "再生";
			this.btnPlay.UseVisualStyleBackColor = true;
			this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
			// 
			// picSpectrum
			// 
			this.picSpectrum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picSpectrum.Location = new System.Drawing.Point(10, 82);
			this.picSpectrum.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.picSpectrum.Name = "picSpectrum";
			this.picSpectrum.Size = new System.Drawing.Size(1681, 224);
			this.picSpectrum.TabIndex = 2;
			this.picSpectrum.TabStop = false;
			// 
			// hsbTime
			// 
			this.hsbTime.Location = new System.Drawing.Point(10, 580);
			this.hsbTime.Name = "hsbTime";
			this.hsbTime.Size = new System.Drawing.Size(1681, 28);
			this.hsbTime.TabIndex = 3;
			// 
			// picWave
			// 
			this.picWave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picWave.Location = new System.Drawing.Point(10, 318);
			this.picWave.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.picWave.Name = "picWave";
			this.picWave.Size = new System.Drawing.Size(1681, 256);
			this.picWave.TabIndex = 4;
			this.picWave.TabStop = false;
			this.picWave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picWave_MouseDown);
			this.picWave.MouseEnter += new System.EventHandler(this.picWave_MouseEnter);
			this.picWave.MouseLeave += new System.EventHandler(this.picWave_MouseLeave);
			this.picWave.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picWave_MouseMove);
			this.picWave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picWave_MouseUp);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// numScale
			// 
			this.numScale.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numScale.Location = new System.Drawing.Point(10, 33);
			this.numScale.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.numScale.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
			this.numScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numScale.Name = "numScale";
			this.numScale.Size = new System.Drawing.Size(163, 37);
			this.numScale.TabIndex = 5;
			this.numScale.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
			this.numScale.ValueChanged += new System.EventHandler(this.numScale_ValueChanged);
			// 
			// picLoop
			// 
			this.picLoop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picLoop.Location = new System.Drawing.Point(10, 82);
			this.picLoop.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.picLoop.Name = "picLoop";
			this.picLoop.Size = new System.Drawing.Size(1681, 256);
			this.picLoop.TabIndex = 6;
			this.picLoop.TabStop = false;
			// 
			// grbMain
			// 
			this.grbMain.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbMain.Controls.Add(this.picSpectrum);
			this.grbMain.Controls.Add(this.picWave);
			this.grbMain.Controls.Add(this.numScale);
			this.grbMain.Controls.Add(this.hsbTime);
			this.grbMain.Location = new System.Drawing.Point(16, 71);
			this.grbMain.Name = "grbMain";
			this.grbMain.Size = new System.Drawing.Size(1706, 648);
			this.grbMain.TabIndex = 7;
			this.grbMain.TabStop = false;
			this.grbMain.Text = "ループ範囲選択";
			// 
			// grbLoop
			// 
			this.grbLoop.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbLoop.Controls.Add(this.numScaleLoop);
			this.grbLoop.Controls.Add(this.picLoop);
			this.grbLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grbLoop.Location = new System.Drawing.Point(16, 737);
			this.grbLoop.Name = "grbLoop";
			this.grbLoop.Size = new System.Drawing.Size(1706, 351);
			this.grbLoop.TabIndex = 8;
			this.grbLoop.TabStop = false;
			this.grbLoop.Text = "ループ範囲表示";
			// 
			// numScaleLoop
			// 
			this.numScaleLoop.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numScaleLoop.Location = new System.Drawing.Point(10, 33);
			this.numScaleLoop.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.numScaleLoop.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
			this.numScaleLoop.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numScaleLoop.Name = "numScaleLoop";
			this.numScaleLoop.Size = new System.Drawing.Size(163, 37);
			this.numScaleLoop.TabIndex = 7;
			this.numScaleLoop.Value = new decimal(new int[] {
            36,
            0,
            0,
            0});
			this.numScaleLoop.ValueChanged += new System.EventHandler(this.numScaleLoop_ValueChanged);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(158, 15);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(132, 46);
			this.btnUpdate.TabIndex = 10;
			this.btnUpdate.Text = "反映";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// WaveInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1735, 1109);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.grbLoop);
			this.Controls.Add(this.grbMain);
			this.Controls.Add(this.btnPlay);
			this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.Name = "WaveInfoForm";
			this.Text = "WaveInfoForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WaveInfoForm_FormClosing);
			this.Load += new System.EventHandler(this.WaveInfoForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.picSpectrum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWave)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picLoop)).EndInit();
			this.grbMain.ResumeLayout(false);
			this.grbLoop.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numScaleLoop)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnPlay;
		private System.Windows.Forms.PictureBox picSpectrum;
		private System.Windows.Forms.HScrollBar hsbTime;
		private System.Windows.Forms.PictureBox picWave;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.NumericUpDown numScale;
		private System.Windows.Forms.PictureBox picLoop;
		private System.Windows.Forms.GroupBox grbMain;
		private System.Windows.Forms.GroupBox grbLoop;
		private System.Windows.Forms.NumericUpDown numScaleLoop;
		private System.Windows.Forms.Button btnUpdate;
	}
}