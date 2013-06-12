using System;

namespace Crash
{
    [EntryType(4)]
    public sealed class T4EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            T4Item[] t4items = new T4Item [items.Length];
            for (int i = 0;i < items.Length;i++)
            {
                t4items[i] = T4Item.Load(items[i]);
                if (t4items[i].Unknown1 != ((i == 0 || i == items.Length - 1) ? 0 : 1))
                {
                    ErrorManager.SignalIgnorableError("T4Entry: Item unknown field is wrong");
                }
            }
            return new T4Entry(t4items,eid);
        }
    }
}
