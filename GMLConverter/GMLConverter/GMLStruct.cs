using System;
using System.Xml;
using System.Collections.Generic;

namespace GMLStruct
{
    #region 基本構造体
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

        public static void Read(XmlReader xml, ref SLine line)
        {
            string[] strLine;
            string[] strPoint;

            if (xml.LocalName == "Curve")
            {
                if (xml.NodeType == XmlNodeType.Element)
                {
                    line.ID = xml.GetAttribute("gml:id");
                    line.Points = new HashSet<GMLStruct.SPoint>();
                }
            }

            if (xml.LocalName == "posList")
            {
                if (xml.NodeType == XmlNodeType.Element)
                {
                    strLine = xml.ReadInnerXml().Split('\n');
                    foreach (var str in strLine)
                    {
                        if (string.IsNullOrWhiteSpace(str)) continue;
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
        public string curLineID;

        public static void Read(XmlReader xml, ref SSurface surface)
        {
            string[] strLine;
            string[] strPoint;

            // 面タグ
            if (xml.LocalName == "Surface")
            {
                if (xml.NodeType == XmlNodeType.Element)
                {
                    surface.ID = xml.GetAttribute("gml:id");
                    surface.Lines = new Dictionary<string, HashSet<SPoint>>();
                }
            }

            // 曲線タグ
            if (xml.LocalName == "Curve")
            {
                if (xml.NodeType == XmlNodeType.Element)
                {
                    surface.curLineID = xml.GetAttribute("gml:id");
                    surface.Lines.Add(surface.curLineID, new HashSet<SPoint>());
                }
            }

            // 位置タグ
            if (xml.LocalName == "posList")
            {
                if (xml.NodeType == XmlNodeType.Element)
                {
                    strLine = xml.ReadInnerXml().Split('\n');
                    foreach (var str in strLine)
                    {
                        if (string.IsNullOrWhiteSpace(str)) continue;
                        strPoint = str.Split(' ');
                        surface.Lines[surface.curLineID].Add(new SPoint(Convert.ToDouble(strPoint[1]), Convert.ToDouble(strPoint[0])));
                    }
                }
            }
        }
    }
    #endregion

    #region 水系パッケージ
    public struct WaterLine
    {
        public string ID;
        public string Type;
        public SLine Line;

        public void Read(XmlReader xml)
        {
            while (xml.Read())
            {
                if (xml.LocalName == "WL")
                {
                    if (xml.NodeType == XmlNodeType.EndElement) break;
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        ID = xml.GetAttribute("gml:id");
                    }
                }

                if (xml.LocalName == "type")
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        Type = xml.ReadInnerXml();
                    }
                }

                SLine.Read(xml, ref Line);
            }
        }
    }
    public struct WaterArea
    {
        public string ID;
        public string Type;
        public SSurface Surface;

        public void Read(XmlReader xml)
        {
            while (xml.Read())
            {
                if (xml.LocalName == "WA")
                {
                    if (xml.NodeType == XmlNodeType.EndElement) break;
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        ID = xml.GetAttribute("gml:id");
                    }
                }

                if (xml.LocalName == "type")
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        Type = xml.ReadInnerXml();
                    }
                }

                SSurface.Read(xml, ref Surface);
            }
        }
    }
    public struct WaterStructureLine
    {
        public string ID;
        public string Type;
        public SLine Line;

        public void Read(XmlReader xml)
        {
            while (xml.Read())
            {
                if (xml.LocalName == "WStrL")
                {
                    if (xml.NodeType == XmlNodeType.EndElement) break;
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        ID = xml.GetAttribute("gml:id");
                    }
                }

                if (xml.LocalName == "type")
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        Type = xml.ReadInnerXml();
                    }
                }

                SLine.Read(xml, ref Line);
            }
        }
    }
    public struct WaterStructureArea
    {
        public string ID;
        public string Type;
        public SSurface Surface;

        public void Read(XmlReader xml)
        {
            while (xml.Read())
            {
                if (xml.LocalName == "WStrA")
                {
                    if (xml.NodeType == XmlNodeType.EndElement) break;
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        ID = xml.GetAttribute("gml:id");
                    }
                }

                if (xml.LocalName == "type")
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        Type = xml.ReadInnerXml();
                    }
                }

                SSurface.Read(xml, ref Surface);
            }
        }
    }
    #endregion

    #region 構造物パッケージ
    public struct RoadComponent
    {
        public string ID;
        public string Type;
        public string AdminOffice;
        public SLine Line;

        public void Read(XmlReader xml)
        {
            while (xml.Read())
            {
                if (xml.LocalName == "RdCompt")
                {
                    if (xml.NodeType == XmlNodeType.EndElement) break;
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        ID = xml.GetAttribute("gml:id");
                    }
                }

                if (xml.LocalName == "type")
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        Type = xml.ReadInnerXml();
                    }
                }

                if (xml.LocalName == "admOffice")
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        AdminOffice = xml.ReadInnerXml();
                    }
                }

                SLine.Read(xml, ref Line);
            }
        }
    }
    public struct RoadEdge
    {
        public string ID;
        public string Type;
        public string AdminOffice;
        public SLine Line;

        public void Read(XmlReader xml)
        {
            while (xml.Read())
            {
                if (xml.LocalName == "RdEdg")
                {
                    if (xml.NodeType == XmlNodeType.EndElement) break;
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        ID = xml.GetAttribute("gml:id");
                    }
                }

                if (xml.LocalName == "type")
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        Type = xml.ReadInnerXml();
                    }
                }

                if (xml.LocalName == "admOffice")
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        AdminOffice = xml.ReadInnerXml();
                    }
                }

                SLine.Read(xml, ref Line);
            }
        }
    }
    public struct Railroad
    {
        public string ID;
        public string Type;
        public SLine Line;

        public void Read(XmlReader xml)
        {
            while (xml.Read())
            {
                if (xml.LocalName == "RailCL")
                {
                    if (xml.NodeType == XmlNodeType.EndElement) break;
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        ID = xml.GetAttribute("gml:id");
                    }
                }

                if (xml.LocalName == "type")
                {
                    if (xml.NodeType == XmlNodeType.Element)
                    {
                        Type = xml.ReadInnerXml();
                    }
                }

                SLine.Read(xml, ref Line);
            }
        }
    }
    #endregion
}
