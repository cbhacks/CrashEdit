using System;

namespace Crash
{
    [EntryType(1,GameVersion.Crash2)]
    public sealed class AnimationEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            Frame[] frames = new Frame [items.Length];
            for (int i = 0;i < frames.Length;i++)
            {
                frames[i] = Frame.Load(items[i]);
            }
            return new AnimationEntry(frames,false,eid);
        }
    }
    
    [EntryType(1,GameVersion.Crash3)]
    public sealed class NewAnimationEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            Frame[] frames = new Frame [items.Length];
            for (int i = 0;i < frames.Length;i++)
            {
                frames[i] = Frame.LoadNew(items[i]);
            }
            return new AnimationEntry(frames,true,eid);
        }
    }
}
