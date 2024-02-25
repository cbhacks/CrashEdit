namespace Crash
{
    [EntryType(1, GameVersion.Crash1Beta1995)]
    [EntryType(1, GameVersion.Crash1BetaMAR08)]
    [EntryType(1, GameVersion.Crash1BetaMAY11)]
    [EntryType(1, GameVersion.Crash1)]
    public sealed class OldAnimationEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid, GameVersion version)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            OldFrame[] frames = new OldFrame[items.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = OldFrame.Load(items[i]);
            }
            return new OldAnimationEntry(frames, eid);
        }
    }
}
