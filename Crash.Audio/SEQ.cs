namespace Crash.Audio
{
    public sealed class SEQ
    {
        public const int Magic = 0x70514553;

        private short resolution;
        private int tempo;
        private short rhythm;
        private byte[] data;

        public SEQ(short resolution,int tempo,short rhythm,byte[] data)
        {
            this.resolution = resolution;
            this.tempo = tempo;
            this.rhythm = rhythm;
            this.data = data;
        }

        public short Resolution
        {
            get { return resolution; }
        }

        public int Tempo
        {
            get { return tempo; }
        }

        public short Rhythm
        {
            get { return rhythm; }
        }

        public byte[] Data
        {
            get { return data; }
        }
    }
}
