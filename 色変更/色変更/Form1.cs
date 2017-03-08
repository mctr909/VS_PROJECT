using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 色変更
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pict.AllowDrop = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void pict_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
            else e.Effect = DragDropEffects.None;
        }

        private void pict_DragDrop(object sender, DragEventArgs e)
        {
            string[] strFileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (strFileNames.Length < 1) return;
            if (!System.IO.File.Exists(strFileNames[0])) return;

            設定 fm = Program.MainForm;

            for (int i = 0; i < fm.BmpWork.Length; ++i)
            {
                fm.BmpWork[i] = null;
            }

            Bitmap input = new Bitmap(strFileNames[0]);
            fm.BmpWork[0] = new Bitmap(input.Width, input.Height);
            Graphics g = Graphics.FromImage(fm.BmpWork[0]);
            g.DrawImage(input, 0, 0, fm.BmpWork[0].Width, fm.BmpWork[0].Height);
            input.Dispose();

            pict.Width = fm.BmpWork[0].Width;
            pict.Height = fm.BmpWork[0].Height;
            this.Width = fm.BmpWork[0].Width + pict.Location.X + 48;
            this.Height = fm.BmpWork[0].Height + pict.Location.Y + 64;
            pict.Image = fm.BmpWork[0];

            fm.btnUndo.Enabled = false;
            fm.btnRedo.Enabled = false;
        }

        public void Draw(Bitmap bmp)
        {
            pict.Image = bmp;
        }
    }
}
