namespace DLSeditor
{
    partial class InstInfoForm
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
            if (disposing && (components != null))
            {
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
			this.tbcInfo = new System.Windows.Forms.TabControl();
			this.tbInfo = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.tbWave = new System.Windows.Forms.TabPage();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.lstWave = new System.Windows.Forms.ListBox();
			this.tbRegion = new System.Windows.Forms.TabPage();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tscLayer = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tstRegionInfo = new System.Windows.Forms.ToolStripTextBox();
			this.pnlRegion = new System.Windows.Forms.Panel();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.tsbImportWaves = new System.Windows.Forms.ToolStripButton();
			this.tsbDeleteWave = new System.Windows.Forms.ToolStripButton();
			this.tsbExportWaves = new System.Windows.Forms.ToolStripButton();
			this.tsbSelectRegion = new System.Windows.Forms.ToolStripButton();
			this.tsbAddRegion = new System.Windows.Forms.ToolStripButton();
			this.tsbDeleteRegion = new System.Windows.Forms.ToolStripButton();
			this.imgRegion = new System.Windows.Forms.PictureBox();
			this.tbcInfo.SuspendLayout();
			this.tbInfo.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tbWave.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.tbRegion.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.pnlRegion.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgRegion)).BeginInit();
			this.SuspendLayout();
			// 
			// tbcInfo
			// 
			this.tbcInfo.Controls.Add(this.tbInfo);
			this.tbcInfo.Controls.Add(this.tbWave);
			this.tbcInfo.Controls.Add(this.tbRegion);
			this.tbcInfo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.tbcInfo.Location = new System.Drawing.Point(12, 12);
			this.tbcInfo.Name = "tbcInfo";
			this.tbcInfo.SelectedIndex = 0;
			this.tbcInfo.Size = new System.Drawing.Size(684, 476);
			this.tbcInfo.TabIndex = 1;
			// 
			// tbInfo
			// 
			this.tbInfo.Controls.Add(this.panel1);
			this.tbInfo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.tbInfo.Location = new System.Drawing.Point(4, 25);
			this.tbInfo.Name = "tbInfo";
			this.tbInfo.Padding = new System.Windows.Forms.Padding(3);
			this.tbInfo.Size = new System.Drawing.Size(676, 447);
			this.tbInfo.TabIndex = 0;
			this.tbInfo.Text = "詳細";
			this.tbInfo.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(6, 6);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(409, 298);
			this.panel1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// tbWave
			// 
			this.tbWave.Controls.Add(this.toolStrip2);
			this.tbWave.Controls.Add(this.lstWave);
			this.tbWave.Location = new System.Drawing.Point(4, 25);
			this.tbWave.Name = "tbWave";
			this.tbWave.Size = new System.Drawing.Size(676, 447);
			this.tbWave.TabIndex = 2;
			this.tbWave.Text = "波形一覧";
			this.tbWave.UseVisualStyleBackColor = true;
			this.tbWave.SizeChanged += new System.EventHandler(this.tbWave_SizeChanged);
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbImportWaves,
            this.tsbDeleteWave,
            this.tsbExportWaves});
			this.toolStrip2.Location = new System.Drawing.Point(0, 0);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(676, 25);
			this.toolStrip2.TabIndex = 2;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// lstWave
			// 
			this.lstWave.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstWave.FormattingEnabled = true;
			this.lstWave.ItemHeight = 15;
			this.lstWave.Location = new System.Drawing.Point(3, 28);
			this.lstWave.Name = "lstWave";
			this.lstWave.Size = new System.Drawing.Size(493, 379);
			this.lstWave.TabIndex = 0;
			// 
			// tbRegion
			// 
			this.tbRegion.Controls.Add(this.toolStrip1);
			this.tbRegion.Controls.Add(this.pnlRegion);
			this.tbRegion.Location = new System.Drawing.Point(4, 25);
			this.tbRegion.Name = "tbRegion";
			this.tbRegion.Padding = new System.Windows.Forms.Padding(3);
			this.tbRegion.Size = new System.Drawing.Size(676, 447);
			this.tbRegion.TabIndex = 1;
			this.tbRegion.Text = "波形範囲";
			this.tbRegion.UseVisualStyleBackColor = true;
			this.tbRegion.SizeChanged += new System.EventHandler(this.tbRegion_SizeChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscLayer,
            this.toolStripSeparator1,
            this.tsbSelectRegion,
            this.tsbAddRegion,
            this.tsbDeleteRegion,
            this.toolStripSeparator2,
            this.tstRegionInfo});
			this.toolStrip1.Location = new System.Drawing.Point(3, 3);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(670, 28);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tscLayer
			// 
			this.tscLayer.Name = "tscLayer";
			this.tscLayer.Size = new System.Drawing.Size(121, 28);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
			// 
			// tstRegionInfo
			// 
			this.tstRegionInfo.Enabled = false;
			this.tstRegionInfo.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.tstRegionInfo.Name = "tstRegionInfo";
			this.tstRegionInfo.Size = new System.Drawing.Size(250, 28);
			// 
			// pnlRegion
			// 
			this.pnlRegion.AutoScroll = true;
			this.pnlRegion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlRegion.Controls.Add(this.imgRegion);
			this.pnlRegion.Location = new System.Drawing.Point(3, 31);
			this.pnlRegion.Name = "pnlRegion";
			this.pnlRegion.Size = new System.Drawing.Size(666, 641);
			this.pnlRegion.TabIndex = 0;
			// 
			// tsbImportWaves
			// 
			this.tsbImportWaves.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbImportWaves.Image = global::DLSeditor.Properties.Resources.plus;
			this.tsbImportWaves.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbImportWaves.Name = "tsbImportWaves";
			this.tsbImportWaves.Size = new System.Drawing.Size(23, 22);
			this.tsbImportWaves.Text = "追加";
			this.tsbImportWaves.Click += new System.EventHandler(this.tsbImportWaves_Click);
			// 
			// tsbDeleteWave
			// 
			this.tsbDeleteWave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDeleteWave.Image = global::DLSeditor.Properties.Resources.minus;
			this.tsbDeleteWave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbDeleteWave.Name = "tsbDeleteWave";
			this.tsbDeleteWave.Size = new System.Drawing.Size(23, 22);
			this.tsbDeleteWave.Text = "削除";
			this.tsbDeleteWave.Click += new System.EventHandler(this.tsbDeleteWave_Click);
			// 
			// tsbExportWaves
			// 
			this.tsbExportWaves.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbExportWaves.Image = global::DLSeditor.Properties.Resources.waveout;
			this.tsbExportWaves.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbExportWaves.Name = "tsbExportWaves";
			this.tsbExportWaves.Size = new System.Drawing.Size(23, 22);
			this.tsbExportWaves.Text = "ファイル出力";
			this.tsbExportWaves.Click += new System.EventHandler(this.tsbExportWaves_Click);
			// 
			// tsbSelectRegion
			// 
			this.tsbSelectRegion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbSelectRegion.Image = global::DLSeditor.Properties.Resources.select;
			this.tsbSelectRegion.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbSelectRegion.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbSelectRegion.Name = "tsbSelectRegion";
			this.tsbSelectRegion.Size = new System.Drawing.Size(23, 25);
			this.tsbSelectRegion.Text = "選択";
			this.tsbSelectRegion.Click += new System.EventHandler(this.tsbSelectRegion_Click);
			// 
			// tsbAddRegion
			// 
			this.tsbAddRegion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbAddRegion.Image = global::DLSeditor.Properties.Resources.plus;
			this.tsbAddRegion.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbAddRegion.Name = "tsbAddRegion";
			this.tsbAddRegion.Size = new System.Drawing.Size(23, 25);
			this.tsbAddRegion.Text = "追加";
			this.tsbAddRegion.Click += new System.EventHandler(this.tsbAddRegion_Click);
			// 
			// tsbDeleteRegion
			// 
			this.tsbDeleteRegion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDeleteRegion.Image = global::DLSeditor.Properties.Resources.minus;
			this.tsbDeleteRegion.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbDeleteRegion.Name = "tsbDeleteRegion";
			this.tsbDeleteRegion.Size = new System.Drawing.Size(23, 25);
			this.tsbDeleteRegion.Text = "削除";
			this.tsbDeleteRegion.Click += new System.EventHandler(this.tsbDeleteRegion_Click);
			// 
			// imgRegion
			// 
			this.imgRegion.Image = global::DLSeditor.Properties.Resources.region;
			this.imgRegion.Location = new System.Drawing.Point(0, 0);
			this.imgRegion.Name = "imgRegion";
			this.imgRegion.Size = new System.Drawing.Size(642, 514);
			this.imgRegion.TabIndex = 0;
			this.imgRegion.TabStop = false;
			this.imgRegion.DoubleClick += new System.EventHandler(this.imgRegion_DoubleClick);
			this.imgRegion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgRegion_MouseDown);
			this.imgRegion.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imgRegion_MouseMove);
			this.imgRegion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imgRegion_MouseUp);
			// 
			// InstInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(705, 500);
			this.Controls.Add(this.tbcInfo);
			this.DoubleBuffered = true;
			this.Name = "InstInfoForm";
			this.Text = "音色情報";
			this.Load += new System.EventHandler(this.InstInfoForm_Load);
			this.SizeChanged += new System.EventHandler(this.InstInfoForm_SizeChanged);
			this.tbcInfo.ResumeLayout(false);
			this.tbInfo.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tbWave.ResumeLayout(false);
			this.tbWave.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.tbRegion.ResumeLayout(false);
			this.tbRegion.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.pnlRegion.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.imgRegion)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imgRegion;
        private System.Windows.Forms.TabControl tbcInfo;
        private System.Windows.Forms.TabPage tbInfo;
        private System.Windows.Forms.TabPage tbRegion;
        private System.Windows.Forms.Panel pnlRegion;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox tscLayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton tsbSelectRegion;
        private System.Windows.Forms.ToolStripButton tsbAddRegion;
        private System.Windows.Forms.ToolStripButton tsbDeleteRegion;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripTextBox tstRegionInfo;
		private System.Windows.Forms.TabPage tbWave;
		private System.Windows.Forms.ListBox lstWave;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton tsbDeleteWave;
		private System.Windows.Forms.ToolStripButton tsbExportWaves;
		private System.Windows.Forms.ToolStripButton tsbImportWaves;
	}
}