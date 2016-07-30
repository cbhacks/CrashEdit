using System.IO;
using System.Collections.Generic;

namespace Crash
{
    public sealed class NewSceneryEntry : Entry
    {
        private byte[] info;
        private List<NewSceneryVertex> vertices;
        private List<SceneryTriangle> triangles;
        private List<SceneryQuad> quads;
        private byte[] item4;
        private List<SceneryColor> colors;
        private byte[] item6;

        public NewSceneryEntry(byte[] info,IEnumerable<NewSceneryVertex> vertices,IEnumerable<SceneryTriangle> triangles,IEnumerable<SceneryQuad> quads,byte[] item4,IEnumerable<SceneryColor> colors,byte[] item6,int eid, int size) : base(eid, size)
        {
            this.info = info;
            this.vertices = new List<NewSceneryVertex>(vertices);
            this.triangles = new List<SceneryTriangle>(triangles);
            this.quads = new List<SceneryQuad>(quads);
            this.item4 = item4;
            this.colors = new List<SceneryColor>(colors);
            this.item6 = item6;
        }

        public override int Type
        {
            get { return 3; }
        }

        public byte[] Info
        {
            get { return info; }
        }

        public IList<NewSceneryVertex> Vertices
        {
            get { return vertices; }
        }

        public IList<SceneryTriangle> Triangles
        {
            get { return triangles; }
        }

        public IList<SceneryQuad> Quads
        {
            get { return quads; }
        }

        public byte[] Item4
        {
            get { return item4; }
        }

        public IList<SceneryColor> Colors
        {
            get { return colors; }
        }

        public byte[] Item6
        {
            get { return item6; }
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

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [7][];
            items[0] = info;
            items[1] = new byte [vertices.Count * 6];
            for (int i = 0;i < vertices.Count;i++)
            {
                vertices[i].SaveXY().CopyTo(items[1],(vertices.Count - 1 - i) * 4);
                vertices[i].SaveZ().CopyTo(items[1],vertices.Count * 4 + i * 2);
            }
            items[2] = new byte [triangles.Count * 6];
            for (int i = 0;i < triangles.Count;i++)
            {
                triangles[i].SaveA().CopyTo(items[2],(triangles.Count - 1 - i) * 4);
                triangles[i].SaveB().CopyTo(items[2],triangles.Count * 4 + i * 2);
            }
            items[3] = new byte [quads.Count * 8];
            for (int i = 0;i < quads.Count;i++)
            {
                quads[i].Save().CopyTo(items[3],i * 8);
            }
            items[4] = item4;
            items[5] = new byte [colors.Count * 4];
            for (int i = 0;i < colors.Count;i++)
            {
                items[5][i * 4 + 0] = colors[i].Red;
                items[5][i * 4 + 1] = colors[i].Green;
                items[5][i * 4 + 2] = colors[i].Blue;
                items[5][i * 4 + 3] = colors[i].Extra;
            }
            items[6] = item6;
            return new UnprocessedEntry(items,EID,Type,Size);
        }

