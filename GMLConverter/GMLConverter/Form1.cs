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
        PointF minP;
        PointF maxP;
        HashSet<GMLStruct.WaterArea> listWA;
        HashSet<GMLStruct.RoadEdge> listWL;
        string types;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string fname = openFileDialog1.FileName;
            if (string.IsNullOrEmpty(fname) || !File.Exists(fname)) return;

            XmlReader xml = XmlReader.Create(fname);

            minP = new PointF(float.MaxValue, float.MaxValue);
            maxP = new PointF(0, 0);
            listWA = new HashSet<GMLStruct.WaterArea>();
            types = "";

            while (!xml.EOF)
            {
                GMLStruct.WaterArea wa = new GMLStruct.WaterArea();
                wa.Read(xml);

                if (wa.Surface.Lines == null || wa.Surface.Lines.Count == 0) continue;

                listWA.Add(wa);

                if (types.IndexOf(wa.Type) < 0)
                {
                    types += wa.Type + "\r\n";
                }

                foreach (var l in wa.Surface.Lines)
                {
                    foreach (var p in l.Value)
                    {
                        if (p.X < minP.X) minP.X = p.X;
                        if (p.Y < minP.Y) minP.Y = p.Y;
                        if (p.X > maxP.X) maxP.X = p.X;
                        if (p.Y > maxP.Y) maxP.Y = p.Y;
                    }
                }
            }

            label1.Text = types;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);

            foreach (var wa in listWA)
            {
                foreach (var l in wa.Surface.Lines)
                {
                    PointF[] points = new PointF[l.Value.Count];
                    int i = 0;
                    foreach (var fp in l.Value)
                    {
                        points[i].X = bmp.Width * (fp.X - minP.X) / (maxP.X - minP.X);
                        points[i].Y = bmp.Height * (fp.Y - minP.Y) / (maxP.Y - minP.Y);
                        i++;
                    }
                    g.FillPolygon(Brushes.Blue, points);
                }
            }
            pictureBox1.Image = bmp;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string fname = openFileDialog1.FileName;
            if (string.IsNullOrEmpty(fname) || !File.Exists(fname)) return;

            XmlReader xml = XmlReader.Create(fname);

            minP = new PointF(float.MaxValue, float.MaxValue);
            maxP = new PointF(0, 0);
            listWL = new HashSet<GMLStruct.RoadEdge>();
            types = "";

            while (!xml.EOF)
            {
                GMLStruct.RoadEdge wa = new GMLStruct.RoadEdge();
                wa.Read(xml);

                if (wa.Line.Points == null || wa.Line.Points.Count == 0) continue;

                if (wa.Type == "真幅道路")
                {
                    listWL.Add(wa);

                    if (types.IndexOf(wa.Type) < 0)
                    {
                        types += wa.Type + "\r\n";
                    }

                    foreach (var p in wa.Line.Points)
                    {
                        if (p.X < minP.X) minP.X = p.X;
                        if (p.Y < minP.Y) minP.Y = p.Y;
                        if (p.X > maxP.X) maxP.X = p.X;
                        if (p.Y > maxP.Y) maxP.Y = p.Y;
                    }
                }
            }

            label1.Text = types;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);

            foreach (var wl in listWL)
            {
                PointF[] points;
                if (wl.Line.Points.Count < 2)
                {
                    points = new PointF[2];
                }
                else
                {
                    points = new PointF[wl.Line.Points.Count];
                }

                int i = 0;
                foreach (var fp in wl.Line.Points)
                {
                    points[i].X = bmp.Width * (fp.X - minP.X) / (maxP.X - minP.X);
                    points[i].Y = bmp.Height * (fp.Y - minP.Y) / (maxP.Y - minP.Y);
                    i++;
                }

                if (i < 2)
                {
                    points[1] = points[0];
                }

                g.DrawLines(new Pen(Color.Blue, 1.0f), points);
            }

            pictureBox1.Image = bmp;
        }
    }
}