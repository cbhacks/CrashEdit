using System;

namespace Crash
{
    public struct OldSceneryVertex : IPosition
    {
        public static OldSceneryVertex Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 8)
                throw new ArgumentException("Value must be 8 bytes long.","data");
            short x = (short)(BitConv.FromInt16(data,4) & 0xFFF8);
            short y = (short)(BitConv.FromInt16(data,6) & 0xFFF8);
            int zhigh = data[6] & 7;
            int zmid = (data[4] & 6) >> 1;
            int zlow = data[3];
            short z = (short)(zhigh << 13 | zmid << 11 | zlow << 3);
            byte red = data[0];
            byte green = data[1];
            byte blue = data[2];
            bool fx = ((data[4] & 1) != 0);
            return new OldSceneryVertex(x,y,z,red,green,blue, fx);
        }

        public OldSceneryVertex(short x,short y,short z,byte red,byte green,byte blue,bool fx)
        {
            if ((x & 0x7) != 0)
                throw new ArgumentException("Value must be a multiple of 8.","x");
            if ((y & 0x7) != 0)
                throw new ArgumentException("Value must be a multiple of 8.","y");
            if ((z & 0x7) != 0)
                throw new ArgumentException("Value must be a multiple of 8.","z");
            X = x;
            Y = y;
            Z = z;
            Red = red;
            Green = green;
            Blue = blue;
            FX = fx;
        }

        public short X { get; }
        public short Y { get; }
        public short Z { get; }
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public bool FX { get; }
        double IPosition.X => X;
        double IPosition.Y => Y;
        double IPosition.Z => Z;

        public byte[] Save()
        {
            int zlow = (Z >> 3) & 0xFF;
            int zmid = (Z >> 11) & 0x3;
            int zhigh = (Z >> 13) & 0x7;
            byte[] data = new byte [8];
            data[0] = Red;
            data[1] = Green;
            data[2] = Blue;
            data[3] = (byte)zlow;
            BitConv.ToInt16(data,4,X);
            data[4] |= (byte)(zmid << 1);
            BitConv.ToInt16(data,6,Y);
            data[6] |= (byte)(zhigh);
            if (FX)
            {
                data[4] |= 1;
            }
            return data;
        }
    }
}
