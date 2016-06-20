using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class VH
    {
        public const int Magic = 0x56414270;
        public const int OldVersion = 6;
        public const int Version = 7;

        public static VH Load(byte[] data)
        {
            if (data.Length < 2592)
            {
                ErrorManager.SignalError("VH: Data is too short");
            }
            int magic = BitConv.FromInt32(data,0);
            int version = BitConv.FromInt32(data,4);
            if (magic != Magic)
            {
                ErrorManager.SignalIgnorableError("VH: Magic number is wrong");
            }
            bool isoldversion;
            if (version == Version)
            {
                isoldversion = false;
            }
            else if (version == OldVersion)
            {
                isoldversion = true;
            }
            else
            {
                ErrorManager.SignalIgnorableError("VH: Version number is wrong");
                isoldversion = true;
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
                ErrorManager.SignalIgnorableError("VH: ID is wrong");
            }
            if (size < data.Length)
            {
                ErrorManager.SignalError("VH: Size field mismatch");
            }
            if ((size - data.Length) % 16 != 0)
            {
                ErrorManager.SignalError("VH: Size field is invalid");
            }
            int vbsize = (size - data.Length) / 16;
            if (reserved1 != -0x1112)
            {
                ErrorManager.SignalIgnorableError("VH: Reserved value 1 is wrong");
            }
            if (programcount < 0 || programcount > 128)
            {
                ErrorManager.SignalError("VH: Program count is invalid");
            }
            if (tonecount < 0 || tonecount > 2048)
            {
                ErrorManager.SignalError("VH: Tone count is invalid");
            }
            if (wavecount < 0 || wavecount > 254)
            {
                ErrorManager.SignalError("VH: Wave count is invalid");
            }
            if (reserved2 != -1)
            {
                ErrorManager.SignalIgnorableError("VH: Reserved value 2 is wrong");
            }
            if (data.Length < 2592 + 32 * 16 * programcount)
            {
                ErrorManager.SignalError("VH: Data is too short");
            }
            Dictionary<int,VHProgram> programs = new Dictionary<int,VHProgram>();
            for (int i = 0;i < 128;i++)
            {
                byte[] programdata = new byte [16];
                Array.Copy(data,32 + 16 * i,programdata,0,16);
                if (programdata[0] == 0)
                {
                    continue;
                }
                if (programs.Count == programcount)
                {
                    ErrorManager.SignalError("VH: Program count field mismatch");
                }
                byte[] tonedata = new byte [32 * 16];
                Array.Copy(data,32 + 16 * 128 + 32 * 16 * programs.Count,tonedata,0,32 * 16);
                programs.Add(i,VHProgram.Load(programdata,tonedata,isoldversion));
            }
            if (programs.Count != programcount)
            {
                ErrorManager.SignalError("VH: Program count field mismatch");
            }
            int[] waves = new int [wavecount];
            for (int i = 0;i < wavecount;i++)
            {
                int wave = BitConv.FromInt16(data,32 + 16 * 128 + 32 * 16 * programcount + 2 + i * 2);
                if (wave % 2 != 0)
                {
                    ErrorManager.SignalError("VH: Wave size is invalid");
                }
                waves[i] = wave / 2;
            }
            return new VH(isoldversion,vbsize,volume,panning,attribute1,attribute2,programs,waves);
        }

        private bool isoldversion;
        private int vbsize;
        private byte volume;
        private byte panning;
        private byte attribute1;
        private byte attribute2;
        private Dictionary<int,VHProgram> programs;
        private List<int> waves;

        public VH(bool isoldversion,int vbsize,byte volume,byte panning,byte attribute1,byte attribute2,IDictionary<int,VHProgram> programs,IEnumerable<int> waves)
        {
            if (programs == null)
                throw new ArgumentNullException("programs");
            if (waves == null)
                throw new ArgumentNullException("waves");
            this.isoldversion = isoldversion;
            this.vbsize = vbsize;
            this.volume = volume;
            this.panning = panning;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.programs = new Dictionary<int,VHProgram>(programs);
            this.waves = new List<int>(waves);
        }

        public bool IsOldVersion
        {
            get { return isoldversion; }
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

        public IDictionary<int,VHProgram> Programs
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
            BitConv.ToInt32(data,4,isoldversion ? OldVersion : Version);
            BitConv.ToInt32(data,8,0);
            BitConv.ToInt32(data,12,data.Length + vbsize * 16);
            BitConv.ToInt16(data,16,-0x1112);
            int tonecount = 0;
            foreach (VHProgram program in programs.Values)
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
                if (programs.ContainsKey(i))
                {
                    programs[i].Save().CopyTo(data,32 + 16 * i);
                }
                else
                {
                    new VHProgram(isoldversion).Save().CopyTo(data,32 + 16 * i);
                }
            }
            int ii = 0;
            foreach (KeyValuePair<int,VHProgram> kvp in programs)
            {
                VHProgram program = kvp.Value;
                for (int j = 0;j < 16;j++)
                {
                    if (j < program.Tones.Count)
                    {
                        program.Tones[j].Save(kvp.Key).CopyTo(data,2080 + 32 * 16 * ii + 32 * j);
                    }
                    else
                    {
                        new VHTone(isoldversion).Save(kvp.Key).CopyTo(data,2080 + 32 * 16 * ii + 32 * j);
                    }
                }
                ii++;
            }
            for (int i = 0;i < waves.Count;i++)
            {
                BitConv.ToInt16(data,2080 + 32 * 16 * programs.Count + 2 + i * 2,(short)(waves[i] * 2));
            }
            return data;
        }
    }
}
