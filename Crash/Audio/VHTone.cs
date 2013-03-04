using System;

namespace Crash.Audio
{
    public sealed class VHTone
    {
        public static VHTone Load(byte[] data)
        {
            if (data.Length != 32)
                throw new ArgumentException("Value must be 32 bytes.","data");
            byte priority = data[0];
            byte mode = data[1];
            byte volume = data[2];
            byte panning = data[3];
            byte centernote = data[4];
            byte pitchshift = data[5];
            byte minimumnote = data[6];
            byte maximumnote = data[7];
            byte vibratowidth = data[8];
            byte vibratotime = data[9];
            byte portamentowidth = data[10];
            byte portamentotime = data[11];
            byte pitchbendminimum = data[12];
            byte pitchbendmaximum = data[13];
            byte reserved1 = data[14];
            byte reserved2 = data[15];
            short adsr1 = BitConv.FromHalf(data,16);
            short adsr2 = BitConv.FromHalf(data,18);
            // Unused 2 bytes here
            short wave = BitConv.FromHalf(data,22);
            short reserved3 = BitConv.FromHalf(data,24);
            short reserved4 = BitConv.FromHalf(data,26);
            short reserved5 = BitConv.FromHalf(data,28);
            short reserved6 = BitConv.FromHalf(data,30);
            return new VHTone(priority,mode,volume,panning,centernote,pitchshift,minimumnote,maximumnote,vibratowidth,vibratotime,portamentowidth,portamentotime,pitchbendminimum,pitchbendmaximum,reserved1,reserved2,adsr1,adsr2,wave,reserved3,reserved4,reserved5,reserved6);
        }

        private byte priority;
        private byte mode;
        private byte volume;
        private byte panning;
        private byte centernote;
        private byte pitchshift;
        private byte minimumnote;
        private byte maximumnote;
        private byte vibratowidth;
        private byte vibratotime;
        private byte portamentowidth;
        private byte portamentotime;
        private byte pitchbendminimum;
        private byte pitchbendmaximum;
        private byte reserved1;
        private byte reserved2;
        private short adsr1;
        private short adsr2;
        private short wave;
        private short reserved3;
        private short reserved4;
        private short reserved5;
        private short reserved6;
        
        public VHTone()
        {
            this.priority = 0;
            this.mode = 0;
            this.volume = 80;
            this.panning = 64;
            this.centernote = 64;
            this.pitchshift = 0;
            this.minimumnote = 64;
            this.maximumnote = 64;
            this.vibratowidth = 0;
            this.vibratotime = 0;
            this.portamentowidth = 0;
            this.portamentotime = 0;
            this.pitchbendminimum = 0;
            this.pitchbendmaximum = 0;
            this.reserved1 = 0xB1;
            this.reserved2 = 0xB2;
            unchecked 
            {
                this.adsr1 = (short)0x80FF;
                this.adsr2 = (short)0x5FDF;
            }
            this.wave = 0;
            this.reserved3 = 0xC0;
            this.reserved4 = 0xC1;
            this.reserved5 = 0xC2;
            this.reserved6 = 0xC3;
        }

        // This is ridiculous! There has to be a better way.
        public VHTone(byte priority,byte mode,byte volume,byte panning,byte centernote,byte pitchshift,byte minimumnote,byte maximumnote,byte vibratowidth,byte vibratotime,byte portamentowidth,byte portamentotime,byte pitchbendminimum,byte pitchbendmaximum,byte reserved1,byte reserved2,short adsr1,short adsr2,short wave,short reserved3,short reserved4,short reserved5,short reserved6)
        {
            this.priority = priority;
            this.mode = mode;
            this.volume = volume;
            this.panning = panning;
            this.centernote = centernote;
            this.pitchshift = pitchshift;
            this.minimumnote = minimumnote;
            this.maximumnote = maximumnote;
            this.vibratowidth = vibratowidth;
            this.vibratotime = vibratotime;
            this.portamentowidth = portamentowidth;
            this.portamentotime = portamentotime;
            this.pitchbendminimum = pitchbendminimum;
            this.pitchbendmaximum = pitchbendmaximum;
            this.reserved1 = reserved1;
            this.reserved2 = reserved2;
            this.adsr1 = adsr1;
            this.adsr2 = adsr2;
            this.wave = wave;
            this.reserved3 = reserved3;
            this.reserved4 = reserved4;
            this.reserved5 = reserved5;
            this.reserved6 = reserved6;
        }

        public byte Priority
        {
            get { return priority; }
        }

        public byte Mode
        {
            get { return mode; }
        }

        public byte Volume
        {
            get { return volume; }
        }

        public byte Panning
        {
            get { return panning; }
        }

        public byte CenterNote
        {
            get { return centernote; }
        }

        public byte PitchShift
        {
            get { return pitchshift; }
        }

        public byte MinimumNote
        {
            get { return minimumnote; }
        }

        public byte MaximumNote
        {
            get { return maximumnote; }
        }

        public byte VibratoWidth
        {
            get { return vibratowidth; }
        }

        public byte VibratoTime
        {
            get { return vibratotime; }
        }

        public byte PortamentoWidth
        {
            get { return portamentowidth; }
        }

        public byte PortamentoTime
        {
            get { return portamentotime; }
        }

        public byte PitchBendMinimum
        {
            get { return pitchbendminimum; }
        }

        public byte PitchBendMaximum
        {
            get { return pitchbendmaximum; }
        }

        public byte Reserved1
        {
            get { return reserved1; }
        }

        public byte Reserved2
        {
            get { return reserved2; }
        }

        public short ADSR1
        {
            get { return adsr1; }
        }

        public short ADSR2
        {
            get { return adsr2; }
        }

        public short Wave
        {
            get { return wave; }
        }

        public short Reserved3
        {
            get { return reserved3; }
        }

        public short Reserved4
        {
            get { return reserved4; }
        }

        public short Reserved5
        {
            get { return reserved5; }
        }

        public short Reserved6
        {
            get { return reserved6; }
        }

        public byte[] Save(int program)
        {
            byte[] data = new byte [32];
            data[0] = priority;
            data[1] = mode;
            data[2] = volume;
            data[3] = panning;
            data[4] = centernote;
            data[5] = pitchshift;
            data[6] = minimumnote;
            data[7] = maximumnote;
            data[8] = vibratowidth;
            data[9] = vibratotime;
            data[10] = portamentowidth;
            data[11] = portamentotime;
            data[12] = pitchbendminimum;
            data[13] = pitchbendmaximum;
            data[14] = reserved1;
            data[15] = reserved2;
            BitConv.ToHalf(data,16,adsr1);
            BitConv.ToHalf(data,18,adsr2);
            BitConv.ToHalf(data,20,(short)program);
            BitConv.ToHalf(data,22,wave);
            BitConv.ToHalf(data,24,reserved3);
            BitConv.ToHalf(data,26,reserved4);
            BitConv.ToHalf(data,28,reserved5);
            BitConv.ToHalf(data,30,reserved6);
            return data;
        }
    }
}
