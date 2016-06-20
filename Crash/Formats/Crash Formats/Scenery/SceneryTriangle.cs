using System;

namespace Crash
{
    public struct SceneryTriangle
    {
        public static SceneryTriangle Load(byte[] adata,byte[] bdata)
        {
            if (adata == null)
                throw new ArgumentNullException("adata");
            if (bdata == null)
                throw new ArgumentNullException("bdata");
            if (adata.Length != 4)
                throw new ArgumentException("Value must be 4 bytes long.","adata");
            if (bdata.Length != 2)
                throw new ArgumentException("Value must be 2 bytes long.","adata");
            int avalue = BitConv.FromInt32(adata,0);
            short bvalue = BitConv.FromInt16(bdata,0);
            int vertexa = (avalue >> 8) & 0xFFF;
            int vertexb = (avalue >> 20) & 0xFFF;
            int vertexc = (bvalue >> 4) & 0xFFF;
            byte unknown1 = (byte)avalue;
            byte unknown2 = (byte)(bvalue & 0xF);
            return new SceneryTriangle(vertexa,vertexb,vertexc,unknown1,unknown2);
        }

        private int vertexa;
        private int vertexb;
        private int vertexc;
        private byte unknown1;
        private byte unknown2;

        public SceneryTriangle(int vertexa,int vertexb,int vertexc,byte unknown1,byte unknown2)
        {
            if (vertexa < 0 || vertexa > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexa");
            if (vertexb < 0 || vertexb > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexb");
            if (vertexc < 0 || vertexc > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexc");
            if (unknown2 < 0 || unknown2 > 0xF)
                throw new ArgumentOutOfRangeException("unknown2");
            this.vertexa = vertexa;
            this.vertexb = vertexb;
            this.vertexc = vertexc;
            this.unknown1 = unknown1;
            this.unknown2 = unknown2;
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

        public byte Unknown1
        {
            get { return unknown1; }
        }

        public byte Unknown2
        {
            get { return unknown2; }
        }

        public byte[] SaveA()
        {
            byte[] data = new byte [4];
            int value = (vertexa << 8) | (vertexb << 20) | unknown1;
            BitConv.ToInt32(data,0,value);
            return data;
        }

        public byte[] SaveB()
        {
            byte[] data = new byte [2];
            int value = (vertexc << 4) | unknown2;
            BitConv.ToInt16(data,0,(short)value);
            return data;
        }
    }
}
