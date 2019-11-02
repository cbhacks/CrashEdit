using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class SEP
    {
        public const int Magic = SEQ.Magic;
        public const short Version = 0;

        public static SEP Load(byte[] data,int seqcount)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (seqcount < 0)
                throw new ArgumentOutOfRangeException("seqcount");
            // All SEP/SEQ stuff is big-endian, like MIDI
            if (data.Length < 6)
            {
                ErrorManager.SignalError("SEP: Data is too short");
            }
            int magic = BEBitConv.FromInt32(data,0);
            short version = BEBitConv.FromInt16(data,4);
            if (magic != Magic)
            {
                ErrorManager.SignalIgnorableError("SEP: Magic number is wrong");
            }
            if (version != Version)
            {
                ErrorManager.SignalIgnorableError("SEP: Version number is wrong");
            }
            int offset = 6;
            SEQ[] seqs = new SEQ [seqcount];
            for (int i = 0;i < seqcount;i++)
            {
                if (data.Length < offset + 13)
                {
                    ErrorManager.SignalError("SEP: Data is too short");
                }
                short seqid = BEBitConv.FromInt16(data,offset);
                short resolution = BEBitConv.FromInt16(data,offset + 2);
                // tempo is 3 (yes, three) bytes
                int tempo = MIDIConv.From3BE(data,offset + 4);
                short rhythm = BEBitConv.FromInt16(data,offset + 7);
                int length = BEBitConv.FromInt32(data,offset + 9);
                if (seqid != i)
                {
                    ErrorManager.SignalIgnorableError("SEP: Track number is wrong");
                }
                if (length < 0)
                {
                    ErrorManager.SignalError("SEP: Track length is negative");
                }
                offset += 13;
                if (data.Length < offset + length)
                {
                    ErrorManager.SignalError("SEP: Data is too short");
                }
                byte[] seqdata = new byte [length];
                Array.Copy(data,offset,seqdata,0,length);
                seqs[i] = new SEQ(resolution,tempo,rhythm,seqdata);
                offset += length;
            }
            return new SEP(seqs);
        }

        private List<SEQ> seqs;

        public SEP(IEnumerable<SEQ> seqs)
        {
            if (seqs == null)
                throw new ArgumentNullException("seqs");
            this.seqs = new List<SEQ>(seqs);
        }

        public IList<SEQ> SEQs => seqs;

        public byte[] Save()
        {
            int length = 6;
            foreach (SEQ seq in seqs)
            {
                length += 13;
                length += seq.Data.Length;
            }
            byte[] data = new byte [length];
            BEBitConv.ToInt32(data,0,Magic);
            BEBitConv.ToInt16(data,4,Version);
            int offset = 6;
            for (int i = 0;i < seqs.Count;i++)
            {
                SEQ seq = seqs[i];
                BEBitConv.ToInt16(data,offset,(short)i);
                BEBitConv.ToInt16(data,offset + 2,seq.Resolution);
                MIDIConv.To3BE(data,offset + 4,seq.Tempo);
                BEBitConv.ToInt16(data,offset + 7,seq.Rhythm);
                BEBitConv.ToInt32(data,offset + 9,seq.Data.Length);
                offset += 13;
                seq.Data.CopyTo(data,offset);
                offset += seq.Data.Length;
            }
            return data;
        }
    }
}
