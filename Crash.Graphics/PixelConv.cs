namespace Crash.Graphics
{
    public static class PixelConv
    {
        public static short Pack1555(byte a1,byte b5,byte c5,byte d5)
        {
            return (short)(a1 << 15 | b5 << 10 | c5 << 5 | d5);
        }

        public static void Unpack1555(short data,out byte a1,out byte b5,out byte c5,out byte d5)
        {
            a1 = (byte)(data >> 15 & 0x1);
            b5 = (byte)(data >> 10 & 0x1F);
            c5 = (byte)(data >> 5 & 0x1F);
            d5 = (byte)(data & 0x1F);
        }
    }
}
