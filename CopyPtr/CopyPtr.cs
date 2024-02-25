public class CopyPtr
{
    /// <summary>
    /// A method to copy data from one pointer to another, byte by byte.
    /// </summary>
    /// <param name="source">The source pointer.</param>
    /// <param name="sourceOffset">An offset into the source buffer.</param>
    /// <param name="destination">The destination pointer.</param>
    /// <param name="destinationOffset">An offset into the destination buffer.</param>
    /// <param name="count">The number of bytes to copy.</param>
    public static unsafe void Copy(IntPtr source, int sourceOffset, IntPtr destination, int destinationOffset, int count)
    {
        byte* src = (byte*)source + sourceOffset;
        byte* dst = (byte*)destination + destinationOffset;
        byte* end = dst + count;

        while (dst != end)
            *dst++ = *src++;
    }
}
