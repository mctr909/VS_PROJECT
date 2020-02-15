namespace Envelope {
    partial class Envelope {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.picTab = new System.Windows.Forms.PictureBox();
            this.picTabPageRow = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.picTabPageCol = new System.Windows.Forms.PictureBox();
            this.picTabPageCell = new System.Windows.Forms.PictureBox();
            this.btnMinimize = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picTab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageCell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimize)).BeginInit();
            this.SuspendLayout();
            // 
            // picTab
            // 
            this.picTab.BackColor = System.Drawing.Color.Transparent;
            this.picTab.Location = new System.Drawing.Point(12, 3);
            this.picTab.Name = "picTab";
            this.picTab.Size = new System.Drawing.Size(175, 28);
            this.picTab.TabIndex = 0;
            this.picTab.TabStop = false;
            // 
            // picTabPageRow
            // 
            this.picTabPageRow.BackColor = System.Drawing.Color.DarkGray;
            this.picTabPageRow.Location = new System.Drawing.Point(12, 37);
            this.picTabPageRow.Name = "picTabPageRow";
            this.picTabPageRow.Size = new System.Drawing.Size(42, 141);
            this.picTabPageRow.TabIndex = 1;
            this.picTabPageRow.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Image = global::Envelope.Properties.Resources.close_leave;
            this.btnClose.Location = new System.Drawing.Point(243, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            // 
            // picTabPageCol
            // 
            this.picTabPageCol.BackColor = System.Drawing.Color.Gray;
            this.picTabPageCol.Location = new System.Drawing.Point(60, 37);
            this.picTabPageCol.Name = "picTabPageCol";
            this.picTabPageCol.Size = new System.Drawing.Size(213, 38);
            this.picTabPageCol.TabIndex = 3;
            this.picTabPageCol.TabStop = false;
            this.picTabPageCol.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picTabPageCol_MouseDown);
            this.picTabPageCol.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTabPageCol_MouseMove);
            this.picTabPageCol.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picTabPageCol_MouseUp);
            // 
            // picTabPageCell
            // 
            this.picTabPageCell.BackColor = System.Drawing.Color.Silver;
            this.picTabPageCell.Location = new System.Drawing.Point(60, 81);
            this.picTabPageCell.Name = "picTabPageCell";
            this.picTabPageCell.Size = new System.Drawing.Size(213, 97);
            this.picTabPageCell.TabIndex = 4;
            this.picTabPageCell.TabStop = false;
            this.picTabPageCell.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picTabPageCell_MouseDown);
            this.picTabPageCell.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTabPageCell_MouseMove);
            this.picTabPageCell.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picTabPageCell_MouseUp);
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Image = global::Envelope.Properties.Resources.minimize_leave;
            this.btnMinimize.Location = new System.Drawing.Point(207, 3);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(30, 30);
            this.btnMinimize.TabIndex = 5;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            this.btnMinimize.MouseEnter += new System.EventHandler(this.btnMinimize_MouseEnter);
            this.btnMinimize.MouseLeave += new System.EventHandler(this.btnMinimize_MouseLeave);
            // 
            // Envelope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(81)))), ((int)(((byte)(144)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(285, 190);
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.picTabPageCell);
            this.Controls.Add(this.picTabPageCol);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.picTabPageRow);
            this.Controls.Add(this.picTab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Envelope";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Envelope_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Envelope_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Envelope_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Envelope_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.picTab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageCell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picTab;
        private System.Windows.Forms.PictureBox picTabPageRow;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox picTabPageCol;
        private System.Windows.Forms.PictureBox picTabPageCell;
        private System.Windows.Forms.PictureBox btnMinimize;
    }
}

