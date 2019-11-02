using System;

namespace Crash
{
    public struct SceneryVertex : IPosition
    {
        public static SceneryVertex Load(byte[] xydata,byte[] zdata)
        {
            if (xydata == null)
                throw new ArgumentNullException("xydata");
            if (zdata == null)
                throw new ArgumentNullException("zdata");
            if (xydata.Length != 4)
                throw new ArgumentException("Value must be 4 bytes long.","xydata");
            if (zdata.Length != 2)
                throw new ArgumentException("Value must be 2 bytes long.","zdata");
            int xy = BitConv.FromInt32(xydata,0);
            short z = BitConv.FromInt16(zdata,0);
            short y = (short)(xy >> 16);
            short x = (short)xy;
            int unknownx = x & 0xF;
            int unknowny = y & 0xF;
            int unknownz = z & 0xF;
            x >>= 4;
            y >>= 4;
            z >>= 4;
            return new SceneryVertex(x,y,z,unknownx,unknowny,unknownz);
        }

        public SceneryVertex(int x,int y,int z,int unknownx,int unknowny,int unknownz)
        {
            if (x < -0x800 || x > 0x7FF)
                throw new ArgumentOutOfRangeException("x");
            if (y < -0x800 || y > 0x7FF)
                throw new ArgumentOutOfRangeException("y");
            if (z < -0x800 || z > 0x7FF)
                throw new ArgumentOutOfRangeException("z");
            if (unknownx < 0 || unknownx > 0xF)
                throw new ArgumentOutOfRangeException("unknownx");
            if (unknowny < 0 || unknowny > 0xF)
                throw new ArgumentOutOfRangeException("unknowny");
            if (unknownz < 0 || unknownz > 0xF)
                throw new ArgumentOutOfRangeException("unknownz");
            X = x;
            Y = y;
            Z = z;
            UnknownX = unknownx;
            UnknownY = unknowny;
            UnknownZ = unknownz;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public int UnknownX { get; }
        public int UnknownY { get; }
        public int UnknownZ { get; }
        public int Color => (UnknownY & 0x3) << 8 | UnknownX << 4 | UnknownZ;
        double IPosition.X => X;
        double IPosition.Y => Y;
        double IPosition.Z => Z;

        public byte[] SaveXY()
        {
            byte[] data = new byte [4];
            int xdata = (X << 4) | UnknownX;
            int ydata = (Y << 4) | UnknownY;
            BitConv.ToInt16(data,0,(short)xdata);
            BitConv.ToInt16(data,2,(short)ydata);
            return data;
        }

        public byte[] SaveZ()
        {
            byte[] data = new byte [2];
            int zdata = (Z << 4) | UnknownZ;
            BitConv.ToInt16(data,0,(short)zdata);
            return data;
        }
    }
}
