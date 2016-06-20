using System;

namespace Crash
{
    [EntryType(4,GameVersion.Crash1Beta1995)]
    [EntryType(4,GameVersion.Crash1BetaMAR08)]
    [EntryType(4,GameVersion.Crash1BetaMAY11)]
    [EntryType(4,GameVersion.Crash1)]
    [EntryType(4,GameVersion.Crash2)]
    [EntryType(4,GameVersion.Crash3)]
    public sealed class T4EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length < 2)
                ErrorManager.SignalError("T4Entry: Item count is wrong");
            //T4Item0 t4itemfirst = T4Item0.Load(items[0]);
            T4Item[] t4items = new T4Item [items.Length];
            for (int i = 0;i < items.Length;i++)
            {
                t4items[i] = T4Item.Load(items[i]);
                /*if (t4items[i].Unknown1 != 1)
                {
                    ErrorManager.SignalIgnorableError("T4Entry: Item unknown field is wrong");
                }*/
            }
            //T4Item0 t4itemlast = T4Item0.Load(items[items.Length - 1]);
            return new T4Entry(t4items,eid);
        }
    }
}
