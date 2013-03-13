using System;

namespace Crash
{
    public static class BitConv
    {
        [Obsolete]
        public static short FromHalf(byte[] str,int offset)
        {
            return FromShortLE(str,offset);
        }

        [Obsolete]
        public static int FromWord(byte[] str,int offset)
        {
            return FromIntLE(str,offset);
        }

        [Obsolete]
        public static short FromShortLE(byte[] str,int offset)
        {
            return FromInt16(str,offset);
        }

        [Obsolete]
        public static int FromIntLE(byte[] str,int offset)
        {
            return FromInt32(str,offset);
        }

        [Obsolete]
        public static short FromShortBE(byte[] str,int offset)
        {
            return BEBitConv.FromInt16(str,offset);
        }

        [Obsolete]
        public static int FromIntBE(byte[] str,int offset)
        {
            return BEBitConv.FromInt32(str,offset);
        }

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

        [Obsolete]
        public static void ToHalf(byte[] str,int offset,short value)
        {
            ToShortLE(str,offset,value);
        }

        [Obsolete]
        public static void ToWord(byte[] str,int offset,int value)
        {
            ToIntLE(str,offset,value);
        }

        [Obsolete]
        public static void ToShortLE(byte[] str,int offset,short value)
        {
            ToInt16(str,offset,value);
        }

        [Obsolete]
        public static void ToIntLE(byte[] str,int offset,int value)
        {
            ToInt32(str,offset,value);
        }

        [Obsolete]
        public static void ToShortBE(byte[] str,int offset,short value)
        {
            BEBitConv.ToInt16(str,offset,value);
        }

        [Obsolete]
        public static void ToIntBE(byte[] str,int offset,int value)
        {
            BEBitConv.ToInt32(str,offset,value);
        }

        public static void ToInt16(byte[] str,int offset,short value)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (offset + 2 > str.Length)
                throw new ArgumentOutOfRangeException("offset");
            str[offset + 0] = (byte)((value >> 8 * 0) & 0xFF);
            str[offset + 1] = (byte)((value >> 8 * 1) & 0xFF);
        }

        public static void ToInt32(byte[] str,int offset,int value)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (offset + 4 > str.Length)
                throw new ArgumentOutOfRangeException("offset");
            str[offset + 0] = (byte)((value >> 8 * 0) & 0xFF);
            str[offset + 1] = (byte)((value >> 8 * 1) & 0xFF);
            str[offset + 2] = (byte)((value >> 8 * 2) & 0xFF);
            str[offset + 3] = (byte)((value >> 8 * 3) & 0xFF);
        }
    }
}
