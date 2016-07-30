using System;

namespace Crash
{
    [EntryType(1,GameVersion.Crash1Beta1995)]
    public sealed class ProtoAnimationEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            ProtoFrame[] frames = new ProtoFrame [items.Length];
            for (int i = 0;i < frames.Length;i++)
            {
                frames[i] = ProtoFrame.Load(items[i]);
            }
            return new ProtoAnimationEntry(frames,eid,size);
        }
    }
}
