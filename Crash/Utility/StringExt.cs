namespace Crash
{
    public static class StringExt
    {
        public static int AmountOf(this string str, char c)
        {
            int count = 0;
            foreach (char str_c in str)
                if (str_c == c) ++count;
            return count;
        }

        public static string Reverse(this string str)
        {
            var arr = str.ToCharArray();
            System.Array.Reverse(arr);
            return new string(arr);
        }
    }
}
