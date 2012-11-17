namespace Crash.Audio
{
    [EntryType(13)]
    public sealed class MusicEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length != 3)
            {
                throw new System.Exception();
            }
            return new MusicEntry(items[0],items[1],items[2],unknown);
        }
    }
}
