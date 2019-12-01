using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class GOOLBox : UserControl
    {
        // TODO : find and fix remaining GOOL opcodes, add or improve comments

        private enum Opcodes {
            ADD = 0,
            SUB = 1,
            MUL = 2,
            DIV = 3,
            CEQ = 4,
            ANDL = 5,
            ORL = 6,
            ANDB = 7,
            ORB = 8,
            SLT = 9,
            SLE = 10,
            SGT = 11,
            SGE = 12,
            MOD = 13,
            XOR = 14,
            TST = 15,
            RND = 16,
            MOV = 17,
            NOTL = 18,
            PATH = 19,
            LEA = 20,
            SHA = 21,
            PUSH = 22,
            NOTB = 23,
            MOVC = 24,
            ABS = 25,
            PAD = 26,
            SPD = 27,
            MSC = 28,
            PRS = 29,
            TICK = 30,
            RGL = 31,
            WGL = 32,
            ANGD = 33,
            APCH = 34,
            CVMR = 35,
            CVMW = 36,
            ROT = 37,
            PSHP = 38,
            ANID = 39,
            EFDU = 40,
            EFDS = 41,
            //
            NOP = 47,
            DBG = 48,
            RET = 49,
            BRA = 50,
            BNEZ = 51,
            BEQZ = 52,
            CST = 53,
            CNEZ = 54,
            CEQZ = 55,
            ANIS = 56,
            ANIF = 57,
            VECA = 58,
            JAL = 59,
            EVNT = 60,
            RSTT = 61,
            RSTF = 62,
            CHLD = 63,
            NTRY = 64,
            SNDA = 65,
            SNDP = 66,
            VECB = 67,
            EVNB = 68,
            EVNU = 69,
            CHLF = 70,
            STCK = 71,
            //
            MIPS = 73
        };

        private enum ObjFields {
            self = 0,
            parent = 1,
            sibling = 2,
            child = 3,
            creator = 4,
            player = 5,
            collider = 6,
            interruptor = 7,
            x = 8,
            y = 9,
            z = 10,
            xrot = 11,
            yrot = 12,
            zrot = 13,
            xsca = 14,
            ysca = 15,
            zsca = 16,
            xvel = 17,
            yvel = 18,
            zvel = 19,
            xtrot = 20,
            ytrot = 21,
            ztrot = 22,
            modea = 23,
            modeb = 24,
            modec = 25,
            flagsa = 26,
            flagsb = 27,
            flagsc = 28,
            subtype = 29,
            id = 30,
            sp = 31,
            pc = 32,
            fp = 33,
            tpc = 34,
            epc = 35,
            hpc = 36,
            misc = 37,
            unk = 38,
            frametime = 39,
            statetime = 40,
            animlag = 41,
            animseq = 42,
            animframe = 43,
            entity = 44,
            pathprog = 45,
            pathlen = 46,
            ground = 47,
            stateflag = 48,
            speed = 49,
            displaymode = 50,
            unk2 = 51,
            landtime = 52,
            landvel = 53,
            zindex = 54,
            eventreceived = 55,
            zoom = 56,
            yzapproach = 57,
            hotspotclip = 58,
            unk3 = 59,
            unk4 = 60,
            unk5 = 61,
            collision = 62,
            mem0 = 63,
            mem1,mem2,mem3,mem4,mem5,mem6,mem7,mem8,mem9,
            mem10,mem11,mem12,mem13,mem14,mem15,mem16,mem17,mem18,mem19,
            mem20,mem21,mem22,mem23,mem24,mem25,mem26,mem27,mem28,mem29,
            mem30,mem31,mem32,mem33,mem34,mem35,mem36,mem37,mem38,mem39,
            mem40,mem41,mem42,mem43,mem44,mem45,mem46,mem47,mem48,mem49,
            mem50,mem51,mem52,mem53,mem54,mem55,mem56,mem57,mem58,mem59,
            mem60,mem61,mem62,mem63
        };

        private enum ObjColors
        {
            backa = 0,
            backr,
            backg,
            backb,
            lightcolr1,
            lightcolg1,
            lightcolb1,
            lightcolr2,
            lightcolg2,
            lightcolb2,
            lightcolr3,
            lightcolg3,
            lightcolb3,
            backintr,
            backintg,
            backintb
        }

        private enum ControllerButtons
        {
            L2 = 0x0001,
            R2 = 0x0002,
            L1 = 0x0004,
            R1 = 0x0008,
            Triangle = 0x0010,
            Circle = 0x0020,
            X = 0x0040,
            Square = 0x0080,
            Select = 0x0100,
            L3 = 0x0200,
            R3 = 0x0400,
            Start = 0x0800,
            Up = 0x1000,
            Right = 0x2000,
            Down = 0x4000,
            Left = 0x8000
        }

        private T11Entry goolentry;

        private ListBox lstCode;

        public GOOLBox(T11Entry goolentry)
        {
            this.goolentry = goolentry;
            lstCode = new ListBox
            {
                Dock = DockStyle.Fill
            };
            int interruptcount = BitConv.FromInt32(goolentry.Items[0],16);
            lstCode.Items.Add($"Type: {BitConv.FromInt32(goolentry.Items[0],0)}");
            lstCode.Items.Add($"Category: {BitConv.FromInt32(goolentry.Items[0],4)}");
            lstCode.Items.Add($"Format: {goolentry.Format}");
            lstCode.Items.Add(string.Format("Stack Start: {0} ({1})",(ObjFields)BitConv.FromInt32(goolentry.Items[0],12),GetNumber(BitConv.FromInt32(goolentry.Items[0],12)*4+0x40)));
            lstCode.Items.Add($"Interrupt Count: {interruptcount}");
            lstCode.Items.Add($"unknown: {BitConv.FromInt32(goolentry.Items[0],20)}");
            List<short> epc_list = new List<short>();
            List<short> tpc_list = new List<short>();
            List<short> cpc_list = new List<short>();
            if (BitConv.FromInt32(goolentry.Items[0], 8) == 1)
            {
                lstCode.Items.Add("");
                lstCode.Items.Add("Interrupts:");
                for (int i = 0; i < interruptcount; ++i)
                {
                    short evt = BitConv.FromInt16(goolentry.Items[3],i*2);
                    if (evt == 255)
                        continue;
                    else if ((evt & 0x8000) != 0)
                        lstCode.Items.Add($"\tInterrupt {i}: Sub_{evt & 0x3FFF}");
                    else
                        lstCode.Items.Add($"\tInterrupt {i}: State_{evt}");
                }
                lstCode.Items.Add($"Available Subtypes: {goolentry.Items[3].Length / 0x2 - interruptcount}");
                for (int i = interruptcount; i < goolentry.Items[3].Length / 0x2; ++i)
                {
                    short subtype = BitConv.FromInt16(goolentry.Items[3],i*2);
                    if (i > interruptcount && i+1 == goolentry.Items[3].Length / 2 && subtype == 0) continue;
                    lstCode.Items.Add($"\tSubtype {i - interruptcount}: {(subtype == 255 ? "invalid" : $"State_{subtype}")}");
                }
                lstCode.Items.Add("");
                for (int i = 0; i < goolentry.Items[4].Length / 0x10; ++i)
                {
                    short epc = (short)(BitConv.FromInt16(goolentry.Items[4],0x10*i+0xA) & 0x3FFF);
                    short tpc = (short)(BitConv.FromInt16(goolentry.Items[4],0x10*i+0xC) & 0x3FFF);
                    short cpc = (short)(BitConv.FromInt16(goolentry.Items[4],0x10*i+0xE) & 0x3FFF);
                    lstCode.Items.Add($"State_{i} [{Entry.EIDToEName(GetConst(BitConv.FromInt16(goolentry.Items[4],0x10*i+8)))}] (State Flags: {string.Format("0x{0:X}",BitConv.FromInt32(goolentry.Items[4],0x10*i+0))} | C-Flags: {string.Format("0x{0:X}",BitConv.FromInt32(goolentry.Items[4],0x10*i+4))})");
                    if (BitConv.FromInt32(goolentry.Items[2], 4 * BitConv.FromInt16(goolentry.Items[4], 0x10 * i + 8)) == goolentry.EID)
                    {
                        epc_list.Add(epc);
                        tpc_list.Add(tpc);
                        cpc_list.Add(cpc);
                    }
                    else
                    {
                        epc_list.Add(0x3FFF);
                        tpc_list.Add(0x3FFF);
                        cpc_list.Add(0x3FFF);
                    }
                    if (epc != 0x3FFF)
                    {
                        lstCode.Items.Add($"\tEPC: {epc}");
                    }
                    else
                        lstCode.Items.Add("\tEvent block unavailable.");
                    if (tpc != 0x3FFF)
                    {
                        lstCode.Items.Add($"\tTPC: {tpc}");
                    }
                    else
                        lstCode.Items.Add("\tTrans block unavailable.");
                    if (cpc != 0x3FFF)
                    {
                        lstCode.Items.Add($"\tCPC: {cpc}");
                    }
                    else
                        lstCode.Items.Add("\tCode block unavailable.");
                }
            }
            lstCode.Items.Add("");
            bool mips = false;
            bool returned = false;
            for (short i = 0; i < goolentry.Items[1].Length / 4; ++i)
            {
                string str;
                if (epc_list.Contains(i) || tpc_list.Contains(i) || cpc_list.Contains(i))
                {
                    str = "State_";
                    if (epc_list.Contains(i))
                        str += $"{epc_list.IndexOf(i)}_epc";
                    if (tpc_list.Contains(i))
                        str += $"{tpc_list.IndexOf(i)}_tpc";
                    if (cpc_list.Contains(i))
                        str += $"{cpc_list.IndexOf(i)}_cpc";
                    str += ":";
                    lstCode.Items.Add(str);
                    returned = false;
                }
                else if (returned)
                {
                    lstCode.Items.Add($"Sub_{i}:");
                    returned = false;
                }
                str = string.Format("{0,-05}\t", i);
                int ins = BitConv.FromInt32(goolentry.Items[1], 4*i);
                if (mips)
                {
                    int prev = BitConv.FromInt32(goolentry.Items[1], 4 * i - 4);
                    if (ins == 0x03E00008 || (prev == 0x03E0A809 && ins == 0)) // native mips returns or ends here
                    {
                        mips = false;
                        returned = ins == 0x03E00008;
                    }
                    str += GetMIPSInstruction(ins);
                }
                else
                {
                    Opcodes opcode = (Opcodes)(ins >> 24 & 0xFF);
                    if (!Enum.IsDefined(typeof(Opcodes), opcode))
                    {
                        str += $"unknown opcode {opcode}";
                    }
                    else
                    {
                        str += $"{opcode} ";
                    }
                    str += string.Format("\t{0,-030}\t{1}", GetArguments(ins), GetComment(ins));
                    mips = opcode == Opcodes.MIPS;
                    returned = opcode == Opcodes.RET;
                }
                lstCode.Items.Add(str);
            }
            Controls.Add(lstCode);
        }
        
        private int GetConst(int i)
        {
            return BitConv.FromInt32(goolentry.Items[2],i*4);
        }

        private string GetArguments(int ins)
        {
            Opcodes opcode = (Opcodes)(ins >> 24 & 0xFF);
            switch (opcode)
            {
                case Opcodes.ADD:
                case Opcodes.SUB:
                case Opcodes.MUL:
                case Opcodes.DIV:
                case Opcodes.CEQ:
                case Opcodes.ANDL:
                case Opcodes.ORL:
                case Opcodes.ANDB:
                case Opcodes.ORB:
                case Opcodes.SLT:
                case Opcodes.SLE:
                case Opcodes.SGT:
                case Opcodes.SGE:
                case Opcodes.MOD:
                case Opcodes.XOR:
                case Opcodes.TST:
                case Opcodes.RND:
                case Opcodes.MOV:
                case Opcodes.NOTL:
                case Opcodes.PATH:
                case Opcodes.LEA:
                case Opcodes.SHA:
                case Opcodes.PUSH:
                case Opcodes.NOTB:

                case Opcodes.ABS:

                case Opcodes.SPD:

                case Opcodes.PRS:
                case Opcodes.TICK:

                case Opcodes.WGL:
                case Opcodes.ANGD:
                case Opcodes.APCH:

                case Opcodes.ROT:
                case Opcodes.PSHP:
                case Opcodes.ANID:
                case Opcodes.EFDU:
                case Opcodes.EFDS:
                case (Opcodes)42:
                case (Opcodes)43:
                case (Opcodes)44:
                case (Opcodes)45:
                case (Opcodes)46:

                case Opcodes.DBG:

                case Opcodes.SNDA:

                case Opcodes.STCK:

                case (Opcodes)72:

                case (Opcodes)76:
                case (Opcodes)77:
                case (Opcodes)78:
                    return $"{GetGOOLReference(ins & 0xFFF)},{GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.PAD:
                    return $"{ins & 0xFFF},{ins >> 12 & 0b11},{ins >> 14 & 0b11},{ins >> 16 & 0b1111},{ins >> 20 & 0b1}";
                case Opcodes.MSC:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 12 & 0b111},{ins >> 15 & 0b11111},{ins >> 20 & 0b1111}";
                case Opcodes.RGL:
                case (Opcodes)74:
                case (Opcodes)75:
                    return $"{GetGOOLReference(ins & 0xFFF)}";
                case Opcodes.MOVC:
                    return $"{ins & 0x3FFF},{ins >> 14 & 1},{(ObjFields)(ins >> 18 & 0x3F)}";
                case Opcodes.CVMR:
                    return $"{ObjFields.self + (ins >> 12 & 0b111)},{ins >> 15 & 0b111111}";
                case Opcodes.CVMW:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ObjFields.self + (ins >> 12 & 0b111)},{ins >> 15 & 0b111111}";
                case Opcodes.NOP:
                case Opcodes.MIPS:
                    return string.Empty;
                case Opcodes.RET:
                    return $"{ins & 0x3FFF},{(ObjFields)(ins >> 14 & 0x3F)}";
                case Opcodes.BRA:
                case Opcodes.BNEZ:
                case Opcodes.BEQZ:
                case Opcodes.CST:
                case Opcodes.CNEZ:
                case Opcodes.CEQZ:
                    return $"{BitConv.SignExtend32(ins & 0x3FF, 9)},{ins >> 10 & 0xF},{(ObjFields)(ins >> 14 & 0x3F)}";
                case Opcodes.ANIS:
                    return $"{ins & 0x7F},{ins >> 7 & 0x1FF},{ins >> 16 & 0x3F},{ins >> 22 & 0b11}";
                case Opcodes.ANIF:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 16 & 0x3F},{ins >> 22 & 0b11}";
                case Opcodes.VECA:
                case Opcodes.EVNT:
                case Opcodes.EVNB:
                case Opcodes.EVNU:
                case Opcodes.VECB:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 12 & 0b111},{ins >> 15 & 0b111},{ins >> 18 & 0b111},{ins >> 21 & 0b111}";
                case Opcodes.JAL:
                    return $"{ins & 0x3FFF},{ins >> 20 & 0b1111}";
                case Opcodes.CHLD:
                case Opcodes.CHLF:
                    return $"{ins & 0x3F},{ins >> 6 & 0x3F},{ins >> 12 & 0xFF},{ins >> 20 & 0xF}";
                case Opcodes.NTRY:
                    return $"{ins & 0xFFF},{ins >> 12 & 0xFFF}";
                case Opcodes.SNDP:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 12 & 0xFF},{ins >> 20 & 0b1111}";
                default:
                    return string.Format("0x{0:X} UNHANDLED",ins & 0xFFFFFF);
            }
        }

        private string GetGOOLReference(int val)
        {
            if (val > 0xFFF) throw new ArgumentOutOfRangeException("val");
            string r;
            if (val == 0b101111100000) // 0xBE0
                r = "[null]";
            else if (val == 0b101111110000) // 0xBF0
                r = "[sp2]";
            else if (val == 0b111000011111) // 0xE1F
                r = "[top]";
            else if ((val & 0b100000000000) == 0)
            {
                if ((val & 0b010000000000) == 0) // ireg
                {
                    if (goolentry.Format == 1) // external GOOL entries will logically not have local data...
                    {
                        int cval = GetConst(val & 0b1111111111);
                        if (cval >= 0x2000000 && (cval & 1) == 1)
                            r = $"({Entry.EIDToEName(cval)})";
                        else
                            r = $"({GetNumber(cval)})";
                    }
                    else
                    {
                        r = $"[pool$({GetNumber(val & 0b1111111111)})]";
                    }
                }
                else // pool
                {
                    if (goolentry.Format == 0) // local GOOL entries will logically not have external data...
                    {
                        int cval = GetConst(val & 0b1111111111);
                        if (cval >= 0x2000000 && (cval & 1) == 1)
                            r = $"({Entry.EIDToEName(cval)})";
                        else
                            r = $"({GetNumber(cval)})";
                    }
                    else
                    {
                        r = $"[ext$({GetNumber(val & 0b1111111111)})]";
                    }
                }
            }
            else
            {
                int hi1 = val >> 9 & 0b11;
                if (hi1 == 0) // int
                {
                    r = $"{GetNumber(BitConv.SignExtend32(val & 0x1FF,8))}";
                }
                else if (hi1 == 1)
                {
                    if ((val >> 8 & 1) == 0) // frac
                        r = $"{GetNumber(BitConv.SignExtend32(val,7)*0x100)}";
                    else // stack
                    {
                        int n = BitConv.SignExtend32(val, 6);
                        r = string.Format("{0}[{1}]", n >= 0 ? "stack" : "arg", GetNumber(n < 0 ? -n - 1 : n));
                    }
                }
                else if (hi1 == 2) // reg
                {
                    r = $"{ObjFields.self + (val >> 6 & 0b111)}->{(ObjFields)(val & 0x3F)}";
                }
                else if (hi1 == 3) // var
                {
                    if (Enum.IsDefined(typeof(ObjFields), val & 0x1FF))
                    {
                        r = ((ObjFields)(val & 0x1FF)).ToString();
                    }
                    else
                        r = $"[var${GetNumber(0x1FF)}]";
                }
                else throw new Exception();
            }
            return r;
        }

        private bool IsNullGOOLReference(int val)
        {
            return val == 0b101111100000; // 0xBE0
        }

        private string GetComment(int ins)
        {
            Opcodes opcode = (Opcodes)(ins >> 24 & 0xFF);
            switch (opcode)
            {
                case Opcodes.ADD:
                    return $"# {GetGOOLReference(ins & 0xFFF)} + {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.SUB:
                    return $"# {GetGOOLReference(ins & 0xFFF)} - {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.MUL:
                    return $"# {GetGOOLReference(ins & 0xFFF)} * {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.DIV:
                    return $"# {GetGOOLReference(ins & 0xFFF)} / {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.CEQ:
                    return $"# {GetGOOLReference(ins & 0xFFF)} == {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.ANDL:
                    return $"# {GetGOOLReference(ins & 0xFFF)} && {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.ANDB:
                    return $"# {GetGOOLReference(ins & 0xFFF)} & {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.ORL:
                case Opcodes.ORB:
                    return $"# {GetGOOLReference(ins & 0xFFF)} | {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.SLT:
                    return $"# {GetGOOLReference(ins & 0xFFF)} < {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.SLE:
                    return $"# {GetGOOLReference(ins & 0xFFF)} <= {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.SGT:
                    return $"# {GetGOOLReference(ins & 0xFFF)} > {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.SGE:
                    return $"# {GetGOOLReference(ins & 0xFFF)} >= {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.MOD:
                    return $"# {GetGOOLReference(ins & 0xFFF)} % {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.XOR:
                    return $"# {GetGOOLReference(ins & 0xFFF)} ^ {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.TST:
                    return $"# {GetGOOLReference(ins & 0xFFF)} has {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.RND:
                    return $"# random number from {GetGOOLReference(ins & 0xFFF)} to {GetGOOLReference(ins >> 12 & 0xFFF)}-1";
                case Opcodes.MOV:
                    return $"# {GetGOOLReference(ins & 0xFFF)} = {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.NOTL:
                    return $"# {GetGOOLReference(ins & 0xFFF)} = !{GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.LEA:
                    return $"# {GetGOOLReference(ins & 0xFFF)} = &{GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.SHA:
                    return $"# {GetGOOLReference(ins & 0xFFF)} << {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.PUSH:
                    if (!IsNullGOOLReference(ins & 0xFFF))
                    {
                        if (!IsNullGOOLReference(ins >> 12 & 0xFFF))
                            return $"# push {GetGOOLReference(ins & 0xFFF)} and {GetGOOLReference(ins >> 12 & 0xFFF)}";
                        else
                            return $"# push {GetGOOLReference(ins & 0xFFF)}";
                    }
                    break;
                case Opcodes.NOTB:
                    return $"# {GetGOOLReference(ins & 0xFFF)} = ~{GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.MOVC:
                    return $"# {(ObjFields)(ins >> 18 & 0x3F)} = {((ins & 0x4000) != 0 ? "ext_" : "")}ins[{ins & 0x3FFF}]";
                case Opcodes.ABS:
                    return $"# {GetGOOLReference(ins & 0xFFF)} = abs({GetGOOLReference(ins >> 12 & 0xFFF)})";
                case Opcodes.PAD:
                    if ((ins >> 14 & 0b11) == 0) // check buttons
                    {
                        string str = string.Empty;
                        int buttons = ins & 0xFFF;
                        bool multiple = false;
                        for (int i = 0; i < 12; ++i)
                        {
                            if ((buttons & (1 << i)) != 0)
                            {
                                if (multiple)
                                {
                                    str += " or ";
                                }
                                str += (ControllerButtons)(buttons & (1 << i));
                                multiple = true;
                            }
                        }
                        if (string.IsNullOrEmpty(str))
                        {
                            str = "nothing";
                        }
                        if ((ins >> 20 & 1) == 0)
                            str += " is ";
                        else
                            str += " is not ";
                        switch (ins >> 12 & 0b11)
                        {
                            case 0:
                                return "# check no controller buttons";
                            case 1:
                                str += "tapped"; break;
                            case 2:
                                str += "held down (1 frame)"; break;
                            case 3:
                                str += "held down (2 frames)"; break;
                        }
                        return $"# check if {str}";
                    }
                    else
                    {
                        int dpad = ins >> 16 & 0xF;
                        string str = ((ControllerButtons)(dpad << (12 + (int)ControllerButtons.Up))).ToString();
                        if ((ins >> 20 & 1) == 0)
                            str += " is ";
                        else
                            str += " is not ";
                        switch (ins >> 14 & 0b11)
                        {
                            case 1:
                                str += "tapped"; break;
                            case 2:
                                str += "held down (1 frame)"; break;
                            case 3:
                                str += "held down (2 frames)"; break;
                        }
                        return $"# check if {str}";
                    }
                case Opcodes.TICK:
                    return $"# ({GetGOOLReference(ins >> 12 & 0xFFF)} + tick) % {GetGOOLReference(ins & 0xFFF)}";
                case Opcodes.RGL:
                    return $"# global[{GetGOOLReference(ins & 0xFFF)}]";
                case Opcodes.WGL:
                    return $"# global[{GetGOOLReference(ins & 0xFFF)}] = {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.APCH:
                    return $"# approach({GetGOOLReference(ins & 0xFFF)}, {GetGOOLReference(ins >> 12 & 0xFFF)}, ?)";
                case Opcodes.CVMR:
                    return $"# {ObjFields.self + (ins >> 12 & 0b111)}->{(ObjColors)(ins >> 15 & 0b111111)}";
                case Opcodes.CVMW:
                    return $"# {ObjFields.self + (ins >> 12 & 0b111)}->{(ObjColors)(ins >> 15 & 0b111111)} = {GetGOOLReference(ins & 0xFFF)}";
                case Opcodes.ANID:
                    return $"# {GetGOOLReference(ins & 0xFFF)} = &{goolentry.EName}.anim[{GetGOOLReference(ins >> 12 & 0xFFF)}]";
                case Opcodes.NOP:
                    return $"\t# no operation";
                case Opcodes.ANIS:
                    return $"# play frame {ins & 0x7F} animation at {ins >> 7 & 0x1FF} (flip {ins >> 22 & 0b11}) for {ins >> 16 & 0x3F} frames";
                case Opcodes.ANIF:
                    return $"# play frame {GetGOOLReference(ins & 0xFFF)} (flip {ins >> 22 & 0b11}) for {ins >> 16 & 0x3F} frames";
                case Opcodes.BRA:
                    return $"# {((ins & 0x3FF) != 0 ? "branch immediately and " : "")}pop {ins >> 10 & 0b1111} values off stack";
                case Opcodes.BNEZ:
                    return $"# if true, {((ins & 0x3FF) != 0 ? "branch and " : "")}pop {ins >> 10 & 0b1111} values off stack";
                case Opcodes.BEQZ:
                    return $"# if false, {((ins & 0x3FF) != 0 ? "branch and " : "")}pop {ins >> 10 & 0b1111} values off stack";
                case Opcodes.MIPS:
                    return $"\t# begin native MIPS bytecode";
            }
            return string.Empty;
        }

        private enum MIPSReg
        {
            zr = 0,
            at,
            v0, v1,
            a0, a1, a2, a3,
            t0, t1, t2, t3, t4, t5, t6, t7,
            s0, s1, s2, s3, s4, s5, s6, s7, s8,
            t8,
            k0, k1,
            gp,
            sp,
            fp,
            ra
        }

        private string GetMIPSInstruction(int ins)
        {
            if (ins == 0)
                return "nop";
            string str = string.Empty;

            int opcode = ins >> 26 & 0b111111;
            int rs = ins >> 21 & 0b11111;
            int rt = ins >> 16 & 0b11111;
            int rd = ins >> 11 & 0b11111;
            int shamt = ins >> 6 & 0b11111;
            int funct = ins & 0b111111;
            short imm = (short)(ins & 0xFFFF);
            int ofs = ins ^ (opcode << 26);

            switch (opcode)
            {
                case 0:
                    switch (funct)
                    {
                        case 0:
                            return $"sll \t{GetMIPSReg(rd)},{GetMIPSReg(rt)},{shamt}";
                        case 2:
                            return $"srl \t{GetMIPSReg(rd)},{GetMIPSReg(rt)},{shamt}";
                        case 3:
                            return $"sra \t{GetMIPSReg(rd)},{GetMIPSReg(rt)},{shamt}";
                        case 4:
                            return $"sllv\t{GetMIPSReg(rd)},{GetMIPSReg(rt)},{GetMIPSReg(rs)}";
                        case 6:
                            return $"srlv\t{GetMIPSReg(rd)},{GetMIPSReg(rt)},{GetMIPSReg(rs)}";
                        case 7:
                            return $"srav\t{GetMIPSReg(rd)},{GetMIPSReg(rt)},{GetMIPSReg(rs)}";
                        case 8:
                            return $"jr  \t{GetMIPSReg(rs)}";
                        case 9:
                            return $"jalr\t{GetMIPSReg(rs)},{GetMIPSReg(rd)}";
                        case 12:
                            return $"syscall";
                        case 13:
                            return $"break";
                        case 16:
                            return $"mfhi\t{GetMIPSReg(rd)}";
                        case 17:
                            return $"mthi\t{GetMIPSReg(rd)}"; // verify
                        case 18:
                            return $"mflo\t{GetMIPSReg(rd)}";
                        case 19:
                            return $"mtlo\t{GetMIPSReg(rd)}"; // verify
                        case 24:
                            return $"mult\t{GetMIPSReg(rt)},{GetMIPSReg(rs)}";
                        case 25:
                            return $"multu\t{GetMIPSReg(rt)},{GetMIPSReg(rs)}";
                        case 26:
                            return $"div \t{GetMIPSReg(rt)},{GetMIPSReg(rs)}";
                        case 27:
                            return $"divu\t{GetMIPSReg(rt)},{GetMIPSReg(rs)}";
                        case 32:
                            return $"add \t{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                        case 33:
                            return $"addu\t{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                        case 34:
                            return $"sub \t{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                        case 35:
                            return $"subu\t{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                        case 36:
                            return $"and \t{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                        case 37:
                            return $"or  \t{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                        case 38:
                            return $"xor \t{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                        case 39:
                            return $"nor \t{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                        case 42:
                            return $"slt \t{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                        case 43:
                            return $"sltu\t{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                    }
                    break;
                case 1:
                    switch (rt)
                    {
                        case 0:
                            return $"bltz\t{GetMIPSReg(rs)},{GetNumber(imm)}";
                        case 1:
                            return $"bgez\t{GetMIPSReg(rs)},{GetNumber(imm)}";
                        case 16:
                            return $"bltzal\t{GetMIPSReg(rs)},{GetNumber(imm)}";
                        case 17:
                            return $"bgezal\t{GetMIPSReg(rs)},{GetNumber(imm)}";
                    }
                    break;
                case 2:
                    return $"j   \t{ofs}";
                case 3:
                    return $"jal \t{ofs}";
                case 4:
                    return $"beq \t{GetMIPSReg(rs)},{GetMIPSReg(rt)},{GetNumber(imm)}";
                case 5:
                    return $"bne \t{GetMIPSReg(rs)},{GetMIPSReg(rt)},{GetNumber(imm)}";
                case 6:
                    return $"blez\t{GetMIPSReg(rs)},{GetNumber(imm)}";
                case 7:
                    return $"bgtz\t{GetMIPSReg(rs)},{GetNumber(imm)}";
                case 8:
                    return $"addi\t{GetMIPSReg(rt)},{GetMIPSReg(rs)},{GetNumber(imm)}";
                case 9:
                    return $"addiu\t{GetMIPSReg(rt)},{GetMIPSReg(rs)},{GetNumber(imm)}";
                case 10:
                    return $"subi\t{GetMIPSReg(rt)},{GetMIPSReg(rs)},{GetNumber(imm)}";
                case 11:
                    return $"subiu\t{GetMIPSReg(rt)},{GetMIPSReg(rs)},{GetNumber(imm)}";
                case 12:
                    return $"andi\t{GetMIPSReg(rt)},{GetMIPSReg(rs)},{GetNumber(imm)}";
                case 13:
                    return $"ori \t{GetMIPSReg(rt)},{GetMIPSReg(rs)},{GetNumber(imm)}";
                case 14:
                    return $"xori\t{GetMIPSReg(rt)},{GetMIPSReg(rs)},{GetNumber(imm)}";
                case 15:
                    return $"xori\t{GetMIPSReg(rt)},{GetNumber(imm)}";
                case 32:
                    return string.Format("lb  \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 33:
                    return string.Format("lh  \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 34:
                    return string.Format("lwl \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 35:
                    return string.Format("lw  \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 36:
                    return string.Format("lbu \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 37:
                    return string.Format("lhu \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 38:
                    return string.Format("lwr \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 40:
                    return string.Format("sb  \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 41:
                    return string.Format("sh  \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 42:
                    return string.Format("swl \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 43:
                    return string.Format("sw  \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
                case 46:
                    return string.Format("swr \t{1},{0}({2})",GetNumber(imm),GetMIPSReg(rt),GetMIPSReg(rs));
            }

            return str;
        }

        private string GetMIPSReg(int reg)
        {
            return $"${(MIPSReg)reg}";
        }

        private string GetNumber(short num)
        {
            if (num > 64 || num < -64)
            {
                if (num < 0)
                    return string.Format("-0x{0:X}",-num);
                else
                    return string.Format("0x{0:X}",num);
            }
            else
                return $"{num}";
        }

        private string GetNumber(int num)
        {
            if (num > 64 || num < -64)
            {
                if (num < 0)
                    return string.Format("-0x{0:X}",-num);
                else
                    return string.Format("0x{0:X}",num);
            }
            else
                return $"{num}";
        }
    }
}
