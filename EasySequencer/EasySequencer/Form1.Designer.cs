namespace EasySequencer
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.picKeyboard = new System.Windows.Forms.PictureBox();
			this.lblPosition = new System.Windows.Forms.Label();
			this.lblTempo = new System.Windows.Forms.Label();
			this.hsbSeek = new System.Windows.Forms.HScrollBar();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.新規作成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.開くOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.上書き保存SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.名前を付けて保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.編集EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnPalyStop = new System.Windows.Forms.Button();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			((System.ComponentModel.ISupportInitialize)(this.picKeyboard)).BeginInit();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.panel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// picKeyboard
			// 
			this.picKeyboard.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picKeyboard.BackgroundImage")));
			this.picKeyboard.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picKeyboard.Image = ((System.Drawing.Image)(resources.GetObject("picKeyboard.Image")));
			this.picKeyboard.Location = new System.Drawing.Point(3, 3);
			this.picKeyboard.Name = "picKeyboard";
			this.picKeyboard.Size = new System.Drawing.Size(940, 644);
			this.picKeyboard.TabIndex = 0;
			this.picKeyboard.TabStop = false;
			this.picKeyboard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picKeyboard_MouseDown);
			this.picKeyboard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picKeyboard_MouseMove);
			this.picKeyboard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picKeyboard_MouseUp);
			// 
			// lblPosition
			// 
			this.lblPosition.AutoSize = true;
			this.lblPosition.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblPosition.Location = new System.Drawing.Point(226, 0);
			this.lblPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblPosition.Name = "lblPosition";
			this.lblPosition.Size = new System.Drawing.Size(134, 24);
			this.lblPosition.TabIndex = 4;
			this.lblPosition.Text = "0001:01:000";
			// 
			// lblTempo
			// 
			this.lblTempo.AutoSize = true;
			this.lblTempo.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblTempo.Location = new System.Drawing.Point(145, 0);
			this.lblTempo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTempo.Name = "lblTempo";
			this.lblTempo.Size = new System.Drawing.Size(77, 24);
			this.lblTempo.TabIndex = 7;
			this.lblTempo.Text = "132.00";
			// 
			// hsbSeek
			// 
			this.hsbSeek.Location = new System.Drawing.Point(0, 27);
			this.hsbSeek.Name = "hsbSeek";
			this.hsbSeek.Size = new System.Drawing.Size(360, 20);
			this.hsbSeek.TabIndex = 8;
			this.hsbSeek.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbSeek_Scroll);
			this.hsbSeek.MouseLeave += new System.EventHandler(this.hsbSeek_MouseLeave);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.編集EToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(988, 24);
			this.menuStrip1.TabIndex = 26;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// ファイルFToolStripMenuItem
			// 
			this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新規作成ToolStripMenuItem,
            this.開くOToolStripMenuItem,
            this.上書き保存SToolStripMenuItem,
            this.名前を付けて保存ToolStripMenuItem});
			this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
			this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
			this.ファイルFToolStripMenuItem.Text = "ファイル(F)";
			// 
			// 新規作成ToolStripMenuItem
			// 
			this.新規作成ToolStripMenuItem.Name = "新規作成ToolStripMenuItem";
			this.新規作成ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.新規作成ToolStripMenuItem.Text = "新規作成(N)";
			// 
			// 開くOToolStripMenuItem
			// 
			this.開くOToolStripMenuItem.Name = "開くOToolStripMenuItem";
			this.開くOToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.開くOToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.開くOToolStripMenuItem.Text = "開く(O)";
			this.開くOToolStripMenuItem.Click += new System.EventHandler(this.開くOToolStripMenuItem_Click);
			// 
			// 上書き保存SToolStripMenuItem
			// 
			this.上書き保存SToolStripMenuItem.Name = "上書き保存SToolStripMenuItem";
			this.上書き保存SToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.上書き保存SToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.上書き保存SToolStripMenuItem.Text = "上書き保存(S)";
			// 
			// 名前を付けて保存ToolStripMenuItem
			// 
			this.名前を付けて保存ToolStripMenuItem.Name = "名前を付けて保存ToolStripMenuItem";
			this.名前を付けて保存ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
			this.名前を付けて保存ToolStripMenuItem.Text = "名前を付けて保存(A)";
			// 
			// 編集EToolStripMenuItem
			// 
			this.編集EToolStripMenuItem.Name = "編集EToolStripMenuItem";
			this.編集EToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			this.編集EToolStripMenuItem.Text = "編集(E)";
			// 
			// btnPalyStop
			// 
			this.btnPalyStop.Location = new System.Drawing.Point(3, 3);
			this.btnPalyStop.Name = "btnPalyStop";
			this.btnPalyStop.Size = new System.Drawing.Size(55, 23);
			this.btnPalyStop.TabIndex = 27;
			this.btnPalyStop.Text = "再生";
			this.btnPalyStop.UseVisualStyleBackColor = true;
			this.btnPalyStop.Click += new System.EventHandler(this.btnPalyStop_Click);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numericUpDown1.Location = new System.Drawing.Point(85, 3);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            24,
            0,
            0,
            -2147483648});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(53, 23);
			this.numericUpDown1.TabIndex = 28;
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.lblPosition);
			this.panel1.Controls.Add(this.lblTempo);
			this.panel1.Controls.Add(this.hsbSeek);
			this.panel1.Controls.Add(this.numericUpDown1);
			this.panel1.Controls.Add(this.btnPalyStop);
			this.panel1.Location = new System.Drawing.Point(12, 27);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(365, 53);
			this.panel1.TabIndex = 35;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(12, 86);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(958, 679);
			this.tabControl1.TabIndex = 36;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(950, 653);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage2.Controls.Add(this.picKeyboard);
			this.tabPage2.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(950, 653);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "演奏画面";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(988, 804);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.picKeyboard)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picKeyboard;
		private System.Windows.Forms.Label lblPosition;
		private System.Windows.Forms.Label lblTempo;
		private System.Windows.Forms.HScrollBar hsbSeek;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 新規作成ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 開くOToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 上書き保存SToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 名前を付けて保存ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 編集EToolStripMenuItem;
		private System.Windows.Forms.Button btnPalyStop;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
	}
}

