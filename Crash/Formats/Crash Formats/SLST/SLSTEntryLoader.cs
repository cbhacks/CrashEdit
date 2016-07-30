using System;

namespace Crash
{
    [EntryType(4,GameVersion.Crash1Beta1995)]
    [EntryType(4,GameVersion.Crash1BetaMAR08)]
    [EntryType(4,GameVersion.Crash1BetaMAY11)]
    [EntryType(4,GameVersion.Crash1)]
    [EntryType(4,GameVersion.Crash2)]
    [EntryType(4,GameVersion.Crash3)]
    public sealed class SLSTEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length < 2)
                ErrorManager.SignalError("SLSTEntry: Item count is wrong");
            //SLSTItem0 slstitemfirst = SLSTItem0.Load(items[0]);
            SLSTItem[] slstitems = new SLSTItem [items.Length];
            for (int i = 0;i < items.Length;i++)
            {
                slstitems[i] = SLSTItem.Load(items[i]);
                /*if (slstitems[i].Unknown1 != 1)
                {
                    ErrorManager.SignalIgnorableError("SLSTEntry: Item unknown field is wrong");
                }*/
            }
            //SLSTItem0 slstitemlast = SLSTItem0.Load(items[items.Length - 1]);
            return new SLSTEntry(slstitems,eid,size);
        }
    }
}
