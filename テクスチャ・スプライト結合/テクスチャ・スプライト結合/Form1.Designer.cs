namespace テクスチャ_スプライト結合
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictArea = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtOne = new System.Windows.Forms.RadioButton();
            this.rbtAll = new System.Windows.Forms.RadioButton();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSprit = new System.Windows.Forms.Button();
            this.cmbCellType = new System.Windows.Forms.ComboBox();
            this.btnSaveUpLeft = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictArea)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictArea
            // 
            this.pictArea.BackColor = System.Drawing.Color.Transparent;
            this.pictArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictArea.Location = new System.Drawing.Point(12, 93);
            this.pictArea.Name = "pictArea";
            this.pictArea.Size = new System.Drawing.Size(384, 256);
            this.pictArea.TabIndex = 0;
            this.pictArea.TabStop = false;
            this.pictArea.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictArea_DragDrop);
            this.pictArea.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictArea_DragEnter);
            this.pictArea.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictArea_MouseDoubleClick);
            this.pictArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictArea_MouseDown);
            this.pictArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictArea_MouseUp);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSave.Location = new System.Drawing.Point(12, 51);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(170, 33);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtOne);
            this.groupBox1.Controls.Add(this.rbtAll);
            this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(188, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(132, 43);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "取込方法";
            // 
            // rbtOne
            // 
            this.rbtOne.AutoSize = true;
            this.rbtOne.Checked = true;
            this.rbtOne.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rbtOne.Location = new System.Drawing.Point(6, 14);
            this.rbtOne.Name = "rbtOne";
            this.rbtOne.Size = new System.Drawing.Size(55, 19);
            this.rbtOne.TabIndex = 8;
            this.rbtOne.TabStop = true;
            this.rbtOne.Text = "単体";
            this.rbtOne.UseVisualStyleBackColor = true;
            // 
            // rbtAll
            // 
            this.rbtAll.AutoSize = true;
            this.rbtAll.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rbtAll.Location = new System.Drawing.Point(67, 14);
            this.rbtAll.Name = "rbtAll";
            this.rbtAll.Size = new System.Drawing.Size(55, 19);
            this.rbtAll.TabIndex = 9;
            this.rbtAll.Text = "全体";
            this.rbtAll.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClear.Location = new System.Drawing.Point(326, 18);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(70, 33);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "クリア";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSprit
            // 
            this.btnSprit.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSprit.Location = new System.Drawing.Point(100, 12);
            this.btnSprit.Name = "btnSprit";
            this.btnSprit.Size = new System.Drawing.Size(82, 33);
            this.btnSprit.TabIndex = 15;
            this.btnSprit.Text = "分割";
            this.btnSprit.UseVisualStyleBackColor = true;
            this.btnSprit.Click += new System.EventHandler(this.btnSprit_Click);
            // 
            // cmbCellType
            // 
            this.cmbCellType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCellType.FormattingEnabled = true;
            this.cmbCellType.Location = new System.Drawing.Point(188, 64);
            this.cmbCellType.Name = "cmbCellType";
            this.cmbCellType.Size = new System.Drawing.Size(208, 20);
            this.cmbCellType.TabIndex = 16;
            this.cmbCellType.SelectedIndexChanged += new System.EventHandler(this.cmbCellType_SelectedIndexChanged);
            // 
            // btnSaveUpLeft
            // 
            this.btnSaveUpLeft.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSaveUpLeft.Location = new System.Drawing.Point(12, 12);
            this.btnSaveUpLeft.Name = "btnSaveUpLeft";
            this.btnSaveUpLeft.Size = new System.Drawing.Size(82, 33);
            this.btnSaveUpLeft.TabIndex = 17;
            this.btnSaveUpLeft.Text = "左上";
            this.btnSaveUpLeft.UseVisualStyleBackColor = true;
            this.btnSaveUpLeft.Click += new System.EventHandler(this.btnSaveUpLeft_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.btnSaveUpLeft);
            this.Controls.Add(this.cmbCellType);
            this.Controls.Add(this.btnSprit);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pictArea);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "Form1";
            this.Text = "テクスチャ・スプライト結合";
            ((System.ComponentModel.ISupportInitialize)(this.pictArea)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictArea;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RadioButton rbtAll;
        private System.Windows.Forms.RadioButton rbtOne;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSprit;
        private System.Windows.Forms.ComboBox cmbCellType;
        private System.Windows.Forms.Button btnSaveUpLeft;
    }
}

