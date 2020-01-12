namespace Test {
    partial class PianoRoll {
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.hsb = new System.Windows.Forms.HScrollBar();
            this.vsb = new System.Windows.Forms.VScrollBar();
            this.picRoll = new System.Windows.Forms.PictureBox();
            this.picKey = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.picMeasure = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tslScale = new System.Windows.Forms.ToolStripLabel();
            this.tslSnap = new System.Windows.Forms.ToolStripLabel();
            this.tslSelect = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picRoll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMeasure)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hsb
            // 
            this.hsb.Location = new System.Drawing.Point(0, 216);
            this.hsb.Name = "hsb";
            this.hsb.Size = new System.Drawing.Size(254, 17);
            this.hsb.TabIndex = 0;
            this.hsb.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsb_Scroll);
            // 
            // vsb
            // 
            this.vsb.Location = new System.Drawing.Point(256, 28);
            this.vsb.Name = "vsb";
            this.vsb.Size = new System.Drawing.Size(17, 185);
            this.vsb.TabIndex = 1;
            this.vsb.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsb_Scroll);
            // 
            // picRoll
            // 
            this.picRoll.Location = new System.Drawing.Point(30, 54);
            this.picRoll.Name = "picRoll";
            this.picRoll.Size = new System.Drawing.Size(223, 159);
            this.picRoll.TabIndex = 2;
            this.picRoll.TabStop = false;
            this.picRoll.Click += new System.EventHandler(this.picRoll_Click);
            this.picRoll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picRoll_MouseDown);
            this.picRoll.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picRoll_MouseMove);
            this.picRoll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picRoll_MouseUp);
            // 
            // picKey
            // 
            this.picKey.Location = new System.Drawing.Point(0, 54);
            this.picKey.Name = "picKey";
            this.picKey.Size = new System.Drawing.Size(24, 159);
            this.picKey.TabIndex = 3;
            this.picKey.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // picMeasure
            // 
            this.picMeasure.Location = new System.Drawing.Point(30, 28);
            this.picMeasure.Name = "picMeasure";
            this.picMeasure.Size = new System.Drawing.Size(224, 20);
            this.picMeasure.TabIndex = 4;
            this.picMeasure.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslScale,
            this.tslSnap,
            this.tslSelect});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(277, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tslScale
            // 
            this.tslScale.Name = "tslScale";
            this.tslScale.Size = new System.Drawing.Size(46, 22);
            this.tslScale.Text = "tslScale";
            // 
            // tslSnap
            // 
            this.tslSnap.Name = "tslSnap";
            this.tslSnap.Size = new System.Drawing.Size(45, 22);
            this.tslSnap.Text = "tslSnap";
            // 
            // tslSelect
            // 
            this.tslSelect.Name = "tslSelect";
            this.tslSelect.Size = new System.Drawing.Size(50, 22);
            this.tslSelect.Text = "tslSelect";
            // 
            // PianoRoll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.picMeasure);
            this.Controls.Add(this.picKey);
            this.Controls.Add(this.picRoll);
            this.Controls.Add(this.vsb);
            this.Controls.Add(this.hsb);
            this.DoubleBuffered = true;
            this.Name = "PianoRoll";
            this.Size = new System.Drawing.Size(277, 240);
            this.Load += new System.EventHandler(this.PianoRoll_Load);
            this.Resize += new System.EventHandler(this.PianoRoll_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picRoll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMeasure)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar hsb;
        private System.Windows.Forms.VScrollBar vsb;
        private System.Windows.Forms.PictureBox picRoll;
        private System.Windows.Forms.PictureBox picKey;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox picMeasure;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel tslScale;
        private System.Windows.Forms.ToolStripLabel tslSnap;
        private System.Windows.Forms.ToolStripLabel tslSelect;
    }
}
