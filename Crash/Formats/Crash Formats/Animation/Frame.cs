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
            //for (int i = 0;i < settings.Length; i++)
            //{
            //    settings[i] = data[24 + i];
            //}
            Array.Copy(data, 24, settings, 0, settings.Length);
            //FrameVertex[] vertices = new FrameVertex [vertexcount - ((headersize - 24 - collision * 40) / 3)];
            FrameVertex[] vertices = new FrameVertex[vertexcount];
            bool[] temporals;
            if (data.Length >= vertexcount * 3 + headersize) // uncompressed
            {
                temporals = null;
                for (int i = 0; i < vertexcount; i++)
                {
                    vertices[i] = new FrameVertex(data[headersize+i*3+0], data[headersize+i*3+1], data[headersize+i*3+2]);
                }
            }
            else
            {
                temporals = new bool[(data.Length - headersize) * 8];
                for (int i = 0; i < data.Length - headersize; ++i)
                {
                    for (int j = 0; j < 8;++j)
                    {
                        temporals[i * 8 + j] = ((data[headersize+i] >> (7 - j)) & 0x1) == 1;
                    }
                }
            }
            return new Frame(xoffset,yoffset,zoffset,unknown,vertexcount,collision,modeleid,headersize,settings,vertices,temporals);
        }

        private List<FrameVertex> vertices;

        public Frame(short xoffset,short yoffset,short zoffset,short unknown,int vertexcount,int collision,int modeleid,int headersize,byte[] settings,IEnumerable<FrameVertex> vertices,bool[] temporals)
        {
            XOffset = xoffset;
            YOffset = yoffset;
            ZOffset = zoffset;
            Unknown = unknown;
            VertexCount = vertexcount;
            Collision = collision;
            ModelEID = modeleid;
            HeaderSize = headersize;
            Settings = settings;
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
        public int VertexCount { get; }
        public IList<FrameVertex> Vertices => vertices;
        public bool[] Temporals { get; }
        public short Unknown { get; }

        public byte[] Save()
        {
            int size = HeaderSize;
            if (Temporals != null)
            {
                size += Temporals.Length / 8;
            }
            else
            {
                size += Vertices.Count * 3;
            }
            byte[] result = new byte [size];
            BitConv.ToInt16(result,0,XOffset);
            BitConv.ToInt16(result,2,YOffset);
            BitConv.ToInt16(result,4,ZOffset);
            BitConv.ToInt16(result,6,Unknown);
            BitConv.ToInt32(result,8,VertexCount);
            BitConv.ToInt32(result,12,Collision);
            BitConv.ToInt32(result,16,ModelEID);
            BitConv.ToInt32(result,20,HeaderSize);
            //for (int i = 0; i < Settings.Length; i++)
            //{
            //    BitConv.ToInt32(result,24 + i,Settings[i]);
            //}
            Array.Copy(Settings, 0, result, 24, Settings.Length);
            if (Temporals != null)
            {
                for (short i = 0; i < Temporals.Length / 8;++i)
                {
                    byte val = 0;
                    for (short j = 0; j < 8;++j)
                    {
                        val |= (byte)(Convert.ToByte(Temporals[i * 8 + j]) << (7 - j));
                    }
                    result[HeaderSize + i] = val;
                }
            }
            else
            {
                for (int i = 0; i < vertices.Count; ++i)
                {
                    result[HeaderSize + i * 3 + 0] = vertices[i].X;
                    result[HeaderSize + i * 3 + 1] = vertices[i].Y;
                    result[HeaderSize + i * 3 + 2] = vertices[i].Z;
                }
            }
            return result;
        }
    }
}
