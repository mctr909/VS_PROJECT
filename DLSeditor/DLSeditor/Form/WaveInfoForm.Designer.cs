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
			this.btnUpdate = new System.Windows.Forms.Button();
			this.grbLoop = new System.Windows.Forms.GroupBox();
			this.numScaleLoop = new System.Windows.Forms.NumericUpDown();
			this.numUnityNote = new System.Windows.Forms.NumericUpDown();
			this.numFineTune = new System.Windows.Forms.NumericUpDown();
			this.lblUnityNote = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblPitch = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnUpdateTone = new System.Windows.Forms.Button();
			this.btnLoopCreate = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picSpectrum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWave)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picLoop)).BeginInit();
			this.grbMain.SuspendLayout();
			this.grbLoop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numScaleLoop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUnityNote)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFineTune)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnPlay
			// 
			this.btnPlay.Location = new System.Drawing.Point(12, 12);
			this.btnPlay.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.btnPlay.Name = "btnPlay";
			this.btnPlay.Size = new System.Drawing.Size(132, 74);
			this.btnPlay.TabIndex = 0;
			this.btnPlay.Text = "再生";
			this.btnPlay.UseVisualStyleBackColor = true;
			this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
			// 
			// picSpectrum
			// 
			this.picSpectrum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picSpectrum.Location = new System.Drawing.Point(10, 49);
			this.picSpectrum.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.picSpectrum.Name = "picSpectrum";
			this.picSpectrum.Size = new System.Drawing.Size(1681, 224);
			this.picSpectrum.TabIndex = 2;
			this.picSpectrum.TabStop = false;
			// 
			// hsbTime
			// 
			this.hsbTime.Location = new System.Drawing.Point(10, 491);
			this.hsbTime.Name = "hsbTime";
			this.hsbTime.Size = new System.Drawing.Size(1681, 28);
			this.hsbTime.TabIndex = 3;
			// 
			// picWave
			// 
			this.picWave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picWave.Location = new System.Drawing.Point(10, 285);
			this.picWave.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.picWave.Name = "picWave";
			this.picWave.Size = new System.Drawing.Size(1681, 200);
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
			this.numScale.Location = new System.Drawing.Point(211, 0);
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
			this.numScale.Size = new System.Drawing.Size(125, 37);
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
			this.picLoop.Location = new System.Drawing.Point(10, 49);
			this.picLoop.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.picLoop.Name = "picLoop";
			this.picLoop.Size = new System.Drawing.Size(1681, 200);
			this.picLoop.TabIndex = 6;
			this.picLoop.TabStop = false;
			// 
			// grbMain
			// 
			this.grbMain.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbMain.Controls.Add(this.btnLoopCreate);
			this.grbMain.Controls.Add(this.picSpectrum);
			this.grbMain.Controls.Add(this.picWave);
			this.grbMain.Controls.Add(this.numScale);
			this.grbMain.Controls.Add(this.hsbTime);
			this.grbMain.Controls.Add(this.btnUpdate);
			this.grbMain.Location = new System.Drawing.Point(12, 96);
			this.grbMain.Name = "grbMain";
			this.grbMain.Size = new System.Drawing.Size(1706, 564);
			this.grbMain.TabIndex = 7;
			this.grbMain.TabStop = false;
			this.grbMain.Text = "ループ範囲選択";
			// 
			// btnUpdate
			// 
			this.btnUpdate.BackColor = System.Drawing.SystemColors.Control;
			this.btnUpdate.Location = new System.Drawing.Point(358, 0);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(199, 42);
			this.btnUpdate.TabIndex = 10;
			this.btnUpdate.Text = "ループ範囲反映";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// grbLoop
			// 
			this.grbLoop.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbLoop.Controls.Add(this.numScaleLoop);
			this.grbLoop.Controls.Add(this.picLoop);
			this.grbLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grbLoop.Location = new System.Drawing.Point(12, 670);
			this.grbLoop.Name = "grbLoop";
			this.grbLoop.Size = new System.Drawing.Size(1706, 261);
			this.grbLoop.TabIndex = 8;
			this.grbLoop.TabStop = false;
			this.grbLoop.Text = "ループ範囲表示";
			// 
			// numScaleLoop
			// 
			this.numScaleLoop.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numScaleLoop.Location = new System.Drawing.Point(211, 0);
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
			this.numScaleLoop.Size = new System.Drawing.Size(125, 37);
			this.numScaleLoop.TabIndex = 7;
			this.numScaleLoop.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
			this.numScaleLoop.ValueChanged += new System.EventHandler(this.numScaleLoop_ValueChanged);
			// 
			// numUnityNote
			// 
			this.numUnityNote.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numUnityNote.Location = new System.Drawing.Point(6, 25);
			this.numUnityNote.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.numUnityNote.Name = "numUnityNote";
			this.numUnityNote.Size = new System.Drawing.Size(120, 36);
			this.numUnityNote.TabIndex = 0;
			this.numUnityNote.ValueChanged += new System.EventHandler(this.numUnityNote_ValueChanged);
			// 
			// numFineTune
			// 
			this.numFineTune.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numFineTune.Location = new System.Drawing.Point(6, 25);
			this.numFineTune.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numFineTune.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
			this.numFineTune.Name = "numFineTune";
			this.numFineTune.Size = new System.Drawing.Size(120, 36);
			this.numFineTune.TabIndex = 2;
			// 
			// lblUnityNote
			// 
			this.lblUnityNote.AutoSize = true;
			this.lblUnityNote.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblUnityNote.Location = new System.Drawing.Point(132, 27);
			this.lblUnityNote.Name = "lblUnityNote";
			this.lblUnityNote.Size = new System.Drawing.Size(83, 29);
			this.lblUnityNote.TabIndex = 11;
			this.lblUnityNote.Text = "label1";
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.groupBox1.Controls.Add(this.numUnityNote);
			this.groupBox1.Controls.Add(this.lblUnityNote);
			this.groupBox1.Location = new System.Drawing.Point(158, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(224, 74);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "基準音";
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
			this.groupBox2.Controls.Add(this.numFineTune);
			this.groupBox2.Location = new System.Drawing.Point(388, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(162, 74);
			this.groupBox2.TabIndex = 15;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "ピッチ(cent)";
			// 
			// lblPitch
			// 
			this.lblPitch.AutoSize = true;
			this.lblPitch.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblPitch.Location = new System.Drawing.Point(6, 22);
			this.lblPitch.Name = "lblPitch";
			this.lblPitch.Size = new System.Drawing.Size(67, 24);
			this.lblPitch.TabIndex = 16;
			this.lblPitch.Text = "label1";
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLight;
			this.groupBox3.Controls.Add(this.lblPitch);
			this.groupBox3.Location = new System.Drawing.Point(556, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(162, 74);
			this.groupBox3.TabIndex = 17;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "音程検出";
			// 
			// btnUpdateTone
			// 
			this.btnUpdateTone.Location = new System.Drawing.Point(724, 26);
			this.btnUpdateTone.Name = "btnUpdateTone";
			this.btnUpdateTone.Size = new System.Drawing.Size(252, 46);
			this.btnUpdateTone.TabIndex = 18;
			this.btnUpdateTone.Text = "検出した音程を反映";
			this.btnUpdateTone.UseVisualStyleBackColor = true;
			this.btnUpdateTone.Click += new System.EventHandler(this.btnUpdateTone_Click);
			// 
			// btnLoopCreate
			// 
			this.btnLoopCreate.BackColor = System.Drawing.SystemColors.Control;
			this.btnLoopCreate.Location = new System.Drawing.Point(563, 0);
			this.btnLoopCreate.Name = "btnLoopCreate";
			this.btnLoopCreate.Size = new System.Drawing.Size(161, 42);
			this.btnLoopCreate.TabIndex = 11;
			this.btnLoopCreate.Text = "ループ作成";
			this.btnLoopCreate.UseVisualStyleBackColor = true;
			this.btnLoopCreate.Click += new System.EventHandler(this.btnLoopCreate_Click);
			// 
			// WaveInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1730, 942);
			this.Controls.Add(this.btnUpdateTone);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
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
			((System.ComponentModel.ISupportInitialize)(this.numUnityNote)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFineTune)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
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
		private System.Windows.Forms.NumericUpDown numUnityNote;
		private System.Windows.Forms.NumericUpDown numFineTune;
		private System.Windows.Forms.Label lblUnityNote;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label lblPitch;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnUpdateTone;
		private System.Windows.Forms.Button btnLoopCreate;
	}
}