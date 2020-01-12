using System.Collections.Generic;

namespace Crash
{
    public sealed class ImageEntry : MysteryMultiItemEntry
    {
        public ImageEntry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type => 15;
    }
}
