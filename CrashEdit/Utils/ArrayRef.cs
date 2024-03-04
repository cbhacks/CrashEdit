namespace CrashEdit.CE
{
    public static class ArrayUtil
    {
        public static ref T GetRefToElt<T>(T[] array, int index) where T : struct
        {
            return ref array[index];
        }
    }
}