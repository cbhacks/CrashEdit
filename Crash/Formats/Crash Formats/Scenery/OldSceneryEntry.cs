using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldSceneryEntry : Entry
    {
        private byte[] info;
        private List<OldSceneryPolygon> polygons;
        private List<OldSceneryVertex> vertices;
        private byte[] extradata;

        public OldSceneryEntry(byte[] info,IEnumerable<OldSceneryPolygon> polygons,IEnumerable<OldSceneryVertex> vertices,byte[] extradata,int eid, int size) : base(eid, size)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            this.info = info;
            this.polygons = new List<OldSceneryPolygon>(polygons);
            this.vertices = new List<OldSceneryVertex>(vertices);
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

        public IList<OldSceneryPolygon> Polygons
        {
            get { return polygons; }
        }

        public IList<OldSceneryVertex> Vertices
        {
            get { return vertices; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [extradata == null ? 3 : 4][];
            items[0] = info;
            items[1] = new byte [polygons.Count * 8];
            for (int i = 0;i < polygons.Count;i++)
            {
                polygons[i].Save().CopyTo(items[1],i * 8);
            }
            items[2] = new byte [vertices.Count * 8];
            for (int i = 0;i < vertices.Count;i++)
            {
                vertices[i].Save().CopyTo(items[2],i * 8);
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
                    foreach (OldSceneryVertex vertex in vertices)
                    {
                        obj.WriteLine("v {0} {1} {2}",vertex.X,vertex.Y,vertex.Z);
                    }
                    obj.WriteLine();
                    obj.WriteLine("# Polygons");
                    foreach (OldSceneryPolygon polygon in polygons)
                    {
                        obj.WriteLine("f {0} {1} {2}",polygon.VertexA + 1,polygon.VertexB + 1,polygon.VertexC + 1);
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
                    foreach (OldSceneryVertex vertex in vertices)
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
                    xmlwriter.WriteAttributeString("name","Y");
                    xmlwriter.WriteAttributeString("type","float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("source");
                    xmlwriter.WriteAttributeString("id","colors");
                    xmlwriter.WriteStartElement("float_array");
                    xmlwriter.WriteAttributeString("id","colors-array");
                    xmlwriter.WriteAttributeString("count",(vertices.Count * 3).ToString());
                    foreach (OldSceneryVertex vertex in vertices)
                    {
                        xmlwriter.WriteValue(vertex.Red / 256.0);
                        xmlwriter.WriteWhitespace(" ");
                        xmlwriter.WriteValue(vertex.Green / 256.0);
                        xmlwriter.WriteWhitespace(" ");
                        xmlwriter.WriteValue(vertex.Blue / 256.0);
                        xmlwriter.WriteWhitespace(" ");
                    }
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("technique_common");
                    xmlwriter.WriteStartElement("accessor");
                    xmlwriter.WriteAttributeString("source","#colors-array");
                    xmlwriter.WriteAttributeString("count",vertices.Count.ToString());
                    xmlwriter.WriteAttributeString("stride","3");
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name","R");
                    xmlwriter.WriteAttributeString("type","float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name","G");
                    xmlwriter.WriteAttributeString("type","float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name","B");
                    xmlwriter.WriteAttributeString("type","float");
                    xmlwriter.WriteEndElement();
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
                    xmlwriter.WriteAttributeString("semantic","COLOR");
                    xmlwriter.WriteAttributeString("source","colors");
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
                    foreach (OldSceneryPolygon polygon in polygons)
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
