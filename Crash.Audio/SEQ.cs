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
                throw new System.ArgumentOutOfRangeException("Tempo must be in the range 0 to 0x00FFFFFF inclusive.");
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
            BitConv.ToShortBE(result,8,resolution);
            MIDIConv.To3BE(result,10,tempo);
            BitConv.ToShortBE(result,13,rhythm);
            data.CopyTo(result,15);
            return result;
        }

        public byte[] ToMIDI()
        {
            byte[] midi = new byte [37 + data.Length];
            midi[0] = (byte)'M';
            midi[1] = (byte)'T';
            midi[2] = (byte)'h';
            midi[3] = (byte)'d';
            BitConv.ToIntBE(midi,4,6);
            BitConv.ToShortBE(midi,8,0);
            BitConv.ToShortBE(midi,10,1);
            BitConv.ToShortBE(midi,12,resolution);
            midi[14] = (byte)'M';
            midi[15] = (byte)'T';
            midi[16] = (byte)'r';
            midi[17] = (byte)'k';
            BitConv.ToIntBE(midi,18,15 + data.Length);
            midi[22] = 0;
            midi[23] = 0xFF;
            midi[24] = 0x51;
            midi[25] = 0x03;
            MIDIConv.To3BE(midi,26,tempo);
            midi[29] = 0;
            midi[30] = 0xFF;
            midi[31] = 0x58;
            midi[32] = 0x04;
            BitConv.ToShortBE(midi,33,rhythm);
            midi[35] = 0x18;
            midi[36] = 0x08;
            data.CopyTo(midi,37);
            return midi;
        }
    }
}
