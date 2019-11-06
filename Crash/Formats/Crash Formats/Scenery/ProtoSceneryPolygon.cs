using System;

namespace Crash
{
    public struct ProtoSceneryPolygon
    {
        public static ProtoSceneryPolygon Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 12)
                throw new ArgumentException("Value must be 12 bytes long.","data");
            int unknownint1 = BitConv.FromInt16(data, 0);
            int unknownint2 = BitConv.FromInt32(data, 4);
            short short1 = BitConv.FromInt16(data, 8);
            short short2 = BitConv.FromInt16(data, 10);
            byte[] vertexarray = new byte[4];
            vertexarray[2] = (byte)(unknownint2 & 0x0F); // Most significant nibble
            vertexarray[1] = (byte)(short2 & 0x0F);
            vertexarray[0] = (byte)((short1 >> 12) & 0x0F); // Least significant nibble
            short vertexa = (short)((((BitConverter.ToInt32(vertexarray, 0) & 0xFF00) >> 4) | vertexarray[0]) | (BitConverter.ToInt32(vertexarray,0) & 0xFF0000) >> 8);
            //short vertexa = (short)(BitConverter.ToInt32(vertexarray, 0));
            //short vertexa = (short)((unknownint2) & 0xFF07); // what
            short vertexb = (short)(short1 & 0x7FF);
            vertexarray[1] = (byte)((short2 >> 4) & 0x07);
            vertexarray[0] = (byte)(short2 >> 8);
            short vertexc = BitConverter.ToInt16(vertexarray, 0);
            return new ProtoSceneryPolygon(vertexa, unknownint1, unknownint2, short1, short2, vertexb,vertexc);
        }

        public ProtoSceneryPolygon(short vertexa, int unknownint1, int unknownint2, short short1, short short2, short vertexb, short vertexc)
        {
            VertexA = vertexa;
            UnknownInt1 = unknownint1;
            UnknownInt2 = unknownint2;
            Short1 = short1;
            Short2 = short2;
            VertexB = vertexb;
            VertexC = vertexc;
        }

        public short VertexA { get; set; }
        public int UnknownInt1 { get; }
        public int UnknownInt2 { get; }
        public short Short1 { get; }
        public short Short2 { get; }
        public short VertexB { get; set; }
        public short VertexC { get; set; }

        public byte[] Save()
        {
            byte[] data = new byte [12];
            BitConv.ToInt32(data,0,UnknownInt1);
            BitConv.ToInt32(data, 4, UnknownInt2);
            BitConv.ToInt16(data, 8, Short1);
            BitConv.ToInt16(data, 10, Short2);
            return data;
        }
    }
}
