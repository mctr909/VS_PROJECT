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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblPosition = new System.Windows.Forms.Label();
			this.lblTempo = new System.Windows.Forms.Label();
			this.hsbSeek = new System.Windows.Forms.HScrollBar();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.checkBox7 = new System.Windows.Forms.CheckBox();
			this.checkBox8 = new System.Windows.Forms.CheckBox();
			this.checkBox9 = new System.Windows.Forms.CheckBox();
			this.checkBox10 = new System.Windows.Forms.CheckBox();
			this.checkBox11 = new System.Windows.Forms.CheckBox();
			this.checkBox12 = new System.Windows.Forms.CheckBox();
			this.checkBox13 = new System.Windows.Forms.CheckBox();
			this.checkBox14 = new System.Windows.Forms.CheckBox();
			this.checkBox15 = new System.Windows.Forms.CheckBox();
			this.checkBox16 = new System.Windows.Forms.CheckBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.新規作成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.開くOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.上書き保存SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.名前を付けて保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.編集EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnPalyStop = new System.Windows.Forms.Button();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.numChannel = new System.Windows.Forms.NumericUpDown();
			this.trkVol = new System.Windows.Forms.TrackBar();
			this.trkCho = new System.Windows.Forms.TrackBar();
			this.trkDel = new System.Windows.Forms.TrackBar();
			this.trkPan = new System.Windows.Forms.TrackBar();
			this.chkSolo = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numChannel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trkVol)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trkCho)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trkDel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trkPan)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 27);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(631, 40);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// lblPosition
			// 
			this.lblPosition.AutoSize = true;
			this.lblPosition.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblPosition.Location = new System.Drawing.Point(4, 70);
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
			this.lblTempo.Location = new System.Drawing.Point(191, 70);
			this.lblTempo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTempo.Name = "lblTempo";
			this.lblTempo.Size = new System.Drawing.Size(77, 24);
			this.lblTempo.TabIndex = 7;
			this.lblTempo.Text = "132.00";
			// 
			// hsbSeek
			// 
			this.hsbSeek.Location = new System.Drawing.Point(8, 94);
			this.hsbSeek.Name = "hsbSeek";
			this.hsbSeek.Size = new System.Drawing.Size(260, 17);
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
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(356, 73);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(44, 16);
			this.checkBox1.TabIndex = 10;
			this.checkBox1.Text = "Ch1";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Checked = true;
			this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox2.Location = new System.Drawing.Point(412, 73);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(44, 16);
			this.checkBox2.TabIndex = 11;
			this.checkBox2.Text = "Ch2";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Checked = true;
			this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox3.Location = new System.Drawing.Point(468, 73);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(44, 16);
			this.checkBox3.TabIndex = 12;
			this.checkBox3.Text = "Ch3";
			this.checkBox3.UseVisualStyleBackColor = true;
			this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.Checked = true;
			this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox4.Location = new System.Drawing.Point(519, 73);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(44, 16);
			this.checkBox4.TabIndex = 13;
			this.checkBox4.Text = "Ch4";
			this.checkBox4.UseVisualStyleBackColor = true;
			this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			// 
			// checkBox5
			// 
			this.checkBox5.AutoSize = true;
			this.checkBox5.Checked = true;
			this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox5.Location = new System.Drawing.Point(575, 73);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(44, 16);
			this.checkBox5.TabIndex = 14;
			this.checkBox5.Text = "Ch5";
			this.checkBox5.UseVisualStyleBackColor = true;
			this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
			// 
			// checkBox6
			// 
			this.checkBox6.AutoSize = true;
			this.checkBox6.Checked = true;
			this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox6.Location = new System.Drawing.Point(631, 73);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(44, 16);
			this.checkBox6.TabIndex = 15;
			this.checkBox6.Text = "Ch6";
			this.checkBox6.UseVisualStyleBackColor = true;
			this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
			// 
			// checkBox7
			// 
			this.checkBox7.AutoSize = true;
			this.checkBox7.Checked = true;
			this.checkBox7.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox7.Location = new System.Drawing.Point(687, 73);
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.Size = new System.Drawing.Size(44, 16);
			this.checkBox7.TabIndex = 16;
			this.checkBox7.Text = "Ch7";
			this.checkBox7.UseVisualStyleBackColor = true;
			this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
			// 
			// checkBox8
			// 
			this.checkBox8.AutoSize = true;
			this.checkBox8.Checked = true;
			this.checkBox8.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox8.Location = new System.Drawing.Point(738, 73);
			this.checkBox8.Name = "checkBox8";
			this.checkBox8.Size = new System.Drawing.Size(44, 16);
			this.checkBox8.TabIndex = 17;
			this.checkBox8.Text = "Ch8";
			this.checkBox8.UseVisualStyleBackColor = true;
			this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
			// 
			// checkBox9
			// 
			this.checkBox9.AutoSize = true;
			this.checkBox9.Checked = true;
			this.checkBox9.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox9.Location = new System.Drawing.Point(356, 95);
			this.checkBox9.Name = "checkBox9";
			this.checkBox9.Size = new System.Drawing.Size(44, 16);
			this.checkBox9.TabIndex = 18;
			this.checkBox9.Text = "Ch9";
			this.checkBox9.UseVisualStyleBackColor = true;
			this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
			// 
			// checkBox10
			// 
			this.checkBox10.AutoSize = true;
			this.checkBox10.Checked = true;
			this.checkBox10.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox10.Location = new System.Drawing.Point(412, 95);
			this.checkBox10.Name = "checkBox10";
			this.checkBox10.Size = new System.Drawing.Size(50, 16);
			this.checkBox10.TabIndex = 19;
			this.checkBox10.Text = "Ch10";
			this.checkBox10.UseVisualStyleBackColor = true;
			this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged);
			// 
			// checkBox11
			// 
			this.checkBox11.AutoSize = true;
			this.checkBox11.Checked = true;
			this.checkBox11.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox11.Location = new System.Drawing.Point(468, 95);
			this.checkBox11.Name = "checkBox11";
			this.checkBox11.Size = new System.Drawing.Size(50, 16);
			this.checkBox11.TabIndex = 20;
			this.checkBox11.Text = "Ch11";
			this.checkBox11.UseVisualStyleBackColor = true;
			this.checkBox11.CheckedChanged += new System.EventHandler(this.checkBox11_CheckedChanged);
			// 
			// checkBox12
			// 
			this.checkBox12.AutoSize = true;
			this.checkBox12.Checked = true;
			this.checkBox12.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox12.Location = new System.Drawing.Point(519, 95);
			this.checkBox12.Name = "checkBox12";
			this.checkBox12.Size = new System.Drawing.Size(50, 16);
			this.checkBox12.TabIndex = 21;
			this.checkBox12.Text = "Ch12";
			this.checkBox12.UseVisualStyleBackColor = true;
			this.checkBox12.CheckedChanged += new System.EventHandler(this.checkBox12_CheckedChanged);
			// 
			// checkBox13
			// 
			this.checkBox13.AutoSize = true;
			this.checkBox13.Checked = true;
			this.checkBox13.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox13.Location = new System.Drawing.Point(575, 95);
			this.checkBox13.Name = "checkBox13";
			this.checkBox13.Size = new System.Drawing.Size(50, 16);
			this.checkBox13.TabIndex = 22;
			this.checkBox13.Text = "Ch13";
			this.checkBox13.UseVisualStyleBackColor = true;
			this.checkBox13.CheckedChanged += new System.EventHandler(this.checkBox13_CheckedChanged);
			// 
			// checkBox14
			// 
			this.checkBox14.AutoSize = true;
			this.checkBox14.Checked = true;
			this.checkBox14.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox14.Location = new System.Drawing.Point(631, 95);
			this.checkBox14.Name = "checkBox14";
			this.checkBox14.Size = new System.Drawing.Size(50, 16);
			this.checkBox14.TabIndex = 23;
			this.checkBox14.Text = "Ch14";
			this.checkBox14.UseVisualStyleBackColor = true;
			this.checkBox14.CheckedChanged += new System.EventHandler(this.checkBox14_CheckedChanged);
			// 
			// checkBox15
			// 
			this.checkBox15.AutoSize = true;
			this.checkBox15.Checked = true;
			this.checkBox15.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox15.Location = new System.Drawing.Point(687, 95);
			this.checkBox15.Name = "checkBox15";
			this.checkBox15.Size = new System.Drawing.Size(50, 16);
			this.checkBox15.TabIndex = 24;
			this.checkBox15.Text = "Ch15";
			this.checkBox15.UseVisualStyleBackColor = true;
			this.checkBox15.CheckedChanged += new System.EventHandler(this.checkBox15_CheckedChanged);
			// 
			// checkBox16
			// 
			this.checkBox16.AutoSize = true;
			this.checkBox16.Checked = true;
			this.checkBox16.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox16.Location = new System.Drawing.Point(738, 95);
			this.checkBox16.Name = "checkBox16";
			this.checkBox16.Size = new System.Drawing.Size(50, 16);
			this.checkBox16.TabIndex = 25;
			this.checkBox16.Text = "Ch16";
			this.checkBox16.UseVisualStyleBackColor = true;
			this.checkBox16.CheckedChanged += new System.EventHandler(this.checkBox16_CheckedChanged);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.編集EToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(822, 24);
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
			this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
			this.ファイルFToolStripMenuItem.Text = "ファイル(F)";
			// 
			// 新規作成ToolStripMenuItem
			// 
			this.新規作成ToolStripMenuItem.Name = "新規作成ToolStripMenuItem";
			this.新規作成ToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.新規作成ToolStripMenuItem.Text = "新規作成(N)";
			// 
			// 開くOToolStripMenuItem
			// 
			this.開くOToolStripMenuItem.Name = "開くOToolStripMenuItem";
			this.開くOToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.開くOToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.開くOToolStripMenuItem.Text = "開く(O)";
			this.開くOToolStripMenuItem.Click += new System.EventHandler(this.開くOToolStripMenuItem_Click);
			// 
			// 上書き保存SToolStripMenuItem
			// 
			this.上書き保存SToolStripMenuItem.Name = "上書き保存SToolStripMenuItem";
			this.上書き保存SToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.上書き保存SToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.上書き保存SToolStripMenuItem.Text = "上書き保存(S)";
			// 
			// 名前を付けて保存ToolStripMenuItem
			// 
			this.名前を付けて保存ToolStripMenuItem.Name = "名前を付けて保存ToolStripMenuItem";
			this.名前を付けて保存ToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.名前を付けて保存ToolStripMenuItem.Text = "名前を付けて保存(A)";
			// 
			// 編集EToolStripMenuItem
			// 
			this.編集EToolStripMenuItem.Name = "編集EToolStripMenuItem";
			this.編集EToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
			this.編集EToolStripMenuItem.Text = "編集(E)";
			// 
			// btnPalyStop
			// 
			this.btnPalyStop.Location = new System.Drawing.Point(8, 114);
			this.btnPalyStop.Name = "btnPalyStop";
			this.btnPalyStop.Size = new System.Drawing.Size(75, 23);
			this.btnPalyStop.TabIndex = 27;
			this.btnPalyStop.Text = "再生";
			this.btnPalyStop.UseVisualStyleBackColor = true;
			this.btnPalyStop.Click += new System.EventHandler(this.btnPalyStop_Click);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numericUpDown1.Location = new System.Drawing.Point(193, 114);
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
			this.numericUpDown1.Size = new System.Drawing.Size(75, 27);
			this.numericUpDown1.TabIndex = 28;
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// numChannel
			// 
			this.numChannel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.numChannel.Location = new System.Drawing.Point(356, 114);
			this.numChannel.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
			this.numChannel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numChannel.Name = "numChannel";
			this.numChannel.Size = new System.Drawing.Size(56, 27);
			this.numChannel.TabIndex = 29;
			this.numChannel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numChannel.ValueChanged += new System.EventHandler(this.numChannel_ValueChanged);
			// 
			// trkVol
			// 
			this.trkVol.Location = new System.Drawing.Point(468, 114);
			this.trkVol.Maximum = 127;
			this.trkVol.Name = "trkVol";
			this.trkVol.Size = new System.Drawing.Size(81, 45);
			this.trkVol.TabIndex = 30;
			this.trkVol.TickFrequency = 16;
			this.trkVol.Scroll += new System.EventHandler(this.trkVol_Scroll);
			this.trkVol.ValueChanged += new System.EventHandler(this.trkVol_ValueChanged);
			this.trkVol.MouseLeave += new System.EventHandler(this.trkVol_MouseLeave);
			// 
			// trkCho
			// 
			this.trkCho.Location = new System.Drawing.Point(614, 114);
			this.trkCho.Maximum = 127;
			this.trkCho.Name = "trkCho";
			this.trkCho.Size = new System.Drawing.Size(81, 45);
			this.trkCho.TabIndex = 31;
			this.trkCho.TickFrequency = 16;
			this.trkCho.Scroll += new System.EventHandler(this.trkCho_Scroll);
			this.trkCho.ValueChanged += new System.EventHandler(this.trkCho_ValueChanged);
			this.trkCho.MouseLeave += new System.EventHandler(this.trkCho_MouseLeave);
			// 
			// trkDel
			// 
			this.trkDel.Location = new System.Drawing.Point(687, 114);
			this.trkDel.Maximum = 127;
			this.trkDel.Name = "trkDel";
			this.trkDel.Size = new System.Drawing.Size(81, 45);
			this.trkDel.TabIndex = 32;
			this.trkDel.TickFrequency = 16;
			this.trkDel.Scroll += new System.EventHandler(this.trkDel_Scroll);
			this.trkDel.ValueChanged += new System.EventHandler(this.trkDel_ValueChanged);
			this.trkDel.MouseLeave += new System.EventHandler(this.trkDel_MouseLeave);
			// 
			// trkPan
			// 
			this.trkPan.Location = new System.Drawing.Point(541, 114);
			this.trkPan.Maximum = 127;
			this.trkPan.Name = "trkPan";
			this.trkPan.Size = new System.Drawing.Size(81, 45);
			this.trkPan.TabIndex = 33;
			this.trkPan.TickFrequency = 16;
			this.trkPan.Scroll += new System.EventHandler(this.trkPan_Scroll);
			this.trkPan.ValueChanged += new System.EventHandler(this.trkPan_ValueChanged);
			this.trkPan.MouseLeave += new System.EventHandler(this.trkPan_MouseLeave);
			// 
			// chkSolo
			// 
			this.chkSolo.AutoSize = true;
			this.chkSolo.Location = new System.Drawing.Point(418, 121);
			this.chkSolo.Name = "chkSolo";
			this.chkSolo.Size = new System.Drawing.Size(46, 16);
			this.chkSolo.TabIndex = 34;
			this.chkSolo.Text = "Solo";
			this.chkSolo.UseVisualStyleBackColor = true;
			this.chkSolo.CheckedChanged += new System.EventHandler(this.chkSolo_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(822, 281);
			this.Controls.Add(this.chkSolo);
			this.Controls.Add(this.trkVol);
			this.Controls.Add(this.numChannel);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.btnPalyStop);
			this.Controls.Add(this.checkBox16);
			this.Controls.Add(this.checkBox15);
			this.Controls.Add(this.checkBox14);
			this.Controls.Add(this.checkBox13);
			this.Controls.Add(this.checkBox12);
			this.Controls.Add(this.checkBox11);
			this.Controls.Add(this.checkBox10);
			this.Controls.Add(this.checkBox9);
			this.Controls.Add(this.checkBox8);
			this.Controls.Add(this.checkBox7);
			this.Controls.Add(this.checkBox6);
			this.Controls.Add(this.checkBox5);
			this.Controls.Add(this.checkBox4);
			this.Controls.Add(this.checkBox3);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.hsbSeek);
			this.Controls.Add(this.lblTempo);
			this.Controls.Add(this.lblPosition);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.trkPan);
			this.Controls.Add(this.trkCho);
			this.Controls.Add(this.trkDel);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numChannel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trkVol)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trkCho)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trkDel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trkPan)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblPosition;
		private System.Windows.Forms.Label lblTempo;
		private System.Windows.Forms.HScrollBar hsbSeek;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.CheckBox checkBox8;
		private System.Windows.Forms.CheckBox checkBox9;
		private System.Windows.Forms.CheckBox checkBox10;
		private System.Windows.Forms.CheckBox checkBox11;
		private System.Windows.Forms.CheckBox checkBox12;
		private System.Windows.Forms.CheckBox checkBox13;
		private System.Windows.Forms.CheckBox checkBox14;
		private System.Windows.Forms.CheckBox checkBox15;
		private System.Windows.Forms.CheckBox checkBox16;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 新規作成ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 開くOToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 上書き保存SToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 名前を付けて保存ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 編集EToolStripMenuItem;
		private System.Windows.Forms.Button btnPalyStop;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.NumericUpDown numChannel;
		private System.Windows.Forms.TrackBar trkVol;
		private System.Windows.Forms.TrackBar trkCho;
		private System.Windows.Forms.TrackBar trkDel;
		private System.Windows.Forms.TrackBar trkPan;
		private System.Windows.Forms.CheckBox chkSolo;
	}
}

