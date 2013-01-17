using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class SampleSet
    {
        public static SampleSet Load(byte[] data)
        {
            if (data == null)
                throw new System.ArgumentNullException("Data cannot be null.");
            if (data.Length % 16 != 0)
            {
                throw new System.Exception();
            }
            int samplelinecount = (data.Length / 16) - 1;
            SampleLine[] samplelines = new SampleLine [samplelinecount];
            for (int i = 0;i < samplelinecount;i++)
            {
                byte[] linedata = new byte [16];
                System.Array.Copy(data,(i + 1) * 16,linedata,0,16);
                samplelines[i] = SampleLine.Load(linedata);
            }
            return new SampleSet(samplelines);
        }

        private List<SampleLine> samplelines;

        public SampleSet(IEnumerable<SampleLine> samplelines)
        {
            if (samplelines == null)
                throw new System.ArgumentNullException("SampleLines cannot be null.");
            this.samplelines = new List<SampleLine>(samplelines);
        }

        public IList<SampleLine> SampleLines
        {
            get { return samplelines; }
        }

        public byte[] Save()
        {
            byte[] data = new byte [(samplelines.Count + 1) * 16];
            for (int i = 0;i < samplelines.Count;i++)
            {
                samplelines[i].Save().CopyTo(data,(i + 1) * 16);
            }
            return data;
        }

        public byte[] ToPCM()
        {
            double s0 = 0;
            double s1 = 0;
            List<byte> data = new List<byte>();
            foreach (SampleLine line in samplelines)
            {
                if ((line.Flags & SampleLineFlags.LoopEnd) != 0)
                {
                    break;
                }
                data.AddRange(line.ToPCM(ref s0,ref s1));
            }
            return data.ToArray();
        }
    }
}
