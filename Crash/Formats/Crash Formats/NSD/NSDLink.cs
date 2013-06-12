namespace Crash
{
    public sealed class NSDLink
    {
        private int chunkid;
        private int entryid;

        public NSDLink(int chunkid,int entryid)
        {
            this.chunkid = chunkid;
            this.entryid = entryid;
        }

        public int ChunkID
        {
            get { return chunkid; }
            set { chunkid = value; }
        }

        public int EntryID
        {
            get { return entryid; }
            set { entryid = value; }
        }
    }
}
