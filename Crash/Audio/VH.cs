using System;
using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class VH
    {
        public const int Magic = 0x56414270;

        public static VH Load(byte[] data)
        {
            if (data.Length < 2592)
            {
                throw new LoadException();
            }
            int magic = BitConv.FromWord(data,0);
            int version = BitConv.FromWord(data,4);
            if (magic != Magic)
            {
                throw new LoadException();
            }
            if (version != 7)
            {
                throw new LoadException();
            }
            int id = BitConv.FromWord(data,8);
            int size = BitConv.FromWord(data,12);
            short reserved1 = BitConv.FromHalf(data,16);
            short programcount = BitConv.FromHalf(data,18);
            short tonecount = BitConv.FromHalf(data,20);
            short wavecount = BitConv.FromHalf(data,22);
            byte volume = data[24];
            byte panning = data[25];
            byte attribute1 = data[26];
            byte attribute2 = data[27];
            int reserved2 = BitConv.FromWord(data,28);
            if (id != 0)
            {
                throw new LoadException();
            }
            if (size < data.Length)
            {
                throw new LoadException();
            }
            if ((size - data.Length) % 16 != 0)
            {
                throw new LoadException();
            }
            int vbsize = (size - data.Length) / 16;
            if (programcount < 0)
            {
                throw new LoadException();
            }
            if (tonecount < 0)
            {
                throw new LoadException();
            }
            if (wavecount < 0)
            {
                throw new LoadException();
            }
            if (wavecount > 254)
            {
                throw new LoadException();
            }
            if (data.Length < 2592 + 32 * 16 * programcount)
            {
                throw new LoadException();
            }
            VHProgram[] programs = new VHProgram [programcount];
            for (int i = 0;i < programcount;i++)
            {
                byte[] programdata = new byte [16];
                byte[] tonedata = new byte [32 * 16];
                Array.Copy(data,32 + 16 * i,programdata,0,16);
                Array.Copy(data,32 + 16 * 128 + 32 * 16 * i,tonedata,0,32 * 16);
                programs[i] = VHProgram.Load(programdata,tonedata);
            }
            int[] waves = new int [wavecount];
            for (int i = 0;i < wavecount;i++)
            {
                int wave = BitConv.FromHalf(data,32 + 16 * 128 + 32 * 16 * programcount + 2 + i * 2);
                if (wave % 2 != 0)
                {
                    throw new LoadException();
                }
                waves[i] = wave / 2;
            }
            return new VH(vbsize,reserved1,volume,panning,attribute1,attribute2,reserved2,programs,waves);
        }

        private int vbsize;
        private short reserved1;
        private byte volume;
        private byte panning;
        private byte attribute1;
        private byte attribute2;
        private int reserved2;
        private List<VHProgram> programs;
        private List<int> waves;

        public VH(int vbsize,short reserved1,byte volume,byte panning,byte attribute1,byte attribute2,int reserved2,IEnumerable<VHProgram> programs,IEnumerable<int> waves)
        {
            if (programs == null)
                throw new ArgumentNullException("programs");
            if (waves == null)
                throw new ArgumentNullException("waves");
            this.vbsize = vbsize;
            this.reserved1 = reserved1;
            this.volume = volume;
            this.panning = panning;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.reserved2 = reserved2;
            this.programs = new List<VHProgram>(programs);
            this.waves = new List<int>(waves);
        }

        public int VBSize
        {
            get { return vbsize; }
        }

        public short Reserved1
        {
            get { return reserved1; }
        }

        public byte Volume
        {
            get { return volume; }
        }

        public byte Panning
        {
            get { return panning; }
        }

        public byte Attribute1
        {
            get { return attribute1; }
        }

        public byte Attribute2
        {
            get { return attribute2; }
        }

        public int Reserved2
        {
            get { return reserved2; }
        }

        public IList<VHProgram> Programs
        {
            get { return programs; }
        }

        public IList<int> Waves
        {
            get { return waves; }
        }

        public byte[] Save()
        {
            byte[] data = new byte [2592 + 32 * 16 * programs.Count];
            BitConv.ToWord(data,0,Magic);
            BitConv.ToWord(data,4,7);
            BitConv.ToWord(data,8,0);
            BitConv.ToWord(data,12,data.Length + vbsize * 16);
            BitConv.ToHalf(data,16,reserved1);
            int tonecount = 0;
            foreach (VHProgram program in programs)
            {
                tonecount += program.Tones.Count;
            }
            BitConv.ToHalf(data,18,(short)programs.Count);
            BitConv.ToHalf(data,20,(short)tonecount);
            BitConv.ToHalf(data,22,(short)waves.Count);
            data[24] = volume;
            data[25] = panning;
            data[26] = attribute1;
            data[27] = attribute2;
            BitConv.ToWord(data,28,reserved2);
            for (int i = 0;i < 128;i++)
            {
                if (i < programs.Count)
                {
                    programs[i].Save().CopyTo(data,32 + 16 * i);
                }
                else
                {
                    new VHProgram().Save().CopyTo(data,32 + 16 * i);
                }
            }
            for (int i = 0;i < programs.Count;i++)
            {
                VHProgram program = programs[i];
                for (int j = 0;j < 16;j++)
                {
                    if (j < program.Tones.Count)
                    {
                        program.Tones[j].Save(i).CopyTo(data,2080 + 32 * 16 * i + 32 * j);
                    }
                    else
                    {
                        new VHTone().Save(i).CopyTo(data,2080 + 32 * 16 * i + 32 * j);
                    }
                }
            }
            for (int i = 0;i < waves.Count;i++)
            {
                BitConv.ToHalf(data,2080 + 32 * 16 * programs.Count + 2 + i * 2,(short)(waves[i] * 2));
            }
            return data;
        }
    }
}
