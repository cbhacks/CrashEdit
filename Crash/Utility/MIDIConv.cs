namespace CrashEdit.Crash
{
    public static class MIDIConv
    {
        public static int From3BE(byte[] str, int offset)
        {
            ArgumentNullException.ThrowIfNull(str);
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + 3 > str.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            int result = 0;
            result |= str[offset + 0] << 8 * 2;
            result |= str[offset + 1] << 8 * 1;
            result |= str[offset + 2] << 8 * 0;
            return result;
        }

        public static void To3BE(byte[] str, int offset, int value)
        {
            ArgumentNullException.ThrowIfNull(str);
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + 3 > str.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if ((value & 0xFFFFFF) != value)
                throw new ArgumentOutOfRangeException(nameof(value));
            str[offset + 0] = (byte)((value >> 8 * 2) & 0xFF);
            str[offset + 1] = (byte)((value >> 8 * 1) & 0xFF);
            str[offset + 2] = (byte)((value >> 8 * 0) & 0xFF);
        }
    }
}
