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
            ArgumentOutOfRangeException.ThrowIfNegative(position);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(alignment);
            if (position > 0) position += -(((position - 1) % alignment) + 1) + alignment;
        }
    }
}
