using System;

namespace Crash
{
    public static class BitConv
    {
        public static short FromInt16(byte[] str,int offset)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (offset + 2 > str.Length)
                throw new ArgumentOutOfRangeException("offset");
            int result = 0;
            result |= str[offset + 0] << 8 * 0;
            result |= str[offset + 1] << 8 * 1;
            return (short)result;
        }

        public static int FromInt24(byte[] str,int offset)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (offset + 3 > str.Length)
                throw new ArgumentOutOfRangeException("offset");
            int result = 0;
            result |= str[offset + 0] << 8 * 0;
            result |= str[offset + 1] << 8 * 1;
            result |= str[offset + 2] << 8 * 2;
            if ((result & 0x800000) != 0)
            {
                unchecked
                {
                    result |= (int)0xFF000000;
                }
            }
            return result;
        }

        public static int FromInt32(byte[] str,int offset)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (offset + 4 > str.Length)
                throw new ArgumentOutOfRangeException("offset");
            int result = 0;
            result |= str[offset + 0] << 8 * 0;
            result |= str[offset + 1] << 8 * 1;
            result |= str[offset + 2] << 8 * 2;
            result |= str[offset + 3] << 8 * 3;
            return result;
        }

        public static void ToInt16(byte[] str,int offset,short value)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (offset + 2 > str.Length)
                throw new ArgumentOutOfRangeException("offset");
            str[offset] = (byte)value;
            str[offset + 1] = (byte)(value >> 8);
        }

        public static void ToInt24(byte[] str,int offset,int value)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (offset + 3 > str.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (value < -8388608 || value > 8388607)
                throw new ArgumentOutOfRangeException("value");
            str[offset] = (byte)value;
            str[offset + 1] = (byte)(value >> 8);
            str[offset + 2] = (byte)(value >> 8 * 2);
        }

        public static void ToInt32(byte[] str,int offset,int value)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (offset + 4 > str.Length)
                throw new ArgumentOutOfRangeException("offset");
            str[offset] = (byte)value;
            str[offset + 1] = (byte)(value >> 8);
            str[offset + 2] = (byte)(value >> 8 * 2);
            str[offset + 3] = (byte)(value >> 8 * 3);
        }

        public static int FromInt32(int? value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            byte [] str = BitConverter.GetBytes((int)value);
            int result = 0;
            result |= str[0];
            result |= str[1] << 8;
            result |= str[2] << 8 * 2;
            result |= str[3] << 8 * 3;
            return result;
        }
    }
}
