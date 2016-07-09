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
            vertexarray[2] = (byte)(unknownint2 & 0x0F); //Most significant nibble
            vertexarray[1] = (byte)(short2 & 0x0F);
            vertexarray[0] = (byte)((short1 >> 12) & 0x0F); //Least significant nibble
            short vertexa = (short)((((BitConverter.ToInt32(vertexarray, 0) & 0xFF00) >> 4) | vertexarray[0]) | (BitConverter.ToInt32(vertexarray,0) & 0xFF0000) >> 8);
            //short vertexa = (short)(BitConverter.ToInt32(vertexarray, 0));
            //short vertexa = (short)((unknownint2) & 0xFF07); //what
            short vertexb = (short)(short1 & 0x7FF);
            vertexarray[1] = (byte)((short2 >> 4) & 0x07);
            vertexarray[0] = (byte)(short2 >> 8);
            short vertexc = BitConverter.ToInt16(vertexarray, 0);
            return new ProtoSceneryPolygon(vertexa, unknownint1, unknownint2, short1, short2, vertexb,vertexc);
        }

        private short vertexa;
        private int unknownint1;
        private int unknownint2;
        private short short1;
        private short short2;
        private short vertexb;
        private short vertexc;

        public ProtoSceneryPolygon(short vertexa, int unknownint1, int unknownint2, short short1, short short2, short vertexb, short vertexc)
        {
            this.vertexa = vertexa;
            this.unknownint1 = unknownint1;
            this.unknownint2 = unknownint2;
            this.short1 = short1;
            this.short2 = short2;
            this.vertexb = vertexb;
            this.vertexc = vertexc;
        }

        public short VertexA
        {
            get { return vertexa; }
            set { vertexa = value; }
        }

        public int UnknownInt1
        {
            get { return unknownint1; }
        }

        public int UnknownInt2
        {
            get { return unknownint2; }
        }

        public short Short1
        {
            get { return short1; }
        }

        public short Short2
        {
            get { return short2; }
        }

        public short VertexB
        {
            get { return vertexb; }
            set { vertexb = value; }
        }

        public short VertexC
        {
            get { return vertexc; }
            set { vertexc = value; }
        }

        public byte[] Save()
        {
            byte[] data = new byte [12];
            BitConv.ToInt32(data,0,unknownint1);
            BitConv.ToInt32(data, 4, unknownint2);
            BitConv.ToInt16(data, 8, short1);
            BitConv.ToInt16(data, 10, short2);
            return data;
        }
    }
}
