using System;

namespace Crash
{
    public static class WaveConv
    {
        public static RIFF ToWave(byte[] data,int samplerate)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            byte[] format = new byte [16];
            BitConv.ToInt16(format,0,1);
            BitConv.ToInt16(format,2,1);
            BitConv.ToInt32(format,4,samplerate);
            BitConv.ToInt32(format,8,samplerate * 2);
            BitConv.ToInt16(format,12,2);
            BitConv.ToInt16(format,14,16);
            RIFF wave = new RIFF("WAVE");
            wave.Items.Add(new RIFFData("fmt ",format));
            wave.Items.Add(new RIFFData("data",data));
            return wave;
        }
    }
}
