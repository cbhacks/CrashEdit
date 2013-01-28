using System;

namespace Crash.Audio
{
    public sealed class SEQ
    {
        public const int Magic = 0x70514553;

        public static SEQ Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            // All SEP/SEQ stuff is big-endian, like MIDI
            if (data.Length < 15)
            {
                throw new LoadException();
            }
            int magic = BitConv.FromIntBE(data,0);
            int version = BitConv.FromIntBE(data,4);
            if (magic != Magic)
            {
                throw new LoadException();
            }
            if (version != 1)
            {
                throw new LoadException();
            }
            short resolution = BitConv.FromShortBE(data,8);
            int tempo = MIDIConv.From3BE(data,10);
            short rhythm = BitConv.FromShortBE(data,13);
            byte[] scoredata = new byte [data.Length - 15];
            Array.Copy(data,15,scoredata,0,scoredata.Length);
            return new SEQ(resolution,tempo,rhythm,scoredata);
        }

        private short resolution;
        private int tempo;
        private short rhythm;
        private byte[] data;

        public SEQ(short resolution,int tempo,short rhythm,byte[] data)
        {
            if ((tempo & 0xFF000000) != 0)
                throw new ArgumentOutOfRangeException("Tempo must be in the range 0 to 0x00FFFFFF inclusive.");
            if (data == null)
                throw new ArgumentNullException("data");
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
            BitConv.ToIntBE(result,4,1);
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
