using System;

namespace Crash
{
    public class EvListEventArgs<T> : EventArgs
    {
        private int index;
        private T item;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public T Item
        {
            get { return item; }
            set { item = value; }
        }
    }
}
