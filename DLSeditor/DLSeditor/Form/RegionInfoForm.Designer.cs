namespace DLSeditor {
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
			this.glbKey = new System.Windows.Forms.GroupBox();
			this.glbVelocity = new System.Windows.Forms.GroupBox();
			this.lblKeyLow = new System.Windows.Forms.Label();
			this.lblKeyHigh = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numKeyLow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numKeyHigh)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numVelocityHigh)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numVelocityLow)).BeginInit();
			this.glbKey.SuspendLayout();
			this.glbVelocity.SuspendLayout();
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
			this.numKeyLow.Value = new decimal(new int[] {
            63,
            0,
            0,
            0});
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
			this.numKeyHigh.Value = new decimal(new int[] {
            63,
            0,
            0,
            0});
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
			this.numVelocityHigh.TabIndex = 3;
			this.numVelocityHigh.Value = new decimal(new int[] {
            127,
            0,
            0,
            0});
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
			this.numVelocityLow.TabIndex = 2;
			this.numVelocityLow.ValueChanged += new System.EventHandler(this.numVelocityLow_ValueChanged);
			// 
			// glbKey
			// 
			this.glbKey.BackColor = System.Drawing.SystemColors.ControlLight;
			this.glbKey.Controls.Add(this.lblKeyLow);
			this.glbKey.Controls.Add(this.lblKeyHigh);
			this.glbKey.Controls.Add(this.numKeyLow);
			this.glbKey.Controls.Add(this.numKeyHigh);
			this.glbKey.Location = new System.Drawing.Point(12, 12);
			this.glbKey.Name = "glbKey";
			this.glbKey.Size = new System.Drawing.Size(280, 97);
			this.glbKey.TabIndex = 4;
			this.glbKey.TabStop = false;
			this.glbKey.Text = "音程";
			// 
			// glbVelocity
			// 
			this.glbVelocity.BackColor = System.Drawing.SystemColors.ControlLight;
			this.glbVelocity.Controls.Add(this.numVelocityLow);
			this.glbVelocity.Controls.Add(this.numVelocityHigh);
			this.glbVelocity.Location = new System.Drawing.Point(298, 12);
			this.glbVelocity.Name = "glbVelocity";
			this.glbVelocity.Size = new System.Drawing.Size(280, 97);
			this.glbVelocity.TabIndex = 5;
			this.glbVelocity.TabStop = false;
			this.glbVelocity.Text = "強弱";
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
			// RegionInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(979, 646);
			this.Controls.Add(this.glbVelocity);
			this.Controls.Add(this.glbKey);
			this.Name = "RegionInfoForm";
			this.Text = "RegionInfoForm";
			((System.ComponentModel.ISupportInitialize)(this.numKeyLow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numKeyHigh)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numVelocityHigh)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numVelocityLow)).EndInit();
			this.glbKey.ResumeLayout(false);
			this.glbKey.PerformLayout();
			this.glbVelocity.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NumericUpDown numKeyLow;
		private System.Windows.Forms.NumericUpDown numKeyHigh;
		private System.Windows.Forms.NumericUpDown numVelocityHigh;
		private System.Windows.Forms.NumericUpDown numVelocityLow;
		private System.Windows.Forms.GroupBox glbKey;
		private System.Windows.Forms.GroupBox glbVelocity;
		private System.Windows.Forms.Label lblKeyLow;
		private System.Windows.Forms.Label lblKeyHigh;
	}
}