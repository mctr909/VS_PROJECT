using System;
using System.Drawing;
using System.Windows.Forms;

namespace Envelope {
    public class CommonCtrl {
        private Form mForm;

        private PictureBox mBtnClose = new PictureBox();
        private PictureBox mBtnMinimize = new PictureBox();

        private bool mMoving = false;
        private Point mCurPos;

        public int FormCtrlLeft { get; private set; }
        public int FormCtrlBottom { get; private set; }

        public CommonCtrl(Form form) {
            mForm = form;

            ((System.ComponentModel.ISupportInitialize)mBtnClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mBtnMinimize).BeginInit();
            //
            // btnClose
            //
            mBtnClose.BackColor = Color.Transparent;
            mBtnClose.Image = Properties.Resources.close_leave;
            mBtnClose.Location = new Point(237, 4);
            mBtnClose.Name = "btnClose";
            mBtnClose.Size = new Size(30, 30);
            mBtnClose.TabIndex = 7;
            mBtnClose.TabStop = false;
            mBtnClose.Click += new EventHandler((object s, EventArgs e) => {
                mForm.Close();
            });
            mBtnClose.MouseEnter += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnClose.Image) {
                    mBtnClose.Image.Dispose();
                    mBtnClose.Image = null;
                }
                mBtnClose.Image = Properties.Resources.close_hover;
            });
            mBtnClose.MouseLeave += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnClose.Image) {
                    mBtnClose.Image.Dispose();
                    mBtnClose.Image = null;
                }
                mBtnClose.Image = Properties.Resources.close_leave;
            });
            //
            // btnMinimize
            //
            mBtnMinimize.BackColor = Color.Transparent;
            mBtnMinimize.Image = Properties.Resources.minimize_leave;
            mBtnMinimize.Location = new Point(201, 4);
            mBtnMinimize.Name = "btnMinimize";
            mBtnMinimize.Size = new Size(30, 30);
            mBtnMinimize.TabIndex = 10;
            mBtnMinimize.TabStop = false;
            mBtnMinimize.Click += new EventHandler((object s, EventArgs e) => {
                mForm.WindowState = FormWindowState.Minimized;
            });
            mBtnMinimize.MouseEnter += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnMinimize.Image) {
                    mBtnMinimize.Image.Dispose();
                    mBtnMinimize.Image = null;
                }
                mBtnMinimize.Image = Properties.Resources.minimize_hover;
            });
            mBtnMinimize.MouseLeave += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnMinimize.Image) {
                    mBtnMinimize.Image.Dispose();
                    mBtnMinimize.Image = null;
                }
                mBtnMinimize.Image = Properties.Resources.minimize_leave;
            });
            //
            // form
            //
            mForm.BackColor = Colors.FormColor;
            mForm.MouseDown += new MouseEventHandler((object s, MouseEventArgs e) => {
                mMoving = true;
                mCurPos = Cursor.Position;
            });
            mForm.MouseUp += new MouseEventHandler((object s, MouseEventArgs e) => {
                mMoving = false;
            });
            mForm.MouseMove += new MouseEventHandler((object s, MouseEventArgs e) => {
                if (mMoving) {
                    var screen = Screen.FromControl(mForm);
                    var sw = screen.Bounds.Width;
                    var sh = screen.Bounds.Height;
                    var dx = Cursor.Position.X - mCurPos.X;
                    var dy = Cursor.Position.Y - mCurPos.Y;
                    var left = mForm.Left + dx;
                    var top = mForm.Top + dy;
                    if (left < 96 - mForm.Width) {
                        left = 96 - mForm.Width;
                    }
                    if (sw - 96 < left) {
                        left = sw - 96;
                    }
                    if (top < 64 - mForm.Height) {
                        top = 64 - mForm.Height;
                    }
                    if (sh - 64 < top) {
                        top = sh - 64;
                    }
                    mForm.Left = left;
                    mForm.Top = top;
                    mCurPos = Cursor.Position;
                }
            });
            mForm.SizeChanged += new EventHandler((object s, EventArgs e) => {
                mBtnClose.Top = 0;
                mBtnMinimize.Top = 0;
                mBtnClose.Left = mForm.Width - mBtnClose.Width;
                mBtnMinimize.Left = mBtnClose.Left - mBtnMinimize.Width - 4;
                FormCtrlLeft = mBtnClose.Left;
                FormCtrlBottom = mBtnClose.Bottom;
            });
            mForm.Controls.Add(mBtnClose);
            mForm.Controls.Add(mBtnMinimize);
            ((System.ComponentModel.ISupportInitialize)mBtnClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)mBtnMinimize).EndInit();
        }
    }
}
