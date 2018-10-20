﻿namespace DLSeditor
{
    partial class InstForm
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
			this.txtInstName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lstPrgNo = new System.Windows.Forms.ListBox();
			this.lstBankMSB = new System.Windows.Forms.ListBox();
			this.lstBankLSB = new System.Windows.Forms.ListBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.rbDrum = new System.Windows.Forms.RadioButton();
			this.rbNote = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtInstName
			// 
			this.txtInstName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.txtInstName.Location = new System.Drawing.Point(26, 190);
			this.txtInstName.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.txtInstName.Name = "txtInstName";
			this.txtInstName.Size = new System.Drawing.Size(875, 37);
			this.txtInstName.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(20, 154);
			this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(103, 30);
			this.label1.TabIndex = 1;
			this.label1.Text = "音色名";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(20, 264);
			this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(259, 30);
			this.label2.TabIndex = 2;
			this.label2.Text = "プログラムナンバー";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label3.Location = new System.Drawing.Point(563, 264);
			this.label3.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(152, 30);
			this.label3.TabIndex = 3;
			this.label3.Text = "バンクMSB";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label4.Location = new System.Drawing.Point(737, 264);
			this.label4.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(146, 30);
			this.label4.TabIndex = 4;
			this.label4.Text = "バンクLSB";
			// 
			// lstPrgNo
			// 
			this.lstPrgNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstPrgNo.FormattingEnabled = true;
			this.lstPrgNo.ItemHeight = 30;
			this.lstPrgNo.Location = new System.Drawing.Point(26, 300);
			this.lstPrgNo.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.lstPrgNo.Name = "lstPrgNo";
			this.lstPrgNo.Size = new System.Drawing.Size(526, 364);
			this.lstPrgNo.TabIndex = 2;
			this.lstPrgNo.SelectedIndexChanged += new System.EventHandler(this.lstPrgNo_SelectedIndexChanged);
			// 
			// lstBankMSB
			// 
			this.lstBankMSB.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstBankMSB.FormattingEnabled = true;
			this.lstBankMSB.ItemHeight = 30;
			this.lstBankMSB.Location = new System.Drawing.Point(570, 300);
			this.lstBankMSB.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.lstBankMSB.Name = "lstBankMSB";
			this.lstBankMSB.Size = new System.Drawing.Size(156, 364);
			this.lstBankMSB.TabIndex = 3;
			this.lstBankMSB.SelectedIndexChanged += new System.EventHandler(this.lstBankMSB_SelectedIndexChanged);
			// 
			// lstBankLSB
			// 
			this.lstBankLSB.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstBankLSB.FormattingEnabled = true;
			this.lstBankLSB.ItemHeight = 30;
			this.lstBankLSB.Location = new System.Drawing.Point(743, 300);
			this.lstBankLSB.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.lstBankLSB.Name = "lstBankLSB";
			this.lstBankLSB.Size = new System.Drawing.Size(156, 364);
			this.lstBankLSB.TabIndex = 4;
			this.lstBankLSB.SelectedIndexChanged += new System.EventHandler(this.lstBankLSB_SelectedIndexChanged);
			// 
			// btnAdd
			// 
			this.btnAdd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnAdd.Location = new System.Drawing.Point(743, 682);
			this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(163, 66);
			this.btnAdd.TabIndex = 5;
			this.btnAdd.Text = "追加";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// rbDrum
			// 
			this.rbDrum.AutoSize = true;
			this.rbDrum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.rbDrum.Location = new System.Drawing.Point(169, 42);
			this.rbDrum.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.rbDrum.Name = "rbDrum";
			this.rbDrum.Size = new System.Drawing.Size(118, 34);
			this.rbDrum.TabIndex = 1;
			this.rbDrum.TabStop = true;
			this.rbDrum.Text = "ドラム";
			this.rbDrum.UseVisualStyleBackColor = true;
			this.rbDrum.CheckedChanged += new System.EventHandler(this.rbDrum_CheckedChanged);
			// 
			// rbNote
			// 
			this.rbNote.AutoSize = true;
			this.rbNote.Checked = true;
			this.rbNote.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.rbNote.Location = new System.Drawing.Point(33, 42);
			this.rbNote.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.rbNote.Name = "rbNote";
			this.rbNote.Size = new System.Drawing.Size(104, 34);
			this.rbNote.TabIndex = 0;
			this.rbNote.TabStop = true;
			this.rbNote.Text = "音階";
			this.rbNote.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbNote);
			this.groupBox1.Controls.Add(this.rbDrum);
			this.groupBox1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.groupBox1.Location = new System.Drawing.Point(26, 24);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.groupBox1.Size = new System.Drawing.Size(325, 106);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "音色の種類";
			// 
			// InstForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(932, 772);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lstBankLSB);
			this.Controls.Add(this.lstBankMSB);
			this.Controls.Add(this.lstPrgNo);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtInstName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
			this.Name = "InstForm";
			this.Text = "音色追加";
			this.Load += new System.EventHandler(this.InstAddForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInstName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lstPrgNo;
        private System.Windows.Forms.ListBox lstBankMSB;
        private System.Windows.Forms.ListBox lstBankLSB;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.RadioButton rbDrum;
        private System.Windows.Forms.RadioButton rbNote;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}