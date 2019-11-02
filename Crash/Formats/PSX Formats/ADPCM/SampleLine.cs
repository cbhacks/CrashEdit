using System;

namespace Crash
{
    public sealed class SampleLine
    {
        public static SampleLine Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 16)
                throw new ArgumentException("Value must be 16 bytes long.","data");
            byte[] newdata = new byte [14];
            Array.Copy(data,2,newdata,0,14);
            return new SampleLine(data[0],(SampleLineFlags)data[1],newdata);
        }

        private byte info;
        private byte[] data;

        public SampleLine(byte info,SampleLineFlags flags,byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 14)
                throw new ArgumentException("Value must be 14 bytes long.","data");
            this.info = info;
            Flags = flags;
            this.data = data;
        }

        public SampleLineFlags Flags { get; }

        public byte[] Save()
        {
            byte[] result = new byte [16];
            result[0] = info;
            result[1] = (byte)Flags;
            data.CopyTo(result,2);
            return result;
        }

        // Based on code by bITmASTER and nextvolume
        // http://psxsdk.googlecode.com/svn-history/r13/trunk/tools/vag2wav.c
        public byte[] ToPCM(ref double s0,ref double s1)
        {
            byte[] result = new byte [28 * 2];
            int factor = info & 0xF;
            int predict = (info >> 4) & 0xF;
            for (int i = 0;i < 14;++i)
            {
                int adl = data[i] & 0xF;
                int adh = (data[i] & 0xF0) >> 4;
                short l = ADPCMConv.FromADPCM(adl,factor,predict,ref s0,ref s1);
                short h = ADPCMConv.FromADPCM(adh,factor,predict,ref s0,ref s1);
                BitConv.ToInt16(result,i * 4 + 0,l);
                BitConv.ToInt16(result,i * 4 + 2,h);
            }
            return result;
        }
    }
}
