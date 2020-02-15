using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Envelope {
    public partial class Comp : Form {
        private static int TableFooterHeight = 30;
        private static int TableLeftFrameWidth = 50;
        private static int AmpDispUnit = 30;

        private static Font FontBold = new Font("Segoe UI", 11.0f, FontStyle.Bold);
        private static Font FontSmall = new Font("Segoe UI", 9.0f, FontStyle.Regular);

        private static StringFormat MiddleCenter = new StringFormat {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        private static StringFormat BottomRight = new StringFormat {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Far
        };
        private static StringFormat MiddleLeft = new StringFormat {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };

        private bool mMoving = false;
        private bool mScrollThreshold = false;
        private bool mScrollGain = false;
        private Point mCurPos;

        private double mThreshold = -6.0;
        private double mGain = 0.0;

        private Bitmap mBmpTabPageRow;
        private Bitmap mBmpTabPageCol;
        private Bitmap mBmpTabPageCell;
        private Graphics mGTabPageRow;
        private Graphics mGTabPageCol;
        private Graphics mGTabPageCell;

        public Comp() {
            InitializeComponent();
        }

        private void Comp_Load(object sender, EventArgs e) {
            BackColor = Colors.FormColor;
            drawBackground();
            draw();
        }

        private void Comp_MouseDown(object sender, MouseEventArgs e) {
            mMoving = true;
            mCurPos = Cursor.Position;
        }

        private void Comp_MouseUp(object sender, MouseEventArgs e) {
            mMoving = false;
        }

        private void Comp_MouseMove(object sender, MouseEventArgs e) {
            if (mMoving) {
                var s = Screen.FromControl(this);
                var sw = s.Bounds.Width;
                var sh = s.Bounds.Height;
                var dx = Cursor.Position.X - mCurPos.X;
                var dy = Cursor.Position.Y - mCurPos.Y;
                var left = Left + dx;
                var top = Top + dy;
                if (left < 96 - Width) {
                    left = 96 - Width;
                }
                if (sw - 96 < left) {
                    left = sw - 96;
                }
                if (top < 64 - Height) {
                    top = 64 - Height;
                }
                if (sh - 64 < top) {
                    top = sh - 64;
                }
                Left = left;
                Top = top;
                mCurPos = Cursor.Position;
            }
        }

        private void btnClose_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnClose_MouseEnter(object sender, EventArgs e) {
            btnClose.Image = Properties.Resources.close_hover;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e) {
            btnClose.Image = Properties.Resources.close_leave;
        }

        private void btnMinimize_Click(object sender, EventArgs e) {
            WindowState = FormWindowState.Minimized;
        }

        private void btnMinimize_MouseEnter(object sender, EventArgs e) {
            btnMinimize.Image = Properties.Resources.minimize_hover;
        }

        private void btnMinimize_MouseLeave(object sender, EventArgs e) {
            btnMinimize.Image = Properties.Resources.minimize_leave;
        }

        private void picTabPageCell_MouseDown(object sender, MouseEventArgs e) {
            var pos = picTabPageCell.PointToClient(Cursor.Position);
            var posX = pos.X * 6.0 / AmpDispUnit - 60;
            var posY = (picTabPageCell.Height - pos.Y) * 6.0 / AmpDispUnit - 60;

            if (mThreshold <= posX + 3 && posX - 3 <= mThreshold &&
                mThreshold <= posY + 3 && posY - 3 <= mThreshold) {
                mScrollThreshold = true;
                Cursor.Current = Cursors.SizeNESW;
            }

            if (pos.X <= AmpDispUnit * 14 && AmpDispUnit * 10 <= pos.X) {
                mScrollGain = true;
                Cursor.Current = Cursors.HSplit;
            }
        }

        private void picTabPageCell_MouseUp(object sender, MouseEventArgs e) {
            mScrollThreshold = false;
            mScrollGain = false;
        }

        private void picTabPageCell_MouseMove(object sender, MouseEventArgs e) {
            var pos = picTabPageCell.PointToClient(Cursor.Position);
            var posX = pos.X * 6.0 / AmpDispUnit - 60;
            var posY = (picTabPageCell.Height - pos.Y) * 6.0 / AmpDispUnit - 60;

            if (mScrollThreshold) {
                mThreshold = posX;
                draw();
                return;
            }
            if (mScrollGain) {
                mGain = posY;
                draw();
                return;
            }
        }

        private void draw() {
            var bmp = new Bitmap(picTabPageCell.Width, picTabPageCell.Height);
            var g = Graphics.FromImage(bmp);

            if (0 < mThreshold) {
                mThreshold = 0;
            }
            if (mThreshold < -60) {
                mThreshold = -60;
            }

            if (mGain < mThreshold) {
                mGain = mThreshold;
            }
            if (0.0 < mGain) {
                mGain = 0.0;
            }

            
            var ratio = -mThreshold / (mGain - mThreshold);
            if (0 == mThreshold) {
                ratio = 1;
            }

            var pThresholdX = (int)((60 + mThreshold) * AmpDispUnit / 6.0);
            var pThresholdY = bmp.Height - pThresholdX;
            var pGainX = (int)((60 + mGain) * AmpDispUnit / 6.0);
            var pGainY = bmp.Height - pGainX;
            var pGainYT = pGainY - (int)(AmpDispUnit * 4 / ratio);

            var psThresholdY = pThresholdY;
            var psGainY = pGainY;
            if (bmp.Height < psThresholdY + 20) {
                psThresholdY = bmp.Height - 20;
            }
            if (bmp.Height < psGainY + 20) {
                psGainY = bmp.Height - 20;
            }

            g.DrawString(ratio.ToString("0.0"),
                FontBold,
                Colors.BFontTable,
                new RectangleF(AmpDispUnit * 10, AmpDispUnit, AmpDispUnit * 4, AmpDispUnit),
                MiddleCenter);

            g.DrawLine(Colors.PGraphLine, 0, bmp.Height, pThresholdX, pThresholdY);
            g.DrawLine(Colors.PGraphLine, pThresholdX, pThresholdY, AmpDispUnit * 10, pGainY);
            g.DrawLine(Colors.PGraphLineAlpha, AmpDispUnit * 10, pGainY, AmpDispUnit * 14, pGainYT);

            g.FillPie(Colors.BTableCell, pThresholdX - 4, pThresholdY - 4, 8, 8, 0, 360);
            g.DrawArc(Colors.PTableBorderLight, pThresholdX - 4, pThresholdY - 4, 8, 8, 0, 360);
            g.FillPie(Colors.BTableCell, AmpDispUnit * 10 - 4, pGainY - 4, 8, 8, 0, 360);
            g.DrawArc(Colors.PTableBorderLight, AmpDispUnit * 10 - 4, pGainY - 4, 8, 8, 0, 360);

            g.DrawString(mThreshold.ToString("0.0db"), FontSmall, Colors.BFontTable, pThresholdX + 3, psThresholdY);
            g.DrawString(mGain.ToString("0.0db"), FontSmall, Colors.BFontTable, AmpDispUnit * 10 + 3, psGainY);

            if (null != picTabPageCell.Image) {
                picTabPageCell.Image.Dispose();
                picTabPageCell.Image = null;
            }
            picTabPageCell.Image = bmp;
        }

        private void drawBackground() {
            releaseImage();

            mBmpTabPageCol = new Bitmap(AmpDispUnit * 14, TableFooterHeight);
            mGTabPageCol = Graphics.FromImage(mBmpTabPageCol);
            mBmpTabPageRow = new Bitmap(TableLeftFrameWidth, AmpDispUnit * 12);
            mGTabPageRow = Graphics.FromImage(mBmpTabPageRow);
            mBmpTabPageCell = new Bitmap(AmpDispUnit * 14, AmpDispUnit * 12);
            mGTabPageCell = Graphics.FromImage(mBmpTabPageCell);

            mGTabPageCol.Clear(Colors.TableHeader);
            mGTabPageCol.FillRectangle(Colors.BTableCell,
                0, TableFooterHeight,
                mBmpTabPageCol.Width, TableFooterHeight);
            mGTabPageRow.Clear(Colors.TableHeader);
            mGTabPageCell.Clear(Colors.TableCell);
            mGTabPageCell.FillRectangle(Colors.BTableHeader,
                mBmpTabPageCell.Width, 0,
                mBmpTabPageCell.Width, mBmpTabPageCell.Height);

            mGTabPageCell.DrawString("Ratio",
                FontBold,
                Colors.BFontTable,
                new RectangleF(AmpDispUnit * 10, 0, AmpDispUnit * 4, AmpDispUnit),
                MiddleCenter);

            mGTabPageCol.DrawLine(Colors.PTableBorderBold,
                0, 1,
                mBmpTabPageCol.Width, 1);
            mGTabPageRow.DrawLine(Colors.PTableBorderBold,
                TableLeftFrameWidth - 1, 0,
                TableLeftFrameWidth - 1, mBmpTabPageRow.Height);

            mGTabPageRow.DrawLine(Colors.PTableBorderLight,
                0, AmpDispUnit * 2,
                mBmpTabPageRow.Width, AmpDispUnit * 2);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 4,
                mBmpTabPageRow.Width, AmpDispUnit * 4);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 6,
                mBmpTabPageRow.Width, AmpDispUnit * 6);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 8,
                mBmpTabPageRow.Width, AmpDispUnit * 8);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 10,
                mBmpTabPageRow.Width, AmpDispUnit * 10);

            mGTabPageRow.DrawString("0db", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);
            mGTabPageRow.DrawString("-12db ", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 3,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);
            mGTabPageRow.DrawString("-24db ", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 5,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);
            mGTabPageRow.DrawString("-36db ", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 7,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);
            mGTabPageRow.DrawString("-48db ", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 9,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);
            mGTabPageRow.DrawString("-60db ", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 11,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);

            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit,
                mBmpTabPageCell.Width, AmpDispUnit);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 3,
                mBmpTabPageCell.Width, AmpDispUnit * 3);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 5,
                mBmpTabPageCell.Width, AmpDispUnit * 5);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 7,
                mBmpTabPageCell.Width, AmpDispUnit * 7);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 9,
                mBmpTabPageCell.Width, AmpDispUnit * 9);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 11,
                mBmpTabPageCell.Width, AmpDispUnit * 11);

            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 4,
                mBmpTabPageCell.Width, AmpDispUnit * 4);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 6,
                mBmpTabPageCell.Width, AmpDispUnit * 6);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 8,
                mBmpTabPageCell.Width, AmpDispUnit * 8);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 10,
                mBmpTabPageCell.Width, AmpDispUnit * 10);

            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit, 0,
                AmpDispUnit, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 3, 0,
                AmpDispUnit * 3, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 5, 0,
                AmpDispUnit * 5, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 7, 0,
                AmpDispUnit * 7, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 9, 0,
                AmpDispUnit * 9, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 11, AmpDispUnit * 2,
                AmpDispUnit * 11, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 13, AmpDispUnit * 2,
                AmpDispUnit * 13, mBmpTabPageCell.Height);

            mGTabPageCell.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 2, 0,
                AmpDispUnit * 2, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 4, 0,
                AmpDispUnit * 4, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 6, 0,
                AmpDispUnit * 6, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 8, 0,
                AmpDispUnit * 8, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 12, AmpDispUnit * 2,
                AmpDispUnit * 12, mBmpTabPageCell.Height);

            mGTabPageCell.DrawLine(Colors.PTableBorderLight,
                0, AmpDispUnit * 2,
                mBmpTabPageCell.Width, AmpDispUnit * 2);
            mGTabPageCell.DrawLine(Colors.PTableBorderLight,
                AmpDispUnit * 10, 0,
                AmpDispUnit * 10, mBmpTabPageCell.Height);

            mGTabPageCol.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 2, 0,
                AmpDispUnit * 2, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 4, 0,
                AmpDispUnit * 4, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 6, 0,
                AmpDispUnit * 6, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 8, 0,
                AmpDispUnit * 8, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorderLight,
                AmpDispUnit * 10, 0,
                AmpDispUnit * 10, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 12, 0,
                AmpDispUnit * 12, mBmpTabPageCol.Height);

            mGTabPageCol.DrawString("-60db", FontSmall, Colors.BFontTable,
                new RectangleF(0, 0,
                AmpDispUnit * 2, mBmpTabPageCol.Height), MiddleLeft);
            mGTabPageCol.DrawString("-48db ", FontSmall, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 2, 0,
                AmpDispUnit * 2, mBmpTabPageCol.Height), MiddleLeft);
            mGTabPageCol.DrawString("-36db ", FontSmall, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 4, 0,
                AmpDispUnit * 2, mBmpTabPageCol.Height), MiddleLeft);
            mGTabPageCol.DrawString("-24db ", FontSmall, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 6, 0,
                AmpDispUnit * 2, mBmpTabPageCol.Height), MiddleLeft);
            mGTabPageCol.DrawString("-12db ", FontSmall, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 8, 0,
                AmpDispUnit * 2, mBmpTabPageCol.Height), MiddleLeft);
            mGTabPageCol.DrawString("0db ", FontSmall, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 10, 0,
                AmpDispUnit * 2, mBmpTabPageCol.Height), MiddleLeft);
            mGTabPageCol.DrawString("+12db ", FontSmall, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 12, 0,
                AmpDispUnit * 2, mBmpTabPageCol.Height), MiddleLeft);

            setImage();
        }

        private void setImage() {
            mGTabPageRow.DrawLine(Colors.PTabBorderBold, 0, 1, mBmpTabPageRow.Width, 1);
            mGTabPageRow.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpTabPageRow.Height);
            mGTabPageRow.DrawLine(Colors.PTabBorderBold,
                0, mBmpTabPageRow.Height - 1,
                mBmpTabPageRow.Width, mBmpTabPageRow.Height - 1);
            mGTabPageCol.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpTabPageCol.Height - 1);
            mGTabPageCol.DrawLine(Colors.PTabBorderBold,
                0, mBmpTabPageCol.Height - 1,
                mBmpTabPageCol.Width - 1, mBmpTabPageCol.Height - 1);
            mGTabPageCol.DrawLine(Colors.PTabBorderBold,
                mBmpTabPageCol.Width - 1, 0,
                mBmpTabPageCol.Width - 1, mBmpTabPageCol.Height);
            mGTabPageCell.DrawLine(Colors.PTabBorderBold,
                mBmpTabPageCell.Width - 1, 0,
                mBmpTabPageCell.Width - 1, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTabBorderBold,
                0, 1,
                mBmpTabPageCell.Width - 1, 1);

            Width = mBmpTabPageRow.Width + mBmpTabPageCell.Width;
            Height = btnMinimize.Height + mBmpTabPageCol.Height + mBmpTabPageCell.Height;
            picTabPageCol.Width = mBmpTabPageCol.Width;
            picTabPageCol.Height = mBmpTabPageCol.Height;
            picTabPageRow.Width = mBmpTabPageRow.Width;
            picTabPageRow.Height = mBmpTabPageRow.Height;
            picTabPageCell.Width = mBmpTabPageCell.Width;
            picTabPageCell.Height = mBmpTabPageCell.Height;
            picTabPageCol.BackgroundImage = mBmpTabPageCol;
            picTabPageRow.BackgroundImage = mBmpTabPageRow;
            picTabPageCell.BackgroundImage = mBmpTabPageCell;

            picTabPageRow.Left = 0;
            picTabPageCell.Left = picTabPageRow.Right;
            picTabPageCol.Left = picTabPageRow.Right;
            btnClose.Left = picTabPageCell.Right - btnClose.Width;
            btnMinimize.Left = btnClose.Left - btnMinimize.Width - 4;
            btnClose.Top = 0;
            btnMinimize.Top = 0;

            picTabPageRow.Top = btnMinimize.Bottom;
            picTabPageCell.Top = btnMinimize.Bottom;
            picTabPageCol.Top = picTabPageCell.Bottom;
        }

        private void releaseImage() {
            if (null != mBmpTabPageCol) {
                mBmpTabPageCol.Dispose();
                mBmpTabPageCol = null;
                mGTabPageCol.Dispose();
                mGTabPageCol = null;
            }
            if (null != mBmpTabPageRow) {
                mBmpTabPageRow.Dispose();
                mBmpTabPageRow = null;
                mGTabPageRow.Dispose();
                mGTabPageRow = null;
            }
            if (null != mBmpTabPageCell) {
                mBmpTabPageCell.Dispose();
                mBmpTabPageCell = null;
                mGTabPageCell.Dispose();
                mGTabPageCell = null;
            }

            if (null != picTabPageCol.BackgroundImage) {
                picTabPageCol.BackgroundImage.Dispose();
                picTabPageCol.BackgroundImage = null;
            }
            if (null != picTabPageRow.BackgroundImage) {
                picTabPageRow.BackgroundImage.Dispose();
                picTabPageRow.BackgroundImage = null;
            }
            if (null != picTabPageCell.BackgroundImage) {
                picTabPageCell.BackgroundImage.Dispose();
                picTabPageCell.BackgroundImage = null;
            }
        }
    }
}