        public byte[] ToOBJ()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter obj = new StreamWriter(stream))
                {
                    obj.WriteLine("# Vertices");
                    foreach (NewSceneryVertex vertex in vertices)
                    {
                        if (vertex.Color < Colors.Count)
                        {
                            SceneryColor color = Colors[vertex.Color];
                            obj.WriteLine("v {0} {1} {2} {3} {4} {5}", vertex.X + XOffset / 16, vertex.Y + YOffset / 16, vertex.Z + ZOffset / 16,color.Red / 255.0,color.Green / 255.0,color.Blue / 255.0);
                        }
                        else
                        {
                            obj.WriteLine("v {0} {1} {2}", vertex.X + XOffset / 16, vertex.Y + YOffset / 16, vertex.Z + ZOffset / 16);
                        }
                    }
                    obj.WriteLine();
                    obj.WriteLine("# Triangles");
                    foreach (SceneryTriangle triangle in triangles)
                    {
                        obj.WriteLine("f {0} {1} {2}", triangle.VertexA + 1, triangle.VertexB + 1, triangle.VertexC + 1);
                    }
                    obj.WriteLine();
                    obj.WriteLine("# Quads");
                    foreach (SceneryQuad quad in quads)
                    {
                        obj.WriteLine("f {0} {1} {2} {3}", quad.VertexA + 1, quad.VertexB + 1, quad.VertexC + 1,quad.VertexD + 1);
                    }
                }
                return stream.ToArray();
            }
        }

        public byte[] ToPLY()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter ply = new StreamWriter(stream))
                {
                    int polycount = 0;
                    foreach (SceneryTriangle triangle in triangles)
                    {
                        if (triangle.VertexA < vertices.Count - 1)
                        {
                            if (triangle.VertexB < vertices.Count - 1)
                            {
                                if (triangle.VertexC < vertices.Count - 1)
                                {
                                    polycount++;
                                }
                            }
                        }
                    }
                    foreach (SceneryQuad quad in quads)
                    {
                        if (quad.VertexA < vertices.Count - 1)
                        {
                            if (quad.VertexB < vertices.Count - 1)
                            {
                                if (quad.VertexC < vertices.Count - 1)
                                {
                                    if (quad.VertexD < vertices.Count - 1)
                                    {
                                        polycount++;
                                    }
                                }
                            }
                        }
                    }
                    ply.WriteLine("ply");
                    ply.WriteLine("format ascii 1.0");
                    ply.WriteLine("element vertex {0}",Vertices.Count);
                    ply.WriteLine("property int x");
                    ply.WriteLine("property int y");
                    ply.WriteLine("property int z");
                    ply.WriteLine("property uchar red");
                    ply.WriteLine("property uchar green");
                    ply.WriteLine("property uchar blue");
                    ply.WriteLine("element face {0}",polycount);
                    ply.WriteLine("property list uchar int vertex_index");
                    ply.WriteLine("end_header");
                    foreach (NewSceneryVertex vertex in vertices)
                    {
                        if (vertex.Color < Colors.Count)
                        {
                            SceneryColor color = Colors[vertex.Color];
                            ply.WriteLine("{0} {1} {2} {3} {4} {5}", vertex.X + XOffset / 16, vertex.Y + YOffset / 16, vertex.Z + ZOffset / 16, color.Red, color.Green, color.Blue);
                        }
                        else
                        {
                            ply.WriteLine("{0} {1} {2} 255 0 255", vertex.X + XOffset / 16, vertex.Y + YOffset / 16, vertex.Z + ZOffset / 16);
                        }
                    }
                    foreach (SceneryTriangle triangle in triangles)
                    {
                        if (triangle.VertexA < vertices.Count - 1)
                        {
                            if (triangle.VertexB < vertices.Count - 1)
                            {
                                if (triangle.VertexC < vertices.Count - 1)
                                {
                                    ply.WriteLine("3 {0} {1} {2}", triangle.VertexA, triangle.VertexB, triangle.VertexC);
                                }
                            }
                        }
                    }
                    foreach (SceneryQuad quad in quads)
                    {
                        if (quad.VertexA < vertices.Count - 1)
                        {
                            if (quad.VertexB < vertices.Count - 1)
                            {
                                if (quad.VertexC < vertices.Count - 1)
                                {
                                    if (quad.VertexD < vertices.Count - 1)
                                    {
                                        ply.WriteLine("4 {0} {1} {2} {3}", quad.VertexA, quad.VertexB, quad.VertexC, quad.VertexD);
                                    }
                                }
                            }
                        }
                    }
                }
                return stream.ToArray();
            }
        }

        /*public byte[] ToCOLLADA()
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
                    foreach (NewSceneryVertex vertex in vertices)
                    {
                        xmlwriter.WriteValue(vertex.X + XOffset / 16);
                        xmlwriter.WriteWhitespace(" ");
                        xmlwriter.WriteValue(vertex.Y + YOffset / 16);
                        xmlwriter.WriteWhitespace(" ");
                        xmlwriter.WriteValue(vertex.Z + ZOffset / 16);
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
                    xmlwriter.WriteAttributeString("name", "Y");
                    xmlwriter.WriteAttributeString("type", "float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("source");
                    xmlwriter.WriteAttributeString("id", "colors");
                    xmlwriter.WriteStartElement("float_array");
                    xmlwriter.WriteAttributeString("id", "colors-array");
                    xmlwriter.WriteAttributeString("count", (vertices.Count * 3).ToString());
                    foreach (NewSceneryVertex vertex in vertices)
                    {
                        if (vertex.Color < Colors.Count)
                        {
                            SceneryColor color = Colors[vertex.Color];
                            xmlwriter.WriteValue(color.Red / 256.0);
                            xmlwriter.WriteWhitespace(" ");
                            xmlwriter.WriteValue(color.Green / 256.0);
                            xmlwriter.WriteWhitespace(" ");
                            xmlwriter.WriteValue(color.Blue / 256.0);
                            xmlwriter.WriteWhitespace(" ");
                        }
                        else
                        {
                            xmlwriter.WriteValue(256.0 / 256.0);
                            xmlwriter.WriteWhitespace(" ");
                            xmlwriter.WriteValue(0 / 256.0);
                            xmlwriter.WriteWhitespace(" ");
                            xmlwriter.WriteValue(256.0 / 256.0);
                            xmlwriter.WriteWhitespace(" ");
                        }
                    }
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("technique_common");
                    xmlwriter.WriteStartElement("accessor");
                    xmlwriter.WriteAttributeString("source", "#colors-array");
                    xmlwriter.WriteAttributeString("count", vertices.Count.ToString());
                    xmlwriter.WriteAttributeString("stride", "3");
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name", "R");
                    xmlwriter.WriteAttributeString("type", "float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name", "G");
                    xmlwriter.WriteAttributeString("type", "float");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("param");
                    xmlwriter.WriteAttributeString("name", "B");
                    xmlwriter.WriteAttributeString("type", "float");
                    xmlwriter.WriteEndElement();
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
                    xmlwriter.WriteAttributeString("semantic", "COLOR");
                    xmlwriter.WriteAttributeString("source", "colors");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("triangles");
                    xmlwriter.WriteAttributeString("count", triangles.Count.ToString());
                    xmlwriter.WriteStartElement("input");
                    xmlwriter.WriteAttributeString("semantic", "VERTEX");
                    xmlwriter.WriteAttributeString("source", "vertices");
                    xmlwriter.WriteAttributeString("offset", "0");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteStartElement("p");
                    foreach (SceneryTriangle triangle in triangles)
                    {
                        xmlwriter.WriteValue(triangle.VertexA);
                        xmlwriter.WriteWhitespace(" ");
                        xmlwriter.WriteValue(triangle.VertexB);
                        xmlwriter.WriteWhitespace(" ");
                        xmlwriter.WriteValue(triangle.VertexC);
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
        }*/
    }
}
