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

        public ModelPosition(int structure)
        {
            X = (byte)((structure >> 25) & 0x7F);
            Z = (byte)(structure >> 17);
            Y = (byte)(structure >> 9);
            XBits = (byte)((structure >> 6) & 0x7);
            ZBits = (byte)((structure >> 3) & 0x7);
            YBits = (byte)(structure & 0x7);
        }

        public byte X { get; }
        public byte Z { get; }
        public byte Y { get; }
        public byte XBits { get; }
        public byte ZBits { get; }
        public byte YBits { get; }

        public byte[] Save()
        {
            byte[] result = new byte[4];
            result[0] = (byte)((XBits << 6) | (ZBits << 3) | YBits);
            result[1] = (byte)((Y << 1) | (XBits >> 2));
            result[2] = (byte)((Z << 1) | (Y >> 7));
            result[3] = (byte)((X << 1) | (Z >> 7));
            return result;
        }
    }
}
