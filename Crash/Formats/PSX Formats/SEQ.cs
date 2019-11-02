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

        public SEQ(short resolution,int tempo,short rhythm,byte[] data)
        {
            if ((tempo & 0xFF000000) != 0)
                throw new ArgumentOutOfRangeException("tempo");
            Resolution = resolution;
            Tempo = tempo;
            Rhythm = rhythm;
            Data = data ?? throw new ArgumentNullException("data");
        }

        public short Resolution { get; }
        public int Tempo { get; }
        public short Rhythm { get; }
        public byte[] Data { get; }

        public byte[] Save()
        {
            byte[] result = new byte [15 + Data.Length];
            BEBitConv.ToInt32(result,0,Magic);
            BEBitConv.ToInt32(result,4,Version);
            BEBitConv.ToInt16(result,8,Resolution);
            MIDIConv.To3BE(result,10,Tempo);
            BEBitConv.ToInt16(result,13,Rhythm);
            Data.CopyTo(result,15);
            return result;
        }

        public byte[] ToMIDI()
        {
            RIFF riff = new RIFF("MIDI");
            byte[] mthd = new byte [6];
            BEBitConv.ToInt16(mthd,0,0);
            BEBitConv.ToInt16(mthd,2,1);
            BEBitConv.ToInt16(mthd,4,Resolution);
            riff.Items.Add(new RIFFData("MThd",mthd));
            byte[] mtrk = new byte [15 + Data.Length];
            mtrk[0] = 0;
            mtrk[1] = 0xFF;
            mtrk[2] = 0x51;
            mtrk[3] = 0x03;
            MIDIConv.To3BE(mtrk,4,Tempo);
            mtrk[7] = 0;
            mtrk[8] = 0xFF;
            mtrk[9] = 0x58;
            mtrk[10] = 0x04;
            BEBitConv.ToInt16(mtrk,11,Rhythm);
            mtrk[13] = 0x18;
            mtrk[14] = 0x08;
            Data.CopyTo(mtrk,15);
            riff.Items.Add(new RIFFData("MTrk",mtrk));
            return riff.SaveBody(Endianness.BigEndian);
        }
    }
}
