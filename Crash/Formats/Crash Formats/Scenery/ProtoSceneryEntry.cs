using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Crash
{
    public sealed class ProtoSceneryEntry : Entry
    {
        private readonly List<ProtoSceneryPolygon> polygons;
        private readonly List<ProtoSceneryVertex> vertices;
        private readonly List<OldModelStruct> structs;

        public ProtoSceneryEntry(byte[] info, IEnumerable<ProtoSceneryPolygon> polygons, IEnumerable<ProtoSceneryVertex> vertices, IEnumerable<OldModelStruct> structs, short? pad, int eid) : base(eid)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            this.polygons = new List<ProtoSceneryPolygon>(polygons);
            this.vertices = new List<ProtoSceneryVertex>(vertices);
            this.structs = new List<OldModelStruct>(structs);
            Pad = pad;
        }

        public override int Type => 3;
        public byte[] Info { get; }
        public short? Pad { get; }

        public int XOffset
        {
            get => BitConv.FromInt32(Info, 0);
            set => BitConv.ToInt32(Info, 0, value);
        }

        public int YOffset
        {
            get => BitConv.FromInt32(Info, 4);
            set => BitConv.ToInt32(Info, 4, value);
        }

        public int ZOffset
        {
            get => BitConv.FromInt32(Info, 8);
            set => BitConv.ToInt32(Info, 8, value);
        }

        public int TPAGCount => BitConv.FromInt32(Info, 0x18);
        public int GetTPAG(int idx) => BitConv.FromInt32(Info, 0x20 + 4 * idx);

        public IList<ProtoSceneryPolygon> Polygons => polygons;
        public IList<ProtoSceneryVertex> Vertices => vertices;
        public IList<OldModelStruct> Structs => structs;

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte[3][];
            items[0] = Info;
            items[1] = new byte[polygons.Count * 12];
            for (int i = 0; i < polygons.Count; i++)
            {
                polygons[i].Save().CopyTo(items[1], i * 12);
            }
            items[2] = new byte[vertices.Count * 6 + (!Pad.HasValue ? 0 : 2)];
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].Save().CopyTo(items[2], i * 6);
            }
            if (Pad.HasValue)
            {
                BitConv.ToInt16(items[2], vertices.Count * 6, Pad.Value);
            }
            return new UnprocessedEntry(items, EID, Type);
        }

        public byte[] ToOBJ()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter obj = new StreamWriter(stream))
                {
                    obj.WriteLine("# Vertices");
                    foreach (ProtoSceneryVertex vertex in vertices)
                    {
                        obj.WriteLine("v {0} {1} {2}", vertex.X + XOffset, vertex.Y + YOffset, vertex.Z + ZOffset);
                    }
                    obj.WriteLine();
                    obj.WriteLine("# Polygons");
                    foreach (ProtoSceneryPolygon polygon in polygons)
                    {
                        obj.WriteLine("f {0} {1} {2}", polygon.VertexA + 1, polygon.VertexB + 1, polygon.VertexC + 1);
                    }
                }
                return stream.ToArray();
            }
        }

        public byte[] ToCOLLADA()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter textwriter = new StreamWriter(stream))
                using (XmlTextWriter xmlwriter = new XmlTextWriter(textwriter))
                {
                    xmlwriter.WriteStartDocument();
                    xmlwriter.WriteStartElement("COLLADA");
                    xmlwriter.WriteAttributeString("xmlns", "http://www.collada.org/2005/11/COLLADASchema");
                    xmlwriter.WriteAttributeString("version", "1.4.1");
                    xmlwriter.WriteStartElement("library_geometries");
                    xmlwriter.WriteStartElement("geometry");
                    xmlwriter.WriteStartElement("mesh");
                    xmlwriter.WriteStartElement("source");
                    xmlwriter.WriteAttributeString("id", "positions");
                    xmlwriter.WriteStartElement("float_array");
                    xmlwriter.WriteAttributeString("id", "positions-array");
                    xmlwriter.WriteAttributeString("count", (vertices.Count * 3).ToString());
                    foreach (ProtoSceneryVertex vertex in vertices)
                    {
                        xmlwriter.WriteValue(vertex.X);
                        xmlwriter.WriteWhitespace(" ");
                        xmlwriter.WriteValue(vertex.Y);
                        xmlwriter.WriteWhitespace(" ");
                        xmlwriter.WriteValue(vertex.Z);
                        xmlwriter.WriteWhitespace(" ");
                    }
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("technique_common");
                    xmlwriter.WriteStartElement("accessor");
                    xmlwriter.WriteAttributeString("source", "#positions-array");
                    xmlwriter.WriteAttributeString("count", vertices.Count.ToString());
                    xmlwriter.WriteAttributeString("stride", "3");
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name", "X");
                    xmlwriter.WriteAttributeString("type", "float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name", "Y");
                    xmlwriter.WriteAttributeString("type", "float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name", "Z");
                    xmlwriter.WriteAttributeString("type", "float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("source");
                    xmlwriter.WriteStartElement("float_array");
                    xmlwriter.WriteAttributeString("count", (vertices.Count * 3).ToString());
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("technique_common");
                    xmlwriter.WriteStartElement("accessor");
                    xmlwriter.WriteAttributeString("count", vertices.Count.ToString());
                    xmlwriter.WriteAttributeString("stride", "3");
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("vertices");
                    xmlwriter.WriteAttributeString("id", "vertices");
                    xmlwriter.WriteStartElement("input");
                    xmlwriter.WriteAttributeString("semantic", "POSITION");
                    xmlwriter.WriteAttributeString("source", "positions");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("input");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("triangles");
                    xmlwriter.WriteAttributeString("count", polygons.Count.ToString());
                    xmlwriter.WriteStartElement("input");
                    xmlwriter.WriteAttributeString("semantic", "VERTEX");
                    xmlwriter.WriteAttributeString("source", "vertices");
                    xmlwriter.WriteAttributeString("offset", "0");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("p");
                    foreach (ProtoSceneryPolygon polygon in polygons)
                    {
                        xmlwriter.WriteValue(polygon.VertexA);
                        xmlwriter.WriteWhitespace(" ");
                        xmlwriter.WriteValue(polygon.VertexB);
                        xmlwriter.WriteWhitespace(" ");
                        xmlwriter.WriteValue(polygon.VertexC);
                        xmlwriter.WriteWhitespace(" ");
                    }
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndDocument();
                }
                return stream.ToArray();
            }
        }
    }
}
