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

        private int vertexa;
        private int vertexb;
        private int vertexc;
        private int unknown1;
        private byte unknown2;
        private byte unknown3;

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
            this.vertexa = vertexa;
            this.vertexb = vertexb;
            this.vertexc = vertexc;
            this.unknown1 = unknown1;
            this.unknown2 = unknown2;
            this.unknown3 = unknown3;
        }

        public int VertexA
        {
            get { return vertexa; }
        }

        public int VertexB
        {
            get { return vertexb; }
        }

        public int VertexC
        {
            get { return vertexc; }
        }

        public int Unknown1
        {
            get { return unknown1; }
        }

        public byte Unknown2
        {
            get { return unknown2; }
        }

        public byte Unknown3
        {
            get { return unknown3; }
        }

        public byte[] Save()
        {
            int worda = 0;
            int wordb = 0;
            worda |= vertexa << 20;
            wordb |= vertexb << 8;
            wordb |= vertexc << 20;
            worda |= unknown1 << 8;
            worda |= unknown2;
            wordb |= unknown3;
            byte[] data = new byte [8];
            BitConv.ToInt32(data,0,worda);
            BitConv.ToInt32(data,4,wordb);
            return data;
        }
    }
}
