using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace GMLConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string fname = openFileDialog1.FileName;
            if (string.IsNullOrEmpty(fname) || !File.Exists(fname)) return;

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);

            XmlReader xml = XmlReader.Create(fname);

            while (!xml.EOF)
            {
                GMLStruct.WaterArea wa = new GMLStruct.WaterArea();
                wa.Read(xml);

                if (wa.Surface.Lines == null || wa.Surface.Lines.Count == 0) continue;

                GMLStruct.SLine[] pa = wa.Surface.Lines.ToArray();
                PointF[] pfa = new PointF[wa.Surface.Points.Count];

                for (int i = 0; i < pfa.Length; i++)
                {
                    pfa[i] = new PointF(3200 * (float)(pa[i].X - 139.14424), 3200 * (float)(pa[i].Y - 35.72316));
                }
                g.DrawCurve(new Pen(Color.Blue, 1.0f), pfa);
            }

            pictureBox1.Image = bmp;
        }
    }
}