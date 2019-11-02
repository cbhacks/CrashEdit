using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class VHProgram
    {
        public static VHProgram Load(byte[] data,byte[] tonedata,bool isoldversion)
        {
            if (data.Length != 16)
                throw new ArgumentException("Value must be 16 bytes long.","data");
            if (tonedata.Length != 512)
                throw new ArgumentException("Value must be 512 bytes long.","tonedata");
            byte tonecount = data[0];
            byte volume = data[1];
            byte priority = data[2];
            byte mode = data[3];
            byte panning = data[4];
            byte reserved1 = data[5];
            short attribute = BitConv.FromInt16(data,6);
            int reserved2 = BitConv.FromInt32(data,8);
            int reserved3 = BitConv.FromInt32(data,12);
            if (tonecount < 0 || tonecount > 16)
            {
                ErrorManager.SignalError("VHProgram: Tone count is wrong");
            }
            if (reserved1 != (isoldversion ? 0x00 : 0xFF))
            {
                ErrorManager.SignalIgnorableError("VHProgram: Reserved value 1 is wrong");
            }
            if (reserved2 != -1)
            {
                ErrorManager.SignalIgnorableError("VHProgram: Reserved value 2 is wrong");
            }
            if (reserved3 != -1)
            {
                ErrorManager.SignalIgnorableError("VHProgram: Reserved value 3 is wrong");
            }
            VHTone[] tones = new VHTone [tonecount];
            for (int i = 0;i < tonecount;i++)
            {
                byte[] thistonedata = new byte [32];
                Array.Copy(tonedata,i * 32,thistonedata,0,32);
                tones[i] = VHTone.Load(thistonedata);
            }
            return new VHProgram(isoldversion,volume,priority,mode,panning,attribute,tones);
        }

        private List<VHTone> tones;

        public VHProgram(bool isoldversion)
        {
            IsOldVersion = isoldversion;
            if (isoldversion)
            {
                Volume = 0;
                Priority = 0;
                Mode = 0x1A;
                Panning = 0;
                Attribute = 0;
            }
            else
            {
                Volume = 127;
                Priority = 255;
                Mode = 255;
                Panning = 64;
                Attribute = 0;
            }
            tones = new List<VHTone>();
        }

        public VHProgram(bool isoldversion,byte volume,byte priority,byte mode,byte panning,short attribute,IEnumerable<VHTone> tones)
        {
            if (tones == null)
                throw new ArgumentNullException("tones");
            IsOldVersion = isoldversion;
            Volume = volume;
            Priority = priority;
            Mode = mode;
            Panning = panning;
            Attribute = attribute;
            this.tones = new List<VHTone>(tones);
        }

        public bool IsOldVersion { get; }
        public byte Volume { get; }
        public byte Priority { get; }
        public byte Mode { get; }
        public byte Panning { get; }
        public short Attribute { get; }
        public IList<VHTone> Tones => tones;

        public byte[] Save()
        {
            byte[] data = new byte [16];
            data[0] = (byte)tones.Count;
            data[1] = Volume;
            data[2] = Priority;
            data[3] = Mode;
            data[4] = Panning;
            data[5] = IsOldVersion ? (byte)0x00 : (byte)0xFF;
            BitConv.ToInt16(data,6,Attribute);
            BitConv.ToInt32(data,8,-1);
            BitConv.ToInt32(data,12,-1);
            return data;
        }

        public RIFF ToDLSInstrument(int programnumber,bool drumkit)
        {
            RIFF ins = new RIFF("ins ");
            byte[] insh = new byte [12];
            BitConv.ToInt32(insh,0,tones.Count);
            BitConv.ToInt32(insh,4,drumkit ? (1 << 31) : 0);
            BitConv.ToInt32(insh,8,programnumber);
            ins.Items.Add(new RIFFData("insh",insh));
            RIFF lrgn = new RIFF("lrgn");
            foreach (VHTone tone in tones)
            {
                lrgn.Items.Add(tone.ToDLSRegion());
            }
            ins.Items.Add(lrgn);
            return ins;
        }
    }
}
