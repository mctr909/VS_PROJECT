using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PianoRoll {
    public partial class Form1 : Form {
        private enum E_EDIT_MODE  {
            NOTE,
            INST,
            VOL,
            EXP,
            PAN,
            PITCH,
            VIB_DEP,
            VIB_RATE,
            REV,
            DEL_DEP,
            DEL_TIME,
            CHO,
            FC,
            Q,
            ATTACK,
            RELEASE,
            TEMPO
        }

        private static readonly Dictionary<string, int> Snaps = new Dictionary<string, int> {
            { "4分",    960 },
            { "3連4分", 640 },
            { "8分",    480 },
            { "5連4分", 384 },
            { "3連8分", 320 },
            { "16分",   240 },
            { "5連8分", 192 },
            { "3連16分",160 },
            { "32分",   120 },
            { "5連16分", 96 },
            { "3連32分", 80 },
            { "64分",    60 },
            { "5連32分", 48 },
            { "3連64分", 40 },
            { "5連64分", 24 }
        };

        private static readonly int[] TimeScales = new int[] {
            120, 160, 240, 320, 480, 640, 960, 1280, 1920, 2560
        };

        private static readonly int[] KeyHeights = new int[] {
            28, 24, 20, 18, 15, 12, 10, 8, 6, 5
        };

        private static readonly int QuarterNoteWidth = 80;
        private static readonly StringFormat NoteNameFormat = new StringFormat() {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };

        private static readonly Pen Gray12 = new Pen(Color.FromArgb(167, 31, 31, 31), 1.0f);
        private static readonly Pen Gray66 = new Pen(Color.FromArgb(167, 168, 168, 168), 1.0f);
        private static readonly Pen Gray80 = new Pen(Color.FromArgb(167, 204, 204, 204), 1.0f);
        private static readonly Pen Gray90 = new Pen(Color.FromArgb(167, 229, 229, 229), 1.0f);
        private static readonly Pen FocusToneColor = new Pen(Color.FromArgb(95, 127, 255, 127), 1.0f);
        private static readonly Brush SolidToneColor = new Pen(Color.FromArgb(255, 10, 224, 10), 1.0f).Brush;
        private static readonly Pen SolidToneColorH = new Pen(Color.FromArgb(255, 95, 255, 95), 1.0f);
        private static readonly Pen SolidToneColorL = new Pen(Color.FromArgb(255, 15, 167, 15), 1.0f);
        private static readonly Pen SelectAreaColor = new Pen(Color.DarkBlue) {
            Width = 1.0f,
            DashStyle = DashStyle.Dot
        };

        private static readonly int MesureBarHeight = 40;
        private Bitmap mBmpRoll;
        private Graphics mgRoll;

        private Font mNoteNameFont = new Font("MS Gothic", 9.0f, FontStyle.Regular);

        private int mTimeScaleIdx = 6;
        private int mTimeScale = TimeScales[6];

        private int mKeyHeightIdx = 5;
        private int mKeyHeight = KeyHeights[5];

        private int mTimeSnap = 240;

        private Point mCursor;
        private Point mDragBegin;
        private Point mDragEnd;

        private int mDispTones;
        private int mBeginTone;
        private int mEndTone;
        private int mBeginTime;
        private int mEndTime;

        private bool mIsDrag = false;
        private bool mPressAlt = false;
        private Keys mPressKey = Keys.None;

        private struct Event {
            public int tick;
            public byte track;
            public byte status;
            public byte data1;
            public byte data2;
            public Event(int tick, params int[] data) {
                this.tick = tick;
                track = 0;
                status = 0;
                data1 = 0;
                data2 = 0;
                switch (data.Length) {
                case 3:
                    track = (byte)data[0];
                    status = (byte)data[1];
                    data1 = (byte)data[2];
                    break;
                case 4:
                    track = (byte)data[0];
                    status = (byte)data[1];
                    data1 = (byte)data[2];
                    data2 = (byte)data[3];
                    break;
                }
            }
        }

        private List<Event> mEventList = new List<Event>();

        public Form1() {
            InitializeComponent();
            setSize();
            setInputMode(tsbRoll);
            setSelectWrite(tsbWrite);
            setEditMode(tsmEditModeNote);
            setTimeDiv(tsmTick960);

            picRoll.MouseWheel += new MouseEventHandler(picRoll_MouseWheel);
            KeyDown += new KeyEventHandler(picRoll_KeyDown);
            KeyUp += new KeyEventHandler(picRoll_KeyUp);

            timer1.Interval = 16;
            timer1.Enabled = true;
            timer1.Start();
            vScroll.Value = vScroll.Minimum < 104 ? 104 : vScroll.Minimum;
        }

        private void Form1_SizeChanged(object sender, EventArgs e) {
            setSize();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            drawRoll();
        }

        private void tsbRoll_Click(object sender, EventArgs e) {
            setInputMode(sender);
        }

        private void tsbEventList_Click(object sender, EventArgs e) {
            setInputMode(sender);
        }

        private void tsbSelect_Click(object sender, EventArgs e) {
            setSelectWrite(tsbSelect);
        }

        private void tsbWrite_Click(object sender, EventArgs e) {
            setSelectWrite(tsbWrite);
        }

        #region tsmTick event
        private void tsmTick960_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick480_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick240_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick120_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick060_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick640_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick320_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick160_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick080_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick040_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick384_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick192_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick096_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick048_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }

        private void tsmTick024_Click(object sender, EventArgs e) {
            setTimeDiv(sender);
        }
        #endregion

        #region tsmEditMode event
        private void tsmEditModeNote_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeInst_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeVol_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeExp_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModePan_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModePitch_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeVibDep_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeVibRate_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeRev_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeDelDep_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeDelTime_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeCho_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeFc_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeFq_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeAttack_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeRelease_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }

        private void tsmEditModeTempo_Click(object sender, EventArgs e) {
            setEditMode(sender);
        }
        #endregion

        #region picRoll event
        private void picRoll_MouseDown(object sender, MouseEventArgs e) {
            switch (e.Button) {
            case MouseButtons.Right:
                break;
            case MouseButtons.Left:
                mBeginTime = hScroll.Value + mTimeScale * mCursor.X / QuarterNoteWidth;
                mBeginTone = 127 + vScroll.Minimum - vScroll.Value - mCursor.Y / mKeyHeight;
                mIsDrag = true;
                break;
            }
            mDragBegin.X = mCursor.X;
            mDragBegin.Y = mCursor.Y;
            mDragEnd.X = mCursor.X;
            mDragEnd.Y = mCursor.Y;
        }

        private void picRoll_MouseUp(object sender, MouseEventArgs e) {
            switch (e.Button) {
            case MouseButtons.Right:
                break;
            case MouseButtons.Left:
                mEndTime = hScroll.Value + mTimeScale * mCursor.X / QuarterNoteWidth;
                if (mEndTime < mBeginTime) {
                    var tmp = mEndTime;
                    mEndTime = mBeginTime + mTimeSnap;
                    mBeginTime = tmp;
                } else {
                    mEndTime += mTimeSnap;
                }
                if (tsbWrite.Checked) {
                    mEndTone = mBeginTone;
                } else {
                    mEndTone = 127 + vScroll.Minimum - vScroll.Value - mCursor.Y / mKeyHeight;
                    if(mEndTone < 0) {
                        mEndTone = 0;
                    }
                }
                if (mEndTone < mBeginTone) {
                    var tmp = mEndTone;
                    mEndTone = mBeginTone;
                    mBeginTone = tmp;
                }
                tslStatus.Text = string.Format("Tone:{0}-{1} Time:{2}-{3}", mBeginTone, mEndTone, mBeginTime, mEndTime);
                if (tsbWrite.Checked && tsmEditModeNote.Checked) {
                    addNoteEvent();
                }
                mIsDrag = false;
                break;
            }
        }

        private void picRoll_MouseMove(object sender, MouseEventArgs e) {
            snapCursor(picRoll.PointToClient(Cursor.Position));
            mDragEnd.X = mCursor.X;
            mDragEnd.Y = mCursor.Y;
            if (tsbWrite.Checked) {
                mDragBegin.Y = mDragEnd.Y;
            }
        }

        private void picRoll_MouseWheel(object sender, MouseEventArgs e) {
            switch (mPressKey) {
            case Keys.None:
                if (0 < Math.Sign(e.Delta)) {
                    if (vScroll.Minimum <= vScroll.Value - 1) {
                        vScroll.Value--;
                    }
                } else {
                    if (vScroll.Value < vScroll.Maximum) {
                        vScroll.Value++;
                    }
                }
                break;
            case Keys.ControlKey:
                if (0 < Math.Sign(e.Delta)) {
                    timeZoom();
                } else {
                    timeZoomout();
                }
                break;
            case Keys.ShiftKey:
                if (0 < Math.Sign(e.Delta)) {
                    toneZoom();
                } else {
                    toneZoomout();
                }
                break;
            default:
                break;
            }
        }

        private void picRoll_KeyDown(object sender, KeyEventArgs e) {
            mPressKey = e.KeyCode;
            mPressAlt = e.Alt;
        }

        private void picRoll_KeyUp(object sender, KeyEventArgs e) {
            mPressKey = Keys.None;
            mPressAlt = false;
        }
        #endregion

        #region zoom event
        private void tsbTimeZoom_Click(object sender, EventArgs e) {
            timeZoom();
        }

        private void tsbTimeZoomout_Click(object sender, EventArgs e) {
            timeZoomout();
        }

        private void tsbToneZoom_Click(object sender, EventArgs e) {
            toneZoom();
        }

        private void tsbToneZoomout_Click(object sender, EventArgs e) {
            toneZoomout();
        }
        #endregion

        private void timeZoom() {
            if (0 < mTimeScaleIdx) {
                mTimeScaleIdx--;
                mTimeScale = TimeScales[mTimeScaleIdx];
            }
            if (0 == mTimeScaleIdx) {
                tsbTimeZoom.Enabled = false;
            }
            tsbTimeZoomout.Enabled = true;
        }
        private void timeZoomout() {
            if (mTimeScaleIdx < TimeScales.Length - 1) {
                mTimeScaleIdx++;
                mTimeScale = TimeScales[mTimeScaleIdx];
            }
            if (mTimeScaleIdx == TimeScales.Length - 1) {
                tsbTimeZoomout.Enabled = false;
            }
            tsbTimeZoom.Enabled = true;
        }
        private void toneZoom() {
            if (0 < mKeyHeightIdx) {
                mKeyHeightIdx--;
                mKeyHeight = KeyHeights[mKeyHeightIdx];
                setSize();
            }
            if (0 == mKeyHeightIdx) {
                tsbToneZoom.Enabled = false;
            }
            tsbToneZoomout.Enabled = true;
            snapCursor(mCursor);
        }
        private void toneZoomout() {
            if (mKeyHeightIdx < KeyHeights.Length - 1) {
                mKeyHeightIdx++;
                mKeyHeight = KeyHeights[mKeyHeightIdx];
                setSize();
            }
            if (mKeyHeightIdx == KeyHeights.Length - 1) {
                tsbToneZoomout.Enabled = false;
            }
            tsbToneZoom.Enabled = true;
            snapCursor(mCursor);
        }

        private void snapCursor(Point pos) {
            var divX = QuarterNoteWidth * mTimeSnap / mTimeScale;
            pos.X = pos.X / divX * divX;
            if (picRoll.Width <= pos.X) {
                pos.X = picRoll.Width - 1;
                if (mIsDrag && hScroll.Value + hScroll.SmallChange < hScroll.Maximum) {
                    hScroll.Value += hScroll.SmallChange;
                    Thread.Sleep(50);
                }
            }
            if (pos.X < 0) {
                pos.X = 0;
                if (mIsDrag && 0 <= hScroll.Value - hScroll.SmallChange) {
                    hScroll.Value -= hScroll.SmallChange;
                    Thread.Sleep(50);
                }
            }

            var ofsY = mBmpRoll.Height % mKeyHeight;
            pos.Y = (pos.Y - ofsY) / mKeyHeight * mKeyHeight;

            if (picRoll.Height <= pos.Y) {
                pos.Y = picRoll.Height - 1;
                if (mIsDrag && vScroll.Value < vScroll.Maximum) {
                    vScroll.Value++;
                }
            }
            if (pos.Y < 0) {
                pos.Y = 0;
                if (mIsDrag && vScroll.Minimum <= vScroll.Value - 1) {
                    vScroll.Value--;
                }
            }

            var tone = pos.Y / mKeyHeight;
            if (tone < 0) {
                tone = 0;
            } else if (127 < tone) {
                tone = 127;
            }

            mCursor.X = pos.X;
            mCursor.Y = tone * mKeyHeight + ofsY;
        }

        private void setSize() {
            if (Height < toolStrip1.Bottom + hScroll.Height + MesureBarHeight + 40) {
                Height = toolStrip1.Bottom + hScroll.Height + MesureBarHeight + 40;
            }

            pnlRoll.Left = 0;
            pnlRoll.Top = toolStrip1.Bottom + MesureBarHeight;
            pnlRoll.Width = Width - 16;
            pnlRoll.Height = Height - toolStrip1.Bottom - 39;
            vScroll.Top = 0;
            vScroll.Left = pnlRoll.Width - vScroll.Width;
            vScroll.Height = pnlRoll.Height - hScroll.Height - MesureBarHeight;
            hScroll.Left = 0;
            hScroll.Top = pnlRoll.Height - hScroll.Height - MesureBarHeight;
            hScroll.Width = pnlRoll.Width - vScroll.Width;
            picRoll.Left = 0;
            picRoll.Top = 0;
            picRoll.Width = vScroll.Left;
            picRoll.Height = hScroll.Top;

            picMesureBar.Left = 0;
            picMesureBar.Top = toolStrip1.Bottom;
            picMesureBar.Width = vScroll.Left;
            picMesureBar.Height = MesureBarHeight;

            mBmpRoll = new Bitmap(picRoll.Width, picRoll.Height);
            mgRoll = Graphics.FromImage(mBmpRoll);

            mDispTones = picRoll.Height / mKeyHeight;
            if (128 < mDispTones) {
                mDispTones = 128;
            }
            vScroll.Maximum = 128;
            vScroll.Minimum = mDispTones;
            vScroll.SmallChange = 1;
            vScroll.LargeChange = 1;
            hScroll.Maximum = 960 * 4 * 16;
            hScroll.LargeChange = mTimeSnap;
            hScroll.SmallChange = mTimeSnap;

            int fontSize = 7;
            while (fontSize < 17) {
                var newFont = new Font(mNoteNameFont.Name, fontSize);
                var fsize = mgRoll.MeasureString("C", newFont).Height - 2;
                if (mKeyHeight <= fsize) {
                    break;
                }
                mNoteNameFont = newFont;
                fontSize++;
            }

            drawRoll();
        }

        private void setInputMode(object item) {
            tsbRoll.Checked = false;
            tsbRoll.Image = Properties.Resources.pianoroll_disable;
            tsbEventList.Checked = false;
            tsbEventList.Image = Properties.Resources.eventlist_disable;

            var obj = (ToolStripButton)item;
            obj.Checked = true;

            switch (obj.Name) {
            case "tsbRoll":
                tsbRoll.Image = Properties.Resources.pianoroll;
                break;
            case "tsbEventList":
                tsbEventList.Image = Properties.Resources.eventlist;
                break;
            }
        }

        private void setSelectWrite(object item) {
            tsbWrite.Checked = false;
            tsbWrite.Image = Properties.Resources.write_disable;
            tsbSelect.Checked = false;
            tsbSelect.Image = Properties.Resources.select_disable;

            var obj = (ToolStripButton)item;
            obj.Checked = true;

            switch (obj.Name) {
            case "tsbWrite":
                tsbWrite.Image = Properties.Resources.write;
                break;
            case "tsbSelect":
                tsbSelect.Image = Properties.Resources.select;
                break;
            }
        }

        private void setEditMode(object item) {
            tsmEditModeNote.Checked = false;
            tsmEditModeInst.Checked = false;
            tsmEditModeVol.Checked = false;
            tsmEditModeExp.Checked = false;
            tsmEditModePan.Checked = false;
            tsmEditModePitch.Checked = false;
            tsmEditModeVib.Checked = false;
            tsmEditModeVibDep.Checked = false;
            tsmEditModeVibRate.Checked = false;
            tsmEditModeRev.Checked = false;
            tsmEditModeDel.Checked = false;
            tsmEditModeDelDep.Checked = false;
            tsmEditModeDelTime.Checked = false;
            tsmEditModeCho.Checked = false;
            tsmEditModeFc.Checked = false;
            tsmEditModeFq.Checked = false;
            tsmEditModeAttack.Checked = false;
            tsmEditModeRelease.Checked = false;
            tsmEditModeTempo.Checked = false;

            var obj = (ToolStripMenuItem)item;
            obj.Checked = true;
            tsdEditMode.Image = obj.Image;
            tsdEditMode.ToolTipText = string.Format("入力種別({0})", obj.Text);

            tsmEditModeVib.Checked = tsmEditModeVibDep.Checked | tsmEditModeVibRate.Checked;
            tsmEditModeDel.Checked = tsmEditModeDelDep.Checked | tsmEditModeDelTime.Checked;
        }

        private void setTimeDiv(object item) {
            tsmTick960.Checked = false;
            tsmTick480.Checked = false;
            tsmTick240.Checked = false;
            tsmTick120.Checked = false;
            tsmTick060.Checked = false;

            tsmTick640.Checked = false;
            tsmTick320.Checked = false;
            tsmTick160.Checked = false;
            tsmTick080.Checked = false;
            tsmTick040.Checked = false;

            tsmTick384.Checked = false;
            tsmTick192.Checked = false;
            tsmTick096.Checked = false;
            tsmTick048.Checked = false;
            tsmTick024.Checked = false;

            var obj = (ToolStripMenuItem)item;
            obj.Checked = true;
            tsdTimeDiv.Image = obj.Image;
            tsdTimeDiv.ToolTipText = string.Format("入力単位({0})", obj.Text);

            mTimeSnap = Snaps[obj.Text];
            hScroll.LargeChange = mTimeSnap;
            hScroll.SmallChange = mTimeSnap;
        }

        private void drawRoll() {
            mgRoll.Clear(Color.Transparent);

            var ofsY = mBmpRoll.Height % mKeyHeight;

            for (int y = mDispTones - 1, no = 128 - vScroll.Value; 0 <= y; y--, no++) {
                var py = mKeyHeight * y + ofsY;
                switch (no % 12) {
                case 1:
                case 3:
                case 6:
                case 8:
                case 10:
                    mgRoll.FillRectangle(Gray80.Brush, 0, py + 1, mBmpRoll.Width, mKeyHeight - 1);
                    mgRoll.DrawLine(Gray66, 0, py, mBmpRoll.Width, py);
                    break;
                case 11:
                    mgRoll.DrawLine(Gray12, 0, py, mBmpRoll.Width, py);
                    break;
                default:
                    mgRoll.DrawLine(Gray66, 0, py, mBmpRoll.Width, py);
                    break;
                }
            }

            if (mIsDrag) {
                var x1 = mDragBegin.X;
                var y1 = mDragBegin.Y;
                var x2 = mDragEnd.X;
                var y2 = mDragEnd.Y;
                if (x1 <= x2) {
                    x2 += QuarterNoteWidth * mTimeSnap / mTimeScale;
                } else {
                    x1 += QuarterNoteWidth * mTimeSnap / mTimeScale;
                }
                if (x2 < x1) {
                    var tmp = x2;
                    x2 = x1;
                    x1 = tmp;
                }
                if (y1 <= y2) {
                    y2 += mKeyHeight;
                } else {
                    y1 += mKeyHeight;
                }
                if (y2 < y1) {
                    var tmp = y2;
                    y2 = y1;
                    y1 = tmp;
                }
                if (tsbSelect.Checked) {
                    mgRoll.DrawRectangle(SelectAreaColor, x1, y1, x2 - x1, y2 - y1);
                }
                if (tsbWrite.Checked) {
                    drawNote(x1, y1, x2, y2);
                }
            } else {
                mgRoll.FillRectangle(FocusToneColor.Brush, 1, mCursor.Y + 1, mBmpRoll.Width - 1, mKeyHeight - 1);
                mgRoll.DrawLine(Pens.Red, mCursor.X, 0, mCursor.X, mBmpRoll.Height - 1);
            }

            for (int y = mDispTones - 1, no = 128 - vScroll.Value; 0 <= y; y--, no++) {
                if (no % 12 == 0) {
                    var py = mKeyHeight * y + ofsY;
                    var name = "C" + (no / 12 - 1);
                    var fsize = mgRoll.MeasureString(name, mNoteNameFont).Height - 2;
                    mgRoll.DrawString(name, mNoteNameFont, Gray12.Brush, 0, py + mKeyHeight - fsize);
                }
            }

            picRoll.Image = mBmpRoll;
        }

        private void drawNote(int x1, int y1, int x2, int y2) {
            mgRoll.FillRectangle(SolidToneColor, x1, y1, x2 - x1 + 1, y2 - y1 + 1);
            mgRoll.DrawLine(SolidToneColorL, x1, y2, x2, y2);
            mgRoll.DrawLine(SolidToneColorH, x1, y1, x2 - 1, y1);
            mgRoll.DrawLine(SolidToneColorL, x2, y2, x2, y1 + 1);
            mgRoll.DrawLine(SolidToneColorH, x1, y1, x1, y2 - 1);
        }

        private void addNoteEvent() {
            mEventList.Add(new Event(mBeginTime, 0, 0x90, mEndTone, 127));
            mEventList.Add(new Event(mEndTime, 0, 0x80, mEndTone, 0));
        }
    }
}
