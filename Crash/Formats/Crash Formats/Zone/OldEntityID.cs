namespace Crash
{
    public struct OldEntityID
    {
        private short id;

        public OldEntityID(short id)
        {
            this.id = id;
        }

        public short ID
        {
            get { return id; }
        }
    }
}
