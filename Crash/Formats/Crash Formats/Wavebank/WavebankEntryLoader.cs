using System;

namespace Crash
{
    [EntryType(14,GameVersion.Crash1BetaMAR08)]
    [EntryType(14,GameVersion.Crash1BetaMAY11)]
    [EntryType(14,GameVersion.Crash1)]
    [EntryType(14,GameVersion.Crash2)]
    [EntryType(14,GameVersion.Crash3)]
    public sealed class WavebankEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 2)
            {
                ErrorManager.SignalError("WavebankEntry: Wrong number of items");
            }
            if (items[0].Length != 8)
            {
                ErrorManager.SignalError("WavebankEntry: First item length is wrong");
            }
            int id = BitConv.FromInt32(items[0],0);
            int length = BitConv.FromInt32(items[0],4);
            if (id < 0 || id > 6)
            {
                ErrorManager.SignalIgnorableError("WavebankEntry: ID is invalid");
            }
            if (length != items[1].Length)
            {
                ErrorManager.SignalIgnorableError("WavebankEntry: Length field mismatch");
            }
            return new WavebankEntry(id,SampleSet.Load(items[1]),eid,size);
        }
    }
}
