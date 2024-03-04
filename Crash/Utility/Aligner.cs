namespace CrashEdit.Crash
{
    public static class Aligner
    {
        public static int Align(int position, int alignment)
        {
            Align(ref position, alignment);
            return position;
        }

        public static void Align(ref int position, int alignment)
        {
            Align(ref position, alignment, 0);
        }

        public static void Align(ref int position, int alignment, int offset)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException(nameof(position));
            if (alignment <= 0)
                throw new ArgumentOutOfRangeException(nameof(alignment));
            if (offset < 0 || offset >= alignment)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (position > 0) position += -(((position - 1) % alignment) + 1) + alignment;
            //irrelevant: position = position ? position - (((position - 1) % alignment) + 1) + alignment : position;
        }
    }
}
