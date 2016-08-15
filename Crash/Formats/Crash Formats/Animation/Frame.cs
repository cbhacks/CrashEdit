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
            short xoffset = BitConv.FromInt16(data,0);
            short yoffset = BitConv.FromInt16(data,2);
            short zoffset = BitConv.FromInt16(data,4);
            short unknown = BitConv.FromInt16(data,6);
            int vertexcount = BitConv.FromInt32(data,8);
            if (vertexcount < 0 || vertexcount > Chunk.Length / 3)
            {
                ErrorManager.SignalError("Frame: Vertex count is invalid");
            }
            int collision = BitConv.FromInt32(data,12);
            int modeleid = BitConv.FromInt32(data,16);
            int headersize = BitConv.FromInt32(data,20);
            if (headersize < 24)
            {
                ErrorManager.SignalError("Frame: Header size value is invalid");
            }
            byte[] settings = new byte[headersize - 24];
            for (int i = 0;i < settings.Length; i++)
            {
                settings[i] = data[24 + i];
            }
            FrameVertex[] vertices = new FrameVertex [vertexcount - ((headersize - 24 - collision * 40) / 3)];
            bool[] temporals = new bool[(data.Length - headersize) * 8];
            if (data.Length >= vertexcount * 3 + 24 + collision * 40)
            {
                for (int i = 0; i < vertexcount - ((headersize - 24 - collision * 40) / 3); i++)
                {
                    byte[] vertexdata = new byte[3];
                    Array.Copy(data,headersize + i * 3,vertexdata,0,vertexdata.Length);
                    vertices[i] = FrameVertex.Load(vertexdata);
                }
                temporals = null;
            }
            else
            {
                for (int i = 0;i < (data.Length - headersize) / 4; i++)
                {
                    int val = BitConv.FromInt32(data,headersize + i * 4);
                    for (int ii = 0;ii < 32;ii++)
                    {
                        temporals[i * 32 + ii] = (((val >> (31 - ii)) & 0x1) == 1);
                    }
                }
            }
            return new Frame(xoffset,yoffset,zoffset,unknown,vertexcount,collision,modeleid,headersize,settings,vertices,temporals);
        }

        private short xoffset;
        private short yoffset;
        private short zoffset;
        private short unknown;
        private int vertexcount;
        private int collision;
        private int modeleid;
        private int headersize;
        private byte[] settings;
        private List<FrameVertex> vertices;
        private bool[] temporals;

        public Frame(short xoffset,short yoffset,short zoffset,short unknown,int vertexcount,int collision,int modeleid,int headersize,byte[] settings,IEnumerable<FrameVertex> vertices,bool[] temporals)
        {
            this.xoffset = xoffset;
            this.yoffset = yoffset;
            this.zoffset = zoffset;
            this.unknown = unknown;
            this.vertexcount = vertexcount;
            this.collision = collision;
            this.modeleid = modeleid;
            this.headersize = headersize;
            this.settings = settings;
            this.vertices = new List<FrameVertex>(vertices);
            this.temporals = temporals;
        }

        public int ModelEID
        {
            get { return modeleid; }
        }

        public short XOffset
        {
            get { return xoffset; }
        }

        public short YOffset
        {
            get { return yoffset; }
        }

        public short ZOffset
        {
            get { return zoffset; }
        }

        public int Collision
        {
            get { return collision; }
        }

        public int HeaderSize
        {
            get { return headersize; }
        }

        public byte[] Settings
        {
            get { return settings; }
        }

        public int VertexCount
        {
            get { return vertexcount; }
        }

        public IList<FrameVertex> Vertices
        {
            get { return vertices; }
        }

        public bool[] Temporals
        {
            get { return temporals; }
        }

        public short Unknown
        {
            get { return unknown; }
        }

        public byte[] Save()
        {
            int bytesize = 0;
            if (Temporals == null)
            {
                bytesize = ((headersize + Vertices.Count * 3) + 4 - ((headersize + Vertices.Count * 3) % 4)) - headersize;
                if ((headersize + Vertices.Count * 3) % 4 == 0)
                    bytesize -= 4;
            }
            else
            {
                bytesize = Temporals.Length / 8;
            }
            byte[] result = new byte [headersize + bytesize];
            BitConv.ToInt16(result,0,xoffset);
            BitConv.ToInt16(result,2,yoffset);
            BitConv.ToInt16(result,4,zoffset);
            BitConv.ToInt16(result,6,unknown);
            BitConv.ToInt32(result,8,vertexcount);
            BitConv.ToInt32(result,12,collision);
            BitConv.ToInt32(result,16,modeleid);
            BitConv.ToInt32(result,20,headersize);
            for (int i = 0; i < settings.Length; i++)
            {
                BitConv.ToInt32(result,24 + i,settings[i]);
            }
            if (Temporals == null)
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].Save().CopyTo(result,headersize + i * 3);
                }
            }
            else
            {
                for (short i = 0; i < Temporals.Length / 32; i++)
                {
                    int val = 0;
                    for (short ii = 0; ii < 32; ii++)
                    {
                        val |= Convert.ToByte(Temporals[i * 32 + ii]) << (31 - ii);
                    }
                    BitConv.ToInt32(result,headersize + i * 4,val);
                }
            }
            return result;
        }
    }
}
