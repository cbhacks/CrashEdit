using System;

namespace Crash
{
    public struct OldSceneryPolygon
    {
        public static OldSceneryPolygon Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 8)
                throw new ArgumentException("Value must be 8 bytes long.","data");
            int worda = BitConv.FromInt32(data,0);
            int wordb = BitConv.FromInt32(data,4);
            int vertexa = (worda >> 20) & 0xFFF;
            int vertexb = (wordb >> 8) & 0xFFF;
            int vertexc = (wordb >> 20) & 0xFFF;
            int unknown1 = (worda >> 8) & 0xFFF;
            byte unknown2 = (byte)(worda & 0xFF);
            byte unknown3 = (byte)(wordb & 0xFF);
            return new OldSceneryPolygon(vertexa,vertexb,vertexc,unknown1,unknown2,unknown3);
        }

        public OldSceneryPolygon(int vertexa,int vertexb,int vertexc,int unknown1,byte unknown2,byte unknown3)
        {
            if (vertexa < 0 || vertexa > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexa");
            if (vertexb < 0 || vertexb > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexb");
            if (vertexc < 0 || vertexc > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexc");
            if (unknown1 < 0 || unknown1 > 0xFFF)
                throw new ArgumentOutOfRangeException("unknown1");
            VertexA = vertexa;
            VertexB = vertexb;
            VertexC = vertexc;
            Unknown1 = unknown1;
            Unknown2 = unknown2;
            Unknown3 = unknown3;
        }

        public int VertexA { get; }
        public int VertexB { get; }
        public int VertexC { get; }
        public int Unknown1 { get; }
        public byte Unknown2 { get; }
        public byte Unknown3 { get; }

        public byte[] Save()
        {
            int worda = 0;
            int wordb = 0;
            worda |= VertexA << 20;
            wordb |= VertexB << 8;
            wordb |= VertexC << 20;
            worda |= Unknown1 << 8;
            worda |= Unknown2;
            wordb |= Unknown3;
            byte[] data = new byte [8];
            BitConv.ToInt32(data,0,worda);
            BitConv.ToInt32(data,4,wordb);
            return data;
        }
    }
}
