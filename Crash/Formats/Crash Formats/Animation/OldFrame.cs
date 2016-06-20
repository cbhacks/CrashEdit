using System;
using System.IO;
using System.Collections.Generic;

namespace Crash
{
    public class OldFrame
    {
        public static OldFrame Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 56)
            {
                ErrorManager.SignalError("OldFrame: Data is too short");
            }
            int vertexcount = BitConv.FromInt32(data,0);
            if (vertexcount < 0 || vertexcount > Chunk.Length / 6)
            {
                ErrorManager.SignalError("OldFrame: Vertex count is invalid");
            }
            if (data.Length < 56 + vertexcount * 6 + 2)
            {
                ErrorManager.SignalError("OldFrame: Data is too short");
            }
            int modeleid = BitConv.FromInt32(data,4);
            int xoffset = BitConv.FromInt32(data,8);
            int yoffset = BitConv.FromInt32(data,12);
            int zoffset = BitConv.FromInt32(data,16);
            int x1 = BitConv.FromInt32(data,20);
            int y1 = BitConv.FromInt32(data,24);
            int z1 = BitConv.FromInt32(data,28);
            int x2 = BitConv.FromInt32(data,32);
            int y2 = BitConv.FromInt32(data,36);
            int z2 = BitConv.FromInt32(data,40);
            int xglobal = BitConv.FromInt32(data,44);
            int yglobal = BitConv.FromInt32(data,48);
            int zglobal = BitConv.FromInt32(data,52);
            OldFrameVertex[] vertices = new OldFrameVertex [vertexcount];
            for (int i = 0;i < vertexcount;i++)
            {
                byte[] vertexdata = new byte [6];
                Array.Copy(data,56 + i * 6,vertexdata,0,vertexdata.Length);
                vertices[i] = OldFrameVertex.Load(vertexdata);
            }
            short unknown = BitConv.FromInt16(data,56 + vertexcount * 6);
            return new OldFrame(modeleid,xoffset,yoffset,zoffset,x1,y1,z1,x2,y2,z2,xglobal,yglobal,zglobal,vertices,unknown);
        }

        private int modeleid;
        private int xoffset;
        private int yoffset;
        private int zoffset;
        private int x1;
        private int y1;
        private int z1;
        private int x2;
        private int y2;
        private int z2;
        private int xglobal;
        private int yglobal;
        private int zglobal;
        private List<OldFrameVertex> vertices;
        private short unknown;

        public OldFrame(int modeleid,int xoffset,int yoffset,int zoffset,int x1,int y1,int z1,int x2,int y2,int z2,int xglobal,int yglobal,int zglobal,IEnumerable<OldFrameVertex> vertices,short unknown)
        {
            this.modeleid = modeleid;
            this.xoffset = xoffset;
            this.yoffset = yoffset;
            this.zoffset = zoffset;
            this.x1 = x1;
            this.y1 = y1;
            this.z1 = z1;
            this.x2 = x2;
            this.y2 = y2;
            this.z2 = z2;
            this.xglobal = xglobal;
            this.yglobal = yglobal;
            this.zglobal = zglobal;
            this.vertices = new List<OldFrameVertex>(vertices);
            this.unknown = unknown;
        }

        public int ModelEID
        {
            get { return modeleid; }
        }

        public int XOffset
        {
            get { return xoffset; }
            set { xoffset = value; }
        }

        public int YOffset
        {
            get { return yoffset; }
            set { yoffset = value; }
        }

        public int ZOffset
        {
            get { return zoffset; }
            set { zoffset = value; }
        }

        public int X1
        {
            get { return x1; }
            set { x1 = value; }
        }

        public int Y1
        {
            get { return y1; }
            set { y1 = value; }
        }

        public int Z1
        {
            get { return z1; }
            set { z1 = value; }
        }

        public int X2
        {
            get { return x2; }
            set { x2 = value; }
        }

        public int Y2
        {
            get { return y2; }
            set { y2 = value; }
        }

        public int Z2
        {
            get { return z2; }
            set { z2 = value; }
        }

        public int XGlobal
        {
            get { return xglobal; }
            set { xglobal = value; }
        }

        public int YGlobal
        {
            get { return yglobal; }
            set { yglobal = value; }
        }

        public int ZGlobal
        {
            get { return zglobal; }
            set { zglobal = value; }
        }

        public IList<OldFrameVertex> Vertices
        {
            get { return vertices; }
        }

        public short Unknown
        {
            get { return unknown; }
            set { unknown = value; }
        }

        public byte[] Save()
        {
            byte[] data = new byte [56 + vertices.Count * 6 + 2];
            BitConv.ToInt32(data,0,vertices.Count);
            BitConv.ToInt32(data,4,modeleid);
            BitConv.ToInt32(data,8,xoffset);
            BitConv.ToInt32(data,12,yoffset);
            BitConv.ToInt32(data,16,zoffset);
            BitConv.ToInt32(data,20,x1);
            BitConv.ToInt32(data,24,y1);
            BitConv.ToInt32(data,28,z1);
            BitConv.ToInt32(data,32,x2);
            BitConv.ToInt32(data,36,y2);
            BitConv.ToInt32(data,40,z2);
            BitConv.ToInt32(data,44,xglobal);
            BitConv.ToInt32(data,48,yglobal);
            BitConv.ToInt32(data,52,zglobal);
            for (int i = 0;i < vertices.Count;i++)
            {
                vertices[i].Save().CopyTo(data,56 + i * 6);
            }
            BitConv.ToInt16(data,56 + vertices.Count * 6,unknown);
            return data;
        }

        public byte[] ToOBJ(OldModelEntry model)
        {
            long xorigin = 0;
            long yorigin = 0;
            long zorigin = 0;
            foreach (OldFrameVertex vertex in vertices)
            {
                xorigin += vertex.X;
                yorigin += vertex.Y;
                zorigin += vertex.Z;
            }
            xorigin /= vertices.Count;
            yorigin /= vertices.Count;
            zorigin /= vertices.Count;
            xorigin -= xoffset;
            yorigin -= yoffset;
            zorigin -= zoffset;
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter obj = new StreamWriter(stream))
                {
                    obj.WriteLine("# Vertices");
                    foreach (OldFrameVertex vertex in vertices)
                    {
                        obj.WriteLine("v {0} {1} {2}",vertex.X - xorigin,vertex.Y - yorigin,vertex.Z - zorigin);
                    }
                    obj.WriteLine();
                    obj.WriteLine("# Polygons");
                    foreach (OldModelPolygon polygon in model.Polygons)
                    {
                        obj.WriteLine("f {0} {1} {2}",polygon.VertexA / 6 + 1,polygon.VertexB / 6 + 1,polygon.VertexC / 6 + 1);
                    }
                }
                return stream.ToArray();
            }
        }
    }
}
