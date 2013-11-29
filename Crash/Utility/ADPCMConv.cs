using System;

// Based on code by bITmASTER and nextvolume
// http://psxsdk.googlecode.com/svn-history/r13/trunk/tools/vag2wav.c
namespace Crash
{
    public static class ADPCMConv
    {
        private static int[] f0 = {0,60,115,98,122,0,0,0,0,0,0,0,0,0,0,0};
        private static int[] f1 = {0,0,-52,-55,-60,0,0,0,0,0,0,0,0,0,0,0};

        public static short FromADPCM(int sample,int factor,int predict,ref double s0,ref double s1)
        {
            if ((sample & 0xF) != sample)
                throw new ArgumentOutOfRangeException("sample");
            if ((factor & 0xF) != factor)
                throw new ArgumentOutOfRangeException("factor");
            if ((predict & 0xF) != predict)
                throw new ArgumentOutOfRangeException("predict");
            if (predict >= 16)
                throw new ArgumentOutOfRangeException("predict");
            sample <<= 12;
            sample = (short)sample; // Sign extend
            sample >>= factor;
            double value = sample;
            value += s0 * f0[predict] / 64;
            value += s1 * f1[predict] / 64;
            s1 = s0;
            s0 = value;
            return (short)Math.Round(value);
        }
    }
}
