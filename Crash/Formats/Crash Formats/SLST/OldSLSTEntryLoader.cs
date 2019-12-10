using System;

namespace Crash
{
    [EntryType(4,GameVersion.Crash1Beta1995)]
    [EntryType(4,GameVersion.Crash1BetaMAR08)]
    [EntryType(4,GameVersion.Crash1BetaMAY11)]
    [EntryType(4,GameVersion.Crash1)]
    public sealed class OldSLSTEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length < 2)
                ErrorManager.SignalError("OldSLSTEntry: Item count is wrong");
            OldSLSTSource sourcestart = OldSLSTSource.Load(items[0]);
            OldSLSTDelta[] deltas = new OldSLSTDelta[items.Length-2];
            for (int i = 0;i < items.Length-2;i++)
            {
                deltas[i] = OldSLSTDelta.Load(items[i+1]);
            }
            OldSLSTSource sourceend = OldSLSTSource.Load(items[items.Length - 1]);
            return new OldSLSTEntry(sourcestart,sourceend,deltas,eid);
        }
    }
}
