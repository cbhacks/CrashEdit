using System;

namespace Crash
{
    public readonly struct ProtoSceneryVertex
    {
        public static ProtoSceneryVertex Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length != 6)
                throw new ArgumentException("Value must be 6 bytes long.", nameof(data));
            short x = BitConv.FromInt16(data, 0);
            short y = BitConv.FromInt16(data, 2);
            short z = BitConv.FromInt16(data, 4);
            return new ProtoSceneryVertex(x, y, z);
        }

        public ProtoSceneryVertex(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public short X { get; }
        public short Y { get; }
        public short Z { get; }

        public byte[] Save()
        {
            byte[] result = new byte[6];
            BitConv.ToInt16(result, 0, X);
            BitConv.ToInt16(result, 2, Y);
            BitConv.ToInt16(result, 4, Z);
            return result;
        }
    }
}
