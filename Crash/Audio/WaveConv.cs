using System;

namespace Crash.Audio
{
    public static class WaveConv
    {
        public static RIFF ToWave(byte[] data,int samplerate)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            byte[] format = new byte [16];
            BitConv.ToShortLE(format,0,1);
            BitConv.ToShortLE(format,2,1);
            BitConv.ToIntLE(format,4,samplerate);
            BitConv.ToIntLE(format,8,samplerate * 2);
            BitConv.ToShortLE(format,12,2);
            BitConv.ToShortLE(format,14,16);
            RIFF wave = new RIFF("WAVE");
            wave.Items.Add(new RIFFData("fmt ",format));
            wave.Items.Add(new RIFFData("data",data));
            return wave;
        }
    }
}
