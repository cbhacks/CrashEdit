using System;

namespace Crash
{
    public sealed class WavebankEntry : Entry
    {
        public const int MaxSampleLines = 4089;

        public WavebankEntry(int id,SampleSet samples,int eid) : base(eid)
        {
            ID = id;
            Samples = samples ?? throw new ArgumentNullException("samples");
        }

        public override int Type => 14;
        public int ID { get; }
        public SampleSet Samples { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[] info = new byte [8];
            byte[] data = Samples.Save();
            BitConv.ToInt32(info,0,ID);
            BitConv.ToInt32(info,4,data.Length);
            byte[][] items = new byte [2][];
            items[0] = info;
            items[1] = data;
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
