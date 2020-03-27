using System;

namespace Crash
{
    public struct OldFrameVertex : IPosition
    {
        public static OldFrameVertex Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 6)
                throw new ArgumentException("Value must be 6 bytes long.","data");
            byte x = data[0];
            byte y = data[1];
            byte z = data[2];
            sbyte normalx = (sbyte)data[3];
            sbyte normaly = (sbyte)data[4];
            sbyte normalz = (sbyte)data[5];
            return new OldFrameVertex(x,y,z,normalx,normaly,normalz);
        }

        public OldFrameVertex(byte x,byte y,byte z,sbyte normalx,sbyte normaly,sbyte normalz)
        {
            X = x;
            Y = y;
            Z = z;
            NormalX = normalx;
            NormalY = normaly;
            NormalZ = normalz;
        }

        public byte X { get; }
        public byte Y { get; }
        public byte Z { get; }
        public sbyte NormalX { get; }
        public sbyte NormalY { get; }
        public sbyte NormalZ { get; }

        double IPosition.X => X;
        double IPosition.Y => Y;
        double IPosition.Z => Z;

        public byte[] Save()
        {
            byte[] data = new byte [6];
            data[0] = X;
            data[1] = Y;
            data[2] = Z;
            data[3] = (byte)NormalX;
            data[4] = (byte)NormalY;
            data[5] = (byte)NormalZ;
            return data;
        }
    }
}
