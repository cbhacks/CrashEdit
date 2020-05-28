namespace Crash
{
    public sealed class MIPSInstruction : GOOLInstruction
    {
        private string mipsname;
        private MIPSInstructionFormats mipsformat;

        private enum MIPSInstructionFormats
        {
            None,
            Shift,
            ShiftVariable,
            JumpRegister,
            JumpAndLinkRegister,
            MoveTo,
            MoveFrom,
            MultDiv,
            ArithmeticLogic,
            ArithmeticLogicImmediate,
            Jump,
            BranchTarget,
            Branch,
            LoadUpperImmediate,
            LoadStore
        }

        private enum MIPSRegisters
        {
            zr = 0,
            at,
            v0, v1,
            a0, a1, a2, a3,
            t0, t1, t2, t3, t4, t5, t6, t7,
            s0, s1, s2, s3, s4, s5, s6, s7,
            t8, t9,
            k0, k1,
            gp,
            sp,
            s8,
            ra
        }

        private void SetInsParams(string name, MIPSInstructionFormats fmt)
        {
            mipsname = name;
            mipsformat = fmt;
        }

        public MIPSInstruction(int value, GOOLEntry gool) : base(value, gool)
        {
            if (value == 0)
            {
                SetInsParams("nop", MIPSInstructionFormats.None);
                return;
            }

            int opcode = value >> 26 & 0b111111;
            int rt = value >> 16 & 0b11111;
            int funct = value & 0b111111;

            switch (opcode)
            {
                case 0:
                    switch (funct)
                    {
                        case 0:
                            SetInsParams("sll", MIPSInstructionFormats.Shift); break;
                        case 2:
                            SetInsParams("srl", MIPSInstructionFormats.Shift); break;
                        case 3:
                            SetInsParams("sra", MIPSInstructionFormats.Shift); break;
                        case 4:
                            SetInsParams("sllv", MIPSInstructionFormats.ShiftVariable); break;
                        case 6:
                            SetInsParams("srlv", MIPSInstructionFormats.ShiftVariable); break;
                        case 7:
                            SetInsParams("srav", MIPSInstructionFormats.ShiftVariable); break;
                        case 8:
                            SetInsParams("jr", MIPSInstructionFormats.JumpRegister); break;
                        case 9:
                            SetInsParams("jalr", MIPSInstructionFormats.JumpAndLinkRegister); break;
                        case 12:
                            SetInsParams("syscall", MIPSInstructionFormats.None); break;
                        case 13:
                            SetInsParams("break", MIPSInstructionFormats.None); break;
                        case 16:
                            SetInsParams("mfhi", MIPSInstructionFormats.MoveFrom); break;
                        case 17:
                            SetInsParams("mthi", MIPSInstructionFormats.MoveFrom); break; // verify
                        case 18:
                            SetInsParams("mflo", MIPSInstructionFormats.MoveFrom); break;
                        case 19:
                            SetInsParams("mtlo", MIPSInstructionFormats.MoveFrom); break; // verify
                        case 24:
                            SetInsParams("mult", MIPSInstructionFormats.MultDiv); break;
                        case 25:
                            SetInsParams("multu", MIPSInstructionFormats.MultDiv); break;
                        case 26:
                            SetInsParams("div", MIPSInstructionFormats.MultDiv); break;
                        case 27:
                            SetInsParams("divu", MIPSInstructionFormats.MultDiv); break;
                        case 32:
                            SetInsParams("add", MIPSInstructionFormats.ArithmeticLogic); break;
                        case 33:
                            SetInsParams("addu", MIPSInstructionFormats.ArithmeticLogic); break;
                        case 34:
                            SetInsParams("sub", MIPSInstructionFormats.ArithmeticLogic); break;
                        case 35:
                            SetInsParams("subu", MIPSInstructionFormats.ArithmeticLogic); break;
                        case 36:
                            SetInsParams("and", MIPSInstructionFormats.ArithmeticLogic); break;
                        case 37:
                            SetInsParams("or", MIPSInstructionFormats.ArithmeticLogic); break;
                        case 38:
                            SetInsParams("xor", MIPSInstructionFormats.ArithmeticLogic); break;
                        case 39:
                            SetInsParams("nor", MIPSInstructionFormats.ArithmeticLogic); break;
                        case 42:
                            SetInsParams("slt", MIPSInstructionFormats.ArithmeticLogic); break;
                        case 43:
                            SetInsParams("sltu", MIPSInstructionFormats.ArithmeticLogic); break;
                    }
                    break;
                case 1:
                    switch (rt)
                    {
                        case 0:
                            SetInsParams("bltz", MIPSInstructionFormats.Branch); break;
                        case 1:
                            SetInsParams("bgez", MIPSInstructionFormats.Branch); break;
                        case 16:
                            SetInsParams("bltzal", MIPSInstructionFormats.Branch); break;
                        case 17:
                            SetInsParams("bgezal", MIPSInstructionFormats.Branch); break;
                    }
                    break;
                case 2:
                    SetInsParams("j", MIPSInstructionFormats.Jump); break;
                case 3:
                    SetInsParams("jal", MIPSInstructionFormats.Jump); break;
                case 4:
                    SetInsParams("beq", MIPSInstructionFormats.BranchTarget); break;
                case 5:
                    SetInsParams("bne", MIPSInstructionFormats.BranchTarget); break;
                case 6:
                    SetInsParams("blez", MIPSInstructionFormats.Branch); break;
                case 7:
                    SetInsParams("bgtz", MIPSInstructionFormats.Branch); break;
                case 8:
                    SetInsParams("addi", MIPSInstructionFormats.ArithmeticLogicImmediate); break;
                case 9:
                    SetInsParams("addiu", MIPSInstructionFormats.ArithmeticLogicImmediate); break;
                case 10:
                    SetInsParams("slti", MIPSInstructionFormats.ArithmeticLogicImmediate); break;
                case 11:
                    SetInsParams("sltiu", MIPSInstructionFormats.ArithmeticLogicImmediate); break;
                case 12:
                    SetInsParams("andi", MIPSInstructionFormats.ArithmeticLogicImmediate); break;
                case 13:
                    SetInsParams("ori", MIPSInstructionFormats.ArithmeticLogicImmediate); break;
                case 14:
                    SetInsParams("xori", MIPSInstructionFormats.ArithmeticLogicImmediate); break;
                case 15:
                    SetInsParams("lui", MIPSInstructionFormats.LoadUpperImmediate); break;
                case 32:
                    SetInsParams("lb", MIPSInstructionFormats.LoadStore); break;
                case 33:
                    SetInsParams("lh", MIPSInstructionFormats.LoadStore); break;
                case 34:
                    SetInsParams("lwl", MIPSInstructionFormats.LoadStore); break;
                case 35:
                    SetInsParams("lw", MIPSInstructionFormats.LoadStore); break;
                case 36:
                    SetInsParams("lbu", MIPSInstructionFormats.LoadStore); break;
                case 37:
                    SetInsParams("lhu", MIPSInstructionFormats.LoadStore); break;
                case 38:
                    SetInsParams("lwr", MIPSInstructionFormats.LoadStore); break;
                case 40:
                    SetInsParams("sb", MIPSInstructionFormats.LoadStore); break;
                case 41:
                    SetInsParams("sh", MIPSInstructionFormats.LoadStore); break;
                case 42:
                    SetInsParams("swl", MIPSInstructionFormats.LoadStore); break;
                case 43:
                    SetInsParams("sw", MIPSInstructionFormats.LoadStore); break;
                case 46:
                    SetInsParams("swr", MIPSInstructionFormats.LoadStore); break;
            }
        }
        
        private string GetMIPSReg(int reg)
        {
            return $"${(MIPSRegisters)reg}";
        }

        public override string Name => mipsname;
        public override string Arguments
        {
            get
            {
                int opcode = Value >> 26 & 0b111111;
                int rs = Value >> 21 & 0b11111;
                int rt = Value >> 16 & 0b11111;
                int rd = Value >> 11 & 0b11111;
                int shamt = Value >> 6 & 0b11111;
                int funct = Value & 0b111111;
                short imm = (short)(Value & 0xFFFF);
                int ofs = Value ^ (opcode << 26);
                switch (mipsformat)
                {
                    case MIPSInstructionFormats.ArithmeticLogic:
                        return $"{GetMIPSReg(rd)},{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                    case MIPSInstructionFormats.ArithmeticLogicImmediate:
                        return $"{GetMIPSReg(rt)},{GetMIPSReg(rs)},{imm.TransformedString()}";
                    case MIPSInstructionFormats.Branch:
                        return $"{GetMIPSReg(rs)},{imm}";
                    case MIPSInstructionFormats.BranchTarget:
                        return $"{GetMIPSReg(rs)},{GetMIPSReg(rt)},{imm}";
                    case MIPSInstructionFormats.Jump:
                        return $"{ofs}";
                    case MIPSInstructionFormats.JumpAndLinkRegister:
                        return $"{GetMIPSReg(rs)},{GetMIPSReg(rd)}";
                    case MIPSInstructionFormats.JumpRegister:
                        return $"{GetMIPSReg(rs)}";
                    case MIPSInstructionFormats.LoadStore:
                        return $"{GetMIPSReg(rt)},{imm.TransformedString()}({GetMIPSReg(rs)})";
                    case MIPSInstructionFormats.LoadUpperImmediate:
                        return $"{GetMIPSReg(rt)},{imm.TransformedString()}";
                    case MIPSInstructionFormats.MoveFrom:
                    case MIPSInstructionFormats.MoveTo:
                        return $"{GetMIPSReg(rd)}";
                    case MIPSInstructionFormats.MultDiv:
                        return $"{GetMIPSReg(rs)},{GetMIPSReg(rt)}";
                    case MIPSInstructionFormats.Shift:
                        return $"{GetMIPSReg(rd)},{GetMIPSReg(rt)},{shamt}";
                    case MIPSInstructionFormats.ShiftVariable:
                        return $"{GetMIPSReg(rd)},{GetMIPSReg(rt)},{GetMIPSReg(rs)}";
                    default:
                        return string.Empty;
                }
            }
        }
        public override string Comment => string.Empty;
        public override string Format => string.Empty;
    }
}
