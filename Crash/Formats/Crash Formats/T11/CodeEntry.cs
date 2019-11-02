using System.Collections.Generic;

namespace Crash
{
    public sealed class CodeEntry : MysteryMultiItemEntry
    {
        public CodeEntry(IEnumerable<byte[]> items,int eid,int size) : base(items,eid,size)
        {
        }

        public override int Type => 11;
    }
}
