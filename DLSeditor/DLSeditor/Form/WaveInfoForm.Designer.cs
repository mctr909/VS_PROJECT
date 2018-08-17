﻿namespace DLSeditor
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.btnPlay = new System.Windows.Forms.Button();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.picSpectrum = new System.Windows.Forms.PictureBox();
			this.hsbTime = new System.Windows.Forms.HScrollBar();
			this.picWave = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.numScale = new System.Windows.Forms.NumericUpDown();
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picSpectrum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picWave)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
			this.SuspendLayout();
			// 
			// btnPlay
			// 
			this.btnPlay.Location = new System.Drawing.Point(12, 12);
			this.btnPlay.Name = "btnPlay";
			this.btnPlay.Size = new System.Drawing.Size(75, 23);
			this.btnPlay.TabIndex = 0;
			this.btnPlay.Text = "再生";
			this.btnPlay.UseVisualStyleBackColor = true;
			this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
			// 
			// chart1
			// 
			chartArea6.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea6);
			legend6.Name = "Legend1";
			this.chart1.Legends.Add(legend6);
			this.chart1.Location = new System.Drawing.Point(93, 12);
			this.chart1.Name = "chart1";
			series6.ChartArea = "ChartArea1";
			series6.Legend = "Legend1";
			series6.Name = "Series1";
			this.chart1.Series.Add(series6);
			this.chart1.Size = new System.Drawing.Size(128, 64);
			this.chart1.TabIndex = 1;
			this.chart1.Text = "chart1";
			// 
			// picSpectrum
			// 
			this.picSpectrum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picSpectrum.Location = new System.Drawing.Point(12, 82);
			this.picSpectrum.Name = "picSpectrum";
			this.picSpectrum.Size = new System.Drawing.Size(768, 132);
			this.picSpectrum.TabIndex = 2;
			this.picSpectrum.TabStop = false;
			// 
			// hsbTime
			// 
			this.hsbTime.Location = new System.Drawing.Point(12, 217);
			this.hsbTime.Name = "hsbTime";
			this.hsbTime.Size = new System.Drawing.Size(768, 22);
			this.hsbTime.TabIndex = 3;
			// 
			// picWave
			// 
			this.picWave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picWave.Location = new System.Drawing.Point(12, 242);
			this.picWave.Name = "picWave";
			this.picWave.Size = new System.Drawing.Size(768, 256);
			this.picWave.TabIndex = 4;
			this.picWave.TabStop = false;
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// numScale
			// 
			this.numScale.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numScale.Location = new System.Drawing.Point(12, 41);
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
			this.numScale.Size = new System.Drawing.Size(75, 22);
			this.numScale.TabIndex = 5;
			this.numScale.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
			this.numScale.ValueChanged += new System.EventHandler(this.numScale_ValueChanged);
			// 
			// timer2
			// 
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// WaveInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(796, 510);
			this.Controls.Add(this.numScale);
			this.Controls.Add(this.picWave);
			this.Controls.Add(this.hsbTime);
			this.Controls.Add(this.picSpectrum);
			this.Controls.Add(this.chart1);
			this.Controls.Add(this.btnPlay);
			this.Name = "WaveInfoForm";
			this.Text = "WaveInfoForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WaveInfoForm_FormClosing);
			this.Load += new System.EventHandler(this.WaveInfoForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picSpectrum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picWave)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnPlay;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
		private System.Windows.Forms.PictureBox picSpectrum;
		private System.Windows.Forms.HScrollBar hsbTime;
		private System.Windows.Forms.PictureBox picWave;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.NumericUpDown numScale;
		private System.Windows.Forms.Timer timer2;
	}
}