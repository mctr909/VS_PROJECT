using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Envelope {
    public partial class Envelope : Form {
        private static int TableColumnWidth = 74;
        private static int TableHeaderHeight = 30;
        private static int TableLeftFrameWidth = 55;

        private static int PitchDispUnit = 100;
        private static int CutoffDispUnit = 32;
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
        private static new StringFormat Right = new StringFormat {
            Alignment = StringAlignment.Far
        };

        private class Values {
            private PictureBox mPixTime;
            private PictureBox mPixValue;
            private Bitmap mBmpTime;
            private Graphics mGTime;

            public int Attack { get; private set; }
            public int Hold { get; private set; }
            public int Decay { get; private set; }
            public int Release { get; private set; }
            public int Range { get; private set; }

            public int DAttack;
            public int DHold;
            public int DDecay;
            public int DRelease;
            public int DRange;

            public double Rise;
            public double Top;
            public double Sustain;
            public double Fall;

            public Values(PictureBox picTime, PictureBox picValue) {
                mPixTime = picTime;
                mBmpTime = new Bitmap(picTime.Width, picTime.Height);
                mGTime = Graphics.FromImage(mBmpTime);
                mPixValue = picValue;
            }

            public void Commit() {
                Attack += DAttack;
                Hold += DHold;
                Decay += DDecay;
                Release += DRelease;
                Range += DRange;
                DAttack = 0;
                DHold = 0;
                DDecay = 0;
                DRelease = 0;
                DRange = 0;
            }

            public void DrawTime() {
                limit();

                var attack = Attack + DAttack;
                var hold = Hold + DHold;
                var decay = Decay + DDecay;
                var release = Release + DRelease;

                mGTime.Clear(Color.Transparent);
                mGTime.DrawString(attack.ToString("0ms"),
                    FontBold,
                    Colors.BFontTable,
                    new RectangleF(0, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    MiddleCenter);
                mGTime.DrawString(hold.ToString("0ms"),
                    FontBold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    MiddleCenter);
                mGTime.DrawString(decay.ToString("0ms"),
                    FontBold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth * 2, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    MiddleCenter);
                mGTime.DrawString(release.ToString("0ms"),
                    FontBold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth * 4, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    MiddleCenter);

                mPixTime.Image = mBmpTime;
            }

            public void DrawTimePitch() {
                limit();

                var attack = Attack + DAttack;
                var hold = Hold + DHold;
                var decay = Decay + DDecay;
                var release = Release + DRelease;
                var range = Range + DRange;

                var maxPitch = Math.Max(Math.Abs(Rise), Math.Max(Math.Abs(Top), Math.Abs(Fall)));
                if (range < maxPitch) {
                    range = (int)(maxPitch / 100.0 + 0.99) * 100;
                    Range = range;
                    DRange = 0;
                }

                DrawValuePitch();

                mGTime.Clear(Color.Transparent);
                mGTime.DrawString(attack.ToString("0ms"),
                    FontBold,
                    Colors.BFontTable,
                    new RectangleF(0, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    MiddleCenter);
                mGTime.DrawString(hold.ToString("0ms"),
                    FontBold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    MiddleCenter);
                mGTime.DrawString(decay.ToString("0ms"),
                    FontBold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth * 2, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    MiddleCenter);
                mGTime.DrawString(release.ToString("0ms"),
                    FontBold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth * 3, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    MiddleCenter);
                mGTime.DrawString(range.ToString("0cent"),
                    FontBold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth * 4, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    MiddleCenter);

                mPixTime.Image = mBmpTime;
            }

            public void DrawValuePitch() {
                var bmp = new Bitmap(mPixValue.Width, mPixValue.Height);
                var g = Graphics.FromImage(bmp);

                var range = Range + DRange;

                if (range < 100) {
                    range = 100;
                }

                if (Rise < -range) {
                    Rise = -range;
                }
                if (range < Rise) {
                    Rise = range;
                }
                if (Top < -range) {
                    Top = -range;
                }
                if (range < Top) {
                    Top = range;
                }
                if (Fall < -range) {
                    Fall = -range;
                }
                if (range < Fall) {
                    Fall = range;
                }

                var pOfs = PitchDispUnit * 2 - 1;
                var pRise = pOfs - (int)(Rise * PitchDispUnit * 2 / range);
                var pTop = pOfs - (int)(Top * PitchDispUnit * 2 / range);
                var pSustain = pOfs;
                var pFall = pOfs - (int)(Fall * PitchDispUnit * 2 / range);
                var psRise = pRise;
                var psTop = pTop;
                var psFall = pFall;

                if (mPixValue.Height < psRise + 20) {
                    psRise = mPixValue.Height - 20;
                }
                if (mPixValue.Height < psTop + 20) {
                    psTop = mPixValue.Height - 20;
                }
                if (mPixValue.Height < psFall + 20) {
                    psFall = mPixValue.Height - 20;
                }

                g.DrawLine(Colors.PGraphLine, 0, pRise, TableColumnWidth, pTop);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth, pTop, TableColumnWidth * 2, pTop);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth * 2, pTop, TableColumnWidth * 3, pSustain);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth * 3, pSustain, TableColumnWidth * 4, pFall);

                g.FillPie(Colors.BTableCell, -4, pRise - 4, 8, 8, 0, 360);
                g.FillPie(Colors.BTableCell, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                g.FillPie(Colors.BTableCell, TableColumnWidth * 4 - 4, pFall - 4, 8, 8, 0, 360);
                g.DrawArc(Colors.PTableBorderLight, -4, pRise - 4, 8, 8, 0, 360);
                g.DrawArc(Colors.PTableBorderLight, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                g.DrawArc(Colors.PTableBorderLight, TableColumnWidth * 4 - 4, pFall - 4, 8, 8, 0, 360);

                g.DrawString(Rise.ToString("0cent"), FontSmall, Colors.BFontTable, 3, psRise);
                g.DrawString(Top.ToString("0cent"), FontSmall, Colors.BFontTable, TableColumnWidth + 3, psTop);
                g.DrawString(Fall.ToString("0cent"), FontSmall, Colors.BFontTable, TableColumnWidth * 4 - 3, psFall, Right);

                if (null != mPixValue.Image) {
                    mPixValue.Image.Dispose();
                    mPixValue.Image = null;
                }
                mPixValue.Image = bmp;
            }

            public void DrawValueCutoff() {
                var bmp = new Bitmap(mPixValue.Width, mPixValue.Height);
                var g = Graphics.FromImage(bmp);

                if (Rise < 32) {
                    Rise = 32;
                }
                if (20000 < Rise) {
                    Rise = 20000;
                }

                if (Top < 32) {
                    Top = 32;
                }
                if (20000 < Top) {
                    Top = 20000;
                }

                if (Sustain < 32) {
                    Sustain = 32;
                }
                if (20000 < Sustain) {
                    Sustain = 20000;
                }

                if (Fall < 32) {
                    Fall = 32;
                }
                if (20000 < Fall) {
                    Fall = 20000;
                }

                var pOfs = bmp.Height + (int)(CutoffDispUnit * 4 * 1.5) - 1;
                var pRise = pOfs - (int)(Math.Log10(Rise) * CutoffDispUnit * 4);
                var pTop = pOfs - (int)(Math.Log10(Top) * CutoffDispUnit * 4);
                var pSustain = pOfs - (int)(Math.Log10(Sustain) * CutoffDispUnit * 4);
                var pFall = pOfs - (int)(Math.Log10(Fall) * CutoffDispUnit * 4);
                var psRise = pRise;
                var psTop = pTop;
                var psSustain = pSustain;
                var psFall = pFall;

                if (mPixValue.Height < psRise + 20) {
                    psRise = mPixValue.Height - 20;
                }
                if (mPixValue.Height < psTop + 20) {
                    psTop = mPixValue.Height - 20;
                }
                if (mPixValue.Height < psSustain + 20) {
                    psSustain = mPixValue.Height - 20;
                }
                if (mPixValue.Height < psFall + 20) {
                    psFall = mPixValue.Height - 20;
                }

                g.DrawLine(Colors.PGraphLine, 0, pRise, TableColumnWidth, pTop);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth, pTop, TableColumnWidth * 2, pTop);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth * 2, pTop, TableColumnWidth * 3, pSustain);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth * 3, pSustain, TableColumnWidth * 4, pSustain);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth * 4, pSustain, TableColumnWidth * 5, pFall);

                g.FillPie(Colors.BTableCell, -4, pRise - 4, 8, 8, 0, 360);
                g.FillPie(Colors.BTableCell, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                g.FillPie(Colors.BTableCell, TableColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);
                g.FillPie(Colors.BTableCell, TableColumnWidth * 5 - 5, pFall - 4, 8, 8, 0, 360);
                g.DrawArc(Colors.PTableBorderLight, -4, pRise - 4, 8, 8, 0, 360);
                g.DrawArc(Colors.PTableBorderLight, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                g.DrawArc(Colors.PTableBorderLight, TableColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);
                g.DrawArc(Colors.PTableBorderLight, TableColumnWidth * 5 - 5, pFall - 4, 8, 8, 0, 360);

                g.DrawString(Rise.ToString("0Hz"), FontSmall, Colors.BFontTable, 3, psRise);
                g.DrawString(Top.ToString("0Hz"), FontSmall, Colors.BFontTable, TableColumnWidth + 3, psTop);
                g.DrawString(Sustain.ToString("0Hz"), FontSmall, Colors.BFontTable, TableColumnWidth * 3 + 3, psSustain);
                g.DrawString(Fall.ToString("0Hz"), FontSmall, Colors.BFontTable, TableColumnWidth * 5 - 3, psFall, Right);

                if (null != mPixValue.Image) {
                    mPixValue.Image.Dispose();
                    mPixValue.Image = null;
                }
                mPixValue.Image = bmp;
            }

            public void DrawValueAmp() {
                var bmp = new Bitmap(mPixValue.Width, mPixValue.Height);
                var g = Graphics.FromImage(bmp);

                if (Top < 1 / 1024.0) {
                    Top = 1 / 1024.0;
                }
                if (1.0 < Top) {
                    Top = 1.0;
                }
                if (Sustain < 1 / 1024.0) {
                    Sustain = 1 / 1024.0;
                }
                if (1.0 < Sustain) {
                    Sustain = 1.0;
                }

                var dbTop = 20 * Math.Log10(Top);
                var dbSustain = 20 * Math.Log10(Sustain);

                var pOfs = bmp.Height - 1;
                var pRise = pOfs;
                var pTop = AmpDispUnit - (int)(dbTop * AmpDispUnit / 6);
                var pSustain = AmpDispUnit - (int)(dbSustain * AmpDispUnit / 6);
                var pFall = pOfs;
                var psTop = pTop;
                var psSustain = pSustain;

                if (mPixValue.Height < psTop + 20) {
                    psTop = mPixValue.Height - 20;
                }
                if (mPixValue.Height < psSustain + 20) {
                    psSustain = mPixValue.Height - 20;
                }

                g.DrawLine(Colors.PGraphLine, 0, pRise, TableColumnWidth, pTop);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth, pTop, TableColumnWidth * 2, pTop);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth * 2, pTop, TableColumnWidth * 3, pSustain);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth * 3, pSustain, TableColumnWidth * 4, pSustain);
                g.DrawLine(Colors.PGraphLine, TableColumnWidth * 4, pSustain, TableColumnWidth * 5, pFall);

                g.FillPie(Colors.BTableCell, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                g.DrawArc(Colors.PTableBorderLight, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                g.FillPie(Colors.BTableCell, TableColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);
                g.DrawArc(Colors.PTableBorderLight, TableColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);

                g.DrawString(dbTop.ToString("0.0db"), FontSmall, Colors.BFontTable, TableColumnWidth + 3, psTop);
                g.DrawString(dbSustain.ToString("0.0db"), FontSmall, Colors.BFontTable, TableColumnWidth * 3 + 3, psSustain);

                if (null != mPixValue.Image) {
                    mPixValue.Image.Dispose();
                    mPixValue.Image = null;
                }
                mPixValue.Image = bmp;
            }

            private void limit() {
                var attack = Attack + DAttack;
                var hold = Hold + DHold;
                var decay = Decay + DDecay;
                var release = Release + DRelease;
                var range = Range + DRange;

                if (attack < 1) {
                    Attack = 1;
                    DAttack = 0;
                }
                if (4000 < attack) {
                    Attack = 4000;
                    DAttack = 0;
                }

                if (hold < 1) {
                    Hold = 1;
                    DHold = 0;
                }
                if (4000 < hold) {
                    Hold = 4000;
                    DHold = 0;
                }

                if (decay < 1) {
                    Decay = 1;
                    DDecay = 0;
                }
                if (4000 < decay) {
                    Decay = 4000;
                    DDecay = 0;
                }

                if (release < 1) {
                    Release = 1;
                    DRelease = 0;
                }
                if (4000 < release) {
                    Release = 4000;
                    DRelease = 0;
                }

                if (range < 100) {
                    Range = 100;
                    DRange = 0;
                }
                if (4800 < range) {
                    Range = 4800;
                    DRange = 0;
                }
            }
        }

        private bool mMoving = false;
        private bool mTimeScroll = false;
        private bool mValueScroll = false;
        private Point mCurPos;

        private Values mPitch;
        private Values mCutoff;
        private Values mAmp;

        private TabButton mTabButtons;
        private Bitmap mBmpTabPageRow;
        private Bitmap mBmpTabPageCol;
        private Bitmap mBmpTabPageCell;
        private Graphics mGTabPageRow;
        private Graphics mGTabPageCol;
        private Graphics mGTabPageCell;

        public Envelope() {
            InitializeComponent();
        }

        private void Envelope_Load(object sender, EventArgs e) {
            BackColor = Colors.FormColor;
            mTabButtons = new TabButton(picTab, tab_Click, 14.0f, new string[] {
                 "Pitch",
                 "Cutoff",
                 "Amp"
            });

            drawBackgroundPitch();

            mPitch = new Values(picTabPageCol, picTabPageCell);
            mPitch.DrawTimePitch();
            mPitch.DrawValuePitch();

            mCutoff = new Values(picTabPageCol, picTabPageCell);
            mCutoff.Rise = 20000;
            mCutoff.Top = 20000;
            mCutoff.Sustain = 20000;
            mCutoff.Fall = 20000;

            mAmp = new Values(picTabPageCol, picTabPageCell);
            mAmp.Top = 1.0;
            mAmp.Sustain = 1.0;
        }

        private void Envelope_MouseDown(object sender, MouseEventArgs e) {
            mMoving = true;
            mCurPos = Cursor.Position;
        }

        private void Envelope_MouseUp(object sender, MouseEventArgs e) {
            mMoving = false;
        }

        private void Envelope_MouseMove(object sender, MouseEventArgs e) {
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

        private void picTabPageCol_MouseDown(object sender, MouseEventArgs e) {
            var pos = picTabPageCol.PointToClient(Cursor.Position);
            if (pos.Y < TableHeaderHeight) {
                return;
            }
            mCurPos = picTabPageCol.PointToClient(Cursor.Position);
            mTimeScroll = true;
            Cursor.Current = Cursors.VSplit;
        }

        private void picTabPageCol_MouseUp(object sender, MouseEventArgs e) {
            mTimeScroll = false;
            mPitch.Commit();
            mCutoff.Commit();
            mAmp.Commit();
        }

        private void picTabPageCol_MouseMove(object sender, MouseEventArgs e) {
            if (!mTimeScroll) {
                return;
            }

            var pos = picTabPageCol.PointToClient(Cursor.Position);
            var delta = pos.X - mCurPos.X;

            if (Math.Abs(delta) < 80) {
                delta /= 4;
            } else if (Math.Abs(delta) < 160) {
                delta /= 2;
            } else if (Math.Abs(delta) < 240) {
                delta *= 2;
            } else {
                delta *= 4;
            }

            switch (mTabButtons.CurrentTab) {
            case "Pitch":
                switch (mCurPos.X / TableColumnWidth) {
                case 0:
                    mPitch.DAttack = delta;
                    break;
                case 1:
                    mPitch.DHold = delta;
                    break;
                case 2:
                    mPitch.DDecay = delta;
                    break;
                case 3:
                    mPitch.DRelease = delta;
                    break;
                case 4:
                    mPitch.DRange = (pos.X - mCurPos.X) / 10 * 100;
                    break;
                }
                mPitch.DrawTimePitch();
                break;

            case "Cutoff":
                switch (mCurPos.X / TableColumnWidth) {
                case 0:
                    mCutoff.DAttack = delta;
                    break;
                case 1:
                    mCutoff.DHold = delta;
                    break;
                case 2:
                    mCutoff.DDecay = delta;
                    break;
                case 4:
                    mCutoff.DRelease = delta;
                    break;
                }
                mCutoff.DrawTime();
                break;

            case "Amp":
                switch (mCurPos.X / TableColumnWidth) {
                case 0:
                    mAmp.DAttack = delta;
                    break;
                case 1:
                    mAmp.DHold = delta;
                    break;
                case 2:
                    mAmp.DDecay = delta;
                    break;
                case 4:
                    mAmp.DRelease = delta;
                    break;
                }
                mAmp.DrawTime();
                break;
            }
        }

        private void picTabPageCell_MouseDown(object sender, MouseEventArgs e) {
            mCurPos = picTabPageCol.PointToClient(Cursor.Position);
            switch (mTabButtons.CurrentTab) {
            case "Pitch":
                switch (mCurPos.X / TableColumnWidth) {
                case 0:
                case 1:
                case 3:
                    mValueScroll = true;
                    Cursor.Current = Cursors.HSplit;
                    break;
                }
                break;
            case "Cutoff":
                switch (mCurPos.X / TableColumnWidth) {
                case 0:
                case 1:
                case 3:
                case 4:
                    mValueScroll = true;
                    Cursor.Current = Cursors.HSplit;
                    break;
                }
                break;
            case "Amp":
                switch (mCurPos.X / TableColumnWidth) {
                case 1:
                case 3:
                    mValueScroll = true;
                    Cursor.Current = Cursors.HSplit;
                    break;
                }
                break;
            }
        }

        private void picTabPageCell_MouseUp(object sender, MouseEventArgs e) {
            mValueScroll = false;
        }

        private void picTabPageCell_MouseMove(object sender, MouseEventArgs e) {
            if (!mValueScroll) {
                return;
            }

            var pos = picTabPageCell.PointToClient(Cursor.Position);

            switch (mTabButtons.CurrentTab) {
            case "Pitch":
                var pitchPos = PitchDispUnit * 2 - pos.Y;
                var pitch = pitchPos * mPitch.Range * 0.5 / PitchDispUnit;
                switch (mCurPos.X / TableColumnWidth) {
                case 0:
                    mPitch.Rise = pitch;
                    break;
                case 1:
                    mPitch.Top = pitch;
                    break;
                case 3:
                    mPitch.Fall = pitch;
                    break;
                }
                mPitch.DrawValuePitch();
                break;
            case "Cutoff":
                var freqPos = picTabPageCell.Height - pos.Y + CutoffDispUnit * 6;
                var freq = Math.Pow(10.0, freqPos * 0.25 / CutoffDispUnit);
                switch (mCurPos.X / TableColumnWidth) {
                case 0:
                    mCutoff.Rise = freq;
                    break;
                case 1:
                    mCutoff.Top = freq;
                    break;
                case 3:
                    mCutoff.Sustain = freq;
                    break;
                case 4:
                    mCutoff.Fall = freq;
                    break;
                }
                mCutoff.DrawValueCutoff();
                break;
            case "Amp":
                var db = (AmpDispUnit - pos.Y) * 6.0 / AmpDispUnit;
                var gain = Math.Pow(10.0, db / 20.0);
                switch (mCurPos.X / TableColumnWidth) {
                case 1:
                    mAmp.Top = gain;
                    break;
                case 3:
                    mAmp.Sustain = gain;
                    break;
                }
                mAmp.DrawValueAmp();
                break;
            }
        }

        private void tab_Click(object sender, EventArgs e) {
            switch (mTabButtons.CurrentTab) {
            case "Pitch":
                drawBackgroundPitch();
                mPitch.DrawTimePitch();
                mPitch.DrawValuePitch();
                break;
            case "Cutoff":
                drawBackgroundCutoff();
                mCutoff.DrawTime();
                mCutoff.DrawValueCutoff();
                break;
            case "Amp":
                drawBackgroundAmp();
                mAmp.DrawTime();
                mAmp.DrawValueAmp();
                break;
            }
        }

        private void drawBackgroundPitch() {
            releaseImage();

            mBmpTabPageCol = new Bitmap(TableColumnWidth * 5, TableHeaderHeight * 2);
            mGTabPageCol = Graphics.FromImage(mBmpTabPageCol);
            mBmpTabPageRow = new Bitmap(TableLeftFrameWidth, PitchDispUnit * 4 + mBmpTabPageCol.Height);
            mGTabPageRow = Graphics.FromImage(mBmpTabPageRow);
            mBmpTabPageCell = new Bitmap(TableColumnWidth * 5, PitchDispUnit * 4);
            mGTabPageCell = Graphics.FromImage(mBmpTabPageCell);

            mGTabPageCol.Clear(Colors.TableHeader);
            mGTabPageCol.FillRectangle(Colors.BTableCell,
                0, TableHeaderHeight,
                mBmpTabPageCol.Width, TableHeaderHeight);
            mGTabPageRow.Clear(Colors.TableHeader);
            mGTabPageCell.Clear(Colors.TableCell);
            mGTabPageCell.FillRectangle(Colors.BTableHeader,
                TableColumnWidth * 4, 0,
                TableColumnWidth * 4, mBmpTabPageCell.Height);

            mGTabPageCol.DrawString("Attack",
                FontBold,
                Colors.BFontTable,
                new RectangleF(0, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Hold",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Decay",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 2, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Release",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 3, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Range",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 4, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);

            mGTabPageCol.DrawLine(Colors.PTableBorderBold,
                0, mBmpTabPageCol.Height - 1,
                mBmpTabPageCol.Width, mBmpTabPageCol.Height - 1);
            mGTabPageCol.DrawLine(Colors.PTableBorderBold,
                TableColumnWidth * 4, 0,
                TableColumnWidth * 4, mBmpTabPageCol.Height);
            mGTabPageRow.DrawLine(Colors.PTableBorderBold,
                0, mBmpTabPageCol.Height - 1,
                mBmpTabPageRow.Width, mBmpTabPageCol.Height - 1);
            mGTabPageRow.DrawLine(Colors.PTableBorderBold,
                TableLeftFrameWidth - 1, 0,
                TableLeftFrameWidth - 1, mBmpTabPageRow.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorderBold,
                TableColumnWidth * 4, 0,
                TableColumnWidth * 4, mBmpTabPageCell.Height);

            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth, 0,
                TableColumnWidth, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 2, 0,
                TableColumnWidth * 2, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 3, 0,
                TableColumnWidth * 3, mBmpTabPageCol.Height);

            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, PitchDispUnit + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, PitchDispUnit + mBmpTabPageCol.Height);
            mGTabPageRow.DrawLine(Colors.PTableBorderLight,
                0, PitchDispUnit * 2 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, PitchDispUnit * 2 + mBmpTabPageCol.Height);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, PitchDispUnit * 3 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, PitchDispUnit * 3 + mBmpTabPageCol.Height);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, PitchDispUnit * 4 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, PitchDispUnit * 4 + mBmpTabPageCol.Height);

            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, PitchDispUnit,
                mBmpTabPageCell.Width - TableColumnWidth, PitchDispUnit);
            mGTabPageCell.DrawLine(Colors.PTableBorderLight,
                0, PitchDispUnit * 2,
                mBmpTabPageCell.Width - TableColumnWidth, PitchDispUnit * 2);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, PitchDispUnit * 3,
                mBmpTabPageCell.Width - TableColumnWidth, PitchDispUnit * 3);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, PitchDispUnit * 4,
                mBmpTabPageCell.Width - TableColumnWidth, PitchDispUnit * 4);

            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth, 0,
                TableColumnWidth, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 2, 0,
                TableColumnWidth * 2, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 3, 0,
                TableColumnWidth * 3, mBmpTabPageCell.Height);

            setImage();
        }

        private void drawBackgroundCutoff() {
            releaseImage();

            var ofs10kTo20k = (int)((Math.Log10(20000) - Math.Log10(10000)) * 4 * CutoffDispUnit);
            var ofs10kTo20k_TabPageColHeight = ofs10kTo20k + TableHeaderHeight * 2;

            mBmpTabPageCol = new Bitmap(TableColumnWidth * 5, TableHeaderHeight * 2);
            mGTabPageCol = Graphics.FromImage(mBmpTabPageCol);
            mBmpTabPageRow = new Bitmap(TableLeftFrameWidth, CutoffDispUnit * 10 + ofs10kTo20k + mBmpTabPageCol.Height);
            mGTabPageRow = Graphics.FromImage(mBmpTabPageRow);
            mBmpTabPageCell = new Bitmap(TableColumnWidth * 5, CutoffDispUnit * 10 + ofs10kTo20k);
            mGTabPageCell = Graphics.FromImage(mBmpTabPageCell);

            mGTabPageCol.Clear(Colors.TableHeader);
            mGTabPageCol.FillRectangle(Colors.BTableCell,
                0, TableHeaderHeight,
                TableColumnWidth * 3, TableHeaderHeight);
            mGTabPageCol.FillRectangle(Colors.BTableCell,
                TableColumnWidth * 4, TableHeaderHeight,
                TableColumnWidth, TableHeaderHeight);
            mGTabPageRow.Clear(Colors.TableHeader);
            mGTabPageCell.Clear(Colors.TableCell);

            mGTabPageCol.DrawString("Attack",
                FontBold,
                Colors.BFontTable,
                new RectangleF(0, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Hold",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Decay",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 2, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Sustain",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 3, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Release",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 4, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);

            mGTabPageCol.DrawLine(Colors.PTableBorderBold,
                0, mBmpTabPageCol.Height - 1,
                mBmpTabPageCol.Width, mBmpTabPageCol.Height - 1);
            mGTabPageRow.DrawLine(Colors.PTableBorderBold,
                0, mBmpTabPageCol.Height - 1,
                mBmpTabPageRow.Width, mBmpTabPageCol.Height - 1);
            mGTabPageRow.DrawLine(Colors.PTableBorderBold,
                TableLeftFrameWidth - 1, 0,
                TableLeftFrameWidth - 1, mBmpTabPageRow.Height);

            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth, 0, TableColumnWidth,
                mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 2, 0,
                TableColumnWidth * 2, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 3, 0,
                TableColumnWidth * 3, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 4, 0,
                TableColumnWidth * 4, mBmpTabPageCol.Height);

            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, ofs10kTo20k_TabPageColHeight,
                mBmpTabPageRow.Width, ofs10kTo20k_TabPageColHeight);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, CutoffDispUnit * 2 + ofs10kTo20k_TabPageColHeight,
                mBmpTabPageRow.Width, CutoffDispUnit * 2 + ofs10kTo20k_TabPageColHeight);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, CutoffDispUnit * 4 + ofs10kTo20k_TabPageColHeight,
                mBmpTabPageRow.Width, CutoffDispUnit * 4 + ofs10kTo20k_TabPageColHeight);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, CutoffDispUnit * 6 + ofs10kTo20k_TabPageColHeight,
                mBmpTabPageRow.Width, CutoffDispUnit * 6 + ofs10kTo20k_TabPageColHeight);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, CutoffDispUnit * 8 + ofs10kTo20k_TabPageColHeight,
                mBmpTabPageRow.Width, CutoffDispUnit * 8 + ofs10kTo20k_TabPageColHeight);

            mGTabPageRow.DrawString("10kHz", FontSmall, Colors.BFontTable, new RectangleF(
                -5, mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, ofs10kTo20k), BottomRight);
            mGTabPageRow.DrawString("3.16kHz", FontSmall, Colors.BFontTable, new RectangleF(
                -5, CutoffDispUnit + ofs10kTo20k_TabPageColHeight,
                mBmpTabPageRow.Width, CutoffDispUnit), BottomRight);
            mGTabPageRow.DrawString("1kHz", FontSmall, Colors.BFontTable, new RectangleF(
                -5, CutoffDispUnit * 3 + ofs10kTo20k_TabPageColHeight,
                mBmpTabPageRow.Width, CutoffDispUnit), BottomRight);
            mGTabPageRow.DrawString("316Hz", FontSmall, Colors.BFontTable, new RectangleF(
                -5, CutoffDispUnit * 5 + ofs10kTo20k_TabPageColHeight,
                mBmpTabPageRow.Width, CutoffDispUnit), BottomRight);
            mGTabPageRow.DrawString("100Hz", FontSmall, Colors.BFontTable, new RectangleF(
                -5, CutoffDispUnit * 7 + ofs10kTo20k_TabPageColHeight,
                mBmpTabPageRow.Width, CutoffDispUnit), BottomRight);
            mGTabPageRow.DrawString("32Hz", FontSmall, Colors.BFontTable, new RectangleF(
                -5, CutoffDispUnit * 9 + ofs10kTo20k_TabPageColHeight,
                mBmpTabPageRow.Width, CutoffDispUnit), BottomRight);

            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, ofs10kTo20k,
                mBmpTabPageCell.Width, ofs10kTo20k);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, CutoffDispUnit * 2 + ofs10kTo20k,
                mBmpTabPageCell.Width, CutoffDispUnit * 2 + ofs10kTo20k);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, CutoffDispUnit * 4 + ofs10kTo20k,
                mBmpTabPageCell.Width, CutoffDispUnit * 4 + ofs10kTo20k);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, CutoffDispUnit * 6 + ofs10kTo20k,
                mBmpTabPageCell.Width, CutoffDispUnit * 6 + ofs10kTo20k);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, CutoffDispUnit * 8 + ofs10kTo20k,
                mBmpTabPageCell.Width, CutoffDispUnit * 8 + ofs10kTo20k);

            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, CutoffDispUnit + ofs10kTo20k,
                mBmpTabPageCell.Width, CutoffDispUnit + ofs10kTo20k);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, CutoffDispUnit * 3 + ofs10kTo20k,
                mBmpTabPageCell.Width, CutoffDispUnit * 3 + ofs10kTo20k);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, CutoffDispUnit * 5 + ofs10kTo20k,
                mBmpTabPageCell.Width, CutoffDispUnit * 5 + ofs10kTo20k);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, CutoffDispUnit * 7 + ofs10kTo20k,
                mBmpTabPageCell.Width, CutoffDispUnit * 7 + ofs10kTo20k);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, CutoffDispUnit * 9 + ofs10kTo20k,
                mBmpTabPageCell.Width, CutoffDispUnit * 9 + ofs10kTo20k);

            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth, 0, TableColumnWidth,
                mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 2, 0,
                TableColumnWidth * 2, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 3, 0,
                TableColumnWidth * 3, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 4, 0,
                TableColumnWidth * 4, mBmpTabPageCell.Height);

            setImage();
        }

        private void drawBackgroundAmp() {
            releaseImage();

            mBmpTabPageCol = new Bitmap(TableColumnWidth * 5, TableHeaderHeight * 2);
            mGTabPageCol = Graphics.FromImage(mBmpTabPageCol);
            mBmpTabPageRow = new Bitmap(TableLeftFrameWidth, AmpDispUnit * 11 + mBmpTabPageCol.Height);
            mGTabPageRow = Graphics.FromImage(mBmpTabPageRow);
            mBmpTabPageCell = new Bitmap(TableColumnWidth * 5, AmpDispUnit * 11);
            mGTabPageCell = Graphics.FromImage(mBmpTabPageCell);

            mGTabPageCol.Clear(Colors.TableHeader);
            mGTabPageCol.FillRectangle(Colors.BTableCell,
                0, TableHeaderHeight,
                TableColumnWidth * 3, TableHeaderHeight);
            mGTabPageCol.FillRectangle(Colors.BTableCell,
                TableColumnWidth * 4, TableHeaderHeight,
                TableColumnWidth, TableHeaderHeight);
            mGTabPageRow.Clear(Colors.TableHeader);
            mGTabPageCell.Clear(Colors.TableCell);

            mGTabPageCol.DrawString("Attack",
                FontBold,
                Colors.BFontTable,
                new RectangleF(0, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Hold",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Decay",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 2, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Sustain",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 3, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);
            mGTabPageCol.DrawString("Release",
                FontBold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 4, 0, TableColumnWidth, TableHeaderHeight),
                MiddleCenter);

            mGTabPageCol.DrawLine(Colors.PTableBorderBold,
                0, mBmpTabPageCol.Height - 1,
                mBmpTabPageCol.Width, mBmpTabPageCol.Height - 1);
            mGTabPageRow.DrawLine(Colors.PTableBorderBold,
                0, mBmpTabPageCol.Height - 1,
                mBmpTabPageRow.Width, mBmpTabPageCol.Height - 1);
            mGTabPageRow.DrawLine(Colors.PTableBorderBold,
                TableLeftFrameWidth - 1, 0,
                TableLeftFrameWidth - 1, mBmpTabPageRow.Height);

            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth, 0,
                TableColumnWidth, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 2, 0,
                TableColumnWidth * 2, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 3, 0,
                TableColumnWidth * 3, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 4, 0,
                TableColumnWidth * 4, mBmpTabPageCol.Height);

            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit + mBmpTabPageCol.Height);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 3 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit * 3 + mBmpTabPageCol.Height);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 5 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit * 5 + mBmpTabPageCol.Height);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 7 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit * 7 + mBmpTabPageCol.Height);
            mGTabPageRow.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 9 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit * 9 + mBmpTabPageCol.Height);

            mGTabPageRow.DrawString("0db", FontSmall, Colors.BFontTable,
                new RectangleF(-5, mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);
            mGTabPageRow.DrawString("-12db ", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 2 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);
            mGTabPageRow.DrawString("-24db ", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 4 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);
            mGTabPageRow.DrawString("-36db ", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 6 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);
            mGTabPageRow.DrawString("-48db ", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 8 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);
            mGTabPageRow.DrawString("-60db ", FontSmall, Colors.BFontTable,
                new RectangleF(-5, AmpDispUnit * 10 + mBmpTabPageCol.Height,
                mBmpTabPageRow.Width, AmpDispUnit), BottomRight);

            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit,
                mBmpTabPageCell.Width, AmpDispUnit);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 3,
                mBmpTabPageCell.Width, AmpDispUnit * 3);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 5,
                mBmpTabPageCell.Width, AmpDispUnit * 5);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 7,
                mBmpTabPageCell.Width, AmpDispUnit * 7);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                0, AmpDispUnit * 9,
                mBmpTabPageCell.Width, AmpDispUnit * 9);

            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 2,
                mBmpTabPageCell.Width, AmpDispUnit * 2);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 4,
                mBmpTabPageCell.Width, AmpDispUnit * 4);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 6,
                mBmpTabPageCell.Width, AmpDispUnit * 6);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 8,
                mBmpTabPageCell.Width, AmpDispUnit * 8);
            mGTabPageCell.DrawLine(Colors.PTableBorderDark,
                0, AmpDispUnit * 10,
                mBmpTabPageCell.Width, AmpDispUnit * 10);

            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth, 0,
                TableColumnWidth, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 2, 0,
                TableColumnWidth * 2, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 3, 0,
                TableColumnWidth * 3, mBmpTabPageCell.Height);
            mGTabPageCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 4, 0,
                TableColumnWidth * 4, mBmpTabPageCell.Height);

            setImage();
        }

        private void setImage() {
            mGTabPageRow.DrawLine(Colors.PTabBorderBold,
                0, mBmpTabPageRow.Height - 1,
                mBmpTabPageRow.Width, mBmpTabPageRow.Height - 1);
            mGTabPageRow.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpTabPageRow.Height);
            mGTabPageCell.DrawLine(Colors.PTabBorderBold,
                0, mBmpTabPageCell.Height - 1,
                mBmpTabPageCell.Width, mBmpTabPageCell.Height - 1);
            mGTabPageCell.DrawLine(Colors.PTabBorderBold,
                mBmpTabPageCell.Width - 1, 0,
                mBmpTabPageCell.Width - 1, mBmpTabPageCell.Height);
            mGTabPageCol.DrawLine(Colors.PTabBorderBold,
                mBmpTabPageCol.Width - 1, 0,
                mBmpTabPageCol.Width - 1, mBmpTabPageCol.Height);
            mGTabPageCol.DrawLine(Colors.PTabBorderBold,
                picTab.Width - mBmpTabPageRow.Width - 3, 1,
                mBmpTabPageCol.Width, 1);

            Width = mBmpTabPageRow.Width + mBmpTabPageCell.Width;
            Height = picTab.Height + mBmpTabPageCol.Height + mBmpTabPageCell.Height;
            picTabPageCol.Width = mBmpTabPageCol.Width;
            picTabPageCol.Height = mBmpTabPageCol.Height;
            picTabPageRow.Width = mBmpTabPageRow.Width;
            picTabPageRow.Height = mBmpTabPageRow.Height;
            picTabPageCell.Width = mBmpTabPageCell.Width;
            picTabPageCell.Height = mBmpTabPageCell.Height;
            picTabPageCol.BackgroundImage = mBmpTabPageCol;
            picTabPageRow.BackgroundImage = mBmpTabPageRow;
            picTabPageCell.BackgroundImage = mBmpTabPageCell;

            picTab.Left = 0;
            picTab.Top = 0;
            picTabPageRow.Left = picTab.Left;
            picTabPageCol.Left = picTabPageRow.Right;
            picTabPageCell.Left = picTabPageRow.Right;
            btnClose.Left = picTabPageCell.Right - btnClose.Width;
            btnMinimize.Left = btnClose.Left - btnMinimize.Width - 4;

            picTabPageCol.Top = picTab.Bottom;
            picTabPageRow.Top = picTab.Bottom;
            picTabPageCell.Top = picTabPageCol.Bottom;
            btnClose.Top = 0;
            btnMinimize.Top = 0;
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
