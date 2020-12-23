namespace Crash
{
    public static class NumberExt
    {
        public static string TransformedString(this int num)
        {
            if (num > 64 || num < -64)
            {
                if (num < 0)
                    return string.Format("-0x{0:X}", -num);
                else
                    return string.Format("0x{0:X}", num);
            }
            else
                return $"{num}";
        }

        public static string TransformedString(this short num)
        {
            if (num > 64 || num < -64)
            {
                if (num < 0)
                    return string.Format("-0x{0:X}", -num);
                else
                    return string.Format("0x{0:X}", num);
            }
            else
                return $"{num}";
        }

        public static float GetFac(float a, float b, float f)
        {
            return a + (b - a) * f;
        }

        /// <summary>
        /// Convert a long number representing an unsigned 32-bit integer into a signed 32-bit integer.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int UInt32ToInt32(this long n)
        {
            if ((n & 0x80000000) != 0)
            {
                return -2147483648 + (int)(n & 0x7FFFFFFF);
            }
            else
            {
                return (int)n;
            }
        }
    }
}
