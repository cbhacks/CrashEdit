namespace Crash.Audio
{
    public static class MIDIConv
    {
        public static int From3BE(byte[] str,int offset)
        {
            if (str == null)
                throw new System.ArgumentNullException("String cannot be null.");
            if (offset < 0)
                throw new System.ArgumentException("Offset cannot be negative.");
            if (offset + 3 > str.Length)
                throw new System.ArgumentOutOfRangeException("Offset exceeds string bounds.");
            int result = 0;
            result |= str[offset + 0] << 8 * 2;
            result |= str[offset + 1] << 8 * 1;
            result |= str[offset + 2] << 8 * 0;
            return result;
        }

        public static void To3BE(byte[] str,int offset,int value)
        {
            if (str == null)
                throw new System.ArgumentNullException("String cannot be null.");
            if (offset < 0)
                throw new System.ArgumentException("Offset cannot be negative.");
            if (offset + 3 > str.Length)
                throw new System.ArgumentOutOfRangeException("Offset exceeds string bounds.");
            if ((value & 0xFF000000) != 0)
                throw new System.ArgumentOutOfRangeException("Value must be in the range 0 to 0x00FFFFFF inclusive.");
            str[offset + 0] = (byte)((value >> 8 * 2) & 0xFF);
            str[offset + 1] = (byte)((value >> 8 * 1) & 0xFF);
            str[offset + 2] = (byte)((value >> 8 * 0) & 0xFF);
        }
    }
}
