using System;

namespace Crash
{
    [EntryType(1,GameVersion.Crash2)]
    public sealed class AnimationEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            Frame[] frames = new Frame [items.Length];
            for (int i = 0;i < frames.Length;i++)
            {
                frames[i] = Frame.Load(items[i]);
            }
            return new AnimationEntry(frames,eid,size);
        }
    }
}
