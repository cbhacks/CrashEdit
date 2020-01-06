using System.Collections.Generic;

namespace Crash
{
    public sealed class PaletteEntry : Entry
    {
        public PaletteEntry(IEnumerable<byte[]> items,int eid) : base(eid)
        {
            Palettes = new List<byte[]>(items).ToArray();
        }

        public override int Type => 18;
        public byte[][] Palettes { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [Palettes.Length][];
            for (int i = 0;i < Palettes.Length; i++)
            {
                items[i] = Palettes[i];
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
