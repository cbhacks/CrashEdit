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
            uint xy = (uint)BitConv.FromInt32(xydata,0);
            ushort z = (ushort)BitConv.FromInt16(zdata,0);
            ushort y = (ushort)(xy >> 16);
            ushort x = (ushort)xy;
            uint unknownx = (uint)x & 0xF;
            uint unknowny = (uint)y & 0xF;
            uint unknownz = (uint)z & 0xF;
            x >>= 4;
            y >>= 4;
            z >>= 4;
            return new NewSceneryVertex(x,y,z,unknownx,unknowny,unknownz);
        }

        private uint x;
        private uint y;
        private uint z;
        private uint unknownx;
        private uint unknowny;
        private uint unknownz;

        public NewSceneryVertex(uint x,uint y,uint z,uint unknownx,uint unknowny,uint unknownz)
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
            this.x = x;
            this.y = y;
            this.z = z;
            this.unknownx = unknownx;
            this.unknowny = unknowny;
            this.unknownz = unknownz;
        }

        public uint X
        {
            get { return x; }
        }

        public uint Y
        {
            get { return y; }
        }

        public uint Z
        {
            get { return z; }
        }

        public uint UnknownX
        {
            get { return unknownx; }
        }

        public uint UnknownY
        {
            get { return unknowny; }
        }

        public uint UnknownZ
        {
            get { return unknownz; }
        }

        public int Color
        {
            get { return ((int)unknowny & 0x3) << 8 | (int)unknownx << 4 | (int)unknownz; }
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
            uint xdata = (x << 4) | unknownx;
            uint ydata = (y << 4) | unknowny;
            BitConv.ToInt16(data,0,(short)xdata);
            BitConv.ToInt16(data,2,(short)ydata);
            return data;
        }

        public byte[] SaveZ()
        {
            byte[] data = new byte [2];
            uint zdata = (z << 4) | unknownz;
            BitConv.ToInt16(data,0,(short)zdata);
            return data;
        }
    }
}
