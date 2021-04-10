using System;

namespace Crash
{
    public static class PixelConv
    {
        internal static byte[] colorTable = new byte[256];
        internal static byte[] colorTableInverse = new byte[32];
        static PixelConv()
        {
            for (int i = 0; i < 256; ++i)
            {
                colorTable[i] = (byte)(i * 31 / 255);
            }
            for (int i = 0; i < 32; ++i)
            {
                for (int j = 0; j < 256; ++j)
                {
                    if (colorTable[j] == i)
                    {
                        colorTableInverse[i] = (byte)j;
                        break;
                    }
                }
            }
        }

        public static short Pack1555(byte a1,byte b5,byte c5,byte d5)
        {
            if ((a1 & 0x1) != a1)
                throw new ArgumentOutOfRangeException("A1 must be 1-bit.");
            if ((b5 & 0x1F) != b5)
                throw new ArgumentOutOfRangeException("B5 must be 5-bit.");
            if ((c5 & 0x1F) != c5)
                throw new ArgumentOutOfRangeException("C5 must be 5-bit.");
            if ((d5 & 0x1F) != d5)
                throw new ArgumentOutOfRangeException("D5 must be 5-bit.");
            return (short)(a1 << 15 | b5 << 10 | c5 << 5 | d5);
        }

        public static void Unpack1555(short data,out byte a1,out byte b5,out byte c5,out byte d5)
        {
            a1 = (byte)(data >> 15 & 0x1);
            b5 = (byte)(data >> 10 & 0x1F);
            c5 = (byte)(data >> 5 & 0x1F);
            d5 = (byte)(data & 0x1F);
        }

        public static int Convert5551_8888(short p, int mode)
        {
            byte r = colorTableInverse[p >> 0 & 0x1F];
            byte g = colorTableInverse[p >> 5 & 0x1F];
            byte b = colorTableInverse[p >> 10 & 0x1F];
            byte a = (byte)(p >> 15 & 1);
            switch (mode)
            {
                case 0: a = a == 1 ? (byte)0x7F : (r+g+b == 0 ? (byte)0 : (byte)0xFF); break;
                case 1: a = a == 1 ? (byte)0xFF : (r+g+b == 0 ? (byte)0 : (byte)0xFF); break;
                case 2: a = a == 1 ? (byte)0xFF : (r+g+b == 0 ? (byte)0 : (byte)0xFF); break;
                case 3: a = a == 1 ? (byte)0xFF : (r+g+b == 0 ? (byte)0 : (byte)0xFF); break;
            }
            return (a << 24) | (r << 16) | (g << 8) | b;
        }
    }
}
