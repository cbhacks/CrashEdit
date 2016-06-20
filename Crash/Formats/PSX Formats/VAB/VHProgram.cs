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

        private bool isoldversion;
        private byte volume;
        private byte priority;
        private byte mode;
        private byte panning;
        private short attribute;
        private List<VHTone> tones;

        public VHProgram(bool isoldversion)
        {
            this.isoldversion = isoldversion;
            if (isoldversion)
            {
                volume = 0;
                priority = 0;
                mode = 0x1A;
                panning = 0;
                attribute = 0;
            }
            else
            {
                volume = 127;
                priority = 255;
                mode = 255;
                panning = 64;
                attribute = 0;
            }
            tones = new List<VHTone>();
        }

        public VHProgram(bool isoldversion,byte volume,byte priority,byte mode,byte panning,short attribute,IEnumerable<VHTone> tones)
        {
            if (tones == null)
                throw new ArgumentNullException("tones");
            this.isoldversion = isoldversion;
            this.volume = volume;
            this.priority = priority;
            this.mode = mode;
            this.panning = panning;
            this.attribute = attribute;
            this.tones = new List<VHTone>(tones);
        }

        public bool IsOldVersion
        {
            get { return isoldversion; }
        }

        public byte Volume
        {
            get { return volume; }
        }

        public byte Priority
        {
            get { return priority; }
        }

        public byte Mode
        {
            get { return mode; }
        }

        public byte Panning
        {
            get { return panning; }
        }

        public short Attribute
        {
            get { return attribute; }
        }
        
        public IList<VHTone> Tones
        {
            get { return tones; }
        }

        public byte[] Save()
        {
            byte[] data = new byte [16];
            data[0] = (byte)tones.Count;
            data[1] = volume;
            data[2] = priority;
            data[3] = mode;
            data[4] = panning;
            data[5] = isoldversion ? (byte)0x00 : (byte)0xFF;
            BitConv.ToInt16(data,6,attribute);
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
