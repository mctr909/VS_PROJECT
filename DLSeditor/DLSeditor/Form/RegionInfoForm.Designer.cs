﻿namespace DLSeditor {
	partial class RegionInfoForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
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
		private void InitializeComponent() {
			this.numKeyLow = new System.Windows.Forms.NumericUpDown();
			this.numKeyHigh = new System.Windows.Forms.NumericUpDown();
			this.numVelocityHigh = new System.Windows.Forms.NumericUpDown();
			this.numVelocityLow = new System.Windows.Forms.NumericUpDown();
			this.grbKey = new System.Windows.Forms.GroupBox();
			this.lblKeyLow = new System.Windows.Forms.Label();
			this.lblKeyHigh = new System.Windows.Forms.Label();
			this.grbVelocity = new System.Windows.Forms.GroupBox();
			this.txtWave = new System.Windows.Forms.TextBox();
			this.grbWave = new System.Windows.Forms.GroupBox();
			this.btnSelectWave = new System.Windows.Forms.Button();
			this.btnEditWave = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.grbFineTune = new System.Windows.Forms.GroupBox();
			this.numFineTune = new System.Windows.Forms.NumericUpDown();
			this.grbUnityNote = new System.Windows.Forms.GroupBox();
			this.numUnityNote = new System.Windows.Forms.NumericUpDown();
			this.lblUnityNote = new System.Windows.Forms.Label();
			this.grbVolume = new System.Windows.Forms.GroupBox();
			this.numVolume = new System.Windows.Forms.NumericUpDown();
			this.chkLoop = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numKeyLow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numKeyHigh)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numVelocityHigh)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numVelocityLow)).BeginInit();
			this.grbKey.SuspendLayout();
			this.grbVelocity.SuspendLayout();
			this.grbWave.SuspendLayout();
			this.grbFineTune.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numFineTune)).BeginInit();
			this.grbUnityNote.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnityNote)).BeginInit();
			this.grbVolume.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numVolume)).BeginInit();
			this.SuspendLayout();
			// 
			// numKeyLow
			// 
			this.numKeyLow.Location = new System.Drawing.Point(6, 30);
			this.numKeyLow.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.numKeyLow.Name = "numKeyLow";
			this.numKeyLow.Size = new System.Drawing.Size(120, 31);
			this.numKeyLow.TabIndex = 0;
			this.numKeyLow.ValueChanged += new System.EventHandler(this.numKeyLow_ValueChanged);
			// 
			// numKeyHigh
			// 
			this.numKeyHigh.Location = new System.Drawing.Point(132, 30);
			this.numKeyHigh.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.numKeyHigh.Name = "numKeyHigh";
			this.numKeyHigh.Size = new System.Drawing.Size(120, 31);
			this.numKeyHigh.TabIndex = 1;
			this.numKeyHigh.ValueChanged += new System.EventHandler(this.numKeyHigh_ValueChanged);
			// 
			// numVelocityHigh
			// 
			this.numVelocityHigh.Location = new System.Drawing.Point(132, 30);
			this.numVelocityHigh.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.numVelocityHigh.Name = "numVelocityHigh";
			this.numVelocityHigh.Size = new System.Drawing.Size(120, 31);
			this.numVelocityHigh.TabIndex = 1;
			this.numVelocityHigh.ValueChanged += new System.EventHandler(this.numVelocityHigh_ValueChanged);
			// 
			// numVelocityLow
			// 
			this.numVelocityLow.Location = new System.Drawing.Point(6, 30);
			this.numVelocityLow.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.numVelocityLow.Name = "numVelocityLow";
			this.numVelocityLow.Size = new System.Drawing.Size(120, 31);
			this.numVelocityLow.TabIndex = 0;
			this.numVelocityLow.ValueChanged += new System.EventHandler(this.numVelocityLow_ValueChanged);
			// 
			// grbKey
			// 
			this.grbKey.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbKey.Controls.Add(this.lblKeyLow);
			this.grbKey.Controls.Add(this.lblKeyHigh);
			this.grbKey.Controls.Add(this.numKeyLow);
			this.grbKey.Controls.Add(this.numKeyHigh);
			this.grbKey.Location = new System.Drawing.Point(12, 12);
			this.grbKey.Name = "grbKey";
			this.grbKey.Size = new System.Drawing.Size(280, 97);
			this.grbKey.TabIndex = 0;
			this.grbKey.TabStop = false;
			this.grbKey.Text = "音程";
			// 
			// lblKeyLow
			// 
			this.lblKeyLow.AutoSize = true;
			this.lblKeyLow.Location = new System.Drawing.Point(6, 64);
			this.lblKeyLow.Name = "lblKeyLow";
			this.lblKeyLow.Size = new System.Drawing.Size(80, 24);
			this.lblKeyLow.TabIndex = 6;
			this.lblKeyLow.Text = "Bb-2 5";
			// 
			// lblKeyHigh
			// 
			this.lblKeyHigh.AutoSize = true;
			this.lblKeyHigh.Location = new System.Drawing.Point(128, 64);
			this.lblKeyHigh.Name = "lblKeyHigh";
			this.lblKeyHigh.Size = new System.Drawing.Size(80, 24);
			this.lblKeyHigh.TabIndex = 7;
			this.lblKeyHigh.Text = "Bb-2 5";
			// 
			// grbVelocity
			// 
			this.grbVelocity.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbVelocity.Controls.Add(this.numVelocityLow);
			this.grbVelocity.Controls.Add(this.numVelocityHigh);
			this.grbVelocity.Location = new System.Drawing.Point(298, 12);
			this.grbVelocity.Name = "grbVelocity";
			this.grbVelocity.Size = new System.Drawing.Size(280, 97);
			this.grbVelocity.TabIndex = 1;
			this.grbVelocity.TabStop = false;
			this.grbVelocity.Text = "強弱";
			// 
			// txtWave
			// 
			this.txtWave.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.txtWave.Location = new System.Drawing.Point(6, 30);
			this.txtWave.Name = "txtWave";
			this.txtWave.ReadOnly = true;
			this.txtWave.Size = new System.Drawing.Size(406, 31);
			this.txtWave.TabIndex = 0;
			// 
			// grbWave
			// 
			this.grbWave.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbWave.Controls.Add(this.btnSelectWave);
			this.grbWave.Controls.Add(this.btnEditWave);
			this.grbWave.Controls.Add(this.txtWave);
			this.grbWave.Location = new System.Drawing.Point(12, 115);
			this.grbWave.Name = "grbWave";
			this.grbWave.Size = new System.Drawing.Size(566, 77);
			this.grbWave.TabIndex = 2;
			this.grbWave.TabStop = false;
			this.grbWave.Text = "波形";
			// 
			// btnSelectWave
			// 
			this.btnSelectWave.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnSelectWave.Location = new System.Drawing.Point(418, 26);
			this.btnSelectWave.Name = "btnSelectWave";
			this.btnSelectWave.Size = new System.Drawing.Size(90, 39);
			this.btnSelectWave.TabIndex = 1;
			this.btnSelectWave.Text = "選択";
			this.btnSelectWave.UseVisualStyleBackColor = true;
			this.btnSelectWave.Click += new System.EventHandler(this.btnSelectWave_Click);
			// 
			// btnEditWave
			// 
			this.btnEditWave.Location = new System.Drawing.Point(476, 26);
			this.btnEditWave.Name = "btnEditWave";
			this.btnEditWave.Size = new System.Drawing.Size(90, 39);
			this.btnEditWave.TabIndex = 2;
			this.btnEditWave.Text = "編集";
			this.btnEditWave.UseVisualStyleBackColor = true;
			this.btnEditWave.Click += new System.EventHandler(this.btnEditWave_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnAdd.Location = new System.Drawing.Point(459, 278);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(119, 63);
			this.btnAdd.TabIndex = 6;
			this.btnAdd.Text = "追加";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// grbFineTune
			// 
			this.grbFineTune.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbFineTune.Controls.Add(this.numFineTune);
			this.grbFineTune.Location = new System.Drawing.Point(242, 198);
			this.grbFineTune.Name = "grbFineTune";
			this.grbFineTune.Size = new System.Drawing.Size(162, 74);
			this.grbFineTune.TabIndex = 4;
			this.grbFineTune.TabStop = false;
			this.grbFineTune.Text = "ピッチ(cent)";
			// 
			// numFineTune
			// 
			this.numFineTune.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numFineTune.Location = new System.Drawing.Point(6, 25);
			this.numFineTune.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
			this.numFineTune.Minimum = new decimal(new int[] {
            1200,
            0,
            0,
            -2147483648});
			this.numFineTune.Name = "numFineTune";
			this.numFineTune.Size = new System.Drawing.Size(120, 36);
			this.numFineTune.TabIndex = 0;
			// 
			// grbUnityNote
			// 
			this.grbUnityNote.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbUnityNote.Controls.Add(this.numUnityNote);
			this.grbUnityNote.Controls.Add(this.lblUnityNote);
			this.grbUnityNote.Location = new System.Drawing.Point(12, 198);
			this.grbUnityNote.Name = "grbUnityNote";
			this.grbUnityNote.Size = new System.Drawing.Size(224, 74);
			this.grbUnityNote.TabIndex = 3;
			this.grbUnityNote.TabStop = false;
			this.grbUnityNote.Text = "基準音";
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
			// grbVolume
			// 
			this.grbVolume.BackColor = System.Drawing.SystemColors.ControlLight;
			this.grbVolume.Controls.Add(this.numVolume);
			this.grbVolume.Location = new System.Drawing.Point(416, 198);
			this.grbVolume.Name = "grbVolume";
			this.grbVolume.Size = new System.Drawing.Size(162, 74);
			this.grbVolume.TabIndex = 5;
			this.grbVolume.TabStop = false;
			this.grbVolume.Text = "音量(%)";
			// 
			// numVolume
			// 
			this.numVolume.DecimalPlaces = 1;
			this.numVolume.Font = new System.Drawing.Font("MS UI Gothic", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numVolume.Location = new System.Drawing.Point(6, 25);
			this.numVolume.Name = "numVolume";
			this.numVolume.Size = new System.Drawing.Size(120, 36);
			this.numVolume.TabIndex = 0;
			this.numVolume.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// chkLoop
			// 
			this.chkLoop.AutoSize = true;
			this.chkLoop.Location = new System.Drawing.Point(12, 298);
			this.chkLoop.Name = "chkLoop";
			this.chkLoop.Size = new System.Drawing.Size(148, 28);
			this.chkLoop.TabIndex = 7;
			this.chkLoop.Text = "ループ有効";
			this.chkLoop.UseVisualStyleBackColor = true;
			// 
			// RegionInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(586, 351);
			this.Controls.Add(this.chkLoop);
			this.Controls.Add(this.grbVolume);
			this.Controls.Add(this.grbFineTune);
			this.Controls.Add(this.grbUnityNote);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.grbWave);
			this.Controls.Add(this.grbVelocity);
			this.Controls.Add(this.grbKey);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "RegionInfoForm";
			this.Text = "RegionInfoForm";
			((System.ComponentModel.ISupportInitialize)(this.numKeyLow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numKeyHigh)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numVelocityHigh)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numVelocityLow)).EndInit();
			this.grbKey.ResumeLayout(false);
			this.grbKey.PerformLayout();
			this.grbVelocity.ResumeLayout(false);
			this.grbWave.ResumeLayout(false);
			this.grbWave.PerformLayout();
			this.grbFineTune.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numFineTune)).EndInit();
			this.grbUnityNote.ResumeLayout(false);
			this.grbUnityNote.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numUnityNote)).EndInit();
			this.grbVolume.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numVolume)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown numKeyLow;
		private System.Windows.Forms.NumericUpDown numKeyHigh;
		private System.Windows.Forms.NumericUpDown numVelocityHigh;
		private System.Windows.Forms.NumericUpDown numVelocityLow;
		private System.Windows.Forms.GroupBox grbKey;
		private System.Windows.Forms.GroupBox grbVelocity;
		private System.Windows.Forms.Label lblKeyLow;
		private System.Windows.Forms.Label lblKeyHigh;
		private System.Windows.Forms.TextBox txtWave;
		private System.Windows.Forms.GroupBox grbWave;
		private System.Windows.Forms.Button btnSelectWave;
		private System.Windows.Forms.Button btnEditWave;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.GroupBox grbFineTune;
		private System.Windows.Forms.NumericUpDown numFineTune;
		private System.Windows.Forms.GroupBox grbUnityNote;
		private System.Windows.Forms.NumericUpDown numUnityNote;
		private System.Windows.Forms.Label lblUnityNote;
		private System.Windows.Forms.GroupBox grbVolume;
		private System.Windows.Forms.NumericUpDown numVolume;
		private System.Windows.Forms.CheckBox chkLoop;
	}
}