using System;

namespace Crash
{
    public struct OldModelPolygon
    {
        public static OldModelPolygon Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 8)
                throw new ArgumentException("Value must be 8 bytes long.","data");
            short vertexa = BitConv.FromInt16(data,0);
            short vertexb = BitConv.FromInt16(data,2);
            short vertexc = BitConv.FromInt16(data,4);
            short unknown = BitConv.FromInt16(data,6);
            return new OldModelPolygon(vertexa,vertexb,vertexc,unknown);
        }

        private short vertexa;
        private short vertexb;
        private short vertexc;
        private short unknown;

        public OldModelPolygon(short vertexa,short vertexb,short vertexc,short unknown)
        {
            this.vertexa = vertexa;
            this.vertexb = vertexb;
            this.vertexc = vertexc;
            this.unknown = unknown;
        }

        public short VertexA
        {
            get { return vertexa; }
        }

        public short VertexB
        {
            get { return vertexb; }
        }

        public short VertexC
        {
            get { return vertexc; }
        }

        public short Unknown
        {
            get { return unknown; }
        }

        public byte[] Save()
        {
            byte[] data = new byte [8];
            BitConv.ToInt16(data,0,vertexa);
            BitConv.ToInt16(data,2,vertexb);
            BitConv.ToInt16(data,4,vertexc);
            BitConv.ToInt16(data,6,unknown);
            return data;
        }
    }
}
