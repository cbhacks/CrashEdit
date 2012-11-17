namespace Crash.Audio
{
    [EntryType(14)]
    public sealed class WavebankEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length != 2)
            {
                throw new System.Exception();
            }
            if (items[0].Length != 8)
            {
                throw new System.Exception();
            }
            int id = BitConv.FromWord(items[0],0);
            int length = BitConv.FromWord(items[0],4);
            if (id < 0 || id > 3)
            {
                throw new System.Exception();
            }
            if (length != items[1].Length)
            {
                throw new System.Exception();
            }
            if (items[1].Length % 16 != 0)
            {
                throw new System.Exception();
            }
            int samplelinecount = (items[1].Length / 16) - 1;
            SampleLine[] samplelines = new SampleLine [samplelinecount];
            for (int i = 0;i < samplelinecount;i++)
            {
                byte[] linedata = new byte [16];
                System.Array.Copy(items[1],(i + 1) * 16,linedata,0,16);
                samplelines[i] = SampleLine.Load(linedata);
            }
            return new WavebankEntry(id,samplelines,unknown);
        }
    }
}
