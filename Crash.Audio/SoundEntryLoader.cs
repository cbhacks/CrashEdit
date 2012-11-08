namespace Crash.Audio
{
    [EntryType(12)]
    public sealed class SoundEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length != 1)
            {
                throw new System.Exception();
            }
            byte[] item = items[0];
            if (item.Length % 16 != 0)
            {
                throw new System.Exception();
            }
            int samplelinecount = (item.Length / 16) - 1;
            SampleLine[] samplelines = new SampleLine [samplelinecount];
            for (int i = 0;i < samplelinecount;i++)
            {
                byte[] linedata = new byte [16];
                System.Array.Copy(item,(i + 1) * 16,linedata,0,16);
                samplelines[i] = SampleLine.Load(linedata);
            }
            return new SoundEntry(samplelines,unknown);
        }
    }
}
