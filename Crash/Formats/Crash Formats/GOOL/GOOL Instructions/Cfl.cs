namespace Crash.GOOLIns
{
    [GOOLInstruction(130, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(130, GameVersion.Crash1BetaMAR08)]
    public sealed class Cfl_95 : GOOLInstruction
    {
        public Cfl_95(int value,GOOLEntry gool) : base(value,gool)
        {
            if (Args['T'].Value == 0)
                Args['I'] = new GOOLArgument(BitConv.SignExtendInt32(Args['I'].Value, 9)); // sign-extension
        }

        public override string Name
        {
            get
            {
                switch (Args['T'].Value)
                {
                    case 0:
                        switch (Args['C'].Value)
                        {
                            case 0:
                                return "BRA";
                            case 1:
                                return "BNEZ";
                            case 2:
                                return "BEQZ";
                        }
                        break;
                    case 1:
                        switch (Args['C'].Value)
                        {
                            case 0:
                                return "CST";
                            case 1:
                                return "CNEZ";
                            case 2:
                                return "CEQZ";
                        }
                        break;
                    case 2:
                        switch (Args['C'].Value)
                        {
                            case 0:
                                return "RET";
                            case 1:
                                return "RNEZ";
                            case 2:
                                return "REQZ";
                        }
                        break;
                }
                return "NOP";
            }
        }
        public override string Format => "IIIIIIIII VVVVV (RRRRRR) CC TT";
        public override string Comment
        {
            get
            {
                int v = Args['V'].Value;
                int i = Args['I'].Value;
                string str = string.Empty;
                switch (Args['C'].Value)
                {
                    case 1:
                        str = $"if {(ObjectFields)Args['R'].Value} is true, ";
                        break;
                    case 2:
                        str = $"if {(ObjectFields)Args['R'].Value} is false, ";
                        break;
                }
                switch (Args['T'].Value)
                {
                    case 0:
                        if (v != 0 && i != 0)
                        {
                            return str + $"move {i} instructions and pop {v} values";
                        }
                        else if (v == 0)
                        {
                            return str + $"move {i} instructions";
                        }
                        else
                        {
                            return str + $"pop {v} values";
                        }
                    case 1:
                        return str + $"change to state {GetArg('I')}"+ (v > 0 ? $" with {GetArg('V')} arguments" : string.Empty);
                    case 2:
                        return str + "return";
                }
                return "NOP";
            }
        }
    }

    [GOOLInstruction(130,GameVersion.Crash1)]
    [GOOLInstruction(130,GameVersion.Crash1BetaMAY11)]
    public sealed class Cfl : GOOLInstruction
    {
        public Cfl(int value,GOOLEntry gool) : base(value,gool)
        {
            if (Args['T'].Value == 0)
                Args['I'] = new GOOLArgument(BitConv.SignExtendInt32(Args['I'].Value, 10)); // sign-extension
        }

        public override string Name
        {
            get
            {
                switch (Args['T'].Value)
                {
                    case 0:
                        switch (Args['C'].Value)
                        {
                            case 0:
                                return "BRA";
                            case 1:
                                return "BNEZ";
                            case 2:
                                return "BEQZ";
                        }
                        break;
                    case 1:
                        switch (Args['C'].Value)
                        {
                            case 0:
                                return "CST";
                            case 1:
                                return "CNEZ";
                            case 2:
                                return "CEQZ";
                        }
                        break;
                    case 2:
                        switch (Args['C'].Value)
                        {
                            case 0:
                                return "RET";
                            case 1:
                                return "RNEZ";
                            case 2:
                                return "REQZ";
                        }
                        break;
                }
                return "NOP";
            }
        }
        public override string Format => "IIIIIIIIII VVVV (RRRRRR) CC TT";
        public override string Comment
        {
            get
            {
                int v = Args['V'].Value;
                int i = Args['I'].Value;
                string str = string.Empty;
                switch (Args['C'].Value)
                {
                    case 1:
                        str = $"if {(ObjectFields)Args['R'].Value} is true, ";
                        break;
                    case 2:
                        str = $"if {(ObjectFields)Args['R'].Value} is false, ";
                        break;
                }
                switch (Args['T'].Value)
                {
                    case 0:
                        if (v != 0 && i != 0)
                        {
                            return str + $"move {i} instructions and pop {v} values";
                        }
                        else if (v == 0)
                        {
                            return str + $"move {i} instructions";
                        }
                        else
                        {
                            return str + $"pop {v} values";
                        }
                    case 1:
                        return str + $"change to state {GetArg('I')}"+ (v > 0 ? $" with {GetArg('V')} arguments" : string.Empty);
                    case 2:
                        return str + "return";
                }
                return "NOP";
            }
        }
    }
}
