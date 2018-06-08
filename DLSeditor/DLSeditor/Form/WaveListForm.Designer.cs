namespace DLSeditor
{
	partial class WaveListForm
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
			this.lstWave = new System.Windows.Forms.ListBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbAddWave = new System.Windows.Forms.ToolStripButton();
			this.tsbDeleteWave = new System.Windows.Forms.ToolStripButton();
			this.tsbOutputWave = new System.Windows.Forms.ToolStripButton();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.編集EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.追加AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.削除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.ファイル出力ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.詳細表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.toolStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstWave
			// 
			this.lstWave.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstWave.FormattingEnabled = true;
			this.lstWave.ItemHeight = 12;
			this.lstWave.Location = new System.Drawing.Point(12, 56);
			this.lstWave.Name = "lstWave";
			this.lstWave.ScrollAlwaysVisible = true;
			this.lstWave.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstWave.Size = new System.Drawing.Size(259, 232);
			this.lstWave.TabIndex = 0;
			this.lstWave.DoubleClick += new System.EventHandler(this.lstWave_DoubleClick);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddWave,
            this.tsbDeleteWave,
            this.tsbOutputWave});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(279, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsbAddWave
			// 
			this.tsbAddWave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbAddWave.Image = global::DLSeditor.Properties.Resources.plus;
			this.tsbAddWave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbAddWave.Name = "tsbAddWave";
			this.tsbAddWave.Size = new System.Drawing.Size(23, 22);
			this.tsbAddWave.Text = "波形追加";
			this.tsbAddWave.Click += new System.EventHandler(this.tsbAddWave_Click);
			// 
			// tsbDeleteWave
			// 
			this.tsbDeleteWave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDeleteWave.Image = global::DLSeditor.Properties.Resources.minus;
			this.tsbDeleteWave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbDeleteWave.Name = "tsbDeleteWave";
			this.tsbDeleteWave.Size = new System.Drawing.Size(23, 22);
			this.tsbDeleteWave.Text = "波形削除";
			this.tsbDeleteWave.Click += new System.EventHandler(this.tsbDeleteWave_Click);
			// 
			// tsbOutputWave
			// 
			this.tsbOutputWave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbOutputWave.Image = global::DLSeditor.Properties.Resources.waveout;
			this.tsbOutputWave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbOutputWave.Name = "tsbOutputWave";
			this.tsbOutputWave.Size = new System.Drawing.Size(23, 22);
			this.tsbOutputWave.Text = "波形ファイル出力";
			this.tsbOutputWave.Click += new System.EventHandler(this.tsbOutputWave_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.編集EToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(279, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// 編集EToolStripMenuItem
			// 
			this.編集EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.追加AToolStripMenuItem,
            this.削除DToolStripMenuItem,
            this.toolStripSeparator1,
            this.ファイル出力ToolStripMenuItem,
            this.toolStripSeparator2,
            this.詳細表示ToolStripMenuItem});
			this.編集EToolStripMenuItem.Name = "編集EToolStripMenuItem";
			this.編集EToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.編集EToolStripMenuItem.Text = "編集(E)";
			// 
			// 追加AToolStripMenuItem
			// 
			this.追加AToolStripMenuItem.Name = "追加AToolStripMenuItem";
			this.追加AToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
			this.追加AToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.追加AToolStripMenuItem.Text = "追加(A)";
			this.追加AToolStripMenuItem.Click += new System.EventHandler(this.追加AToolStripMenuItem_Click);
			// 
			// 削除DToolStripMenuItem
			// 
			this.削除DToolStripMenuItem.Name = "削除DToolStripMenuItem";
			this.削除DToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.削除DToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.削除DToolStripMenuItem.Text = "削除(D)";
			this.削除DToolStripMenuItem.Click += new System.EventHandler(this.削除DToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
			// 
			// ファイル出力ToolStripMenuItem
			// 
			this.ファイル出力ToolStripMenuItem.Name = "ファイル出力ToolStripMenuItem";
			this.ファイル出力ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.ファイル出力ToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.ファイル出力ToolStripMenuItem.Text = "ファイル出力";
			this.ファイル出力ToolStripMenuItem.Click += new System.EventHandler(this.ファイル出力ToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(175, 6);
			// 
			// 詳細表示ToolStripMenuItem
			// 
			this.詳細表示ToolStripMenuItem.Name = "詳細表示ToolStripMenuItem";
			this.詳細表示ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)));
			this.詳細表示ToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.詳細表示ToolStripMenuItem.Text = "詳細表示";
			this.詳細表示ToolStripMenuItem.Click += new System.EventHandler(this.詳細表示ToolStripMenuItem_Click);
			// 
			// WaveListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(279, 301);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.lstWave);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "WaveListForm";
			this.Text = "PCM一覧";
			this.Load += new System.EventHandler(this.WaveListForm_Load);
			this.SizeChanged += new System.EventHandler(this.WaveListForm_SizeChanged);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstWave;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbAddWave;
		private System.Windows.Forms.ToolStripButton tsbDeleteWave;
		private System.Windows.Forms.ToolStripButton tsbOutputWave;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem 編集EToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 追加AToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 削除DToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 詳細表示ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ファイル出力ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
	}
}