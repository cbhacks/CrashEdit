namespace Crash.Audio
{
    public sealed class SampleLine
    {
        public static SampleLine Load(byte[] data)
        {
            if (data == null)
                throw new System.ArgumentNullException("Data cannot be null.");
            if (data.Length != 16)
                throw new System.ArgumentException("Data must be 16 bytes long.");
            byte[] newdata = new byte [14];
            System.Array.Copy(data,2,newdata,0,14);
            return new SampleLine(data[0],(SampleLineFlags)data[1],newdata);
        }

        private byte info;
        private SampleLineFlags flags;
        private byte[] data;

        public SampleLine(byte info,SampleLineFlags flags,byte[] data)
        {
            if (data == null)
                throw new System.ArgumentNullException("Data cannot be null.");
            if (data.Length != 14)
            {
                throw new System.Exception();
            }
            this.info = info;
            this.flags = flags;
            this.data = data;
        }

        public SampleLineFlags Flags
        {
            get { return flags; }
        }

        public byte[] Save()
        {
            byte[] result = new byte [16];
            result[0] = info;
            result[1] = (byte)flags;
            data.CopyTo(result,2);
            return result;
        }

        // Based on code by bITmASTER and nextvolume
        // http://psxsdk.googlecode.com/svn-history/r13/trunk/tools/vag2wav.c
        public byte[] ToPCM(ref double s0,ref double s1)
        {
            double[] f0 = {0.0,60.0 / 64.0,115.0 / 64.0,98.0 / 64.0,122.0 / 64.0};
            double[] f1 = {0.0,0.0,-52.0 / 64.0,-55.0 / 64.0,-60.0 / 64.0};
            byte[] result = new byte [28 * 2];
            double[] samples = new double [28];
            int factor = info & 0xF;
            for (int i = 0;i < 14;i++)
            {
                short h = (short)((data[i] & 0xF) << 12);
                short l = (short)((data[i] & 0xF0) << 8);
                samples[i * 2] = h >> factor;
                samples[i * 2 + 1] = l >> factor;
            }
            int predict = (info >> 4) & 0xF;
            for (int i = 0;i < 28;i++)
            {
                samples[i] += s0 * f0[predict] + s1 * f1[predict];
                s1 = s0;
                s0 = samples[i];
                short d = (short)(samples[i] + 0.5);
                BitConv.ToHalf(result,i * 2,(short)samples[i]);
            }
            return result;
        }
    }
}
