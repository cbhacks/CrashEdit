namespace Crash.Audio
{
    public sealed class SEQ
    {
        public const int Magic = 0x70514553;

        public static SEQ Load(byte[] data)
        {
            if (data == null)
                throw new System.ArgumentNullException("Data cannot be null.");
            throw new System.NotImplementedException();
        }

        private short resolution;
        private int tempo;
        private short rhythm;
        private byte[] data;

        public SEQ(short resolution,int tempo,short rhythm,byte[] data)
        {
            if ((tempo & 0xFF000000) != 0)
                throw new System.ArgumentOutOfRangeException("Tempo must be in the range 0 to 0xFFFFFF inclusive.");
            if (data == null)
                throw new System.ArgumentNullException("Data cannot be null.");
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

        public byte[] Save()
        {
            byte[] result = new byte [15 + data.Length];
            BitConv.ToIntBE(result,0,Magic);
            BitConv.ToIntBE(result,4,0);
            BitConv.ToIntBE(result,9,tempo);
            // tempo is 3 (yes, three) bytes
            // write it before resolution, so resolution overwrites the extra byte
            BitConv.ToShortBE(result,8,resolution);
            BitConv.ToShortBE(result,13,rhythm);
            data.CopyTo(result,15);
            return result;
        }
    }
}
