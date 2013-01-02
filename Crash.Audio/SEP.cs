using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class SEP
    {
        public const int Magic = SEQ.Magic;

        public static SEP Load(byte[] data,int seqcount)
        {
            if (data == null)
                throw new System.ArgumentNullException("Data cannot be null.");
            if (seqcount < 0)
                throw new System.ArgumentOutOfRangeException("SEQCount cannot be negative.");
            // All SEP/SEQ stuff is big-endian, like MIDI
            if (data.Length < 6)
            {
                throw new System.Exception();
            }
            int magic = BitConv.FromIntBE(data,0);
            short version = BitConv.FromShortBE(data,4);
            if (magic != Magic)
            {
                throw new System.Exception();
            }
            if (version != 0)
            {
                throw new System.Exception();
            }
            int offset = 6;
            SEQ[] seqs = new SEQ [seqcount];
            for (int i = 0;i < seqcount;i++)
            {
                if (data.Length < offset + 13)
                {
                    throw new System.Exception();
                }
                short seqid = BitConv.FromShortBE(data,offset);
                short resolution = BitConv.FromShortBE(data,offset + 2);
                // tempo is 3 (yes, three) bytes
                int tempo = MIDIConv.From3BE(data,offset + 4);
                short rhythm = BitConv.FromShortBE(data,offset + 7);
                int length = BitConv.FromIntBE(data,offset + 9);
                if (seqid != i)
                {
                    throw new System.Exception();
                }
                if (length < 0)
                {
                    throw new System.Exception();
                }
                offset += 13;
                if (data.Length < offset + length)
                {
                    throw new System.Exception();
                }
                byte[] seqdata = new byte [length];
                System.Array.Copy(data,offset,seqdata,0,length);
                seqs[i] = new SEQ(resolution,tempo,rhythm,seqdata);
                offset += length;
            }
            return new SEP(seqs);
        }

        private List<SEQ> seqs;

        public SEP(IEnumerable<SEQ> seqs)
        {
            if (seqs == null)
                throw new System.ArgumentNullException("SEQs cannot be null.");
            this.seqs = new List<SEQ>(seqs);
        }

        public IList<SEQ> SEQs
        {
            get { return seqs; }
        }

        public byte[] Save()
        {
            int length = 6;
            foreach (SEQ seq in seqs)
            {
                length += 13;
                length += seq.Data.Length;
            }
            byte[] data = new byte [length];
            BitConv.ToIntBE(data,0,Magic);
            BitConv.ToShortBE(data,4,0);
            int offset = 6;
            for (int i = 0;i < seqs.Count;i++)
            {
                SEQ seq = seqs[i];
                BitConv.ToShortBE(data,offset,(short)i);
                BitConv.ToShortBE(data,offset + 2,seq.Resolution);
                MIDIConv.To3BE(data,offset + 4,seq.Tempo);
                BitConv.ToShortBE(data,offset + 7,seq.Rhythm);
                BitConv.ToIntBE(data,offset + 9,seq.Data.Length);
                offset += 13;
                seq.Data.CopyTo(data,offset);
                offset += seq.Data.Length;
            }
            return data;
        }
    }
}
