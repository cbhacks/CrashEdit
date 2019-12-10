using System;

namespace Crash
{
    [EntryType(4,GameVersion.Crash2)]
    [EntryType(4,GameVersion.Crash3)]
    public sealed class SLSTEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length < 2)
                ErrorManager.SignalError("SLSTEntry: Item count is wrong");
            SLSTSource sourcestart = SLSTSource.Load(items[0]);
            SLSTDelta[] deltas = new SLSTDelta[items.Length - 2];
            for (int i = 0; i < items.Length - 2; i++)
            {
                deltas[i] = SLSTDelta.Load(items[i+1]);
            }
            SLSTSource sourceend = SLSTSource.Load(items[items.Length - 1]);
            return new SLSTEntry(sourcestart,sourceend,deltas,eid);
        }
    }
}
