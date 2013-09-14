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
            bool lightingflag = ((data[4] & 1) != 0);
            return new OldSceneryVertex(x,y,z,red,green,blue,lightingflag);
        }

        private short x;
        private short y;
        private short z;
        private byte red;
        private byte green;
        private byte blue;
        private bool lightingflag;

        public OldSceneryVertex(short x,short y,short z,byte red,byte green,byte blue,bool lightingflag)
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
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.lightingflag = lightingflag;
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

        public byte Red
        {
            get { return red; }
        }

        public byte Green
        {
            get { return green; }
        }

        public byte Blue
        {
            get { return blue; }
        }

        public bool LightingFlag
        {
            get { return lightingflag; }
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
            int zlow = (z >> 3) & 0xFF;
            int zmid = (z >> 11) & 0x3;
            int zhigh = (z >> 13) & 0x7;
            byte[] data = new byte [8];
            data[0] = red;
            data[1] = green;
            data[2] = blue;
            data[3] = (byte)zlow;
            BitConv.ToInt16(data,4,x);
            data[4] |= (byte)(zmid << 1);
            BitConv.ToInt16(data,6,y);
            data[6] |= (byte)(zhigh);
            if (lightingflag)
            {
                data[4] |= 1;
            }
            return data;
        }
    }
}
