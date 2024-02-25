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

        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }
    }
}
