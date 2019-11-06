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

        public VHTone(bool isoldversion)
        {
            if (isoldversion)
            {
                Priority = 0;
                Mode = 0;
                Volume = 0;
                Panning = 0;
                CenterNote = 0;
                PitchShift = 0;
                MinimumNote = 0;
                MaximumNote = 0;
                VibratoWidth = 0;
                VibratoTime = 0;
                PortamentoWidth = 0;
                PortamentoTime = 0;
                PitchBendMinimum = 0;
                PitchBendMaximum = 0;
                unchecked
                {
                    ADSR1 = (short)0x80DF;
                    ADSR2 = 0x5FDF;
                }
            }
            else
            {
                Priority = 0;
                Mode = 0;
                Volume = 80;
                Panning = 64;
                CenterNote = 64;
                PitchShift = 0;
                MinimumNote = 64;
                MaximumNote = 64;
                VibratoWidth = 0;
                VibratoTime = 0;
                PortamentoWidth = 0;
                PortamentoTime = 0;
                PitchBendMinimum = 0;
                PitchBendMaximum = 0;
                unchecked
                {
                    ADSR1 = (short)0x80DF;
                    ADSR2 = 0x5FDF;
                }
            }
            Wave = 0;
        }

        // This is ridiculous! There has to be a better way.
        public VHTone(byte priority,byte mode,byte volume,byte panning,byte centernote,byte pitchshift,byte minimumnote,byte maximumnote,byte vibratowidth,byte vibratotime,byte portamentowidth,byte portamentotime,byte pitchbendminimum,byte pitchbendmaximum,short adsr1,short adsr2,short wave)
        {
            Priority = priority;
            Mode = mode;
            Volume = volume;
            Panning = panning;
            CenterNote = centernote;
            PitchShift = pitchshift;
            MinimumNote = minimumnote;
            MaximumNote = maximumnote;
            VibratoWidth = vibratowidth;
            VibratoTime = vibratotime;
            PortamentoWidth = portamentowidth;
            PortamentoTime = portamentotime;
            PitchBendMinimum = pitchbendminimum;
            PitchBendMaximum = pitchbendmaximum;
            ADSR1 = adsr1;
            ADSR2 = adsr2;
            Wave = wave;
        }

        public byte Priority { get; }
        public byte Mode { get; }
        public byte Volume { get; }
        public byte Panning { get; }
        public byte CenterNote { get; }
        public byte PitchShift { get; }
        public byte MinimumNote { get; }
        public byte MaximumNote { get; }
        public byte VibratoWidth { get; }
        public byte VibratoTime { get; }
        public byte PortamentoWidth { get; }
        public byte PortamentoTime { get; }
        public byte PitchBendMinimum { get; }
        public byte PitchBendMaximum { get; }
        public short ADSR1 { get; }
        public short ADSR2 { get; }
        public short Wave { get; }

        public byte[] Save(int program)
        {
            byte[] data = new byte [32];
            data[0] = Priority;
            data[1] = Mode;
            data[2] = Volume;
            data[3] = Panning;
            data[4] = CenterNote;
            data[5] = PitchShift;
            data[6] = MinimumNote;
            data[7] = MaximumNote;
            data[8] = VibratoWidth;
            data[9] = VibratoTime;
            data[10] = PortamentoWidth;
            data[11] = PortamentoTime;
            data[12] = PitchBendMinimum;
            data[13] = PitchBendMaximum;
            data[14] = 0xB1;
            data[15] = 0xB2;
            BitConv.ToInt16(data,16,ADSR1);
            BitConv.ToInt16(data,18,ADSR2);
            BitConv.ToInt16(data,20,(short)program);
            BitConv.ToInt16(data,22,Wave);
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
            BitConv.ToInt16(rgnh,0,MinimumNote);
            BitConv.ToInt16(rgnh,2,MaximumNote);
            BitConv.ToInt16(rgnh,4,0);
            BitConv.ToInt16(rgnh,6,127);
            BitConv.ToInt16(rgnh,8,0);
            BitConv.ToInt16(rgnh,10,0);
            rgn.Items.Add(new RIFFData("rgnh",rgnh));
            byte[] wsmp = new byte [20 /* 36 */];
            BitConv.ToInt32(wsmp,0,20);
            BitConv.ToInt16(wsmp,4,CenterNote);
            BitConv.ToInt16(wsmp,6,PitchShift);
            BitConv.ToInt32(wsmp,8,Volume - 64 << 16);
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
            BitConv.ToInt32(wlnk,8,Wave - 1);
            rgn.Items.Add(new RIFFData("wlnk",wlnk));
            return rgn;
        }
    }
}
