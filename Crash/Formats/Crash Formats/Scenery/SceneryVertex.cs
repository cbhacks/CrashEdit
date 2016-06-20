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

        private int x;
        private int y;
        private int z;
        private int unknownx;
        private int unknowny;
        private int unknownz;

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
            this.x = x;
            this.y = y;
            this.z = z;
            this.unknownx = unknownx;
            this.unknowny = unknowny;
            this.unknownz = unknownz;
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public int Z
        {
            get { return z; }
        }

        public int UnknownX
        {
            get { return unknownx; }
        }

        public int UnknownY
        {
            get { return unknowny; }
        }

        public int UnknownZ
        {
            get { return unknownz; }
        }

        public int Color
        {
            get { return (unknowny & 0x3) << 8 | unknownx << 4 | unknownz; }
        }

        double IPosition.X
        {
            get { return x; }
        }

        double IPosition.Y
        {
            get { return y; }
        }

        double IPosition.Z
        {
            get { return z; }
        }

        public byte[] SaveXY()
        {
            byte[] data = new byte [4];
            int xdata = (x << 4) | unknownx;
            int ydata = (y << 4) | unknowny;
            BitConv.ToInt16(data,0,(short)xdata);
            BitConv.ToInt16(data,2,(short)ydata);
            return data;
        }

        public byte[] SaveZ()
        {
            byte[] data = new byte [2];
            int zdata = (z << 4) | unknownz;
            BitConv.ToInt16(data,0,(short)zdata);
            return data;
        }
    }
}
