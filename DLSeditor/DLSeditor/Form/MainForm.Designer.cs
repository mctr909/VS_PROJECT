﻿namespace DLSeditor
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.新規作成NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.開くOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.上書き保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.名前を付けて保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.編集EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.追加AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.削除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.コピーCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.貼り付けPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tbpPcmList = new System.Windows.Forms.TabPage();
			this.lstWave = new System.Windows.Forms.ListBox();
			this.toolStrip3 = new System.Windows.Forms.ToolStrip();
			this.tsbAddWave = new System.Windows.Forms.ToolStripButton();
			this.tsbDeleteWave = new System.Windows.Forms.ToolStripButton();
			this.tsbOutputWave = new System.Windows.Forms.ToolStripButton();
			this.tbpInstList = new System.Windows.Forms.TabPage();
			this.lstInst = new System.Windows.Forms.ListBox();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.tsbAddInst = new System.Windows.Forms.ToolStripButton();
			this.tsbDeleteInst = new System.Windows.Forms.ToolStripButton();
			this.tsbCopyInst = new System.Windows.Forms.ToolStripButton();
			this.tsbPasteInst = new System.Windows.Forms.ToolStripButton();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.menuStrip1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tbpPcmList.SuspendLayout();
			this.toolStrip3.SuspendLayout();
			this.tbpInstList.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.編集EToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(13, 4, 0, 4);
			this.menuStrip1.Size = new System.Drawing.Size(1686, 44);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// ファイルFToolStripMenuItem
			// 
			this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新規作成NToolStripMenuItem,
            this.toolStripSeparator1,
            this.開くOToolStripMenuItem,
            this.toolStripSeparator2,
            this.上書き保存ToolStripMenuItem,
            this.名前を付けて保存ToolStripMenuItem});
			this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
			this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(121, 36);
			this.ファイルFToolStripMenuItem.Text = "ファイル(F)";
			// 
			// 新規作成NToolStripMenuItem
			// 
			this.新規作成NToolStripMenuItem.Name = "新規作成NToolStripMenuItem";
			this.新規作成NToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.新規作成NToolStripMenuItem.Size = new System.Drawing.Size(336, 38);
			this.新規作成NToolStripMenuItem.Text = "新規作成(N)";
			this.新規作成NToolStripMenuItem.Click += new System.EventHandler(this.新規作成NToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(333, 6);
			// 
			// 開くOToolStripMenuItem
			// 
			this.開くOToolStripMenuItem.Name = "開くOToolStripMenuItem";
			this.開くOToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.開くOToolStripMenuItem.Size = new System.Drawing.Size(336, 38);
			this.開くOToolStripMenuItem.Text = "開く(O)";
			this.開くOToolStripMenuItem.Click += new System.EventHandler(this.開くOToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(333, 6);
			// 
			// 上書き保存ToolStripMenuItem
			// 
			this.上書き保存ToolStripMenuItem.Name = "上書き保存ToolStripMenuItem";
			this.上書き保存ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.上書き保存ToolStripMenuItem.Size = new System.Drawing.Size(336, 38);
			this.上書き保存ToolStripMenuItem.Text = "上書き保存(S)";
			this.上書き保存ToolStripMenuItem.Click += new System.EventHandler(this.上書き保存ToolStripMenuItem_Click);
			// 
			// 名前を付けて保存ToolStripMenuItem
			// 
			this.名前を付けて保存ToolStripMenuItem.Name = "名前を付けて保存ToolStripMenuItem";
			this.名前を付けて保存ToolStripMenuItem.Size = new System.Drawing.Size(336, 38);
			this.名前を付けて保存ToolStripMenuItem.Text = "名前を付けて保存(A)";
			this.名前を付けて保存ToolStripMenuItem.Click += new System.EventHandler(this.名前を付けて保存ToolStripMenuItem_Click);
			// 
			// 編集EToolStripMenuItem
			// 
			this.編集EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.追加AToolStripMenuItem,
            this.削除DToolStripMenuItem,
            this.コピーCToolStripMenuItem,
            this.貼り付けPToolStripMenuItem});
			this.編集EToolStripMenuItem.Name = "編集EToolStripMenuItem";
			this.編集EToolStripMenuItem.Size = new System.Drawing.Size(101, 36);
			this.編集EToolStripMenuItem.Text = "編集(E)";
			// 
			// 追加AToolStripMenuItem
			// 
			this.追加AToolStripMenuItem.Name = "追加AToolStripMenuItem";
			this.追加AToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
			this.追加AToolStripMenuItem.Size = new System.Drawing.Size(322, 38);
			this.追加AToolStripMenuItem.Text = "追加(A)";
			this.追加AToolStripMenuItem.Click += new System.EventHandler(this.追加AToolStripMenuItem_Click);
			// 
			// 削除DToolStripMenuItem
			// 
			this.削除DToolStripMenuItem.Name = "削除DToolStripMenuItem";
			this.削除DToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.削除DToolStripMenuItem.Size = new System.Drawing.Size(322, 38);
			this.削除DToolStripMenuItem.Text = "削除(D)";
			this.削除DToolStripMenuItem.Click += new System.EventHandler(this.削除DToolStripMenuItem_Click);
			// 
			// コピーCToolStripMenuItem
			// 
			this.コピーCToolStripMenuItem.Name = "コピーCToolStripMenuItem";
			this.コピーCToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.コピーCToolStripMenuItem.Size = new System.Drawing.Size(322, 38);
			this.コピーCToolStripMenuItem.Text = "コピー(C)";
			this.コピーCToolStripMenuItem.Click += new System.EventHandler(this.コピーCToolStripMenuItem_Click);
			// 
			// 貼り付けPToolStripMenuItem
			// 
			this.貼り付けPToolStripMenuItem.Name = "貼り付けPToolStripMenuItem";
			this.貼り付けPToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.貼り付けPToolStripMenuItem.Size = new System.Drawing.Size(322, 38);
			this.貼り付けPToolStripMenuItem.Text = "貼り付け(P)";
			this.貼り付けPToolStripMenuItem.Click += new System.EventHandler(this.貼り付けPToolStripMenuItem_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tbpPcmList);
			this.tabControl.Controls.Add(this.tbpInstList);
			this.tabControl.Location = new System.Drawing.Point(16, 50);
			this.tabControl.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(1640, 734);
			this.tabControl.TabIndex = 4;
			// 
			// tbpPcmList
			// 
			this.tbpPcmList.Controls.Add(this.lstWave);
			this.tbpPcmList.Controls.Add(this.toolStrip3);
			this.tbpPcmList.Location = new System.Drawing.Point(8, 39);
			this.tbpPcmList.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.tbpPcmList.Name = "tbpPcmList";
			this.tbpPcmList.Size = new System.Drawing.Size(1624, 687);
			this.tbpPcmList.TabIndex = 3;
			this.tbpPcmList.Text = "波形一覧";
			this.tbpPcmList.UseVisualStyleBackColor = true;
			// 
			// lstWave
			// 
			this.lstWave.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstWave.FormattingEnabled = true;
			this.lstWave.ItemHeight = 24;
			this.lstWave.Location = new System.Drawing.Point(7, 62);
			this.lstWave.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.lstWave.Name = "lstWave";
			this.lstWave.ScrollAlwaysVisible = true;
			this.lstWave.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstWave.Size = new System.Drawing.Size(576, 460);
			this.lstWave.TabIndex = 1;
			this.lstWave.DoubleClick += new System.EventHandler(this.lstWave_DoubleClick);
			// 
			// toolStrip3
			// 
			this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip3.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddWave,
            this.tsbDeleteWave,
            this.tsbOutputWave});
			this.toolStrip3.Location = new System.Drawing.Point(0, 0);
			this.toolStrip3.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.toolStrip3.Name = "toolStrip3";
			this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStrip3.Size = new System.Drawing.Size(109, 35);
			this.toolStrip3.TabIndex = 0;
			this.toolStrip3.Text = "toolStrip3";
			// 
			// tsbAddWave
			// 
			this.tsbAddWave.AutoSize = false;
			this.tsbAddWave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbAddWave.Image = global::DLSeditor.Properties.Resources.plus;
			this.tsbAddWave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbAddWave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbAddWave.Name = "tsbAddWave";
			this.tsbAddWave.Size = new System.Drawing.Size(32, 32);
			this.tsbAddWave.Text = "波形追加";
			this.tsbAddWave.Click += new System.EventHandler(this.tsbAddWave_Click);
			// 
			// tsbDeleteWave
			// 
			this.tsbDeleteWave.AutoSize = false;
			this.tsbDeleteWave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDeleteWave.Image = global::DLSeditor.Properties.Resources.minus;
			this.tsbDeleteWave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbDeleteWave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbDeleteWave.Name = "tsbDeleteWave";
			this.tsbDeleteWave.Size = new System.Drawing.Size(32, 32);
			this.tsbDeleteWave.Text = "波形削除";
			this.tsbDeleteWave.Click += new System.EventHandler(this.tsbDeleteWave_Click);
			// 
			// tsbOutputWave
			// 
			this.tsbOutputWave.AutoSize = false;
			this.tsbOutputWave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbOutputWave.Image = global::DLSeditor.Properties.Resources.waveout;
			this.tsbOutputWave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbOutputWave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbOutputWave.Name = "tsbOutputWave";
			this.tsbOutputWave.Size = new System.Drawing.Size(32, 32);
			this.tsbOutputWave.Text = "波形ファイル出力";
			this.tsbOutputWave.Click += new System.EventHandler(this.tsbOutputWave_Click);
			// 
			// tbpInstList
			// 
			this.tbpInstList.Controls.Add(this.lstInst);
			this.tbpInstList.Controls.Add(this.toolStrip2);
			this.tbpInstList.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.tbpInstList.Location = new System.Drawing.Point(8, 39);
			this.tbpInstList.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.tbpInstList.Name = "tbpInstList";
			this.tbpInstList.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.tbpInstList.Size = new System.Drawing.Size(1624, 687);
			this.tbpInstList.TabIndex = 0;
			this.tbpInstList.Text = "音色一覧";
			this.tbpInstList.UseVisualStyleBackColor = true;
			// 
			// lstInst
			// 
			this.lstInst.FormattingEnabled = true;
			this.lstInst.ItemHeight = 38;
			this.lstInst.Location = new System.Drawing.Point(7, 62);
			this.lstInst.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.lstInst.Name = "lstInst";
			this.lstInst.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstInst.Size = new System.Drawing.Size(255, 156);
			this.lstInst.TabIndex = 3;
			this.lstInst.DoubleClick += new System.EventHandler(this.lstInst_DoubleClick);
			// 
			// toolStrip2
			// 
			this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip2.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddInst,
            this.tsbDeleteInst,
            this.tsbCopyInst,
            this.tsbPasteInst});
			this.toolStrip2.Location = new System.Drawing.Point(0, 0);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStrip2.Size = new System.Drawing.Size(141, 35);
			this.toolStrip2.TabIndex = 2;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// tsbAddInst
			// 
			this.tsbAddInst.AutoSize = false;
			this.tsbAddInst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbAddInst.Image = global::DLSeditor.Properties.Resources.plus;
			this.tsbAddInst.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbAddInst.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbAddInst.Name = "tsbAddInst";
			this.tsbAddInst.Size = new System.Drawing.Size(32, 32);
			this.tsbAddInst.Text = "toolStripButton1";
			this.tsbAddInst.ToolTipText = "音色追加";
			this.tsbAddInst.Click += new System.EventHandler(this.tsbAddInst_Click);
			// 
			// tsbDeleteInst
			// 
			this.tsbDeleteInst.AutoSize = false;
			this.tsbDeleteInst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDeleteInst.Image = global::DLSeditor.Properties.Resources.minus;
			this.tsbDeleteInst.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbDeleteInst.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbDeleteInst.Name = "tsbDeleteInst";
			this.tsbDeleteInst.Size = new System.Drawing.Size(32, 32);
			this.tsbDeleteInst.Text = "toolStripButton2";
			this.tsbDeleteInst.ToolTipText = "音色削除";
			this.tsbDeleteInst.Click += new System.EventHandler(this.tsbDeleteInst_Click);
			// 
			// tsbCopyInst
			// 
			this.tsbCopyInst.AutoSize = false;
			this.tsbCopyInst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbCopyInst.Image = global::DLSeditor.Properties.Resources.copy;
			this.tsbCopyInst.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbCopyInst.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbCopyInst.Name = "tsbCopyInst";
			this.tsbCopyInst.Size = new System.Drawing.Size(32, 32);
			this.tsbCopyInst.Text = "toolStripButton3";
			this.tsbCopyInst.ToolTipText = "音色コピー";
			this.tsbCopyInst.Click += new System.EventHandler(this.tsbCopyInst_Click);
			// 
			// tsbPasteInst
			// 
			this.tsbPasteInst.AutoSize = false;
			this.tsbPasteInst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbPasteInst.Image = global::DLSeditor.Properties.Resources.paste;
			this.tsbPasteInst.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbPasteInst.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbPasteInst.Name = "tsbPasteInst";
			this.tsbPasteInst.Size = new System.Drawing.Size(32, 32);
			this.tsbPasteInst.Text = "toolStripButton4";
			this.tsbPasteInst.ToolTipText = "音色貼り付け";
			this.tsbPasteInst.Click += new System.EventHandler(this.tsbPasteInst_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1686, 814);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.Name = "MainForm";
			this.Text = "DLS editor";
			this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabControl.ResumeLayout(false);
			this.tbpPcmList.ResumeLayout(false);
			this.tbpPcmList.PerformLayout();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.tbpInstList.ResumeLayout(false);
			this.tbpInstList.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新規作成NToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 開くOToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 上書き保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 名前を付けて保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 編集EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem コピーCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 貼り付けPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 削除DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 追加AToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tbpInstList;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton tsbAddInst;
		private System.Windows.Forms.ToolStripButton tsbDeleteInst;
		private System.Windows.Forms.ToolStripButton tsbCopyInst;
		private System.Windows.Forms.ToolStripButton tsbPasteInst;
		private System.Windows.Forms.ListBox lstInst;
		private System.Windows.Forms.TabPage tbpPcmList;
		private System.Windows.Forms.ListBox lstWave;
		private System.Windows.Forms.ToolStrip toolStrip3;
		private System.Windows.Forms.ToolStripButton tsbAddWave;
		private System.Windows.Forms.ToolStripButton tsbDeleteWave;
		private System.Windows.Forms.ToolStripButton tsbOutputWave;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
	}
}

