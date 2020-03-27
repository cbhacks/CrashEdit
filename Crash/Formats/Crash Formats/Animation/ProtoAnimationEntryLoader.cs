using System;

namespace Crash
{
    [EntryType(1,GameVersion.Crash1Beta1995)]
    public sealed class ProtoAnimationEntryLoader : EntryLoader
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
            return new ProtoAnimationEntry(frames,!frames[0].Proto,eid);
        }
    }
}
