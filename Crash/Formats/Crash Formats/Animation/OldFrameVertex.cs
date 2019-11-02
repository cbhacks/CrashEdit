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
            byte normalx = data[3];
            byte normaly = data[4];
            byte normalz = data[5];
            return new OldFrameVertex(x,y,z,normalx,normaly,normalz);
        }

        public OldFrameVertex(byte x,byte y,byte z,byte normalx,byte normaly,byte normalz)
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
        public byte NormalX { get; }
        public byte NormalY { get; }
        public byte NormalZ { get; }

        double IPosition.X => X;
        double IPosition.Y => Y;
        double IPosition.Z => Z;

        public byte[] Save()
        {
            byte[] data = new byte [6];
            data[0] = X;
            data[1] = Y;
            data[2] = Z;
            data[3] = NormalX;
            data[4] = NormalY;
            data[5] = NormalZ;
            return data;
        }
    }
}
