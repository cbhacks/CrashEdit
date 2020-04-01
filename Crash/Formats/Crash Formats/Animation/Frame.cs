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
            int collisioncount = BitConv.FromInt32(data,12);
            int modeleid = BitConv.FromInt32(data,16);
            int headersize = BitConv.FromInt32(data,20);
            if (headersize < 24)
            {
                ErrorManager.SignalError("Frame: Header size value is invalid");
            }
            FrameCollision[] collision = new FrameCollision[collisioncount];
            for (int i = 0; i < collisioncount; ++i) // these vertices are NEVER compressed
            {
                collision[i] = new FrameCollision(BitConv.FromInt32(data,24+i*0x28),
                    BitConv.FromInt32(data,28+i*0x28),BitConv.FromInt32(data,32+i*0x28),BitConv.FromInt32(data,36+i*0x28),
                    BitConv.FromInt32(data,40+i*0x28),BitConv.FromInt32(data,44+i*0x28),BitConv.FromInt32(data,48+i*0x28),
                    BitConv.FromInt32(data,52+i*0x28),BitConv.FromInt32(data,56+i*0x28),BitConv.FromInt32(data,60+i*0x28));
            }
            int fake_headersize = 24 + collisioncount * 0x28;
            int specialvertexcount = (headersize - fake_headersize) / 3;
            FrameVertex[] vertices = new FrameVertex[vertexcount];
            for (int i = 0; i < specialvertexcount; ++i) // these vertices are NEVER compressed
            {
                vertices[i] = new FrameVertex(data[fake_headersize + i * 3], data[fake_headersize + i * 3 + 1], data[fake_headersize + i * 3 + 2]);
            }
            if (((data.Length - fake_headersize) % 4) != 0)
            {
                ErrorManager.SignalError("Frame: Invalid data alignment");
            }
            bool[] temporals = new bool[(data.Length - fake_headersize) / 4 * 32];
            for (int i = 0; i < (data.Length - fake_headersize) / 4; ++i)
            {
                int val = BitConv.FromInt32(data, fake_headersize + i * 4);
                for (int j = 0; j < 32; ++j) // reverse endianness for decompression
                {
                    temporals[i * 32 + j] = (val >> (31 - j) & 0x1) == 1;
                }
            }
            return new Frame(xoffset,yoffset,zoffset,unknown,modeleid,headersize,collision,vertices,specialvertexcount,temporals,false);
        }

        public static Frame LoadNew(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 28)
            {
                ErrorManager.SignalError("NewFrame: Data is too short");
            }
            short xoffset = BitConv.FromInt16(data,0);
            short yoffset = BitConv.FromInt16(data,2);
            short zoffset = BitConv.FromInt16(data,4);
            short unknown = BitConv.FromInt16(data,6);
            int unknown2 = BitConv.FromInt32(data,8);
            if (BitConv.FromInt32(data,12) != 0)
            {
                ErrorManager.SignalIgnorableError("NewFrame: Reserved value is not blank");
            }
            int vertexcount = BitConv.FromInt32(data,16);
            if (vertexcount < 0 || vertexcount > Chunk.Length / 3)
            {
                ErrorManager.SignalError("NewFrame: Vertex count is invalid");
            }
            int collisioncount = BitConv.FromInt32(data,20);
            int headersize = BitConv.FromInt32(data,24);
            if (headersize < 28)
            {
                ErrorManager.SignalError("NewFrame: Header size value is invalid");
            }
            FrameCollision[] collision = new FrameCollision[collisioncount];
            for (int i = 0; i < collisioncount; ++i) // these vertices are NEVER compressed
            {
                collision[i] = new FrameCollision(BitConv.FromInt32(data,28+i*0x28),
                    BitConv.FromInt32(data,32+i*0x28),BitConv.FromInt32(data,36+i*0x28),BitConv.FromInt32(data,40+i*0x28),
                    BitConv.FromInt32(data,44+i*0x28),BitConv.FromInt32(data,48+i*0x28),BitConv.FromInt32(data,52+i*0x28),
                    BitConv.FromInt32(data,56+i*0x28),BitConv.FromInt32(data,60+i*0x28),BitConv.FromInt32(data,64+i*0x28));
            }
            int fake_headersize = 28 + collisioncount * 0x28;
            int specialvertexcount = (headersize - fake_headersize) / 3;
            FrameVertex[] vertices = new FrameVertex[vertexcount];
            for (int i = 0; i < specialvertexcount; ++i) // these vertices are NEVER compressed
            {
                vertices[i] = new FrameVertex(data[fake_headersize + i * 3], data[fake_headersize + i * 3 + 1], data[fake_headersize + i * 3 + 2]);
            }
            if (((data.Length - fake_headersize) % 4) != 0)
            {
                ErrorManager.SignalError("NewFrame: Invalid data alignment");
            }
            bool[] temporals = new bool[(data.Length - fake_headersize) / 4 * 32];
            for (int i = 0; i < (data.Length - fake_headersize) / 4; ++i)
            {
                int val = BitConv.FromInt32(data, fake_headersize + i * 4);
                for (int j = 0; j < 32; ++j) // reverse endianness for decompression
                {
                    temporals[i * 32 + j] = (val >> (31 - j) & 0x1) == 1;
                }
            }
            return new Frame(xoffset,yoffset,zoffset,unknown,unknown2,headersize,collision,vertices,specialvertexcount,temporals,true);
        }

        private List<FrameCollision> collision;
        private List<FrameVertex> vertices;

        public Frame(short xoffset,short yoffset,short zoffset,short unknown,int modeleid,int headersize,IEnumerable<FrameCollision> collision,IEnumerable<FrameVertex> vertices,int specialvertexcount, bool[] temporals,bool isnew)
        {
            IsNew = isnew;
            XOffset = xoffset;
            YOffset = yoffset;
            ZOffset = zoffset;
            Unknown = unknown;
            ModelEID = modeleid;
            HeaderSize = headersize;
            SpecialVertexCount = specialvertexcount;
            this.collision = new List<FrameCollision>(collision);
            this.vertices = new List<FrameVertex>(vertices);
            Temporals = temporals;
        }

        public int ModelEID { get; }
        public short XOffset { get; }
        public short YOffset { get; }
        public short ZOffset { get; }
        public int HeaderSize { get; }
        public IList<FrameCollision> Collision => collision;
        public IList<FrameVertex> Vertices => vertices;
        public int SpecialVertexCount { get; }
        public bool[] Temporals { get; }
        public short Unknown { get; }

        public bool IsNew { get; }

        public bool Decompressed { get; set; } = false;

        public byte[] Save()
        {
            if (IsNew)
                return SaveC3();
            else
                return SaveC2();
        }

        public byte[] SaveC2()
        {
            int fake_headersize = 24 + collision.Count * 0x28;
            int size = fake_headersize + Temporals.Length / 8;
            byte[] result = new byte [size];
            BitConv.ToInt16(result,0,XOffset);
            BitConv.ToInt16(result,2,YOffset);
            BitConv.ToInt16(result,4,ZOffset);
            BitConv.ToInt16(result,6,Unknown);
            BitConv.ToInt32(result,8,vertices.Count);
            BitConv.ToInt32(result,12,collision.Count);
            BitConv.ToInt32(result,16,ModelEID);
            BitConv.ToInt32(result,20,HeaderSize);
            for (int i = 0; i < collision.Count; ++i)
            {
                BitConv.ToInt32(result,24+i*0x28+0x00,collision[i].U);
                BitConv.ToInt32(result,24+i*0x28+0x04,collision[i].XO);
                BitConv.ToInt32(result,24+i*0x28+0x08,collision[i].YO);
                BitConv.ToInt32(result,24+i*0x28+0x0C,collision[i].ZO);
                BitConv.ToInt32(result,24+i*0x28+0x10,collision[i].X1);
                BitConv.ToInt32(result,24+i*0x28+0x14,collision[i].Y1);
                BitConv.ToInt32(result,24+i*0x28+0x18,collision[i].Z1);
                BitConv.ToInt32(result,24+i*0x28+0x1C,collision[i].X2);
                BitConv.ToInt32(result,24+i*0x28+0x20,collision[i].Y2);
                BitConv.ToInt32(result,24+i*0x28+0x24,collision[i].Z2);
            }
            for (short i = 0; i < Temporals.Length / 32; ++i)
            {
                int val = 0;
                for (short j = 0; j < 32; ++j)
                {
                    val |= Convert.ToInt32(Temporals[i*32+j]) << (31-j);
                }
                BitConv.ToInt32(result, fake_headersize+i*4, val);
            }
            return result;
        }

        public byte[] SaveC3()
        {
            int fake_headersize = 28 + collision.Count * 0x28;
            int size = fake_headersize + Temporals.Length / 8;
            byte[] result = new byte [size];
            BitConv.ToInt16(result,0,XOffset);
            BitConv.ToInt16(result,2,YOffset);
            BitConv.ToInt16(result,4,ZOffset);
            BitConv.ToInt16(result,6,Unknown);
            BitConv.ToInt32(result,8,ModelEID);
            BitConv.ToInt32(result,12,0);
            BitConv.ToInt32(result,16,vertices.Count);
            BitConv.ToInt32(result,20,collision.Count);
            BitConv.ToInt32(result,24,HeaderSize);
            for (int i = 0; i < collision.Count; ++i)
            {
                BitConv.ToInt32(result,28+i*0x28+0x00,collision[i].U);
                BitConv.ToInt32(result,28+i*0x28+0x04,collision[i].XO);
                BitConv.ToInt32(result,28+i*0x28+0x08,collision[i].YO);
                BitConv.ToInt32(result,28+i*0x28+0x0C,collision[i].ZO);
                BitConv.ToInt32(result,28+i*0x28+0x10,collision[i].X1);
                BitConv.ToInt32(result,28+i*0x28+0x14,collision[i].Y1);
                BitConv.ToInt32(result,28+i*0x28+0x18,collision[i].Z1);
                BitConv.ToInt32(result,28+i*0x28+0x1C,collision[i].X2);
                BitConv.ToInt32(result,28+i*0x28+0x20,collision[i].Y2);
                BitConv.ToInt32(result,28+i*0x28+0x24,collision[i].Z2);
            }
            for (short i = 0; i < Temporals.Length / 32; ++i)
            {
                int val = 0;
                for (short j = 0; j < 32; ++j)
                {
                    val |= Convert.ToInt32(Temporals[i*32+j]) << (31-j);
                }
                BitConv.ToInt32(result, fake_headersize+i*4, val);
            }
            return result;
        }
    }
}
