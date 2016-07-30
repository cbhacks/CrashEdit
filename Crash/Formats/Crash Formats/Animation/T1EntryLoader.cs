using System;

namespace Crash
{
    //[EntryType(1,GameVersion.Crash2)]
    [EntryType(1,GameVersion.Crash3)]
    public sealed class T1EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new T1Entry(items,eid,size);
        }
    }
}
