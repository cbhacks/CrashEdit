namespace Crash
{
    public abstract class EntryLoader
    {
        public abstract Entry Load(byte[][] items,int eid,int size);
    }
}
