using System;
using System.Collections.Generic;

namespace Crash
{
    public class Frame
    {
        public static Frame Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 24)
            {
                ErrorManager.SignalError("Frame: Data is too short");
            }
            int unknown1 = BitConv.FromInt32(data,0);
            int unknown2 = BitConv.FromInt32(data,4);
            short vertexcount = BitConv.FromInt16(data,8);
            if (vertexcount < 0 || vertexcount > Chunk.Length / 3)
            {
                ErrorManager.SignalError("Frame: Vertex count is invalid");
            }
            if (data.Length < 24 + vertexcount * 3)
            {
                ErrorManager.SignalError("Frame: Data is too short");
            }
            short xoffset = BitConv.FromInt16(data,10);
            short yoffset = BitConv.FromInt16(data,12);
            short zoffset = BitConv.FromInt16(data,14);
            int modeleid = BitConv.FromInt32(data,16);
            int headersize = BitConv.FromInt32(data,20);
            if (data.Length < headersize + vertexcount * 3)
            {
                ErrorManager.SignalError("Frame: Data is too short");
            }
            if (headersize < 24)
            {
                ErrorManager.SignalError("Frame: Header size value is invalid");
            }
            int[] settings = new int[(headersize - 24) / 4];
            for (int i = 0;i < (headersize - 24) / 4;i++)
            {
                settings[i] = BitConv.FromInt32(data,24 + i * 4);
            }
            FrameVertex[] vertices = new FrameVertex [vertexcount];
            for (int i = 0;i < vertexcount;i++)
            {
                byte[] vertexdata = new byte [3];
                Array.Copy(data,headersize + i * 3,vertexdata,0,vertexdata.Length);
                vertices[i] = FrameVertex.Load(vertexdata);
            }
            return new Frame(unknown1,unknown2,vertexcount,xoffset,yoffset,zoffset,modeleid,headersize,settings,vertices);
        }

        private int unknown1;
        private int unknown2;
        private short vertexcount;
        private short xoffset;
        private short yoffset;
        private short zoffset;
        private int modeleid;
        private int headersize;
        private int[] settings;
        private List<FrameVertex> vertices;

        public Frame(int unknown1,int unknown2,short vertexcount,short xoffset,short yoffset,short zoffset,int modeleid,int headersize,int[] settings,IEnumerable<FrameVertex> vertices)
        {
            this.unknown1 = unknown1;
            this.unknown2 = unknown2;
            this.vertexcount = vertexcount;
            this.xoffset = xoffset;
            this.yoffset = yoffset;
            this.zoffset = zoffset;
            this.modeleid = modeleid;
            this.headersize = headersize;
            this.settings = settings;
            this.vertices = new List<FrameVertex>(vertices);
        }

        public int ModelEID
        {
            get { return modeleid; }
        }

        public short XOffset
        {
            get { return xoffset; }
            set { xoffset = value; }
        }

        public short YOffset
        {
            get { return yoffset; }
            set { yoffset = value; }
        }

        public short ZOffset
        {
            get { return zoffset; }
            set { zoffset = value; }
        }

        public int HeaderSize
        {
            get { return headersize; }
            set { headersize = value; }
        }

        public int[] Settings
        {
            get { return settings; }
        }

        public short VertexCount
        {
            get { return vertexcount; }
            set { vertexcount = value; }
        }

        public IList<FrameVertex> Vertices
        {
            get { return vertices; }
        }

        public int Unknown1
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        public int Unknown2
        {
            get { return unknown2; }
            set { unknown2 = value; }
        }

        public byte[] Save()
        {
            byte[] result = new byte [headersize + vertices.Count * 3];
            BitConv.ToInt32(result,0,unknown1);
            BitConv.ToInt32(result,4,unknown2);
            BitConv.ToInt16(result,8,vertexcount);
            BitConv.ToInt16(result, 10, yoffset);
            BitConv.ToInt16(result, 12, yoffset);
            BitConv.ToInt16(result, 14, zoffset);
            BitConv.ToInt32(result, 16, modeleid);
            BitConv.ToInt32(result, 20, headersize);
            for (int i = 0; i < (headersize - 24) / 4; i++)
            {
                BitConv.ToInt32(result,24 + i * 4,settings[i]);
            }
            for (int i = 0;i < vertices.Count;i++)
            {
                vertices[i].Save().CopyTo(result,headersize + i * 3);
            }
            return result;
        }

        /*public byte[] ToOBJ(ModelEntry model)
        {
            long xorigin = 0;
            long yorigin = 0;
            long zorigin = 0;
            foreach (FrameVertex vertex in vertices)
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
        }*/
    }
}
