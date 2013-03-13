using System;
using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class VH
    {
        public const int Magic = 0x56414270;
        public const int Version = 7;

        public static VH Load(byte[] data)
        {
            if (data.Length < 2592)
            {
                throw new LoadException();
            }
            int magic = BitConv.FromInt32(data,0);
            int version = BitConv.FromInt32(data,4);
            if (magic != Magic)
            {
                throw new LoadException();
            }
            if (version != Version)
            {
                throw new LoadException();
            }
            int id = BitConv.FromInt32(data,8);
            int size = BitConv.FromInt32(data,12);
            short reserved1 = BitConv.FromInt16(data,16);
            short programcount = BitConv.FromInt16(data,18);
            short tonecount = BitConv.FromInt16(data,20);
            short wavecount = BitConv.FromInt16(data,22);
            byte volume = data[24];
            byte panning = data[25];
            byte attribute1 = data[26];
            byte attribute2 = data[27];
            int reserved2 = BitConv.FromInt32(data,28);
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
            if (reserved1 != -0x1112)
            {
                throw new LoadException();
            }
            if (programcount < 0 || programcount > 128)
            {
                throw new LoadException();
            }
            if (tonecount < 0 || tonecount > 2048)
            {
                throw new LoadException();
            }
            if (wavecount < 0 || wavecount > 254)
            {
                throw new LoadException();
            }
            if (reserved2 != -1)
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
                int wave = BitConv.FromInt16(data,32 + 16 * 128 + 32 * 16 * programcount + 2 + i * 2);
                if (wave % 2 != 0)
                {
                    throw new LoadException();
                }
                waves[i] = wave / 2;
            }
            return new VH(vbsize,volume,panning,attribute1,attribute2,programs,waves);
        }

        private int vbsize;
        private byte volume;
        private byte panning;
        private byte attribute1;
        private byte attribute2;
        private List<VHProgram> programs;
        private List<int> waves;

        public VH(int vbsize,byte volume,byte panning,byte attribute1,byte attribute2,IEnumerable<VHProgram> programs,IEnumerable<int> waves)
        {
            if (programs == null)
                throw new ArgumentNullException("programs");
            if (waves == null)
                throw new ArgumentNullException("waves");
            this.vbsize = vbsize;
            this.volume = volume;
            this.panning = panning;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.programs = new List<VHProgram>(programs);
            this.waves = new List<int>(waves);
        }

        public int VBSize
        {
            get { return vbsize; }
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
            BitConv.ToInt32(data,0,Magic);
            BitConv.ToInt32(data,4,Version);
            BitConv.ToInt32(data,8,0);
            BitConv.ToInt32(data,12,data.Length + vbsize * 16);
            BitConv.ToInt16(data,16,-0x1112);
            int tonecount = 0;
            foreach (VHProgram program in programs)
            {
                tonecount += program.Tones.Count;
            }
            BitConv.ToInt16(data,18,(short)programs.Count);
            BitConv.ToInt16(data,20,(short)tonecount);
            BitConv.ToInt16(data,22,(short)waves.Count);
            data[24] = volume;
            data[25] = panning;
            data[26] = attribute1;
            data[27] = attribute2;
            BitConv.ToInt32(data,28,-1);
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
                BitConv.ToInt16(data,2080 + 32 * 16 * programs.Count + 2 + i * 2,(short)(waves[i] * 2));
            }
            return data;
        }
    }
}
