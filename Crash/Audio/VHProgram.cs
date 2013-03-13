using System;
using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class VHProgram
    {
        public static VHProgram Load(byte[] data,byte[] tonedata)
        {
            if (data.Length != 16)
                throw new ArgumentException("Value must be 16 bytes long.","data");
            if (tonedata.Length != 32 * 16)
                throw new ArgumentException("Value must be 512 bytes long.","tonedata");
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
            if (reserved1 != 0xFF)
            {
                throw new LoadException();
            }
            if (reserved2 != -1)
            {
                throw new LoadException();
            }
            if (reserved3 != -1)
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
            return new VHProgram(volume,priority,mode,panning,attribute,tones);
        }

        private byte volume;
        private byte priority;
        private byte mode;
        private byte panning;
        private short attribute;
        private List<VHTone> tones;

        public VHProgram()
        {
            this.volume = 127;
            this.priority = 255;
            this.mode = 255;
            this.panning = 64;
            this.attribute = 0;
            this.tones = new List<VHTone>();
        }

        public VHProgram(byte volume,byte priority,byte mode,byte panning,short attribute,IEnumerable<VHTone> tones)
        {
            if (tones == null)
                throw new ArgumentNullException("tones");
            this.volume = volume;
            this.priority = priority;
            this.mode = mode;
            this.panning = panning;
            this.attribute = attribute;
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
            data[5] = 0xFF;
            BitConv.ToHalf(data,6,attribute);
            BitConv.ToWord(data,8,-1);
            BitConv.ToWord(data,12,-1);
            return data;
        }
    }
}
