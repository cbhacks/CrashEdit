using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldGOOLBox : UserControl
    {
        // TODO : fix remaining GOOL opcodes, add or improve comments

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

            DBG = 128,
            NOP = 129,
            CFL = 130, // has subtypes
            ANIS = 131,
            ANIF = 132,
            VECA = 133,
            JAL = 134,
            EVNT = 135,
            RSTT = 136,
            RSTF = 137,
            CHLD = 138,
            NTRY = 139,
            SNDA = 140,
            SNDP = 141,
            VECB = 142,
            EVNB = 143,
            EVNU = 144,
            CHLF = 145
        };

        private enum CflNames {
            BRA = 0,
            BNEZ = 1,
            BEQZ = 2,
            CST = 4,
            CSN = 5,
            CSE = 6,
            RET = 8
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
            lightsrc11 = 0,
            lightsrc12,
            lightsrc13,
            lightsrc21,
            lightsrc22,
            lightsrc23,
            lightsrc31,
            lightsrc32,
            lightsrc33,
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

        private OldT11Entry goolentry;

        private ListBox lstCode;

        public OldGOOLBox(OldT11Entry goolentry)
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
            lstCode.Items.Add(string.Format("Stack Start: {0} ({1})",(ObjFields)BitConv.FromInt32(goolentry.Items[0],12),GetNumber(BitConv.FromInt32(goolentry.Items[0],12)*4)));
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
                    if (BitConv.FromInt16(goolentry.Items[3],i*2) == 255) continue;
                    lstCode.Items.Add($"\tInterrupt {i}: State_{goolentry.Items[3][i * 2]}");
                }
                lstCode.Items.Add($"Available Subtypes: {goolentry.Items[3].Length / 0x2 - interruptcount}");
                for (int i = interruptcount; i < goolentry.Items[3].Length / 0x2; ++i)
                {
                    if (i > interruptcount && i+1 == goolentry.Items[3].Length / 2 && (interruptcount & 1) == 1 && goolentry.Items[3][i * 2] == 0) continue;
                    lstCode.Items.Add($"\tSubtype {i - interruptcount}: {(BitConv.FromInt16(goolentry.Items[3],i*2) == 255 ? "invalid" : $"State_{goolentry.Items[3][i * 2]}")}");
                }
                lstCode.Items.Add("");
                for (int i = 0; i < goolentry.Items[4].Length / 0x10; ++i)
                {
                    short epc = BitConv.FromInt16(goolentry.Items[4],0x10*i+0xA);
                    short tpc = BitConv.FromInt16(goolentry.Items[4],0x10*i+0xC);
                    short cpc = BitConv.FromInt16(goolentry.Items[4],0x10*i+0xE);
                    lstCode.Items.Add($"State_{i} [{Entry.EIDToEName(GetConst(BitConv.FromInt16(goolentry.Items[4],0x10*i+8)))}] (State Flags: {BitConv.FromInt32(goolentry.Items[4],0x10*i+0)} | C-Flags: {BitConv.FromInt32(goolentry.Items[4],0x10*i+4)})");
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
                }
                int ins = BitConv.FromInt32(goolentry.Items[1], 4 * i);
                Opcodes opcode = (Opcodes)(ins >> 24 & 0xFF);
                str = string.Format("{0,-05}\t", i);
                if (!Enum.IsDefined(typeof(Opcodes), opcode))
                {
                    str += $"invalid opcode {opcode}";
                }
                else
                {
                    if (opcode == Opcodes.CFL)
                    {
                        str += $"{((CflNames)(ins >> 20 & 0xF)).ToString()} ";
                    }
                    else
                    {
                        str += $"{opcode.ToString()} ";
                    }
                }
                str += string.Format("\t{0,-030}\t{1}", GetArguments(ins), GetComment(ins));
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
                case Opcodes.SHA:
                case Opcodes.ANGD:
                case Opcodes.APCH:
                case Opcodes.ROT:
                case Opcodes.DBG:

                case Opcodes.RND:
                case Opcodes.PATH:
                case Opcodes.PUSH:
                case Opcodes.SNDA:

                case Opcodes.MOV:
                case Opcodes.NOTL:
                case Opcodes.LEA:
                case Opcodes.NOTB:
                case Opcodes.ABS:

                case Opcodes.SPD:
                case Opcodes.PRS:

                case Opcodes.TICK:

                case Opcodes.WGL:

                case Opcodes.PSHP:
                case Opcodes.ANID:
                    return $"{GetGOOLReference(ins & 0xFFF)},{GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.RGL:
                    return $"{GetGOOLReference(ins & 0xFFF)}";
                case Opcodes.CVMR:
                    return $"{ObjFields.self + (ins >> 12 & 0b111)},{ins >> 15 & 0b111111}";
                case Opcodes.CVMW:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ObjFields.self + (ins >> 12 & 0b111)},{ins >> 15 & 0b111111}";
                case Opcodes.ANIS:
                    return $"{ins & 0x7F},{ins >> 7 & 0x1FF},{ins >> 16 & 0x3F},{ins >> 22 & 0b11}";
                case Opcodes.ANIF:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 16 & 0x3F},{ins >> 22 & 0b11}";
                case Opcodes.VECA:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 12 & 0b111},{ins >> 15 & 0b111},{ins >> 18 & 0b111},{ins >> 21 & 0b111}";
                case Opcodes.JAL:
                    return $"{ins & 0x3FFF},{ins >> 20 & 0b1111}";
                case Opcodes.CHLD:
                case Opcodes.CHLF:
                    return $"{ins & 0x3F},{ins >> 6 & 0x3F},{ins >> 12 & 0xFF},{ins >> 20 & 0xF}";
                case Opcodes.SNDP:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 12 & 0xFF},{ins >> 20 & 0b1111}";
                case Opcodes.EVNB:
                case Opcodes.EVNU:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 12 & 0b111111},{ins >> 18 & 0b111},{ins >> 21 & 0b111}";
                case Opcodes.CFL:
                    {
                        switch ((CflNames)(ins >> 20 & 0xF))
                        {
                            case CflNames.BRA:
                            case CflNames.BNEZ:
                            case CflNames.BEQZ:
                                return $"{BitConv.SignExtend32(ins & 0x1FF, 8)},{ins >> 10 & 0xF},{(ObjFields)(ins >> 14 & 0x3F)}";
                            case CflNames.CST:
                            case CflNames.CSN:
                            case CflNames.CSE:
                            case CflNames.RET:
                                return $"{ins & 0x3FFF},{(ObjFields)(ins >> 14 & 0x3F)}";
                            default:
                                return string.Empty;
                        }
                    }
                case Opcodes.MOVC:
                    return $"{ins & 0x3FFF},{(ObjFields)(ins >> 14 & 0x3F)}";
                case Opcodes.PAD:
                    return $"{ins & 0xFFF},{ins >> 12 & 0b11},{ins >> 14 & 0b11},{ins >> 16 & 0b1111},{ins >> 20 & 0b1}";
                case Opcodes.MSC:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 12 & 0b111},{ins >> 15 & 0b11111},{ins >> 20 & 0b1111}";
                case Opcodes.NTRY:
                    return $"{ins & 0xFFF},{ins >> 12 & 0xFFF}";
                case Opcodes.VECB:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 12 & 0b111},{ins >> 15 & 0b111},{ins >> 18 & 0b111},{ins >> 21 & 0b111}";
                case Opcodes.EVNT:
                    return $"{GetGOOLReference(ins & 0xFFF)},{ins >> 12 & 0b111111},{ins >> 18 & 0b111},{ins >> 21 & 0b111}";
                case Opcodes.NOP:
                    return string.Empty;
                default:
                    return string.Format("0x{0:X} UNHANDLED", ins & 0xFFFFFF);
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
                case Opcodes.ORL:
                    return $"# {GetGOOLReference(ins & 0xFFF)} || {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.ANDB:
                    return $"# {GetGOOLReference(ins & 0xFFF)} & {GetGOOLReference(ins >> 12 & 0xFFF)}";
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
                    return $"# {(ObjFields)(ins >> 14 & 0x3F)} = ins[{ins & 0x3FFF}]";
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
                                str += "tapped";
                                break;
                            case 2:
                                str += "held down (1 frame)";
                                break;
                            case 3:
                                str += "held down (2 frames)";
                                break;
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
                case Opcodes.SPD:
                    return $"# {GetGOOLReference(ins & 0xFFF)} + (({GetGOOLReference(ins >> 12 & 0xFFF)}*gvel) >> 10)";
                case Opcodes.PRS:
                    return $"# psine({GetGOOLReference(ins & 0xFFF)}, {GetGOOLReference(ins >> 12 & 0xFFF)})";
                case Opcodes.TICK:
                    return $"# ({GetGOOLReference(ins >> 12 & 0xFFF)} + tick) % {GetGOOLReference(ins & 0xFFF)}";
                case Opcodes.RGL:
                    return $"# global[{GetGOOLReference(ins & 0xFFF)}]";
                case Opcodes.WGL:
                    return $"# global[{GetGOOLReference(ins & 0xFFF)}] = {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.ANGD:
                    return $"# angdist({GetGOOLReference(ins & 0xFFF)}, {GetGOOLReference(ins >> 12 & 0xFFF)})";
                case Opcodes.APCH:
                    return $"# approach({GetGOOLReference(ins & 0xFFF)}, {GetGOOLReference(ins >> 12 & 0xFFF)}, 0x100)";
                case Opcodes.CVMR:
                    return $"# {ObjFields.self + (ins >> 12 & 0b111)}->{(ObjColors)(ins >> 15 & 0b111111)}";
                case Opcodes.CVMW:
                    return $"# {ObjFields.self + (ins >> 12 & 0b111)}->{(ObjColors)(ins >> 15 & 0b111111)} = {GetGOOLReference(ins & 0xFFF)}";
                case Opcodes.ROT:
                    return $"# rotate({GetGOOLReference(ins & 0xFFF)}, {GetGOOLReference(ins >> 12 & 0xFFF)}, 0x100, 0)";
                case Opcodes.PSHP:
                    if (!IsNullGOOLReference(ins & 0xFFF))
                    {
                        if (!IsNullGOOLReference(ins >> 12 & 0xFFF))
                            return $"# push &{GetGOOLReference(ins & 0xFFF)} and &{GetGOOLReference(ins >> 12 & 0xFFF)}";
                        else
                            return $"# push &{GetGOOLReference(ins & 0xFFF)}";
                    }
                    break;
                case Opcodes.ANID:
                    return $"# {GetGOOLReference(ins & 0xFFF)} = &{goolentry.EName}.anim[{GetGOOLReference(ins >> 12 & 0xFFF)}]";
                case Opcodes.ANIS:
                    return $"# play frame {ins & 0x7F} animation at {ins >> 7 & 0x1FF} (flip {ins >> 22 & 0b11}) for {ins >> 16 & 0x3F} frames";
                case Opcodes.ANIF:
                    return $"# play frame {GetGOOLReference(ins & 0xFFF)} (flip {ins >> 22 & 0b11}) for {ins >> 16 & 0x3F} frames";
                case Opcodes.DBG:
                    return $"# print {GetGOOLReference(ins & 0xFFF)} and {GetGOOLReference(ins >> 12 & 0xFFF)}";
                case Opcodes.NOP:
                    return $"# no operation";
            }
            return string.Empty;
        }

        private string GetNumber(int num)
        {
            if (num > 64 || num < -64)
            {
                if (num < 0)
                    return string.Format("-0x{0:X}", -num);
                else
                    return string.Format("0x{0:X}", num);
            }
            else
                return $"{num}";
        }
    }
}
