using System;

namespace Crash
{
    public sealed class SEQ
    {
        public const int Magic = 0x70514553;
        public const int Version = 1;

        public static SEQ Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            // All SEP/SEQ stuff is big-endian, like MIDI
            if (data.Length < 15)
            {
                ErrorManager.SignalError("SEQ: Data is too short");
            }
            int magic = BEBitConv.FromInt32(data,0);
            int version = BEBitConv.FromInt32(data,4);
            if (magic != Magic)
            {
                ErrorManager.SignalIgnorableError("SEQ: Magic number is wrong");
            }
            if (version != Version)
            {
                ErrorManager.SignalIgnorableError("SEQ: Version number is wrong");
            }
            short resolution = BEBitConv.FromInt16(data,8);
            int tempo = MIDIConv.From3BE(data,10);
            short rhythm = BEBitConv.FromInt16(data,13);
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
                throw new ArgumentOutOfRangeException("tempo");
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
            BEBitConv.ToInt32(result,0,Magic);
            BEBitConv.ToInt32(result,4,Version);
            BEBitConv.ToInt16(result,8,resolution);
            MIDIConv.To3BE(result,10,tempo);
            BEBitConv.ToInt16(result,13,rhythm);
            data.CopyTo(result,15);
            return result;
        }

        public byte[] ToMIDI()
        {
            RIFF riff = new RIFF("MIDI");
            byte[] mthd = new byte [6];
            BEBitConv.ToInt16(mthd,0,0);
            BEBitConv.ToInt16(mthd,2,1);
            BEBitConv.ToInt16(mthd,4,resolution);
            riff.Items.Add(new RIFFData("MThd",mthd));
            byte[] mtrk = new byte [15 + data.Length];
            mtrk[0] = 0;
            mtrk[1] = 0xFF;
            mtrk[2] = 0x51;
            mtrk[3] = 0x03;
            MIDIConv.To3BE(mtrk,4,tempo);
            mtrk[7] = 0;
            mtrk[8] = 0xFF;
            mtrk[9] = 0x58;
            mtrk[10] = 0x04;
            BEBitConv.ToInt16(mtrk,11,rhythm);
            mtrk[13] = 0x18;
            mtrk[14] = 0x08;
            data.CopyTo(mtrk,15);
            riff.Items.Add(new RIFFData("MTrk",mtrk));
            return riff.SaveBody(Endianness.BigEndian);
        }
    }
}
