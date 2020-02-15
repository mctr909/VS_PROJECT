namespace Envelope {
    partial class Comp {
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
            this.btnMinimize = new System.Windows.Forms.PictureBox();
            this.picTabPageCell = new System.Windows.Forms.PictureBox();
            this.picTabPageCol = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.picTabPageRow = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageCell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageRow)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Image = global::Envelope.Properties.Resources.minimize_leave;
            this.btnMinimize.Location = new System.Drawing.Point(201, 4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(30, 30);
            this.btnMinimize.TabIndex = 10;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            this.btnMinimize.MouseEnter += new System.EventHandler(this.btnMinimize_MouseEnter);
            this.btnMinimize.MouseLeave += new System.EventHandler(this.btnMinimize_MouseLeave);
            // 
            // picTabPageCell
            // 
            this.picTabPageCell.BackColor = System.Drawing.Color.Silver;
            this.picTabPageCell.Location = new System.Drawing.Point(54, 38);
            this.picTabPageCell.Name = "picTabPageCell";
            this.picTabPageCell.Size = new System.Drawing.Size(213, 97);
            this.picTabPageCell.TabIndex = 9;
            this.picTabPageCell.TabStop = false;
            this.picTabPageCell.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picTabPageCell_MouseDown);
            this.picTabPageCell.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTabPageCell_MouseMove);
            this.picTabPageCell.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picTabPageCell_MouseUp);
            // 
            // picTabPageCol
            // 
            this.picTabPageCol.BackColor = System.Drawing.Color.Gray;
            this.picTabPageCol.Location = new System.Drawing.Point(54, 141);
            this.picTabPageCol.Name = "picTabPageCol";
            this.picTabPageCol.Size = new System.Drawing.Size(213, 38);
            this.picTabPageCol.TabIndex = 8;
            this.picTabPageCol.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Image = global::Envelope.Properties.Resources.close_leave;
            this.btnClose.Location = new System.Drawing.Point(237, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 7;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            // 
            // picTabPageRow
            // 
            this.picTabPageRow.BackColor = System.Drawing.Color.DarkGray;
            this.picTabPageRow.Location = new System.Drawing.Point(6, 38);
            this.picTabPageRow.Name = "picTabPageRow";
            this.picTabPageRow.Size = new System.Drawing.Size(42, 141);
            this.picTabPageRow.TabIndex = 6;
            this.picTabPageRow.TabStop = false;
            // 
            // Comp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 185);
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.picTabPageCell);
            this.Controls.Add(this.picTabPageCol);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.picTabPageRow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Comp";
            this.Text = "Comp";
            this.Load += new System.EventHandler(this.Comp_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Comp_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Comp_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Comp_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageCell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTabPageRow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox btnMinimize;
        private System.Windows.Forms.PictureBox picTabPageCell;
        private System.Windows.Forms.PictureBox picTabPageCol;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox picTabPageRow;
    }
}