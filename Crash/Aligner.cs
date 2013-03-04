using System;

namespace Crash
{
    public static class Aligner
    {
        public static void Align(ref int position,int alignment)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");
            if (alignment <= 0)
                throw new ArgumentOutOfRangeException("alignment");
            if (position % alignment != 0)
            {
                position += alignment - position % alignment;
            }
        }
    }
}
