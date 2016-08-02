using System;

namespace Crash
{
    public struct ModelPosition
    {
        public static ModelPosition Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 4)
                throw new ArgumentException("Value must be 4 bytes long.","data");
            int structure = BitConv.FromInt32(data,0);
            return new ModelPosition(structure);
        }
        
        private byte xbits;
        private byte ybits;
        private byte zbits;
        private byte x;
        private byte y;
        private byte z;

        public ModelPosition(int structure)
        {
            x = (byte)((structure >> 25) & 0x7F);
            z = (byte)(structure >> 17);
            y = (byte)(structure >> 9);
            xbits = (byte)((structure >> 6) & 0x7);
            zbits = (byte)((structure >> 3) & 0x7);
            ybits = (byte)(structure & 0x7);
        }

        public byte X
        {
            get { return x; }
        }

        public byte Z
        {
            get { return z; }
        }

        public byte Y
        {
            get { return y; }
        }

        public byte XBits
        {
            get { return xbits; }
        }

        public byte ZBits
        {
            get { return zbits; }
        }

        public byte YBits
        {
            get { return ybits; }
        }

        public byte[] Save()
        {
            //ErrorManager.SignalError("ModelPosition cannot be saved.");
            byte[] result = new byte[4];
            result[0] = (byte)((xbits << 6) | (zbits << 3) | ybits);
            result[1] = (byte)((y << 1) | (xbits >> 2));
            result[2] = (byte)((z << 1) | (y >> 7));
            result[3] = (byte)((x << 1) | (z >> 7));
            return result;
        }
    }
}
