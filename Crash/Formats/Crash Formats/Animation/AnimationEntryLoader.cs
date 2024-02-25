namespace CrashEdit.Crash
{
    [EntryType(1, GameVersion.Crash2)]
    [EntryType(1, GameVersion.Crash3)]
    public sealed class AnimationEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid, GameVersion version)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            bool isnew = version == GameVersion.Crash3;
            Frame[] frames = new Frame[items.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = !isnew ? Frame.Load(items[i]) : Frame.LoadNew(items[i]);
            }
            return new AnimationEntry(frames, isnew, eid);
        }
    }
}
