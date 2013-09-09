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
            short x = (short)(BitConv.FromInt16(data,4) >> 3);
            short y = (short)(BitConv.FromInt16(data,6) >> 3);
            int zhigh = data[6] & 7;
            int zmid = (data[4] & 6) >> 1;
            int zlow = data[3];
            short z = (short)(zhigh << 13 | zmid << 11 | zlow << 3);
            // Sign extend!
            z >>= 3;
            byte f0 = data[0];
            byte f1 = data[1];
            byte f2 = data[2];
            if ((data[4] & 1) != 0)
            {
                // FIXME
                //ErrorManager.SignalIgnorableError("OldSceneryVertex: Unknown bit is set");
            }
            return new OldSceneryVertex(x,y,z,f0,f1,f2);
        }

        private short x;
        private short y;
        private short z;
        private byte f0;
        private byte f1;
        private byte f2;

        public OldSceneryVertex(short x,short y,short z,byte f0,byte f1,byte f2)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.f0 = f0;
            this.f1 = f1;
            this.f2 = f2;
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

        public byte F0
        {
            get { return f0; }
        }

        public byte F1
        {
            get { return f1; }
        }

        public byte F2
        {
            get { return f2; }
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
    }
}
