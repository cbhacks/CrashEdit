namespace Crash
{
    public static class BitConv
    {
        public static short FromHalf(byte[] str,int offset)
        {
            return FromShortLE(str,offset);
        }

        public static int FromWord(byte[] str,int offset)
        {
            return FromIntLE(str,offset);
        }

        public static short FromShortLE(byte[] str,int offset)
        {
            if (str == null)
                throw new System.ArgumentNullException("String cannot be null.");
            if (offset < 0)
                throw new System.ArgumentException("Offset cannot be negative.");
            if (offset + 2 > str.Length)
                throw new System.ArgumentOutOfRangeException("Offset exceeds string bounds.");
            int result = 0;
            result |= str[offset + 0] << 8 * 0;
            result |= str[offset + 1] << 8 * 1;
            return (short)result;
        }

        public static int FromIntLE(byte[] str,int offset)
        {
            if (str == null)
                throw new System.ArgumentNullException("String cannot be null.");
            if (offset < 0)
                throw new System.ArgumentException("Offset cannot be negative.");
            if (offset + 4 > str.Length)
                throw new System.ArgumentOutOfRangeException("Offset exceeds string bounds.");
            int result = 0;
            result |= str[offset + 0] << 8 * 0;
            result |= str[offset + 1] << 8 * 1;
            result |= str[offset + 2] << 8 * 2;
            result |= str[offset + 3] << 8 * 3;
            return result;
        }

        public static short FromShortBE(byte[] str,int offset)
        {
            if (str == null)
                throw new System.ArgumentNullException("String cannot be null.");
            if (offset < 0)
                throw new System.ArgumentException("Offset cannot be negative.");
            if (offset + 2 > str.Length)
                throw new System.ArgumentOutOfRangeException("Offset exceeds string bounds.");
            int result = 0;
            result |= str[offset + 0] << 8 * 1;
            result |= str[offset + 1] << 8 * 0;
            return (short)result;
        }

        public static int FromIntBE(byte[] str,int offset)
        {
            if (str == null)
                throw new System.ArgumentNullException("String cannot be null.");
            if (offset < 0)
                throw new System.ArgumentException("Offset cannot be negative.");
            if (offset + 4 > str.Length)
                throw new System.ArgumentOutOfRangeException("Offset exceeds string bounds.");
            int result = 0;
            result |= str[offset + 0] << 8 * 3;
            result |= str[offset + 1] << 8 * 2;
            result |= str[offset + 2] << 8 * 1;
            result |= str[offset + 3] << 8 * 0;
            return result;
        }

        public static string FromASCII(byte[] str,int offset,int length)
        {
            return System.Text.Encoding.ASCII.GetString(str,offset,length);
        }

        public static void ToHalf(byte[] str,int offset,short value)
        {
            ToShortLE(str,offset,value);
        }

        public static void ToWord(byte[] str,int offset,int value)
        {
            ToIntLE(str,offset,value);
        }

        public static void ToShortLE(byte[] str,int offset,short value)
        {
            if (str == null)
                throw new System.ArgumentNullException("String cannot be null.");
            if (offset < 0)
                throw new System.ArgumentException("Offset cannot be negative.");
            if (offset + 2 > str.Length)
                throw new System.ArgumentOutOfRangeException("Offset exceeds string bounds.");
            str[offset + 0] = (byte)((value >> 8 * 0) & 0xFF);
            str[offset + 1] = (byte)((value >> 8 * 1) & 0xFF);
        }

        public static void ToIntLE(byte[] str,int offset,int value)
        {
            if (str == null)
                throw new System.ArgumentNullException("String cannot be null.");
            if (offset < 0)
                throw new System.ArgumentException("Offset cannot be negative.");
            if (offset + 4 > str.Length)
                throw new System.ArgumentOutOfRangeException("Offset exceeds string bounds.");
            str[offset + 0] = (byte)((value >> 8 * 0) & 0xFF);
            str[offset + 1] = (byte)((value >> 8 * 1) & 0xFF);
            str[offset + 2] = (byte)((value >> 8 * 2) & 0xFF);
            str[offset + 3] = (byte)((value >> 8 * 3) & 0xFF);
        }

        public static void ToShortBE(byte[] str,int offset,short value)
        {
            if (str == null)
                throw new System.ArgumentNullException("String cannot be null.");
            if (offset < 0)
                throw new System.ArgumentException("Offset cannot be negative.");
            if (offset + 2 > str.Length)
                throw new System.ArgumentOutOfRangeException("Offset exceeds string bounds.");
            str[offset + 0] = (byte)((value >> 8 * 1) & 0xFF);
            str[offset + 1] = (byte)((value >> 8 * 0) & 0xFF);
        }

        public static void ToIntBE(byte[] str,int offset,int value)
        {
            if (str == null)
                throw new System.ArgumentNullException("String cannot be null.");
            if (offset < 0)
                throw new System.ArgumentException("Offset cannot be negative.");
            if (offset + 4 > str.Length)
                throw new System.ArgumentOutOfRangeException("Offset exceeds string bounds.");
            str[offset + 0] = (byte)((value >> 8 * 3) & 0xFF);
            str[offset + 1] = (byte)((value >> 8 * 2) & 0xFF);
            str[offset + 2] = (byte)((value >> 8 * 1) & 0xFF);
            str[offset + 3] = (byte)((value >> 8 * 0) & 0xFF);
        }
    }
}
