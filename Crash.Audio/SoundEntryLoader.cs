namespace Crash.Audio
{
    [EntryType(12)]
    public sealed class SoundEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length != 1)
            {
                throw new System.Exception();
            }
            return new SoundEntry(SampleSet.Load(items[0]),unknown);
        }
    }
}
