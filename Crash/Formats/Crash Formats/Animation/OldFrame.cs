using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldFrame
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
            if (data.Length < 56 + vertexcount * 6)
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
            return new OldFrame(modeleid,xoffset,yoffset,zoffset,x1,y1,z1,x2,y2,z2,xglobal,yglobal,zglobal,vertices);
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

        public OldFrame(int modeleid,int xoffset,int yoffset,int zoffset,int x1,int y1,int z1,int x2,int y2,int z2,int xglobal,int yglobal,int zglobal,IEnumerable<OldFrameVertex> vertices)
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
        }

        public int ModelEID
        {
            get { return modeleid; }
        }

        public int XOffset
        {
            get { return xoffset; }
        }

        public int YOffset
        {
            get { return yoffset; }
        }

        public int ZOffset
        {
            get { return zoffset; }
        }

        public int X1
        {
            get { return x1; }
        }

        public int Y1
        {
            get { return y1; }
        }

        public int Z1
        {
            get { return z1; }
        }

        public int X2
        {
            get { return x2; }
        }

        public int Y2
        {
            get { return y2; }
        }

        public int Z2
        {
            get { return z2; }
        }

        public int XGlobal
        {
            get { return xglobal; }
        }

        public int YGlobal
        {
            get { return yglobal; }
        }

        public int ZGlobal
        {
            get { return zglobal; }
        }

        public IList<OldFrameVertex> Vertices
        {
            get { return vertices; }
        }

        public byte[] Save()
        {
            byte[] data = new byte [56 + vertices.Count * 6 + 1];
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
            return data;
        }
    }
}
