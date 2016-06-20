using System;

namespace Crash
{
    public sealed class VHTone
    {
        public static VHTone Load(byte[] data)
        {
            if (data.Length != 32)
                throw new ArgumentException("Value must be 32 bytes long.","data");
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
            short adsr1 = BitConv.FromInt16(data,16);
            short adsr2 = BitConv.FromInt16(data,18);
            // Unused 2 bytes here
            short wave = BitConv.FromInt16(data,22);
            short reserved3 = BitConv.FromInt16(data,24);
            short reserved4 = BitConv.FromInt16(data,26);
            short reserved5 = BitConv.FromInt16(data,28);
            short reserved6 = BitConv.FromInt16(data,30);
            if (reserved1 != 0xB1)
            {
                ErrorManager.SignalIgnorableError("VHTone: Reserved value 1 is wrong");
            }
            if (reserved2 != 0xB2)
            {
                ErrorManager.SignalIgnorableError("VHTone: Reserved value 2 is wrong");
            }
            if (reserved3 != 0xC0)
            {
                ErrorManager.SignalIgnorableError("VHTone: Reserved value 3 is wrong");
            }
            if (reserved4 != 0xC1)
            {
                ErrorManager.SignalIgnorableError("VHTone: Reserved value 4 is wrong");
            }
            if (reserved5 != 0xC2)
            {
                ErrorManager.SignalIgnorableError("VHTone: Reserved value 5 is wrong");
            }
            if (reserved6 != 0xC3)
            {
                ErrorManager.SignalIgnorableError("VHTone: Reserved value 6 is wrong");
            }
            return new VHTone(priority,mode,volume,panning,centernote,pitchshift,minimumnote,maximumnote,vibratowidth,vibratotime,portamentowidth,portamentotime,pitchbendminimum,pitchbendmaximum,adsr1,adsr2,wave);
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
        private short adsr1;
        private short adsr2;
        private short wave;
        
        public VHTone(bool isoldversion)
        {
            if (isoldversion)
            {
                priority = 0;
                mode = 0;
                volume = 0;
                panning = 0;
                centernote = 0;
                pitchshift = 0;
                minimumnote = 0;
                maximumnote = 0;
                vibratowidth = 0;
                vibratotime = 0;
                portamentowidth = 0;
                portamentotime = 0;
                pitchbendminimum = 0;
                pitchbendmaximum = 0;
                unchecked
                {
                    adsr1 = (short)0x80DF;
                    adsr2 = 0x5FDF;
                }
            }
            else
            {
                priority = 0;
                mode = 0;
                volume = 80;
                panning = 64;
                centernote = 64;
                pitchshift = 0;
                minimumnote = 64;
                maximumnote = 64;
                vibratowidth = 0;
                vibratotime = 0;
                portamentowidth = 0;
                portamentotime = 0;
                pitchbendminimum = 0;
                pitchbendmaximum = 0;
                unchecked
                {
                    adsr1 = (short)0x80DF;
                    adsr2 = 0x5FDF;
                }
            }
            wave = 0;
        }

        // This is ridiculous! There has to be a better way.
        public VHTone(byte priority,byte mode,byte volume,byte panning,byte centernote,byte pitchshift,byte minimumnote,byte maximumnote,byte vibratowidth,byte vibratotime,byte portamentowidth,byte portamentotime,byte pitchbendminimum,byte pitchbendmaximum,short adsr1,short adsr2,short wave)
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
            this.adsr1 = adsr1;
            this.adsr2 = adsr2;
            this.wave = wave;
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
            data[14] = 0xB1;
            data[15] = 0xB2;
            BitConv.ToInt16(data,16,adsr1);
            BitConv.ToInt16(data,18,adsr2);
            BitConv.ToInt16(data,20,(short)program);
            BitConv.ToInt16(data,22,wave);
            BitConv.ToInt16(data,24,0xC0);
            BitConv.ToInt16(data,26,0xC1);
            BitConv.ToInt16(data,28,0xC2);
            BitConv.ToInt16(data,30,0xC3);
            return data;
        }

        public RIFF ToDLSRegion()
        {
            RIFF rgn = new RIFF("rgn ");
            byte[] rgnh = new byte [12];
            BitConv.ToInt16(rgnh,0,minimumnote);
            BitConv.ToInt16(rgnh,2,maximumnote);
            BitConv.ToInt16(rgnh,4,0);
            BitConv.ToInt16(rgnh,6,127);
            BitConv.ToInt16(rgnh,8,0);
            BitConv.ToInt16(rgnh,10,0);
            rgn.Items.Add(new RIFFData("rgnh",rgnh));
            byte[] wsmp = new byte [20 /* 36 */];
            BitConv.ToInt32(wsmp,0,20);
            BitConv.ToInt16(wsmp,4,centernote);
            BitConv.ToInt16(wsmp,6,pitchshift);
            BitConv.ToInt32(wsmp,8,volume - 64 << 16);
            BitConv.ToInt32(wsmp,12,0);
            BitConv.ToInt32(wsmp,16,0 /* 1 */);
            /*BitConv.ToInt32(wsmp,20,16);
            BitConv.ToInt32(wsmp,24,0);
            BitConv.ToInt32(wsmp,28,LOOPSTART);
            BitConv.ToInt32(wsmp,28,LOOPLENGTH);*/
            rgn.Items.Add(new RIFFData("wsmp",wsmp));
            byte[] wlnk = new byte [12];
            BitConv.ToInt16(wlnk,0,0);
            BitConv.ToInt16(wlnk,2,0);
            BitConv.ToInt32(wlnk,4,3); // ???
            BitConv.ToInt32(wlnk,8,wave - 1);
            rgn.Items.Add(new RIFFData("wlnk",wlnk));
            return rgn;
        }
    }
}
