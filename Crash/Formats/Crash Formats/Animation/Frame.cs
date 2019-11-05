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
            int fake_headersize = 24 + collision * 0x28;
            byte[] settings = new byte[collision * 0x28]; // FIXME
            Array.Copy(data, 24, settings, 0, settings.Length);
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
            return new Frame(xoffset,yoffset,zoffset,unknown,collision,modeleid,headersize,settings,vertices,specialvertexcount,temporals);
        }

        private List<FrameVertex> vertices;

        public Frame(short xoffset,short yoffset,short zoffset,short unknown,int collision,int modeleid,int headersize,byte[] settings,IEnumerable<FrameVertex> vertices,int specialvertexcount, bool[] temporals)
        {
            XOffset = xoffset;
            YOffset = yoffset;
            ZOffset = zoffset;
            Unknown = unknown;
            Collision = collision;
            ModelEID = modeleid;
            HeaderSize = headersize;
            Settings = settings;
            SpecialVertexCount = specialvertexcount;
            this.vertices = new List<FrameVertex>(vertices);
            Temporals = temporals;
        }

        public int ModelEID { get; }
        public short XOffset { get; }
        public short YOffset { get; }
        public short ZOffset { get; }
        public int Collision { get; }
        public int HeaderSize { get; }
        public byte[] Settings { get; }
        public IList<FrameVertex> Vertices => vertices;
        public int SpecialVertexCount { get; }
        public bool[] Temporals { get; }
        public short Unknown { get; }

        public bool Decompressed { get; set; } = false;

        public byte[] Save()
        {
            int fake_headersize = 24 + Collision * 0x28;
            int size = fake_headersize + Temporals.Length / 8;
            byte[] result = new byte [size];
            BitConv.ToInt16(result,0,XOffset);
            BitConv.ToInt16(result,2,YOffset);
            BitConv.ToInt16(result,4,ZOffset);
            BitConv.ToInt16(result,6,Unknown);
            BitConv.ToInt32(result,8,vertices.Count);
            BitConv.ToInt32(result,12,Collision);
            BitConv.ToInt32(result,16,ModelEID);
            BitConv.ToInt32(result,20,HeaderSize);
            Array.Copy(Settings,0,result,24,Settings.Length);
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
