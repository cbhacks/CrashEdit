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

        private byte x;
        private byte y;
        private byte z;
        private byte normalx;
        private byte normaly;
        private byte normalz;

        public OldFrameVertex(byte x,byte y,byte z,byte normalx,byte normaly,byte normalz)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.normalx = normalx;
            this.normaly = normaly;
            this.normalz = normalz;
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

        public byte NormalX
        {
            get { return normalx; }
        }

        public byte NormalY
        {
            get { return normaly; }
        }

        public byte NormalZ
        {
            get { return normalz; }
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
            byte[] data = new byte [6];
            data[0] = x;
            data[1] = y;
            data[2] = z;
            data[3] = normalx;
            data[4] = normaly;
            data[5] = normalz;
            return data;
        }
    }
}
