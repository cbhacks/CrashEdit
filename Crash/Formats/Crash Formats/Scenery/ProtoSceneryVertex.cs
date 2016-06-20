using System;

namespace Crash
{
    public struct ProtoSceneryVertex : IPosition
    {
        public static ProtoSceneryVertex Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 4)
                throw new ArgumentException("Value must be 4 bytes long.","data");
            short x = (short)(data[0]);
            short y = (short)(BitConv.FromInt16(data,1));
            short z = (short)(data[3]);
            return new ProtoSceneryVertex(x,y,z);
        }

        private short x;
        private short y;
        private short z;

        public ProtoSceneryVertex(short x,short y,short z)
        {
            if ((x & 0x7) != 0)
                throw new ArgumentException("Value must be a multiple of 8.","x");
            if ((y & 0x7) != 0)
                throw new ArgumentException("Value must be a multiple of 8.","y");
            if ((z & 0x7) != 0)
                throw new ArgumentException("Value must be a multiple of 8.","z");
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public short X
        {
            get { return x; }
        }

        public short Y
        {
            get { return y; }
        }

        public short Z
        {
            get { return z; }
        }double IPosition.X
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

        public byte[] Save()
        {
            byte[] result = new byte [4];
            BitConv.ToInt16(result,0 & 0xFFF0,x);
            return result;
        }
    }
}
