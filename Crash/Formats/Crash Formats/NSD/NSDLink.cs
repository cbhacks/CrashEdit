namespace Crash
{
    public sealed class NSDLink
    {
        public NSDLink(int chunkid,int entryid)
        {
            ChunkID = chunkid;
            EntryID = entryid;
        }

        public int ChunkID { get; set; }
        public int EntryID { get; set; }
    }
}
