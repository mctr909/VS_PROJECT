namespace DLSeditor
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
			this.表示VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.詳細表示IToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pCM一覧表示PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lstInst = new System.Windows.Forms.ListBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tbpInstList = new System.Windows.Forms.TabPage();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.tsbAddInst = new System.Windows.Forms.ToolStripButton();
			this.tsbDeleteInst = new System.Windows.Forms.ToolStripButton();
			this.tsbCopyInst = new System.Windows.Forms.ToolStripButton();
			this.tsbPasteInst = new System.Windows.Forms.ToolStripButton();
			this.tbpInstAttribute = new System.Windows.Forms.TabPage();
			this.pnlInstAttribute = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.numAmpRelease = new System.Windows.Forms.NumericUpDown();
			this.numAmpSustain = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.numAmpDecay = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.numAmpAttack = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.menuStrip1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tbpInstList.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.tbpInstAttribute.SuspendLayout();
			this.pnlInstAttribute.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAmpRelease)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numAmpSustain)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numAmpDecay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numAmpAttack)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.編集EToolStripMenuItem,
            this.表示VToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(832, 24);
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
			this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
			this.ファイルFToolStripMenuItem.Text = "ファイル(F)";
			// 
			// 新規作成NToolStripMenuItem
			// 
			this.新規作成NToolStripMenuItem.Name = "新規作成NToolStripMenuItem";
			this.新規作成NToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.新規作成NToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.新規作成NToolStripMenuItem.Text = "新規作成(N)";
			this.新規作成NToolStripMenuItem.Click += new System.EventHandler(this.新規作成NToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
			// 
			// 開くOToolStripMenuItem
			// 
			this.開くOToolStripMenuItem.Name = "開くOToolStripMenuItem";
			this.開くOToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.開くOToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.開くOToolStripMenuItem.Text = "開く(O)";
			this.開くOToolStripMenuItem.Click += new System.EventHandler(this.開くOToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
			// 
			// 上書き保存ToolStripMenuItem
			// 
			this.上書き保存ToolStripMenuItem.Name = "上書き保存ToolStripMenuItem";
			this.上書き保存ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.上書き保存ToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.上書き保存ToolStripMenuItem.Text = "上書き保存(S)";
			this.上書き保存ToolStripMenuItem.Click += new System.EventHandler(this.上書き保存ToolStripMenuItem_Click);
			// 
			// 名前を付けて保存ToolStripMenuItem
			// 
			this.名前を付けて保存ToolStripMenuItem.Name = "名前を付けて保存ToolStripMenuItem";
			this.名前を付けて保存ToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
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
			this.編集EToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
			this.編集EToolStripMenuItem.Text = "編集(E)";
			// 
			// 追加AToolStripMenuItem
			// 
			this.追加AToolStripMenuItem.Name = "追加AToolStripMenuItem";
			this.追加AToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
			this.追加AToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.追加AToolStripMenuItem.Text = "追加(A)";
			this.追加AToolStripMenuItem.Click += new System.EventHandler(this.追加AToolStripMenuItem_Click);
			// 
			// 削除DToolStripMenuItem
			// 
			this.削除DToolStripMenuItem.Name = "削除DToolStripMenuItem";
			this.削除DToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.削除DToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.削除DToolStripMenuItem.Text = "削除(D)";
			this.削除DToolStripMenuItem.Click += new System.EventHandler(this.削除DToolStripMenuItem_Click);
			// 
			// コピーCToolStripMenuItem
			// 
			this.コピーCToolStripMenuItem.Name = "コピーCToolStripMenuItem";
			this.コピーCToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.コピーCToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.コピーCToolStripMenuItem.Text = "コピー(C)";
			this.コピーCToolStripMenuItem.Click += new System.EventHandler(this.コピーCToolStripMenuItem_Click);
			// 
			// 貼り付けPToolStripMenuItem
			// 
			this.貼り付けPToolStripMenuItem.Name = "貼り付けPToolStripMenuItem";
			this.貼り付けPToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.貼り付けPToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.貼り付けPToolStripMenuItem.Text = "貼り付け(P)";
			this.貼り付けPToolStripMenuItem.Click += new System.EventHandler(this.貼り付けPToolStripMenuItem_Click);
			// 
			// 表示VToolStripMenuItem
			// 
			this.表示VToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.詳細表示IToolStripMenuItem,
            this.pCM一覧表示PToolStripMenuItem});
			this.表示VToolStripMenuItem.Name = "表示VToolStripMenuItem";
			this.表示VToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.表示VToolStripMenuItem.Text = "表示(V)";
			// 
			// 詳細表示IToolStripMenuItem
			// 
			this.詳細表示IToolStripMenuItem.Name = "詳細表示IToolStripMenuItem";
			this.詳細表示IToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)));
			this.詳細表示IToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
			this.詳細表示IToolStripMenuItem.Text = "詳細表示(I)";
			this.詳細表示IToolStripMenuItem.Click += new System.EventHandler(this.詳細表示IToolStripMenuItem_Click);
			// 
			// pCM一覧表示PToolStripMenuItem
			// 
			this.pCM一覧表示PToolStripMenuItem.Name = "pCM一覧表示PToolStripMenuItem";
			this.pCM一覧表示PToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.pCM一覧表示PToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
			this.pCM一覧表示PToolStripMenuItem.Text = "PCM一覧表示(P)";
			this.pCM一覧表示PToolStripMenuItem.Click += new System.EventHandler(this.pCM一覧表示PToolStripMenuItem_Click);
			// 
			// lstInst
			// 
			this.lstInst.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstInst.FormattingEnabled = true;
			this.lstInst.IntegralHeight = false;
			this.lstInst.ItemHeight = 15;
			this.lstInst.Location = new System.Drawing.Point(3, 31);
			this.lstInst.Name = "lstInst";
			this.lstInst.Size = new System.Drawing.Size(631, 307);
			this.lstInst.TabIndex = 1;
			this.lstInst.DoubleClick += new System.EventHandler(this.lstInst_DoubleClick);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tbpInstList);
			this.tabControl.Controls.Add(this.tbpInstAttribute);
			this.tabControl.Location = new System.Drawing.Point(12, 27);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(757, 367);
			this.tabControl.TabIndex = 4;
			// 
			// tbpInstList
			// 
			this.tbpInstList.Controls.Add(this.toolStrip2);
			this.tbpInstList.Controls.Add(this.lstInst);
			this.tbpInstList.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.tbpInstList.Location = new System.Drawing.Point(4, 22);
			this.tbpInstList.Name = "tbpInstList";
			this.tbpInstList.Padding = new System.Windows.Forms.Padding(3);
			this.tbpInstList.Size = new System.Drawing.Size(749, 341);
			this.tbpInstList.TabIndex = 0;
			this.tbpInstList.Text = "音色一覧";
			this.tbpInstList.UseVisualStyleBackColor = true;
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddInst,
            this.tsbDeleteInst,
            this.tsbCopyInst,
            this.tsbPasteInst});
			this.toolStrip2.Location = new System.Drawing.Point(3, 3);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(743, 25);
			this.toolStrip2.TabIndex = 2;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// tsbAddInst
			// 
			this.tsbAddInst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbAddInst.Image = global::DLSeditor.Properties.Resources.plus;
			this.tsbAddInst.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbAddInst.Name = "tsbAddInst";
			this.tsbAddInst.Size = new System.Drawing.Size(23, 22);
			this.tsbAddInst.Text = "toolStripButton1";
			this.tsbAddInst.Click += new System.EventHandler(this.tsbAddInst_Click);
			// 
			// tsbDeleteInst
			// 
			this.tsbDeleteInst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDeleteInst.Image = global::DLSeditor.Properties.Resources.minus;
			this.tsbDeleteInst.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbDeleteInst.Name = "tsbDeleteInst";
			this.tsbDeleteInst.Size = new System.Drawing.Size(23, 22);
			this.tsbDeleteInst.Text = "toolStripButton2";
			this.tsbDeleteInst.Click += new System.EventHandler(this.tsbDeleteInst_Click);
			// 
			// tsbCopyInst
			// 
			this.tsbCopyInst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbCopyInst.Image = global::DLSeditor.Properties.Resources.copy;
			this.tsbCopyInst.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbCopyInst.Name = "tsbCopyInst";
			this.tsbCopyInst.Size = new System.Drawing.Size(23, 22);
			this.tsbCopyInst.Text = "toolStripButton3";
			this.tsbCopyInst.Click += new System.EventHandler(this.tsbCopyInst_Click);
			// 
			// tsbPasteInst
			// 
			this.tsbPasteInst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbPasteInst.Image = global::DLSeditor.Properties.Resources.paste;
			this.tsbPasteInst.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbPasteInst.Name = "tsbPasteInst";
			this.tsbPasteInst.Size = new System.Drawing.Size(23, 22);
			this.tsbPasteInst.Text = "toolStripButton4";
			this.tsbPasteInst.Click += new System.EventHandler(this.tsbPasteInst_Click);
			// 
			// tbpInstAttribute
			// 
			this.tbpInstAttribute.Controls.Add(this.pnlInstAttribute);
			this.tbpInstAttribute.Location = new System.Drawing.Point(4, 22);
			this.tbpInstAttribute.Name = "tbpInstAttribute";
			this.tbpInstAttribute.Padding = new System.Windows.Forms.Padding(3);
			this.tbpInstAttribute.Size = new System.Drawing.Size(749, 341);
			this.tbpInstAttribute.TabIndex = 1;
			this.tbpInstAttribute.Text = "音色設定";
			this.tbpInstAttribute.UseVisualStyleBackColor = true;
			// 
			// pnlInstAttribute
			// 
			this.pnlInstAttribute.AutoScroll = true;
			this.pnlInstAttribute.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlInstAttribute.Controls.Add(this.groupBox1);
			this.pnlInstAttribute.Location = new System.Drawing.Point(3, 6);
			this.pnlInstAttribute.Name = "pnlInstAttribute";
			this.pnlInstAttribute.Size = new System.Drawing.Size(743, 329);
			this.pnlInstAttribute.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.numAmpAttack);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.numAmpDecay);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.numAmpSustain);
			this.groupBox1.Controls.Add(this.numAmpRelease);
			this.groupBox1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(337, 58);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "振幅";
			// 
			// numAmpRelease
			// 
			this.numAmpRelease.DecimalPlaces = 3;
			this.numAmpRelease.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
			this.numAmpRelease.Location = new System.Drawing.Point(252, 30);
			this.numAmpRelease.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
			this.numAmpRelease.Name = "numAmpRelease";
			this.numAmpRelease.Size = new System.Drawing.Size(76, 19);
			this.numAmpRelease.TabIndex = 5;
			// 
			// numAmpSustain
			// 
			this.numAmpSustain.DecimalPlaces = 1;
			this.numAmpSustain.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numAmpSustain.Location = new System.Drawing.Point(170, 30);
			this.numAmpSustain.Name = "numAmpSustain";
			this.numAmpSustain.Size = new System.Drawing.Size(76, 19);
			this.numAmpSustain.TabIndex = 4;
			this.numAmpSustain.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.label2.Location = new System.Drawing.Point(86, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 12);
			this.label2.TabIndex = 7;
			this.label2.Text = "ディケイ(sec)";
			// 
			// numAmpDecay
			// 
			this.numAmpDecay.DecimalPlaces = 3;
			this.numAmpDecay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
			this.numAmpDecay.Location = new System.Drawing.Point(88, 30);
			this.numAmpDecay.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
			this.numAmpDecay.Name = "numAmpDecay";
			this.numAmpDecay.Size = new System.Drawing.Size(76, 19);
			this.numAmpDecay.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(168, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(71, 12);
			this.label3.TabIndex = 8;
			this.label3.Text = "サスティン(%)";
			// 
			// numAmpAttack
			// 
			this.numAmpAttack.DecimalPlaces = 3;
			this.numAmpAttack.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
			this.numAmpAttack.Location = new System.Drawing.Point(6, 30);
			this.numAmpAttack.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
			this.numAmpAttack.Name = "numAmpAttack";
			this.numAmpAttack.Size = new System.Drawing.Size(76, 19);
			this.numAmpAttack.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label4.Location = new System.Drawing.Point(252, 15);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 12);
			this.label4.TabIndex = 9;
			this.label4.Text = "リリース(sec)";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.label1.Location = new System.Drawing.Point(6, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 12);
			this.label1.TabIndex = 6;
			this.label1.Text = "アタック(sec)";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(832, 407);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "DLS editor";
			this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabControl.ResumeLayout(false);
			this.tbpInstList.ResumeLayout(false);
			this.tbpInstList.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.tbpInstAttribute.ResumeLayout(false);
			this.pnlInstAttribute.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAmpRelease)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numAmpSustain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numAmpDecay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numAmpAttack)).EndInit();
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
        private System.Windows.Forms.ListBox lstInst;
        private System.Windows.Forms.ToolStripMenuItem 削除DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 追加AToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.ToolStripMenuItem 表示VToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 詳細表示IToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pCM一覧表示PToolStripMenuItem;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tbpInstList;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton tsbAddInst;
		private System.Windows.Forms.ToolStripButton tsbDeleteInst;
		private System.Windows.Forms.ToolStripButton tsbCopyInst;
		private System.Windows.Forms.ToolStripButton tsbPasteInst;
		private System.Windows.Forms.TabPage tbpInstAttribute;
		private System.Windows.Forms.Panel pnlInstAttribute;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numAmpAttack;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numAmpDecay;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numAmpSustain;
		private System.Windows.Forms.NumericUpDown numAmpRelease;
	}
}

