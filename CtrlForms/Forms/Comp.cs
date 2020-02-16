using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Envelope {
    public partial class Comp : Form {
        private static int TableFooterHeight = 30;
        private static int TableLeftFrameWidth = 50;
        private static int AmpDispUnit = 30;

        private bool mScrollThreshold = false;
        private bool mScrollGain = false;

        private double mThreshold = -6.0;
        private double mGain = 0.0;

        private CommonCtrl mCommonCtrl;

        private Bitmap mBmpRow;
        private Bitmap mBmpCol;
        private Bitmap mBmpCell;
        private Bitmap mBmpValue;
        private Graphics mGRow;
        private Graphics mGCol;
        private Graphics mGCell;
        private Graphics mGValue;

        public Comp() {
            InitializeComponent();
        }

        private void Comp_Load(object sender, EventArgs e) {
            mCommonCtrl = new CommonCtrl(this);
            drawBackground();
            draw();
        }

        private void picCell_MouseDown(object sender, MouseEventArgs e) {
            var pos = picCell.PointToClient(Cursor.Position);
            var posX = pos.X * 6.0 / AmpDispUnit - 60;
            var posY = (picCell.Height - pos.Y) * 6.0 / AmpDispUnit - 60;

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

        private void picCell_MouseUp(object sender, MouseEventArgs e) {
            mScrollThreshold = false;
            mScrollGain = false;
        }

        private void picCell_MouseMove(object sender, MouseEventArgs e) {
            var pos = picCell.PointToClient(Cursor.Position);
            var posX = pos.X * 6.0 / AmpDispUnit - 60;
            var posY = (picCell.Height - pos.Y) * 6.0 / AmpDispUnit - 60;

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
            if (mThreshold < -60) {
                mThreshold = -60;
            }
            if (0 < mThreshold) {
                mThreshold = 0;
            }

            if (mGain < -60.0) {
                mGain = -60.0;
            }
            if (0.0 < mGain) {
                mGain = 0.0;
            }
            if (mGain < mThreshold) {
                mGain = mThreshold;
            }

            var ratio = -mThreshold / (mGain - mThreshold);
            if (0 == mThreshold) {
                ratio = 1;
            }

            var pThresholdX = (int)((60 + mThreshold) * AmpDispUnit / 6.0);
            var pThresholdY = picCell.Height - pThresholdX;
            var pGainX = (int)((60 + mGain) * AmpDispUnit / 6.0);
            var pGainY = picCell.Height - pGainX;
            var pGainYT = pGainY - (int)(AmpDispUnit * 4 / ratio);

            var psThresholdY = pThresholdY;
            var psGainY = pGainY;
            if (picCell.Height < psThresholdY + 20) {
                psThresholdY = picCell.Height - 20;
            }
            if (picCell.Height < psGainY + 20) {
                psGainY = picCell.Height - 20;
            }

            if (null != picCell.Image) {
                picCell.Image.Dispose();
                picCell.Image = null;
            }
            if (null != mBmpValue) {
                mBmpValue.Dispose();
                mBmpValue = null;
                mGValue.Dispose();
                mGValue = null;
            }
            mBmpValue = new Bitmap(picCell.Width, picCell.Height);
            mGValue = Graphics.FromImage(mBmpValue);

            mGValue.DrawString(ratio.ToString("0.0"),
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(AmpDispUnit * 10, AmpDispUnit, AmpDispUnit * 4, AmpDispUnit),
                Fonts.AlignMC);

            mGValue.DrawLine(Colors.PGraphLine, 0, picCell.Height, pThresholdX, pThresholdY);
            mGValue.DrawLine(Colors.PGraphLine, pThresholdX, pThresholdY, AmpDispUnit * 10, pGainY);
            mGValue.DrawLine(Colors.PGraphLineAlpha, AmpDispUnit * 10, pGainY, AmpDispUnit * 14, pGainYT);

            mGValue.FillPie(Colors.BTableCell, pThresholdX - 4, pThresholdY - 4, 8, 8, 0, 360);
            mGValue.DrawArc(Colors.PTableBorderLight, pThresholdX - 4, pThresholdY - 4, 8, 8, 0, 360);
            mGValue.FillPie(Colors.BTableCell, AmpDispUnit * 10 - 4, pGainY - 4, 8, 8, 0, 360);
            mGValue.DrawArc(Colors.PTableBorderLight, AmpDispUnit * 10 - 4, pGainY - 4, 8, 8, 0, 360);

            mGValue.DrawString(mThreshold.ToString("0.0db"), Fonts.Small, Colors.BFontTable, pThresholdX + 3, psThresholdY);
            mGValue.DrawString(mGain.ToString("0.0db"), Fonts.Small, Colors.BFontTable, AmpDispUnit * 10 + 3, psGainY);

            picCell.Image = mBmpValue;
        }

        private void drawBackground() {
            releaseImage();

            mBmpCol = new Bitmap(AmpDispUnit * 14, TableFooterHeight);
            mGCol = Graphics.FromImage(mBmpCol);
            mBmpRow = new Bitmap(TableLeftFrameWidth, AmpDispUnit * 12);
            mGRow = Graphics.FromImage(mBmpRow);
            mBmpCell = new Bitmap(AmpDispUnit * 14, AmpDispUnit * 12);
            mGCell = Graphics.FromImage(mBmpCell);

            mGCol.Clear(Colors.TableHeader);
            mGCol.FillRectangle(Colors.BTableCell,
                0, TableFooterHeight,
                mBmpCol.Width, TableFooterHeight);
            mGRow.Clear(Colors.TableHeader);
            mGCell.Clear(Colors.TableCell);
            mGCell.FillRectangle(Colors.BTableHeader,
                mBmpCell.Width, 0,
                mBmpCell.Width, mBmpCell.Height);

            mGCell.DrawString("Ratio",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(AmpDispUnit * 10, 0, AmpDispUnit * 4, AmpDispUnit),
                Fonts.AlignMC);

            mGCol.DrawLine(Colors.PTableBorderBold,
                0, 1,
                mBmpCol.Width, 1);
            mGRow.DrawLine(Colors.PTableBorderBold,
                TableLeftFrameWidth - 1, 0,
                TableLeftFrameWidth - 1, mBmpRow.Height);

            mGRow.DrawLine(Colors.PTableBorderLight,
                0, AmpDispUnit * 2,
                mBmpRow.Width, AmpDispUnit * 2);
            mGRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 4,
                mBmpRow.Width, AmpDispUnit * 4);
            mGRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 6,
                mBmpRow.Width, AmpDispUnit * 6);
            mGRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 8,
                mBmpRow.Width, AmpDispUnit * 8);
            mGRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 10,
                mBmpRow.Width, AmpDispUnit * 10);

            mGRow.DrawString("0db", Fonts.Small, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit,
                mBmpRow.Width, AmpDispUnit), Fonts.AlignBR);
            mGRow.DrawString("-12db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 3,
                mBmpRow.Width, AmpDispUnit), Fonts.AlignBR);
            mGRow.DrawString("-24db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 5,
                mBmpRow.Width, AmpDispUnit), Fonts.AlignBR);
            mGRow.DrawString("-36db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 7,
                mBmpRow.Width, AmpDispUnit), Fonts.AlignBR);
            mGRow.DrawString("-48db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 9,
                mBmpRow.Width, AmpDispUnit), Fonts.AlignBR);
            mGRow.DrawString("-60db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 11,
                mBmpRow.Width, AmpDispUnit), Fonts.AlignBR);

            mGCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit,
                mBmpCell.Width, AmpDispUnit);
            mGCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 3,
                mBmpCell.Width, AmpDispUnit * 3);
            mGCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 5,
                mBmpCell.Width, AmpDispUnit * 5);
            mGCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 7,
                mBmpCell.Width, AmpDispUnit * 7);
            mGCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 9,
                mBmpCell.Width, AmpDispUnit * 9);
            mGCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 11,
                mBmpCell.Width, AmpDispUnit * 11);

            mGCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 4,
                mBmpCell.Width, AmpDispUnit * 4);
            mGCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 6,
                mBmpCell.Width, AmpDispUnit * 6);
            mGCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 8,
                mBmpCell.Width, AmpDispUnit * 8);
            mGCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 10,
                mBmpCell.Width, AmpDispUnit * 10);

            mGCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit, 0,
                AmpDispUnit, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 3, 0,
                AmpDispUnit * 3, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 5, 0,
                AmpDispUnit * 5, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 7, 0,
                AmpDispUnit * 7, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 9, 0,
                AmpDispUnit * 9, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 11, AmpDispUnit * 2,
                AmpDispUnit * 11, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorderDark,
                AmpDispUnit * 13, AmpDispUnit * 2,
                AmpDispUnit * 13, mBmpCell.Height);

            mGCell.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 2, 0,
                AmpDispUnit * 2, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 4, 0,
                AmpDispUnit * 4, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 6, 0,
                AmpDispUnit * 6, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 8, 0,
                AmpDispUnit * 8, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 12, AmpDispUnit * 2,
                AmpDispUnit * 12, mBmpCell.Height);

            mGCell.DrawLine(Colors.PTableBorderLight,
                0, AmpDispUnit * 2,
                mBmpCell.Width, AmpDispUnit * 2);
            mGCell.DrawLine(Colors.PTableBorderLight,
                AmpDispUnit * 10, 0,
                AmpDispUnit * 10, mBmpCell.Height);

            mGCol.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 2, 0,
                AmpDispUnit * 2, mBmpCol.Height);
            mGCol.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 4, 0,
                AmpDispUnit * 4, mBmpCol.Height);
            mGCol.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 6, 0,
                AmpDispUnit * 6, mBmpCol.Height);
            mGCol.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 8, 0,
                AmpDispUnit * 8, mBmpCol.Height);
            mGCol.DrawLine(Colors.PTableBorderLight,
                AmpDispUnit * 10, 0,
                AmpDispUnit * 10, mBmpCol.Height);
            mGCol.DrawLine(Colors.PTableBorder,
                AmpDispUnit * 12, 0,
                AmpDispUnit * 12, mBmpCol.Height);

            mGCol.DrawString("-60db", Fonts.Small, Colors.BFontTable,
                new RectangleF(0, 0,
                AmpDispUnit * 2, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("-48db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 2, 0,
                AmpDispUnit * 2, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("-36db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 4, 0,
                AmpDispUnit * 2, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("-24db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 6, 0,
                AmpDispUnit * 2, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("-12db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 8, 0,
                AmpDispUnit * 2, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("0db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 10, 0,
                AmpDispUnit * 2, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("+12db ", Fonts.Small, Colors.BFontTable,
                new RectangleF(AmpDispUnit * 12, 0,
                AmpDispUnit * 2, mBmpCol.Height), Fonts.AlignML);

            setImage();
        }

        private void setImage() {
            mGRow.DrawLine(Colors.PTabBorderBold, 0, 1, mBmpRow.Width, 1);
            mGRow.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpRow.Height);
            mGRow.DrawLine(Colors.PTabBorderBold,
                0, mBmpRow.Height - 1,
                mBmpRow.Width, mBmpRow.Height - 1);
            mGCol.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpCol.Height - 1);
            mGCol.DrawLine(Colors.PTabBorderBold,
                0, mBmpCol.Height - 1,
                mBmpCol.Width - 1, mBmpCol.Height - 1);
            mGCol.DrawLine(Colors.PTabBorderBold,
                mBmpCol.Width - 1, 0,
                mBmpCol.Width - 1, mBmpCol.Height);
            mGCell.DrawLine(Colors.PTabBorderBold,
                mBmpCell.Width - 1, 0,
                mBmpCell.Width - 1, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTabBorderBold,
                0, 1,
                mBmpCell.Width - 1, 1);

            Width = mBmpRow.Width + mBmpCell.Width;
            Height = mCommonCtrl.FormCtrlBottom + mBmpCol.Height + mBmpCell.Height;

            picFooter.Width = mBmpCol.Width;
            picFooter.Height = mBmpCol.Height;
            picRow.Width = mBmpRow.Width;
            picRow.Height = mBmpRow.Height;
            picCell.Width = mBmpCell.Width;
            picCell.Height = mBmpCell.Height;
            picFooter.BackgroundImage = mBmpCol;
            picRow.BackgroundImage = mBmpRow;
            picCell.BackgroundImage = mBmpCell;

            picRow.Left = 0;
            picCell.Left = picRow.Right;
            picFooter.Left = picRow.Right;

            picRow.Top = mCommonCtrl.FormCtrlBottom;
            picCell.Top = mCommonCtrl.FormCtrlBottom;
            picFooter.Top = picCell.Bottom;
        }

        private void releaseImage() {
            if (null != mBmpCol) {
                mBmpCol.Dispose();
                mBmpCol = null;
                mGCol.Dispose();
                mGCol = null;
            }
            if (null != mBmpRow) {
                mBmpRow.Dispose();
                mBmpRow = null;
                mGRow.Dispose();
                mGRow = null;
            }
            if (null != mBmpCell) {
                mBmpCell.Dispose();
                mBmpCell = null;
                mGCell.Dispose();
                mGCell = null;
            }

            if (null != picFooter.BackgroundImage) {
                picFooter.BackgroundImage.Dispose();
                picFooter.BackgroundImage = null;
            }
            if (null != picRow.BackgroundImage) {
                picRow.BackgroundImage.Dispose();
                picRow.BackgroundImage = null;
            }
            if (null != picCell.BackgroundImage) {
                picCell.BackgroundImage.Dispose();
                picCell.BackgroundImage = null;
            }
        }
    }
}
