using System;
using System.Collections;
using System.Collections.Generic;

namespace Crash
{
    public sealed class EvList<T> : IList<T>
    {
        private List<T> list;

        public event EvListEventHandler<T> ItemAdded;
        public event EvListEventHandler<T> ItemRemoved;

        public EvList()
        {
            this.list = new List<T>();
        }

        public EvList(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            this.list = new List<T>(collection);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public T this[int i]
        {
            get { return list[i]; }
            set
            {
                list[i] = value;
                EvListEventArgs<T> e = new EvListEventArgs<T>();
                e.Index = i;
                e.Item = value;
                if (ItemRemoved != null)
                {
                    ItemRemoved(this, e);
                }
                if (ItemAdded != null)
                {
                    ItemAdded(this, e);
                }
            }
        }

        public Command CmSet(int i, T item)
        {
            if (i < 0 || i >= list.Count)
                throw new ArgumentOutOfRangeException("i");
            return new SetCommand(this, i, item);
        }

        public void Add(T item)
        {
            list.Add(item);
            if (ItemAdded != null)
            {
                EvListEventArgs<T> e = new EvListEventArgs<T>();
                e.Index = list.Count - 1;
                e.Item = item;
                ItemAdded(this, e);
            }
        }

        public Command CmAdd(T item)
        {
            return new InsertCommand(this, list.Count, item);
        }

        public void Clear()
        {
            while (list.Count > 0)
            {
                T item = list[0];
                list.RemoveAt(0);
                if (ItemRemoved != null)
                {
                    EvListEventArgs<T> e = new EvListEventArgs<T>();
                    e.Index = 0;
                    e.Item = item;
                    ItemRemoved(this, e);
                }
            }
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int i)
        {
            list.CopyTo(array, i);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int i, T item)
        {
            list.Insert(i, item);
            if (ItemAdded != null)
            {
                EvListEventArgs<T> e = new EvListEventArgs<T>();
                e.Index = i;
                e.Item = item;
                ItemAdded(this, e);
            }
        }

        public Command CmInsert(int i, T item)
        {
            if (i < 0 || i > list.Count)
                throw new ArgumentOutOfRangeException("i");
            return new InsertCommand(this, i, item);
        }

        public bool Remove(T item)
        {
            int i = list.IndexOf(item);
            if (i != -1)
            {
                if (ItemRemoved != null)
                {
                    EvListEventArgs<T> e = new EvListEventArgs<T>();
                    e.Index = i;
                    e.Item = item;
                    ItemRemoved(this, e);
                }
                list.RemoveAt(i);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Command CmRemove(T item)
        {
            int i = list.IndexOf(item);
            if (i != -1)
            {
                return new RemoveCommand(this, i);
            }
            else
            {
                return null;
            }
        }

        public void RemoveAt(int i)
        {
            T item = list[i];
            list.RemoveAt(i);
            if (ItemRemoved != null)
            {
                EvListEventArgs<T> e = new EvListEventArgs<T>();
                e.Index = i;
                e.Item = item;
                ItemRemoved(this, e);
            }
        }

        public Command CmRemoveAt(int i)
        {
            if (i < 0 || i >= list.Count)
                throw new ArgumentOutOfRangeException("i");
            return new RemoveCommand(this, i);
        }

        public void Populate(EvListEventHandler<T> method)
        {
            for (int i = 0; i < Count; i++)
            {
                EvListEventArgs<T> e = new EvListEventArgs<T>();
                e.Item = this[i];
                e.Index = i;
                method(this, e);
            }
        }

        private sealed class SetCommand : Command
        {
            EvList<T> list;
            int i;
            T item;

            public SetCommand(EvList<T> list, int i, T item)
            {
                this.list = list;
                this.i = i;
                this.item = item;
            }

            protected override Command RunImpl()
            {
                T olditem = list[i];
                list[i] = item;
                return new SetCommand(list, i, olditem);
            }
        }

        private sealed class InsertCommand : Command
        {
            EvList<T> list;
            int i;
            T item;

            public InsertCommand(EvList<T> list, int i, T item)
            {
                this.list = list;
                this.i = i;
                this.item = item;
            }

            protected override Command RunImpl()
            {
                list.Insert(i, item);
                return new RemoveCommand(list, i);
            }
        }

        private sealed class RemoveCommand : Command
        {
            EvList<T> list;
            int i;

            public RemoveCommand(EvList<T> list, int i)
            {
                this.list = list;
                this.i = i;
            }

            protected override Command RunImpl()
            {
                T item = list[i];
                list.RemoveAt(i);
                return new InsertCommand(list, i, item);
            }
        }
    }
}
