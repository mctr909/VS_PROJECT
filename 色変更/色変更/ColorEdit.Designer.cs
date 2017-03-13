namespace 色変更
{
    partial class ColorEdit
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCondLight = new System.Windows.Forms.Label();
            this.picCondLight = new System.Windows.Forms.PictureBox();
            this.trkCondALight = new System.Windows.Forms.TrackBar();
            this.trkCondBLight = new System.Windows.Forms.TrackBar();
            this.lblCondSaturation = new System.Windows.Forms.Label();
            this.picCondSaturation = new System.Windows.Forms.PictureBox();
            this.lblCondHueWidth = new System.Windows.Forms.Label();
            this.trkCondHueWidth = new System.Windows.Forms.TrackBar();
            this.lblCondHue = new System.Windows.Forms.Label();
            this.picCondHue = new System.Windows.Forms.PictureBox();
            this.trkCondASaturation = new System.Windows.Forms.TrackBar();
            this.trkCondBSaturation = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDeleteSetting = new System.Windows.Forms.Button();
            this.btnSaveSetting = new System.Windows.Forms.Button();
            this.cmbSetting = new System.Windows.Forms.ComboBox();
            this.chkLightNoChg = new System.Windows.Forms.CheckBox();
            this.chkSaturationNoChg = new System.Windows.Forms.CheckBox();
            this.chkHueNoChg = new System.Windows.Forms.CheckBox();
            this.lblChgLight = new System.Windows.Forms.Label();
            this.picChgLight = new System.Windows.Forms.PictureBox();
            this.lblChgSaturation = new System.Windows.Forms.Label();
            this.trkChgALight = new System.Windows.Forms.TrackBar();
            this.picChgSaturation = new System.Windows.Forms.PictureBox();
            this.trkChgBLight = new System.Windows.Forms.TrackBar();
            this.trkChgASaturation = new System.Windows.Forms.TrackBar();
            this.trkChgBSaturation = new System.Windows.Forms.TrackBar();
            this.lblChgHue = new System.Windows.Forms.Label();
            this.picChgHue = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCondLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkCondALight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkCondBLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCondSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkCondHueWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCondHue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkCondASaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkCondBSaturation)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picChgLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkChgALight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picChgSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkChgBLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkChgASaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkChgBSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picChgHue)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCondLight);
            this.groupBox1.Controls.Add(this.picCondLight);
            this.groupBox1.Controls.Add(this.trkCondALight);
            this.groupBox1.Controls.Add(this.trkCondBLight);
            this.groupBox1.Controls.Add(this.lblCondSaturation);
            this.groupBox1.Controls.Add(this.picCondSaturation);
            this.groupBox1.Controls.Add(this.lblCondHueWidth);
            this.groupBox1.Controls.Add(this.trkCondHueWidth);
            this.groupBox1.Controls.Add(this.lblCondHue);
            this.groupBox1.Controls.Add(this.picCondHue);
            this.groupBox1.Controls.Add(this.trkCondASaturation);
            this.groupBox1.Controls.Add(this.trkCondBSaturation);
            this.groupBox1.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(263, 614);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "抽出条件";
            // 
            // lblCondLight
            // 
            this.lblCondLight.AutoSize = true;
            this.lblCondLight.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCondLight.Location = new System.Drawing.Point(6, 500);
            this.lblCondLight.Name = "lblCondLight";
            this.lblCondLight.Size = new System.Drawing.Size(77, 19);
            this.lblCondLight.TabIndex = 12;
            this.lblCondLight.Text = "明度(0%)";
            // 
            // picCondLight
            // 
            this.picCondLight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCondLight.Location = new System.Drawing.Point(18, 547);
            this.picCondLight.Margin = new System.Windows.Forms.Padding(0);
            this.picCondLight.Name = "picCondLight";
            this.picCondLight.Size = new System.Drawing.Size(226, 20);
            this.picCondLight.TabIndex = 9;
            this.picCondLight.TabStop = false;
            // 
            // trkCondALight
            // 
            this.trkCondALight.AutoSize = false;
            this.trkCondALight.Location = new System.Drawing.Point(6, 522);
            this.trkCondALight.Maximum = 100;
            this.trkCondALight.Name = "trkCondALight";
            this.trkCondALight.Size = new System.Drawing.Size(250, 45);
            this.trkCondALight.TabIndex = 10;
            this.trkCondALight.Tag = "";
            this.trkCondALight.TickFrequency = 10;
            this.trkCondALight.Value = 100;
            this.trkCondALight.Scroll += new System.EventHandler(this.trkCondALight_Scroll);
            // 
            // trkCondBLight
            // 
            this.trkCondBLight.AutoSize = false;
            this.trkCondBLight.Location = new System.Drawing.Point(6, 561);
            this.trkCondBLight.Maximum = 100;
            this.trkCondBLight.Name = "trkCondBLight";
            this.trkCondBLight.Size = new System.Drawing.Size(250, 45);
            this.trkCondBLight.TabIndex = 11;
            this.trkCondBLight.TickFrequency = 10;
            this.trkCondBLight.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trkCondBLight.Scroll += new System.EventHandler(this.trkCondBLight_Scroll);
            // 
            // lblCondSaturation
            // 
            this.lblCondSaturation.AutoSize = true;
            this.lblCondSaturation.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCondSaturation.Location = new System.Drawing.Point(6, 402);
            this.lblCondSaturation.Name = "lblCondSaturation";
            this.lblCondSaturation.Size = new System.Drawing.Size(77, 19);
            this.lblCondSaturation.TabIndex = 8;
            this.lblCondSaturation.Text = "彩度(0%)";
            // 
            // picCondSaturation
            // 
            this.picCondSaturation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCondSaturation.Location = new System.Drawing.Point(18, 449);
            this.picCondSaturation.Margin = new System.Windows.Forms.Padding(0);
            this.picCondSaturation.Name = "picCondSaturation";
            this.picCondSaturation.Size = new System.Drawing.Size(226, 20);
            this.picCondSaturation.TabIndex = 5;
            this.picCondSaturation.TabStop = false;
            // 
            // lblCondHueWidth
            // 
            this.lblCondHueWidth.AutoSize = true;
            this.lblCondHueWidth.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCondHueWidth.Location = new System.Drawing.Point(6, 332);
            this.lblCondHueWidth.Name = "lblCondHueWidth";
            this.lblCondHueWidth.Size = new System.Drawing.Size(97, 19);
            this.lblCondHueWidth.TabIndex = 4;
            this.lblCondHueWidth.Text = "色相幅(±0°)";
            // 
            // trkCondHueWidth
            // 
            this.trkCondHueWidth.Location = new System.Drawing.Point(6, 354);
            this.trkCondHueWidth.Maximum = 180;
            this.trkCondHueWidth.Name = "trkCondHueWidth";
            this.trkCondHueWidth.Size = new System.Drawing.Size(250, 45);
            this.trkCondHueWidth.TabIndex = 3;
            this.trkCondHueWidth.TickFrequency = 15;
            this.trkCondHueWidth.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trkCondHueWidth.Value = 15;
            this.trkCondHueWidth.Scroll += new System.EventHandler(this.trkCondHueWidth_Scroll);
            // 
            // lblCondHue
            // 
            this.lblCondHue.AutoSize = true;
            this.lblCondHue.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCondHue.Location = new System.Drawing.Point(6, 33);
            this.lblCondHue.Name = "lblCondHue";
            this.lblCondHue.Size = new System.Drawing.Size(39, 19);
            this.lblCondHue.TabIndex = 2;
            this.lblCondHue.Text = "色相";
            // 
            // picCondHue
            // 
            this.picCondHue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCondHue.Location = new System.Drawing.Point(6, 55);
            this.picCondHue.Name = "picCondHue";
            this.picCondHue.Size = new System.Drawing.Size(250, 261);
            this.picCondHue.TabIndex = 1;
            this.picCondHue.TabStop = false;
            this.picCondHue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCondHue_MouseDown);
            this.picCondHue.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCondHue_MouseMove);
            this.picCondHue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picCondHue_MouseUp);
            // 
            // trkCondASaturation
            // 
            this.trkCondASaturation.AutoSize = false;
            this.trkCondASaturation.Location = new System.Drawing.Point(6, 424);
            this.trkCondASaturation.Margin = new System.Windows.Forms.Padding(0);
            this.trkCondASaturation.Maximum = 100;
            this.trkCondASaturation.Name = "trkCondASaturation";
            this.trkCondASaturation.Size = new System.Drawing.Size(250, 45);
            this.trkCondASaturation.TabIndex = 6;
            this.trkCondASaturation.TickFrequency = 10;
            this.trkCondASaturation.Value = 100;
            this.trkCondASaturation.Scroll += new System.EventHandler(this.trkCondASaturation_Scroll);
            // 
            // trkCondBSaturation
            // 
            this.trkCondBSaturation.AutoSize = false;
            this.trkCondBSaturation.Location = new System.Drawing.Point(6, 463);
            this.trkCondBSaturation.Margin = new System.Windows.Forms.Padding(0);
            this.trkCondBSaturation.Maximum = 100;
            this.trkCondBSaturation.Name = "trkCondBSaturation";
            this.trkCondBSaturation.Size = new System.Drawing.Size(250, 45);
            this.trkCondBSaturation.TabIndex = 7;
            this.trkCondBSaturation.TickFrequency = 10;
            this.trkCondBSaturation.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trkCondBSaturation.Scroll += new System.EventHandler(this.trkCondBSaturation_Scroll);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDeleteSetting);
            this.groupBox2.Controls.Add(this.btnSaveSetting);
            this.groupBox2.Controls.Add(this.cmbSetting);
            this.groupBox2.Controls.Add(this.chkLightNoChg);
            this.groupBox2.Controls.Add(this.chkSaturationNoChg);
            this.groupBox2.Controls.Add(this.chkHueNoChg);
            this.groupBox2.Controls.Add(this.lblChgLight);
            this.groupBox2.Controls.Add(this.picChgLight);
            this.groupBox2.Controls.Add(this.lblChgSaturation);
            this.groupBox2.Controls.Add(this.trkChgALight);
            this.groupBox2.Controls.Add(this.picChgSaturation);
            this.groupBox2.Controls.Add(this.trkChgBLight);
            this.groupBox2.Controls.Add(this.trkChgASaturation);
            this.groupBox2.Controls.Add(this.trkChgBSaturation);
            this.groupBox2.Controls.Add(this.lblChgHue);
            this.groupBox2.Controls.Add(this.picChgHue);
            this.groupBox2.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox2.Location = new System.Drawing.Point(295, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(263, 614);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "変更色";
            // 
            // btnDeleteSetting
            // 
            this.btnDeleteSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDeleteSetting.Location = new System.Drawing.Point(98, 365);
            this.btnDeleteSetting.Name = "btnDeleteSetting";
            this.btnDeleteSetting.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteSetting.TabIndex = 21;
            this.btnDeleteSetting.Text = "設定を削除";
            this.btnDeleteSetting.UseVisualStyleBackColor = true;
            this.btnDeleteSetting.Click += new System.EventHandler(this.btnDeleteSetting_Click);
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSaveSetting.Location = new System.Drawing.Point(179, 365);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(77, 23);
            this.btnSaveSetting.TabIndex = 6;
            this.btnSaveSetting.Text = "設定を保存";
            this.btnSaveSetting.UseVisualStyleBackColor = true;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // cmbSetting
            // 
            this.cmbSetting.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbSetting.FormattingEnabled = true;
            this.cmbSetting.Location = new System.Drawing.Point(6, 332);
            this.cmbSetting.Name = "cmbSetting";
            this.cmbSetting.Size = new System.Drawing.Size(250, 27);
            this.cmbSetting.TabIndex = 6;
            this.cmbSetting.SelectedIndexChanged += new System.EventHandler(this.cmbSetting_SelectedIndexChanged);
            // 
            // chkLightNoChg
            // 
            this.chkLightNoChg.AutoSize = true;
            this.chkLightNoChg.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkLightNoChg.Location = new System.Drawing.Point(134, 500);
            this.chkLightNoChg.Name = "chkLightNoChg";
            this.chkLightNoChg.Size = new System.Drawing.Size(123, 23);
            this.chkLightNoChg.TabIndex = 20;
            this.chkLightNoChg.Text = "抽出条件と同じ";
            this.chkLightNoChg.UseVisualStyleBackColor = true;
            this.chkLightNoChg.CheckedChanged += new System.EventHandler(this.chkLightNoChg_CheckedChanged);
            // 
            // chkSaturationNoChg
            // 
            this.chkSaturationNoChg.AutoSize = true;
            this.chkSaturationNoChg.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkSaturationNoChg.Location = new System.Drawing.Point(134, 402);
            this.chkSaturationNoChg.Name = "chkSaturationNoChg";
            this.chkSaturationNoChg.Size = new System.Drawing.Size(123, 23);
            this.chkSaturationNoChg.TabIndex = 19;
            this.chkSaturationNoChg.Text = "抽出条件と同じ";
            this.chkSaturationNoChg.UseVisualStyleBackColor = true;
            this.chkSaturationNoChg.CheckedChanged += new System.EventHandler(this.chkSaturationNoChg_CheckedChanged);
            // 
            // chkHueNoChg
            // 
            this.chkHueNoChg.AutoSize = true;
            this.chkHueNoChg.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkHueNoChg.Location = new System.Drawing.Point(134, 32);
            this.chkHueNoChg.Name = "chkHueNoChg";
            this.chkHueNoChg.Size = new System.Drawing.Size(123, 23);
            this.chkHueNoChg.TabIndex = 17;
            this.chkHueNoChg.Text = "抽出条件と同じ";
            this.chkHueNoChg.UseVisualStyleBackColor = true;
            this.chkHueNoChg.CheckedChanged += new System.EventHandler(this.chkHueNoChg_CheckedChanged);
            // 
            // lblChgLight
            // 
            this.lblChgLight.AutoSize = true;
            this.lblChgLight.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblChgLight.Location = new System.Drawing.Point(6, 500);
            this.lblChgLight.Name = "lblChgLight";
            this.lblChgLight.Size = new System.Drawing.Size(77, 19);
            this.lblChgLight.TabIndex = 16;
            this.lblChgLight.Text = "明度(0%)";
            // 
            // picChgLight
            // 
            this.picChgLight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picChgLight.Location = new System.Drawing.Point(18, 547);
            this.picChgLight.Name = "picChgLight";
            this.picChgLight.Size = new System.Drawing.Size(226, 20);
            this.picChgLight.TabIndex = 13;
            this.picChgLight.TabStop = false;
            // 
            // lblChgSaturation
            // 
            this.lblChgSaturation.AutoSize = true;
            this.lblChgSaturation.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblChgSaturation.Location = new System.Drawing.Point(6, 402);
            this.lblChgSaturation.Name = "lblChgSaturation";
            this.lblChgSaturation.Size = new System.Drawing.Size(77, 19);
            this.lblChgSaturation.TabIndex = 12;
            this.lblChgSaturation.Text = "彩度(0%)";
            // 
            // trkChgALight
            // 
            this.trkChgALight.Location = new System.Drawing.Point(6, 522);
            this.trkChgALight.Maximum = 100;
            this.trkChgALight.Name = "trkChgALight";
            this.trkChgALight.Size = new System.Drawing.Size(250, 45);
            this.trkChgALight.TabIndex = 14;
            this.trkChgALight.Tag = "";
            this.trkChgALight.TickFrequency = 10;
            this.trkChgALight.Value = 100;
            this.trkChgALight.Scroll += new System.EventHandler(this.trkChgALight_Scroll);
            // 
            // picChgSaturation
            // 
            this.picChgSaturation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picChgSaturation.Location = new System.Drawing.Point(18, 449);
            this.picChgSaturation.Name = "picChgSaturation";
            this.picChgSaturation.Size = new System.Drawing.Size(226, 20);
            this.picChgSaturation.TabIndex = 9;
            this.picChgSaturation.TabStop = false;
            // 
            // trkChgBLight
            // 
            this.trkChgBLight.Location = new System.Drawing.Point(6, 561);
            this.trkChgBLight.Maximum = 100;
            this.trkChgBLight.Name = "trkChgBLight";
            this.trkChgBLight.Size = new System.Drawing.Size(250, 45);
            this.trkChgBLight.TabIndex = 15;
            this.trkChgBLight.TickFrequency = 10;
            this.trkChgBLight.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trkChgBLight.Scroll += new System.EventHandler(this.trkChgBLight_Scroll);
            // 
            // trkChgASaturation
            // 
            this.trkChgASaturation.Location = new System.Drawing.Point(6, 424);
            this.trkChgASaturation.Maximum = 100;
            this.trkChgASaturation.Name = "trkChgASaturation";
            this.trkChgASaturation.Size = new System.Drawing.Size(250, 45);
            this.trkChgASaturation.TabIndex = 10;
            this.trkChgASaturation.TickFrequency = 10;
            this.trkChgASaturation.Value = 100;
            this.trkChgASaturation.Scroll += new System.EventHandler(this.trkChgASaturation_Scroll);
            // 
            // trkChgBSaturation
            // 
            this.trkChgBSaturation.Location = new System.Drawing.Point(6, 463);
            this.trkChgBSaturation.Maximum = 100;
            this.trkChgBSaturation.Name = "trkChgBSaturation";
            this.trkChgBSaturation.Size = new System.Drawing.Size(250, 45);
            this.trkChgBSaturation.TabIndex = 11;
            this.trkChgBSaturation.TickFrequency = 10;
            this.trkChgBSaturation.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trkChgBSaturation.Scroll += new System.EventHandler(this.trkChgBSaturation_Scroll);
            // 
            // lblChgHue
            // 
            this.lblChgHue.AutoSize = true;
            this.lblChgHue.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblChgHue.Location = new System.Drawing.Point(6, 33);
            this.lblChgHue.Name = "lblChgHue";
            this.lblChgHue.Size = new System.Drawing.Size(39, 19);
            this.lblChgHue.TabIndex = 3;
            this.lblChgHue.Text = "色相";
            // 
            // picChgHue
            // 
            this.picChgHue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picChgHue.Location = new System.Drawing.Point(6, 55);
            this.picChgHue.Name = "picChgHue";
            this.picChgHue.Size = new System.Drawing.Size(250, 261);
            this.picChgHue.TabIndex = 2;
            this.picChgHue.TabStop = false;
            this.picChgHue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picChgHue_MouseDown);
            this.picChgHue.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picChgHue_MouseMove);
            this.picChgHue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picChgHue_MouseUp);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSave.Location = new System.Drawing.Point(564, 28);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(118, 36);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPreview.Location = new System.Drawing.Point(564, 90);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(118, 36);
            this.btnPreview.TabIndex = 3;
            this.btnPreview.Text = "プレビュー";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Enabled = false;
            this.btnUndo.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnUndo.Location = new System.Drawing.Point(564, 153);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(118, 36);
            this.btnUndo.TabIndex = 4;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Enabled = false;
            this.btnRedo.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnRedo.Location = new System.Drawing.Point(564, 195);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(118, 36);
            this.btnRedo.TabIndex = 5;
            this.btnRedo.Text = "Redo";
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // 設定
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 638);
            this.Controls.Add(this.btnRedo);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "設定";
            this.Text = "編集";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCondLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkCondALight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkCondBLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCondSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkCondHueWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCondHue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkCondASaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkCondBSaturation)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picChgLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkChgALight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picChgSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkChgBLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkChgASaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkChgBSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picChgHue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picCondHue;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox picChgHue;
        private System.Windows.Forms.Label lblCondHue;
        private System.Windows.Forms.Label lblChgHue;
        private System.Windows.Forms.Label lblCondHueWidth;
        private System.Windows.Forms.TrackBar trkCondHueWidth;
        private System.Windows.Forms.PictureBox picCondSaturation;
        private System.Windows.Forms.TrackBar trkCondASaturation;
        private System.Windows.Forms.TrackBar trkCondBSaturation;
        private System.Windows.Forms.Label lblCondSaturation;
        private System.Windows.Forms.Label lblChgSaturation;
        private System.Windows.Forms.PictureBox picChgSaturation;
        private System.Windows.Forms.TrackBar trkChgASaturation;
        private System.Windows.Forms.TrackBar trkChgBSaturation;
        private System.Windows.Forms.Label lblCondLight;
        private System.Windows.Forms.PictureBox picCondLight;
        private System.Windows.Forms.TrackBar trkCondALight;
        private System.Windows.Forms.TrackBar trkCondBLight;
        private System.Windows.Forms.Label lblChgLight;
        private System.Windows.Forms.PictureBox picChgLight;
        private System.Windows.Forms.TrackBar trkChgALight;
        private System.Windows.Forms.TrackBar trkChgBLight;
        private System.Windows.Forms.CheckBox chkLightNoChg;
        private System.Windows.Forms.CheckBox chkSaturationNoChg;
        private System.Windows.Forms.CheckBox chkHueNoChg;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPreview;
        public System.Windows.Forms.Button btnUndo;
        public System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ComboBox cmbSetting;
        private System.Windows.Forms.Button btnSaveSetting;
        private System.Windows.Forms.Button btnDeleteSetting;
    }
}