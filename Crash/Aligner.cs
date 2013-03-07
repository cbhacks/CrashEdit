using System;

namespace Crash
{
    public static class Aligner
    {
        public static void Align(ref int position,int alignment)
        {
            Align(ref position,alignment,0);
        }

        public static void Align(ref int position,int alignment,int offset)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");
            if (alignment <= 0)
                throw new ArgumentOutOfRangeException("alignment");
            if (offset < 0 || offset >= alignment)
                throw new ArgumentOutOfRangeException("offset");
            while (position % alignment != offset)
            {
                // Ugly hack
                // Change this to an if and use proper math some day
                position++;
            }
        }
    }
}
