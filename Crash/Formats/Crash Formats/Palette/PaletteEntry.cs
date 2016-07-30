using System.Collections.Generic;

namespace Crash
{
    public sealed class PaletteEntry : MysteryMultiItemEntry
    {
        public PaletteEntry(IEnumerable<byte[]> items,int eid,int size) : base(items,eid,size)
        {
        }

        public override int Type
        {
            get { return 18; }
        }
    }
}
