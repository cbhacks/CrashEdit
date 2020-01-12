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

        public OldModelPolygon(short vertexa,short vertexb,short vertexc,short unknown)
        {
            VertexA = vertexa;
            VertexB = vertexb;
            VertexC = vertexc;
            Unknown = unknown;
        }

        public short VertexA { get; }
        public short VertexB { get; }
        public short VertexC { get; }
        public short Unknown { get; }

        public byte[] Save()
        {
            byte[] data = new byte [8];
            BitConv.ToInt16(data,0,VertexA);
            BitConv.ToInt16(data,2,VertexB);
            BitConv.ToInt16(data,4,VertexC);
            BitConv.ToInt16(data,6,Unknown);
            return data;
        }
    }
}
