using System.Collections.Generic;

namespace Crash
{
    public static class ListExt
    {
        public static IList<T> Swap<T>(this IList<T> list, int a, int b)
        {
            T temp = list[a];
            list[a] = list[b];
            list[b] = temp;
            return list;
        }
    }
}
