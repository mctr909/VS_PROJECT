using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Test {
    public partial class PianoRoll : UserControl {
        private readonly int QuarterNoteWidth = 96;
        private readonly int KeyHeight = 12;

        private Font NoteNameFont = new Font("MS Gothic", 9.0f);

        private Pen Gray50 = new Pen(Color.FromArgb(167, 127, 127, 127), 1.0f);
        private Pen Gray75 = new Pen(Color.FromArgb(167, 191, 191, 191), 1.0f);
        private Pen Gray80 = new Pen(Color.FromArgb(167, 204, 204, 204), 1.0f);
        private Pen Gray90 = new Pen(Color.FromArgb(167, 229, 229, 229), 1.0f);
        private Pen FocusToneColor = new Pen(Color.FromArgb(95, 191, 255, 191), 1.0f);
        private Pen SelectAreaColor = new Pen(Color.DarkBlue, 1.0f);

        private bool mIsDrag = false;
        private bool mPressAlt = false;
        private Keys mPressKey = Keys.None;
        private int mSelectSnap = 15;
        private int mDispTones;
        private int mFocusTone;
        private int mBeginTone;
        private int mEndTone;
        private int mBeginTime;
        private int mEndTime;

        private Point mBeginPos;
        private Point mEndPos;

        private Bitmap mBmpMeasure;
        private Bitmap mBmpKey;
        private Bitmap mBmpRoll;
        private Bitmap mBmpCtrl;
        private Graphics mgMeasure;
        private Graphics mgKey;
        private Graphics mgRoll;
        private Graphics mgCtrl;

        public int TimeScale { get; private set; } = 960;
        public int TimeSnap { get; private set; } = 480;

        private int[] Snaps = new int[] {
            15, 20, 24,
            30, 40, 48,
            60, 80, 96,
            120, 160, 192,
            240, 320, 384,
            480, 640,
            960
        };

        private string[] SnapNames = new string[] {
            "256", "3_64", "5_32",
            "128", "3_32", "5_16",
            "64", "3_16", "5_8",
            "32", "3_8", "5_4",
            "16", "3_4", "5_2",
            "8", "3_2",
            "4"
        };

        public PianoRoll() {
            InitializeComponent();
        }

        #region event
        private void PianoRoll_Load(object sender, EventArgs e) {
            setPos();
            KeyDown += new KeyEventHandler(PianoRoll_KeyDown);
            KeyUp += new KeyEventHandler(PianoRoll_KeyUp);
            picRoll.MouseWheel += new MouseEventHandler(picRoll_MouseWheel);
            SelectAreaColor.DashStyle = DashStyle.Dash;
            hsb.Value = 0;
            hsb.Minimum = 0;
            hsb.Maximum = 960 * 4 * 4;
            hsb.SmallChange = 240;
            hsb.LargeChange = 960 * 4;
            timer1.Interval = 10;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void PianoRoll_Resize(object sender, EventArgs e) {
            setPos();
        }

        private void PianoRoll_KeyDown(object sender, KeyEventArgs e) {
            mPressKey = e.KeyCode;
            mPressAlt = e.Alt;
        }

        private void PianoRoll_KeyUp(object sender, KeyEventArgs e) {
            mPressKey = Keys.None;
            mPressAlt = false;
        }

        private void picRoll_Click(object sender, EventArgs e) {

        }

        private void picRoll_MouseDown(object sender, MouseEventArgs e) {
            var pos = picRoll.PointToClient(Cursor.Position);
            var div = QuarterNoteWidth * TimeSnap / TimeScale;
            pos.X = pos.X / div * div;
            pos.Y = pos.Y / KeyHeight * KeyHeight + KeyHeight / 2;
            if (picRoll.Width <= pos.X) {
                pos.X = picRoll.Width - 1;
            }
            if (picRoll.Height <= pos.Y) {
                pos.Y = picRoll.Height - 1;
            }
            if (pos.X < 0) {
                pos.X = 0;
            }
            if (pos.Y < 0) {
                pos.Y = 0;
            }

            switch (e.Button) {
            case MouseButtons.Right:
                break;
            case MouseButtons.Left:
                mBeginPos = pos;
                mBeginTime = hsb.Value + TimeScale * pos.X / QuarterNoteWidth;
                mBeginTone = 128 + 10 - vsb.Value - pos.Y / KeyHeight;
                tslSelect.Text = string.Format("no:{0} time:{1}", mBeginTone, mBeginTime);
                mIsDrag = true;
                break;
            }
        }

        private void picRoll_MouseUp(object sender, MouseEventArgs e) {
            var pos = picRoll.PointToClient(Cursor.Position);
            var div = QuarterNoteWidth * TimeSnap / TimeScale;
            pos.X = pos.X / div * div;
            pos.Y = pos.Y / KeyHeight * KeyHeight + KeyHeight / 2;
            if (picRoll.Width <= pos.X) {
                pos.X = picRoll.Width - 1;
            }
            if (picRoll.Height <= pos.Y) {
                pos.Y = picRoll.Height - 1;
            }
            if (pos.X < 0) {
                pos.X = 0;
            }
            if (pos.Y < 0) {
                pos.Y = 0;
            }

            switch (e.Button) {
            case MouseButtons.Right:
                break;
            case MouseButtons.Left:
                mEndTime = hsb.Value + TimeScale * pos.X / QuarterNoteWidth;
                mEndTone = 128 + 10 - vsb.Value - pos.Y / KeyHeight;
                if (mEndTone < 0) {
                    mEndTone = 0;
                }
                tslSelect.Text = string.Format("no:{0} time:{1}", mEndTone, mEndTime);
                mIsDrag = false;
                break;
            }
        }

        private void picRoll_MouseMove(object sender, MouseEventArgs e) {
            var pos = picRoll.PointToClient(Cursor.Position);
            var div = QuarterNoteWidth * TimeSnap / TimeScale;
            pos.X = pos.X / div * div;
            if (picRoll.Width <= pos.X) {
                pos.X = picRoll.Width - 1;
                if (mIsDrag && hsb.Value + hsb.SmallChange < hsb.Maximum) {
                    hsb.Value += hsb.SmallChange;
                }
            }
            if (picRoll.Height <= pos.Y) {
                pos.Y = picRoll.Height - 1;
                if (mIsDrag && vsb.Value < vsb.Maximum) {
                    vsb.Value++;
                }
            }
            if (pos.X < 0) {
                pos.X = 0;
                if (mIsDrag && 0 <= hsb.Value - hsb.SmallChange) {
                    hsb.Value -= hsb.SmallChange;
                }
            }
            if (pos.Y < 0) {
                pos.Y = 0;
                if (mIsDrag && vsb.Minimum <= vsb.Value - 1) {
                    vsb.Value--;
                }
            }
            pos.Y = pos.Y / KeyHeight * KeyHeight + KeyHeight / 2;
            mEndPos = pos;
            mFocusTone = pos.Y / KeyHeight;
        }

        private void picRoll_MouseWheel(object sender, MouseEventArgs e) {
            switch (mPressKey) {
            case Keys.ControlKey:
                if (0 < Math.Sign(e.Delta)) {
                    if (240 < TimeScale) {
                        TimeScale >>= 1;
                        tslScale.Text = "縮尺:" + (960.0 / TimeScale);
                    }
                } else {
                    if (TimeScale < 3840) {
                        var tmpSnap = TimeSnap;
                        var tmpScale = TimeScale << 1;
                        while (16 * tmpSnap / tmpScale <= 0) {
                            mSelectSnap++;
                            tmpSnap = Snaps[mSelectSnap];
                        }
                        TimeSnap = tmpSnap;
                        TimeScale = tmpScale;
                        tslSnap.Text = "入力単位:" + SnapNames[mSelectSnap];
                        tslScale.Text = "縮尺:" + (960.0 / TimeScale);
                    }
                }
                break;
            case Keys.ShiftKey:
                if (0 < Math.Sign(e.Delta)) {
                    mSelectSnap++;
                    if (Snaps.Length <= mSelectSnap) {
                        mSelectSnap = Snaps.Length - 1;
                    }
                    TimeSnap = Snaps[mSelectSnap];
                    tslSnap.Text = "入力単位:" + SnapNames[mSelectSnap];
                } else {
                    var tmpIdx = mSelectSnap - 1;
                    if (tmpIdx < 0) {
                        tmpIdx = 0;
                    }
                    if (0 < 16 * Snaps[tmpIdx] / TimeScale) {
                        TimeSnap = Snaps[tmpIdx];
                        mSelectSnap = tmpIdx;
                        tslSnap.Text = "入力単位:" + SnapNames[tmpIdx];
                    }
                }
                break;
            case Keys.None:
                if (0 < Math.Sign(e.Delta)) {
                    if (vsb.Minimum <= vsb.Value - 1) {
                        vsb.Value--;
                    }
                } else {
                    if (vsb.Value < vsb.Maximum) {
                        vsb.Value++;
                    }
                }
                break;
            default:
                break;
            }
        }

        private void hsb_Scroll(object sender, ScrollEventArgs e) {

        }

        private void vsb_Scroll(object sender, ScrollEventArgs e) {

        }
        #endregion

        private void setPos() {
            var height = Height;
            var width = Width;
            if (height < 100) {
                height = 100;
            }
            if (width < 100) {
                width = 100;
            }

            picKey.Left = 0;
            picKey.Width = 24;

            picMeasure.Top = toolStrip1.Bottom;
            picMeasure.Height = 24;

            picMeasure.Left = picKey.Right;
            picMeasure.Width = width - picKey.Width - vsb.Width;

            picKey.Top = picMeasure.Bottom;
            picKey.Height = height - toolStrip1.Height - picMeasure.Height - hsb.Height;

            picRoll.Top = picMeasure.Bottom;
            picRoll.Left = picKey.Right;
            picRoll.Width = width - vsb.Width - picKey.Width;
            picRoll.Height = height - toolStrip1.Height - picMeasure.Height - hsb.Height;

            hsb.Top = picRoll.Bottom;
            hsb.Width = width - vsb.Width;

            vsb.Top = toolStrip1.Bottom;
            vsb.Left = picRoll.Right;
            vsb.Height = height - picMeasure.Height - hsb.Height;

            if (null != picMeasure.Image) {
                picMeasure.Image.Dispose();
                picMeasure.Image = null;
            }
            if (null != picKey.Image) {
                picKey.Image.Dispose();
                picKey.Image = null;
            }
            if (null != picRoll.Image) {
                picRoll.Image.Dispose();
                picRoll.Image = null;
            }

            mBmpMeasure = new Bitmap(picMeasure.Width, picMeasure.Height);
            mBmpKey = new Bitmap(picKey.Width, picKey.Height);
            mBmpRoll = new Bitmap(picRoll.Width, picRoll.Height);
            mgMeasure = Graphics.FromImage(mBmpMeasure);
            mgKey = Graphics.FromImage(mBmpKey);
            mgRoll = Graphics.FromImage(mBmpRoll);
            mDispTones = picRoll.Height / KeyHeight;
            if (127 < mDispTones) {
                mDispTones = 127;
            }
            vsb.Maximum = 128;
            vsb.Minimum = mDispTones;
            vsb.SmallChange = 1;
            vsb.LargeChange = 1;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            mgMeasure.Clear(Gray90.Color);
            mgRoll.Clear(Color.Transparent);
            mgKey.Clear(Color.Transparent);

            for (int y = mDispTones - 1, no = 128 - vsb.Value; 0 <= y; y--, no++) {
                switch (no % 12) {
                case 1:
                case 3:
                case 6:
                case 8:
                case 10:
                    mgRoll.FillRectangle(Gray80.Brush, 0, KeyHeight * y + 1, mBmpRoll.Width, KeyHeight - 1);
                    mgRoll.DrawLine(Gray75, 0, KeyHeight * y, mBmpRoll.Width, KeyHeight * y);
                    mgKey.FillRectangle(Gray80.Brush, 0, KeyHeight * y + 1, mBmpKey.Width, KeyHeight - 1);
                    mgKey.DrawLine(Gray75, 0, KeyHeight * y, mBmpKey.Width, KeyHeight * y);
                    break;
                case 11:
                    mgRoll.DrawLine(Gray50, 0, KeyHeight * y, mBmpRoll.Width, KeyHeight * y);
                    mgKey.DrawLine(Gray50, 0, KeyHeight * y, mBmpKey.Width, KeyHeight * y);
                    break;
                default:
                    mgRoll.DrawLine(Gray75, 0, KeyHeight * y, mBmpRoll.Width, KeyHeight * y);
                    mgKey.DrawLine(Gray75, 0, KeyHeight * y, mBmpKey.Width, KeyHeight * y);
                    break;
                }
                if (no % 12 == 0) {
                    mgKey.DrawString("C" + (no / 12 - 1), NoteNameFont, Brushes.Black, 2, KeyHeight * y + 1);
                }
            }

            mgMeasure.DrawLine(Gray50, 0, 0, 0, mBmpMeasure.Height - 1);
            mgMeasure.DrawLine(Gray50, 0, mBmpMeasure.Height - 1, mBmpMeasure.Width - 1, mBmpMeasure.Height - 1);
            mgRoll.DrawLine(Gray50, 0, 0, 0, mBmpRoll.Height - 1);

            if (mIsDrag) {
                var x1 = mBeginPos.X;
                var y1 = mBeginPos.Y;
                var x2 = mEndPos.X;
                var y2 = mEndPos.Y;
                if (x2 < x1) {
                    var tmp = x2;
                    x2 = x1;
                    x1 = tmp;
                }
                if (y2 < y1) {
                    var tmp = y2;
                    y2 = y1;
                    y1 = tmp;
                }
                y1 -= KeyHeight / 2;
                y2 += KeyHeight / 2;
                mgRoll.DrawRectangle(SelectAreaColor, x1, y1, x2 - x1, y2 - y1);
            } else {
                mgRoll.FillRectangle(FocusToneColor.Brush, 1, KeyHeight * mFocusTone + 1, mBmpRoll.Width - 1, KeyHeight - 1);
                mgRoll.DrawLine(Pens.Red, mEndPos.X, 0, mEndPos.X, mBmpRoll.Height - 1);
            }

            picMeasure.Image = mBmpMeasure;
            picRoll.Image = mBmpRoll;
            picKey.Image = mBmpKey;
        }
    }
}
