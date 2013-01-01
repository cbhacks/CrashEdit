namespace Crash.Audio
{
    [EntryType(13)]
    public sealed class MusicEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            if (items.Length != 3)
            {
                throw new System.Exception();
            }
            byte[] unknown1 = items[0];
            int seqcount = BitConv.FromWord(unknown1,0);
            byte[] vab = items[1];
            SEP sep = SEP.Load(items[2],seqcount);
            return new MusicEntry(unknown1,vab,sep,unknown);
        }
    }
}
