using System;
using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class VHProgram
    {
        public static VHProgram Load(byte[] data,byte[] tonedata)
        {
            if (data.Length != 16)
                throw new ArgumentException("Value must be 16 bytes.","data");
            if (tonedata.Length != 32 * 16)
                throw new ArgumentException("Value must be 512 bytes.","tonedata");
            byte tonecount = data[0];
            byte volume = data[1];
            byte priority = data[2];
            byte mode = data[3];
            byte panning = data[4];
            byte reserved1 = data[5];
            short attribute = BitConv.FromHalf(data,6);
            int reserved2 = BitConv.FromWord(data,8);
            int reserved3 = BitConv.FromWord(data,12);
            if (tonecount < 0 || tonecount > 16)
            {
                throw new LoadException();
            }
            VHTone[] tones = new VHTone [tonecount];
            for (int i = 0;i < tonecount;i++)
            {
                byte[] thistonedata = new byte [32];
                Array.Copy(tonedata,i * 32,thistonedata,0,32);
                tones[i] = VHTone.Load(thistonedata);
            }
            return new VHProgram(volume,priority,mode,panning,reserved1,attribute,reserved2,reserved2,tones);
        }

        private byte volume;
        private byte priority;
        private byte mode;
        private byte panning;
        private byte reserved1;
        private short attribute;
        private int reserved2;
        private int reserved3;
        private List<VHTone> tones;

        public VHProgram()
        {
            this.volume = 127;
            this.priority = 255;
            this.mode = 255;
            this.panning = 64;
            this.reserved1 = 255;
            this.attribute = 0;
            this.reserved2 = -1;
            this.reserved3 = -1;
            this.tones = new List<VHTone>();
        }

        public VHProgram(byte volume,byte priority,byte mode,byte panning,byte reserved1,short attribute,int reserved2,int reserved3,IEnumerable<VHTone> tones)
        {
            this.volume = volume;
            this.priority = priority;
            this.mode = mode;
            this.panning = panning;
            this.reserved1 = reserved1;
            this.attribute = attribute;
            this.reserved2 = reserved2;
            this.reserved3 = reserved3;
            this.tones = new List<VHTone>(tones);
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

        public byte Reserved1
        {
            get { return reserved1; }
        }

        public short Attribute
        {
            get { return attribute; }
        }

        public int Reserved2
        {
            get { return reserved2; }
        }

        public int Reserved3
        {
            get { return reserved3; }
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
            data[5] = reserved1;
            BitConv.ToHalf(data,6,attribute);
            BitConv.ToWord(data,8,reserved2);
            BitConv.ToWord(data,12,reserved3);
            return data;
        }
    }
}
