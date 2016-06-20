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

        private int vertexa;
        private int vertexb;
        private int vertexc;
        private int vertexd;
        private byte unknown2;
        private byte unknown3;

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
            this.vertexa = vertexa;
            this.vertexb = vertexb;
            this.vertexc = vertexc;
            this.vertexd = vertexd;
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

        public int VertexD
        {
            get { return vertexd; }
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
            byte[] data = new byte [8];
            int worda = (vertexa << 8) | (vertexb << 20) | unknown2;
            int wordb = (vertexd << 8) | (vertexc << 20) | unknown3;
            BitConv.ToInt32(data,0,worda);
            BitConv.ToInt32(data,4,wordb);
            return data;
        }
    }
}
