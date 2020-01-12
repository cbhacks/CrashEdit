using System;

namespace Crash
{
    [EntryType(20,GameVersion.Crash1BetaMAR08)]
    [EntryType(20,GameVersion.Crash1BetaMAY11)]
    [EntryType(20,GameVersion.Crash1)]
    public sealed class ColoredAnimationEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            OldFrame[] frames = new OldFrame [items.Length];
            for (int i = 0;i < frames.Length;i++)
            {
                frames[i] = OldFrame.Load(items[i]);
            }
            return new ColoredAnimationEntry(frames,eid);
        }
    }
}
