using System.Windows.Forms;

namespace DLSeditor {
	partial class InstInfoForm : Form {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstInfoForm));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.lstRegion = new System.Windows.Forms.ListBox();
			this.pnlRegion = new System.Windows.Forms.Panel();
			this.pictRange = new System.Windows.Forms.PictureBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbAddRange = new System.Windows.Forms.ToolStripButton();
			this.tsbDeleteRange = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbRangeList = new System.Windows.Forms.ToolStripButton();
			this.tsbRangeKey = new System.Windows.Forms.ToolStripButton();
			this.tabControl.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.pnlRegion.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictRange)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.tabPage2);
			this.tabControl.Location = new System.Drawing.Point(12, 13);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(1477, 852);
			this.tabControl.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(8, 39);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1461, 805);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "音色設定";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.lstRegion);
			this.tabPage2.Controls.Add(this.pnlRegion);
			this.tabPage2.Controls.Add(this.toolStrip1);
			this.tabPage2.Location = new System.Drawing.Point(8, 39);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1461, 805);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "レイヤー設定";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// lstRegion
			// 
			this.lstRegion.FormattingEnabled = true;
			this.lstRegion.ItemHeight = 24;
			this.lstRegion.Location = new System.Drawing.Point(21, 47);
			this.lstRegion.Name = "lstRegion";
			this.lstRegion.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstRegion.Size = new System.Drawing.Size(120, 76);
			this.lstRegion.TabIndex = 5;
			this.lstRegion.DoubleClick += new System.EventHandler(this.lstRegion_DoubleClick);
			// 
			// pnlRegion
			// 
			this.pnlRegion.AutoScroll = true;
			this.pnlRegion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlRegion.Controls.Add(this.pictRange);
			this.pnlRegion.Location = new System.Drawing.Point(22, 153);
			this.pnlRegion.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.pnlRegion.Name = "pnlRegion";
			this.pnlRegion.Size = new System.Drawing.Size(1439, 731);
			this.pnlRegion.TabIndex = 4;
			this.pnlRegion.Visible = false;
			// 
			// pictRange
			// 
			this.pictRange.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictRange.BackgroundImage")));
			this.pictRange.InitialImage = null;
			this.pictRange.Location = new System.Drawing.Point(0, 0);
			this.pictRange.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.pictRange.Name = "pictRange";
			this.pictRange.Size = new System.Drawing.Size(1664, 768);
			this.pictRange.TabIndex = 0;
			this.pictRange.TabStop = false;
			this.pictRange.DoubleClick += new System.EventHandler(this.pictRange_DoubleClick);
			this.pictRange.MouseHover += new System.EventHandler(this.pictRange_MouseHover);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddRange,
            this.tsbDeleteRange,
            this.toolStripSeparator1,
            this.tsbRangeList,
            this.tsbRangeKey});
			this.toolStrip1.Location = new System.Drawing.Point(3, 3);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStrip1.Size = new System.Drawing.Size(111, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsbAddRange
			// 
			this.tsbAddRange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbAddRange.Image = global::DLSeditor.Properties.Resources.plus;
			this.tsbAddRange.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbAddRange.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbAddRange.Name = "tsbAddRange";
			this.tsbAddRange.Size = new System.Drawing.Size(23, 22);
			this.tsbAddRange.Text = "toolStripButton1";
			this.tsbAddRange.Click += new System.EventHandler(this.tsbAddRange_Click);
			// 
			// tsbDeleteRange
			// 
			this.tsbDeleteRange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDeleteRange.Image = global::DLSeditor.Properties.Resources.minus;
			this.tsbDeleteRange.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbDeleteRange.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbDeleteRange.Name = "tsbDeleteRange";
			this.tsbDeleteRange.Size = new System.Drawing.Size(23, 22);
			this.tsbDeleteRange.Text = "toolStripButton1";
			this.tsbDeleteRange.Click += new System.EventHandler(this.tsbDeleteRange_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbRangeList
			// 
			this.tsbRangeList.Checked = true;
			this.tsbRangeList.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsbRangeList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbRangeList.Image = global::DLSeditor.Properties.Resources.list;
			this.tsbRangeList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbRangeList.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbRangeList.Name = "tsbRangeList";
			this.tsbRangeList.Size = new System.Drawing.Size(23, 22);
			this.tsbRangeList.Text = "リスト表示";
			this.tsbRangeList.Click += new System.EventHandler(this.tsbRangeList_Click);
			// 
			// tsbRangeKey
			// 
			this.tsbRangeKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbRangeKey.Image = global::DLSeditor.Properties.Resources.key;
			this.tsbRangeKey.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbRangeKey.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbRangeKey.Name = "tsbRangeKey";
			this.tsbRangeKey.Size = new System.Drawing.Size(23, 22);
			this.tsbRangeKey.Text = "グラフィック表示";
			this.tsbRangeKey.Click += new System.EventHandler(this.tsbRangeKey_Click);
			// 
			// InstInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1998, 1029);
			this.Controls.Add(this.tabControl);
			this.Name = "InstInfoForm";
			this.Text = "InstInfoForm";
			this.SizeChanged += new System.EventHandler(this.InstInfoForm_SizeChanged);
			this.tabControl.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.pnlRegion.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictRange)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbAddRange;
		private System.Windows.Forms.Panel pnlRegion;
		private System.Windows.Forms.PictureBox pictRange;
		private ToolStripButton tsbDeleteRange;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton tsbRangeList;
		private ToolStripButton tsbRangeKey;
		private ListBox lstRegion;
	}
}