namespace Crash.Audio
{
    [EntryType(20)]
    public sealed class SpeechEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            if (items.Length != 1)
            {
                throw new System.Exception();
            }
            return new SpeechEntry(SampleSet.Load(items[0]),unknown);
        }
    }
}
