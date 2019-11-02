using System;

namespace Crash
{
    public struct SceneryQuad
    {
        public static SceneryQuad Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 8)
                throw new ArgumentException("Value must be 8 bytes long.","data");
            int worda = BitConv.FromInt32(data,0);
            int wordb = BitConv.FromInt32(data,4);
            int vertexa = (worda >> 8) & 0xFFF;
            int vertexb = (worda >> 20) & 0xFFF;
            int vertexd = (wordb >> 8) & 0xFFF;
            int vertexc = (wordb >> 20) & 0xFFF;
            byte unknown2 = (byte)worda;
            byte unknown3 = (byte)wordb;
            return new SceneryQuad(vertexa,vertexb,vertexc,vertexd,unknown2,unknown3);
        }

        public SceneryQuad(int vertexa,int vertexb,int vertexc,int vertexd,byte unknown2,byte unknown3)
        {
            if (vertexa < 0 || vertexa > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexa");
            if (vertexb < 0 || vertexb > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexb");
            if (vertexc < 0 || vertexc > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexc");
            if (vertexd < 0 || vertexd > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexd");
            VertexA = vertexa;
            VertexB = vertexb;
            VertexC = vertexc;
            VertexD = vertexd;
            Unknown2 = unknown2;
            Unknown3 = unknown3;
        }

        public int VertexA { get; }
        public int VertexB { get; }
        public int VertexC { get; }
        public int VertexD { get; }
        public byte Unknown2 { get; }
        public byte Unknown3 { get; }

        public byte[] Save()
        {
            byte[] data = new byte [8];
            int worda = (VertexA << 8) | (VertexB << 20) | Unknown2;
            int wordb = (VertexD << 8) | (VertexC << 20) | Unknown3;
            BitConv.ToInt32(data,0,worda);
            BitConv.ToInt32(data,4,wordb);
            return data;
        }
    }
}
