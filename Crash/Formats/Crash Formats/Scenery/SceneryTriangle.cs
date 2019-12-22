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
                throw new ArgumentException("Value must be 2 bytes long.","bdata");
            int avalue = BitConv.FromInt32(adata,0);
            short bvalue = BitConv.FromInt16(bdata,0);
            int vertexa = (avalue >> 8) & 0xFFF;
            int vertexb = (avalue >> 20) & 0xFFF;
            int vertexc = (bvalue >> 4) & 0xFFF;
            short texture = (short)(((byte)bvalue & 0xF) | (((byte)avalue & 0x7F) << 4));
            bool animated = (avalue & 0x80) != 0;
            return new SceneryTriangle(vertexa,vertexb,vertexc,texture,animated);
        }

        public SceneryTriangle(int vertexa,int vertexb,int vertexc,short texture,bool animated)
        {
            if (vertexa < 0 || vertexa > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexa");
            if (vertexb < 0 || vertexb > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexb");
            if (vertexc < 0 || vertexc > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexc");
            if (texture < 0 || texture > 0x7FF)
                throw new ArgumentOutOfRangeException("texture");
            VertexA = vertexa;
            VertexB = vertexb;
            VertexC = vertexc;
            Texture = texture;
            Animated = animated;
        }

        public int VertexA { get; }
        public int VertexB { get; }
        public int VertexC { get; }
        public short Texture { get; }
        public bool Animated { get; }

        public byte[] SaveA()
        {
            byte[] data = new byte [4];
            int value = (VertexA << 8) | (VertexB << 20) | (Convert.ToInt32(Animated) << 7) | (Texture >> 4 & 0x7F);
            BitConv.ToInt32(data,0,value);
            return data;
        }

        public byte[] SaveB()
        {
            byte[] data = new byte [2];
            int value = (VertexC << 4) | (Texture & 0xF);
            BitConv.ToInt16(data,0,(short)value);
            return data;
        }
    }
}
