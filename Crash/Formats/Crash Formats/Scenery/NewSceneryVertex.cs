using System;

namespace Crash
{
    public struct NewSceneryVertex : IPosition
    {
        public static NewSceneryVertex Load(byte[] xydata,byte[] zdata)
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
            ushort z = (ushort)BitConv.FromInt16(zdata,0);
            ushort y = (ushort)(xy >> 16);
            ushort x = (ushort)xy;
            int unknownx = x & 0xF;
            int unknowny = y & 0xF;
            int unknownz = z & 0xF;
            x >>= 4;
            y >>= 4;
            z >>= 4;
            return new NewSceneryVertex(x,y,z,unknownx,unknowny,unknownz);
        }

        public NewSceneryVertex(uint x,uint y,uint z,int unknownx,int unknowny,int unknownz)
        {
            if (x > 0xFFF)
                throw new ArgumentOutOfRangeException("x");
            if (y > 0xFFF)
                throw new ArgumentOutOfRangeException("y");
            if (z > 0xFFF)
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

        public uint X { get; }
        public uint Y { get; }
        public uint Z { get; }
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
            int xdata = (int)((X << 4) | UnknownX);
            int ydata = (int)((Y << 4) | UnknownY);
            BitConv.ToInt16(data,0,(short)xdata);
            BitConv.ToInt16(data,2,(short)ydata);
            return data;
        }

        public byte[] SaveZ()
        {
            byte[] data = new byte [2];
            int zdata = (int)((Z << 4) | UnknownZ);
            BitConv.ToInt16(data,0,(short)zdata);
            return data;
        }
    }
}
