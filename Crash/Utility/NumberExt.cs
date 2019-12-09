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
    }
}
