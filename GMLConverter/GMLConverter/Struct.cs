using System;
using System.Xml;
using System.Collections.Generic;

namespace GMLStruct
{
    public struct SPoint
    {
        public double X;
        public double Y;

        public SPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
    public struct SLine
    {
        public string ID;
        public HashSet<SPoint> Points;

        public static void Read(XmlReader xml, SLine line)
        {
            string[] strLine;
            string[] strPoint;

            if (xml.LocalName == "Curve")
            {
                if (xml.NodeType == XmlNodeType.Element)
                {
                    line.ID = xml.GetAttribute("id");
                    line.Points = new HashSet<GMLStruct.SPoint>();
                }
            }

            if (xml.LocalName == "posList")
            {
                if (xml.NodeType == XmlNodeType.EndElement)
                {
                    strLine = xml.ReadInnerXml().Split('n');
                    foreach (var str in strLine)
                    {
                        strPoint = str.Split(' ');
                        line.Points.Add(new SPoint(Convert.ToDouble(strPoint[1]), Convert.ToDouble(strPoint[0])));
                    }
                }
            }
        }
    }
    public struct SSurface
    {
        public string ID;
        public Dictionary<string, HashSet<SPoint>> Lines;

        public static void Read(XmlReader xml, SSurface surface)
        {
            string curLineID = "";
            string[] strLine;
            string[] strPoint;

            // 面タグ
            if (xml.LocalName == "Surface")
            {
                if (xml.NodeType == XmlNodeType.Element)
                {
                    surface.ID = xml.GetAttribute("id");
                    surface.Lines = new Dictionary<string, HashSet<SPoint>>();
                }
            }

            // 曲線タグ
            if (xml.LocalName == "Curve")
            {
                if (xml.NodeType == XmlNodeType.Element)
                {
                    curLineID = xml.GetAttribute("id");
                    surface.Lines.Add(curLineID, new HashSet<SPoint>());
                }
            }

            // 位置タグ
            if (xml.LocalName == "posList")
            {
                if (xml.NodeType == XmlNodeType.EndElement)
                {
                    strLine = xml.ReadInnerXml().Split('n');
                    foreach (var str in strLine)
                    {
                        strPoint = str.Split(' ');
                        surface.Lines[curLineID].Add(new SPoint(Convert.ToDouble(strPoint[1]), Convert.ToDouble(strPoint[0])));
                    }
                }
            }
        }
    }

    #region 水系パッケージ
    public struct WaterArea
    {
        public string ID;
        public string Type;
        public SSurface Value;

        public void Read(XmlReader xml)
        {
            while (xml.Read())
            {
                if (xml.LocalName == "WA")
                {
                    if (xml.NodeType == XmlNodeType.EndElement) break;
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        ID = xml.GetAttribute("id");
                    }
                }

                if (xml.LocalName == "type")
                {
                    if (xml.NodeType == XmlNodeType.EndElement)
                    {
                        Type = xml.ReadInnerXml();
                    }
                }

                SSurface.Read(xml, Value);
            }
        }
    }
    public struct WaterLine
    {
        public string ID;
        public string Type;
        public SLine Value;
    }
    public struct WaterStructureLine
    {
        public string ID;
        public string Type;
        public SLine Value;
    }
    public struct WaterStructureArea
    {
        public string ID;
        public string Type;
        public SSurface Value;
    }
    #endregion

    #region 構造物パッケージ
    public struct RoadEdge
    {
        public string ID;
        public string Type;
        public string AdminOffice;
        public SSurface Value;
    }
    public struct RoadComponent
    {
        public string ID;
        public string Type;
        public string AdminOffice;
        public SLine Value;
    }
    public struct Railroad
    {
        public string ID;
        public string Type;
        public SLine Value;
    }
    #endregion
}
