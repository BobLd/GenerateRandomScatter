using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GenerateRandomScatter.PascalVOC
{
    [Serializable]
    [XmlRoot("annotation")]
    public class PascalVocAnnotation
    {
        [XmlElement("folder")]
        public string Folder { get; set; }

        [XmlElement("filename")]
        public string FileName { get; set; }

        [XmlElement("path")]
        public string Path { get; set; }

        [XmlElement("source")]
        public PascalVocSource Source { get; set; }

        [XmlElement("size")]
        public PascalVocSize Size { get; set; }

        [XmlElement("segmented")]
        public int Segmented { get; set; }

        [XmlElement("object")]
        public PascalVocObject[] Objects { get; set; }
    }

    public class PascalVocSource
    {
        [XmlElement("database")]
        public string Database { get; set; }
    }

    public class PascalVocSize
    {
        [XmlElement("width")]
        public int Width { get; set; }

        [XmlElement("height")]
        public int Height { get; set; }

        [XmlElement("depth")]
        public int Depth { get; set; }
    }

    public class PascalVocObject
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("pose")]
        public string Pose { get; set; }

        [XmlElement("truncated")]
        public int Truncated { get; set; }

        [XmlElement("difficult")]
        public int Difficult { get; set; }

        [XmlElement("bndbox")]
        public PascalVocObjectBndBox BoundingBox { get; set; }
    }

    public class PascalVocObjectBndBox
    {
        [XmlElement("xmin")]
        public int XMin { get; set; }

        [XmlElement("ymin")]
        public int YMin { get; set; }

        [XmlElement("xmax")]
        public int XMax { get; set; }

        [XmlElement("ymax")]
        public int YMax { get; set; }

        public Rectangle ToRectangle()
        {
            return new Rectangle(XMin, YMin, XMax - XMin, YMax - YMin);
        }

        public static PascalVocObjectBndBox FromRectangle(Rectangle rectangle)
        {
            return new PascalVocObjectBndBox()
            {
                XMin = rectangle.X,
                YMin = rectangle.Y,
                XMax = rectangle.Right,
                YMax = rectangle.Bottom
            };
        }

        public static PascalVocObjectBndBox FromRectangle(RectangleF rectangle)
        {
            return new PascalVocObjectBndBox()
            {
                XMin = (int)rectangle.X,
                YMin = (int)rectangle.Y,
                XMax = (int)rectangle.Right,
                YMax = (int)rectangle.Bottom
            };
        }
    }
}
