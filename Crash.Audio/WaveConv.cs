namespace Crash.Audio
{
    public static class WaveConv
    {
        public static byte[] ToWave(byte[] data,int samplerate)
        {
            if (data == null)
                throw new System.ArgumentNullException("Data cannot be null.");
            int headerlen = 8 + 4 + 8 + 16 + 8;
            int wavelen = headerlen + data.Length;
            byte[] wave = new byte [wavelen];
            wave[0] = (byte)'R';
            wave[1] = (byte)'I';
            wave[2] = (byte)'F';
            wave[3] = (byte)'F';
            BitConv.ToWord(wave,4,wavelen - 8);
            wave[8] = (byte)'W';
            wave[9] = (byte)'A';
            wave[10] = (byte)'V';
            wave[11] = (byte)'E';
            wave[12] = (byte)'f';
            wave[13] = (byte)'m';
            wave[14] = (byte)'t';
            wave[15] = (byte)' ';
            BitConv.ToWord(wave,16,16);
            BitConv.ToHalf(wave,20,1);
            BitConv.ToHalf(wave,22,1);
            BitConv.ToWord(wave,24,samplerate);
            BitConv.ToWord(wave,28,samplerate * 2);
            BitConv.ToHalf(wave,32,2);
            BitConv.ToHalf(wave,34,16);
            wave[36] = (byte)'d';
            wave[37] = (byte)'a';
            wave[38] = (byte)'t';
            wave[39] = (byte)'a';
            BitConv.ToWord(wave,40,data.Length);
            data.CopyTo(wave,44);
            return wave;
        }
    }
}
