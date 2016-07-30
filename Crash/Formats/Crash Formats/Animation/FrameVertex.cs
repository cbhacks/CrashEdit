using System;

namespace Crash
{
    public struct FrameVertex : IPosition
    {
        public static FrameVertex Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 3)
                throw new ArgumentException("Value must be 3 bytes long.","data");
            byte x = data[0];
            byte y = data[1];
            byte z = data[2];
            return new FrameVertex(x,y,z);
        }

        private byte x;
        private byte y;
        private byte z;

        public FrameVertex(byte x,byte y,byte z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public byte X
        {
            get { return x; }
        }

        public byte Y
        {
            get { return y; }
        }

        public byte Z
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
            byte[] data = new byte [3];
            data[0] = x;
            data[1] = y;
            data[2] = z;
            return data;
        }
    }
}
