using System;

namespace Crash
{
    [EntryType(17,GameVersion.Crash3)]
    public sealed class T17EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new T17Entry(items,eid,size);
        }
    }
}
