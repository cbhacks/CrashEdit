using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Crash
{
    public sealed class ProtoSceneryEntry : Entry
    {
        private byte[] info;
        private List<ProtoSceneryPolygon> polygons;
        private List<ProtoSceneryVertex> vertices;
        private byte[] extradata;

        public ProtoSceneryEntry(byte[] info,IEnumerable<ProtoSceneryPolygon> polygons,IEnumerable<ProtoSceneryVertex> vertices,byte[] extradata,int eid,int size) : base(eid,size)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            this.info = info;
            this.polygons = new List<ProtoSceneryPolygon>(polygons);
            this.vertices = new List<ProtoSceneryVertex>(vertices);
            this.extradata = extradata;
        }

        public override int Type
        {
            get { return 3; }
        }

        public byte[] Info
        {
            get { return info; }
        }

        public byte[] ExtraData
        {
            get { return extradata; }
        }

        public int XOffset
        {
            get { return BitConv.FromInt32(info,0); }
        }

        public int YOffset
        {
            get { return BitConv.FromInt32(info,4); }
        }

        public int ZOffset
        {
            get { return BitConv.FromInt32(info,8); }
        }

        public IList<ProtoSceneryPolygon> Polygons
        {
            get { return polygons; }
        }

        public IList<ProtoSceneryVertex> Vertices
        {
            get { return vertices; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [extradata == null ? 3 : 4][];
            items[0] = info;
            items[1] = new byte [polygons.Count * 12];
            for (int i = 0;i < polygons.Count;i++)
            {
                polygons[i].Save().CopyTo(items[1],i * 12);
            }
            items[2] = new byte [vertices.Count * 6 + vertices.Count * 6 % 4];
            for (int i = 0;i < vertices.Count;i++)
            {
                vertices[i].Save().CopyTo(items[2],i * 6);
            }
            if (extradata != null)
            {
                items[3] = extradata;
            }
            return new UnprocessedEntry(items,EID,Type,Size);
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
                        obj.WriteLine("v {0} {1} {2}",vertex.X + XOffset,vertex.Y + YOffset, vertex.Z + ZOffset);
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
                    xmlwriter.WriteAttributeString("xmlns","http://www.collada.org/2005/11/COLLADASchema");
                    xmlwriter.WriteAttributeString("version","1.4.1");
                    xmlwriter.WriteStartElement("library_geometries");
                    xmlwriter.WriteStartElement("geometry");
                    xmlwriter.WriteStartElement("mesh");
                    xmlwriter.WriteStartElement("source");
                    xmlwriter.WriteAttributeString("id","positions");
                    xmlwriter.WriteStartElement("float_array");
                    xmlwriter.WriteAttributeString("id","positions-array");
                    xmlwriter.WriteAttributeString("count",(vertices.Count * 3).ToString());
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
                    xmlwriter.WriteAttributeString("source","#positions-array");
                    xmlwriter.WriteAttributeString("count",vertices.Count.ToString());
                    xmlwriter.WriteAttributeString("stride","3");
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name","X");
                    xmlwriter.WriteAttributeString("type","float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name","Y");
                    xmlwriter.WriteAttributeString("type","float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name","Z");
                    xmlwriter.WriteAttributeString("type","float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("source");
                    xmlwriter.WriteStartElement("float_array");
                    xmlwriter.WriteAttributeString("count",(vertices.Count * 3).ToString());
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("technique_common");
                    xmlwriter.WriteStartElement("accessor");
                    xmlwriter.WriteAttributeString("count",vertices.Count.ToString());
                    xmlwriter.WriteAttributeString("stride","3");
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("vertices");
                    xmlwriter.WriteAttributeString("id","vertices");
                    xmlwriter.WriteStartElement("input");
                    xmlwriter.WriteAttributeString("semantic","POSITION");
                    xmlwriter.WriteAttributeString("source","positions");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("input");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("triangles");
                    xmlwriter.WriteAttributeString("count",polygons.Count.ToString());
                    xmlwriter.WriteStartElement("input");
                    xmlwriter.WriteAttributeString("semantic","VERTEX");
                    xmlwriter.WriteAttributeString("source","vertices");
                    xmlwriter.WriteAttributeString("offset","0");
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
