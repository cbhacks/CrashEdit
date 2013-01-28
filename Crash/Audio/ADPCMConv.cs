using System;

// Based on code by bITmASTER and nextvolume
// http://psxsdk.googlecode.com/svn-history/r13/trunk/tools/vag2wav.c
namespace Crash.Audio
{
    public static class ADPCMConv
    {
        private static int[] f0 = {0,60,115,98,122};
        private static int[] f1 = {0,0,-52,-55,-60};

        public static short FromADPCM(int sample,int factor,int predict,ref double s0,ref double s1)
        {
            if ((sample & 0xF) != sample)
                throw new ArgumentOutOfRangeException("Sample must be 4-bit.");
            if ((factor & 0xF) != factor)
                throw new ArgumentOutOfRangeException("Factor must be 4-bit.");
            if ((predict & 0xF) != predict)
                throw new ArgumentOutOfRangeException("Predict must be 4-bit.");
            if (predict >= 5)
                throw new ArgumentOutOfRangeException("Predict must be less than 5.");
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
