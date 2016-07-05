using System;

namespace Crash
{
    public struct ProtoSceneryVertex : IPosition
    {
        public static ProtoSceneryVertex Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 6)
                throw new ArgumentException("Value must be 6 bytes long.","data");
            short x = BitConv.FromInt16(data, 0);
            short y = BitConv.FromInt16(data, 2);
            short z = BitConv.FromInt16(data, 4);
            return new ProtoSceneryVertex(x,y,z);
        }

        private short x;
        private short y;
        private short z;

        public ProtoSceneryVertex(short x,short y,short z)
        {
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

        public byte[] Save()
        {
            byte[] result = new byte [6];
            BitConv.ToInt16(result, 0, x);
            BitConv.ToInt16(result, 2, y);
            BitConv.ToInt16(result, 4, z);
            return result;
        }
    }
}
