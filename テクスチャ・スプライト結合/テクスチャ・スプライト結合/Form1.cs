using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace テクスチャ_スプライト結合
{
    public partial class Form1 : Form
    {
        private readonly KeyValuePair<string, CellType>[] CellTypes = {
            new KeyValuePair<string, CellType>("RPGツクールMV 144x192 4列2段", new CellType(144, 192, 4, 2)),
            new KeyValuePair<string, CellType>("RPGツクールMV 144x144 4列2段", new CellType(144, 144, 4, 2)),
            new KeyValuePair<string, CellType>("RPGツクールMV 64x64 9列6段", new CellType(64, 64, 9, 6)),
            new KeyValuePair<string, CellType>("Unity 512x512 2列2段", new CellType(512, 512, 2, 2)),
            new KeyValuePair<string, CellType>("Unity 256x256 4列4段", new CellType(256, 256, 4, 4)),
            new KeyValuePair<string, CellType>("Unity 256x256 2列2段", new CellType(256, 256, 2, 2)),
            new KeyValuePair<string, CellType>("Unity 128x128 8列8段", new CellType(128, 128, 8, 8)),
            new KeyValuePair<string, CellType>("Unity 128x128 4列4段", new CellType(128, 128, 4, 4)),
            new KeyValuePair<string, CellType>("Unity 128x128 2列2段", new CellType(128, 128, 2, 2)),
            new KeyValuePair<string, CellType>("Unity 64x64 8列8段", new CellType(64, 64, 8, 8)),
            new KeyValuePair<string, CellType>("Unity 64x64 4列4段", new CellType(64, 64, 4, 4)),
            new KeyValuePair<string, CellType>("Unity 64x64 2列2段", new CellType(64, 64, 2, 2))
        };

        public struct CellType
        {
            public CellType(int width, int height, int cols, int rows)
            {
                Size = new Point(width, height);
                Cols = cols;
                Rows = rows;
            }

            public Point Size;
            public int Cols;
            public int Rows;
        }

        private Point selectFrom;
        private CellType cell;
        private Bitmap bmpWork;
        private string tempFileName;

        public Form1()
        {
            InitializeComponent();
            pictArea.AllowDrop = true;

            foreach (var type in CellTypes)
            {
                cmbCellType.Items.Add(type.Key);
            }
            cmbCellType.SelectedIndex = 0;

            cell.Cols = CellTypes[0].Value.Cols;
            cell.Rows = CellTypes[0].Value.Rows;
            cell.Size = CellTypes[0].Value.Size;

            bmpWork = new Bitmap(cell.Cols * cell.Size.X, cell.Rows * cell.Size.Y);
            pictArea.Width = bmpWork.Width;
            pictArea.Height = bmpWork.Height;

            tempFileName = "";
        }

        private void pictArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
            else e.Effect = DragDropEffects.None;
        }

        private void pictArea_DragDrop(object sender, DragEventArgs e)
        {
            string[] strFileNames = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (strFileNames.Length < 1) return;
            if (!System.IO.File.Exists(strFileNames[0])) return;

            cmbCellType.Enabled = false;
            tempFileName = "";

            try
            {
                //*** 単体取り込み ***//
                if (rbtOne.Checked)
                {
                    //*** 複数選択取り込み ***//
                    if (strFileNames.Length > 1)
                    {
                        int colCnt = 0;
                        int rowCnt = 0;

                        foreach (string strFileName in strFileNames)
                        {
                            if (!System.IO.File.Exists(strFileName)) continue;
                            if (cell.Rows == (rowCnt - 1) && cell.Cols == colCnt) break;

                            ImportBmp(strFileName, cell.Size, new Point(cell.Size.X * colCnt, cell.Size.Y * rowCnt));

                            rowCnt += (++colCnt) / cell.Cols;
                            colCnt %= cell.Cols;
                        }
                    }

                    //*** 個別取り込み ***//
                    else
                    {
                        ImportBmp(strFileNames[0], cell.Size, GetLocation());
                    }
                }

                //*** 全体取り込み ***//
                if (rbtAll.Checked)
                {
                    tempFileName = strFileNames[0];
                    ImportBmp(strFileNames[0], new Point(0, 0), new Point(0, 0));
                }

                pictArea.Image = bmpWork;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void pictArea_MouseDown(object sender, MouseEventArgs e)
        {
            selectFrom = GetLocation();
        }

        private void pictArea_MouseUp(object sender, MouseEventArgs e)
        {
            SwapBmp(selectFrom, GetLocation());
            pictArea.Image = bmpWork;
        }

        private void pictArea_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point cur = GetLocation();

            for (int py = cur.Y; py < (cur.Y + cell.Size.Y); ++py)
            {
                for (int px = cur.X; px < (cur.X + cell.Size.X); ++px)
                {
                    bmpWork.SetPixel(px, py, Color.FromArgb(0, 255, 255, 255));
                }
            }

            pictArea.Image = bmpWork;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "PNGファイル(*.png)|*.png";
            saveFileDialog1.ShowDialog();
            string saveFileName = saveFileDialog1.FileName;
            if (string.IsNullOrEmpty(saveFileName)) return;
            bmpWork.Save(saveFileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        private void btnSprit_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(tempFileName);
            saveFileDialog1.Filter = "PNGファイル(*.png)|*.png";
            saveFileDialog1.ShowDialog();

            string saveFileName = saveFileDialog1.FileName;
            if (string.IsNullOrEmpty(saveFileName)) return;
            saveFileName = saveFileName.Replace(".png", "");

            Bitmap bmpOut;

            for (int row = 0; row < cell.Rows; ++row)
            {
                for (int col = 0; col < cell.Cols; ++col)
                {
                    bmpOut = new Bitmap(cell.Size.X, cell.Size.Y);
                    for (int dstY = 0, srcY = (row * cell.Size.Y); dstY < bmpOut.Height; ++dstY, ++srcY)
                    {
                        for (int dstX = 0, srcX = (col * cell.Size.X); dstX < bmpOut.Width; ++dstX, ++srcX)
                        {
                            bmpOut.SetPixel(dstX, dstY, bmpWork.GetPixel(srcX, srcY));
                        }
                    }
                    bmpOut.Save(saveFileName + "_" + row + col + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    bmpOut.Dispose();
                }
            }
        }

        private void btnSaveUpLeft_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(tempFileName) + (tempFileName.Length == 0 ? "" : "_00");
            saveFileDialog1.Filter = "PNGファイル(*.png)|*.png";
            saveFileDialog1.ShowDialog();

            string saveFileName = saveFileDialog1.FileName;
            if (string.IsNullOrEmpty(saveFileName)) return;

            Bitmap bmpOut = new Bitmap(cell.Size.X, cell.Size.Y);
            for (int dstY = 0, srcY = 0; dstY < bmpOut.Height; ++dstY, ++srcY)
            {
                for (int dstX = 0, srcX = 0; dstX < bmpOut.Width; ++dstX, ++srcX)
                {
                    bmpOut.SetPixel(dstX, dstY, bmpWork.GetPixel(srcX, srcY));
                }
            }
            bmpOut.Save(saveFileName, System.Drawing.Imaging.ImageFormat.Png);
            bmpOut.Dispose();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            bmpWork = new Bitmap(pictArea.Width, pictArea.Height);
            pictArea.Image = bmpWork;

            cmbCellType.Enabled = true;
        }

        private void cmbCellType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cell.Cols = CellTypes[cmbCellType.SelectedIndex].Value.Cols;
            cell.Rows = CellTypes[cmbCellType.SelectedIndex].Value.Rows;
            cell.Size = CellTypes[cmbCellType.SelectedIndex].Value.Size;

            bmpWork = new Bitmap(cell.Cols * cell.Size.X, cell.Rows * cell.Size.Y);
            pictArea.Width = bmpWork.Width;
            pictArea.Height = bmpWork.Height;
        }

        private Point GetLocation()
        {
            Point p = new Point(
                System.Windows.Forms.Cursor.Position.X - (this.Location.X + pictArea.Location.X + 9),
                System.Windows.Forms.Cursor.Position.Y - (this.Location.Y + pictArea.Location.Y + 31)
            );

            p.X /= cell.Size.X;
            p.Y /= cell.Size.Y;
            p.X *= cell.Size.X;
            p.Y *= cell.Size.Y;

            if (p.X >= pictArea.Width)
            {
                p.X = pictArea.Width - cell.Size.X;
            }

            if (p.Y >= pictArea.Height)
            {
                p.Y = pictArea.Height - cell.Size.Y;
            }

            return p;
        }

        private void SwapBmp(Point src, Point dst)
        {
            Color tempPix;

            for (int dstY = dst.Y, srcY = src.Y; dstY < (dst.Y + cell.Size.Y); ++dstY, ++srcY)
            {
                for (int dstX = dst.X, srcX = src.X; dstX < (dst.X + cell.Size.X); ++dstX, ++srcX)
                {
                    tempPix = bmpWork.GetPixel(srcX, srcY);
                    bmpWork.SetPixel(srcX, srcY, bmpWork.GetPixel(dstX, dstY));
                    bmpWork.SetPixel(dstX, dstY, tempPix);
                }
            }
        }

        private void ImportBmp(string strFileName, Point size, Point cur)
        {
            Bitmap bmpSrc = new Bitmap(strFileName);

            if ((size.X + size.Y) == 0)
            {
                size = new Point(bmpSrc.Width, bmpSrc.Height);
            }

            for (int dstY = cur.Y, srcY = 0;
                (dstY < (cur.Y + size.Y)) &&
                (dstY < bmpWork.Height) &&
                (srcY < bmpSrc.Height);
                ++dstY, ++srcY
            )
            {
                for (int dstX = cur.X, srcX = 0;
                    (dstX < (cur.X + size.X)) &&
                    (dstX < bmpWork.Width) &&
                    (srcX < bmpSrc.Width);
                    ++dstX, ++srcX
                )
                {
                    bmpWork.SetPixel(dstX, dstY, bmpSrc.GetPixel(srcX, srcY));
                }
            }

            bmpSrc.Dispose();
        }
    }
}
