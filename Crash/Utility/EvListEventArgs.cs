using System;

namespace Crash
{
    public class EvListEventArgs<T> : EventArgs
    {
        public int Index { get; set; }
        public T Item { get; set; }
    }
}
