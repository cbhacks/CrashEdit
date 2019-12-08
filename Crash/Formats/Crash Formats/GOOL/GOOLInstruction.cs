using System;
using System.Collections.Generic;

namespace Crash
{
    public abstract class GOOLInstruction
    {
        protected const string DefaultFormat = "[AAAAAAAAAAAA] [BBBBBBBBBBBB]";
        protected const string DefaultFormatLR = "[LLLLLLLLLLLL] [RRRRRRRRRRRR]";
        protected const string DefaultFormatDS = "[DDDDDDDDDDDD] [SSSSSSSSSSSS]";
        protected const string NullFormat = "000001111101";
        protected const int NullRef = 0xBE0;
        protected const int DoubleStackRef = 0xBF0;
        
        private Dictionary<char, GOOLArgument> args;

        public abstract string Name { get; }
        public abstract string Format { get; } // [] means a GOOL ref, () means a process field, valid digits are any letter + 0, 1 and -, and each correspond to one bit. Bitfields must be contiguous. Spaces are removed in parsing.
        public abstract string Comment { get; }
        public virtual string Arguments => GetArguments();
        public GOOLEntry GOOL { get; }
        public int Value { get; }
        public int UnusedArg { get; private set; }
        public int Opcode => Value >> 24 & 0xFF;
        public IDictionary<char, GOOLArgument> Args => args;

        public GOOLInstruction(int value,GOOLEntry gool)
        {
            GOOL = gool;
            Value = value;
            args = new Dictionary<char, GOOLArgument>();
            if (!(this is MIPSInstruction))
                LoadFormat();
        }

        private void LoadFormat()
        {
            args.Clear();
            int vbits = 0;
            int lastv = 0;
            int lastbits = 0;
            char lasta = '\0';
            GOOLArgumentTypes type = GOOLArgumentTypes.None;
            for (int i = 0; i < Format.Length; ++i)
            {
                char c = Format[i];
                if (char.IsWhiteSpace(c) && !char.IsLetter(c) && (c != '-' && c != '0' && c != '1' && c != '[' && c != ']' && c != '(' && c != ')')) continue;
                if (c == '[')
                {
                    if (type != GOOLArgumentTypes.None)
                        ErrorManager.SignalError("GOOLInstruction: Bad format");
                    if (c != lasta && c != '0' && c != '1' && c != '-')
                    {
                        if (vbits - lastbits > 0)
                            args.Add(lasta, new GOOLArgument(lastv));

                        lastbits = vbits;
                        lasta = c;
                        lastv = 0;
                    }
                    type = GOOLArgumentTypes.Ref;
                }
                else if (c == '(')
                {
                    if (type != GOOLArgumentTypes.None)
                        ErrorManager.SignalError("GOOLInstruction: Bad format");
                    if (c != lasta && c != '0' && c != '1' && c != '-')
                    {
                        if (vbits - lastbits > 0)
                            args.Add(lasta, new GOOLArgument(lastv));

                        lastbits = vbits;
                        lasta = c;
                        lastv = 0;
                    }
                    type = GOOLArgumentTypes.ProcessField;
                }
                else if (c == ']')
                {
                    if (type != GOOLArgumentTypes.Ref)
                        ErrorManager.SignalError("GOOLInstruction: Bad format");
                    if (vbits - lastbits < 12)
                    {
                        ErrorManager.SignalError("GOOLInstruction: Bad format, GOOL ref must be at least 12 bits wide");
                    }
                    if (vbits - lastbits > 0)
                        args.Add(lasta, new GOOLArgument(lastv, type));

                    lastbits = vbits;
                    lasta = c;
                    lastv = 0;
                    type = GOOLArgumentTypes.None;
                }
                else if (c == ')')
                {
                    if (type != GOOLArgumentTypes.ProcessField)
                        ErrorManager.SignalError("GOOLInstruction: Bad format");
                    if (vbits - lastbits > 0)
                        args.Add(lasta, new GOOLArgument(lastv, type));

                    lastbits = vbits;
                    lasta = c;
                    lastv = 0;
                    type = GOOLArgumentTypes.None;
                }
                else
                {
                    if (c != lasta && c != '0' && c != '1' && c != '-')
                    {
                        if (vbits - lastbits > 0)
                            args.Add(lasta, new GOOLArgument(lastv, type));

                        lastbits = vbits;
                        lasta = c;
                        lastv = 0;
                    }
                    if (c == '0' || c == '1')
                    {
                        if ((c == '0' && (Value >> vbits & 1) != 0) || (c == '1' && (Value >> vbits & 1) != 1))
                            ErrorManager.SignalIgnorableError("GOOLInstruction: Constant bit had unexpected value.");
                    }
                    else if (c == '-')
                    {
                        UnusedArg |= Value & (1 << vbits);
                    }
                    else
                    {
                        lastv |= (Value >> vbits & 1) << (vbits - lastbits);
                    }
                    ++vbits;
                }
            }
            if (vbits != 24)
            {
                ErrorManager.SignalError("GOOLInstruction: Format has wrong length");
            }
            if (type != GOOLArgumentTypes.None)
            {
                ErrorManager.SignalError("GOOLInstruction: Bad format, ended unexpectedly");
            }
            if (vbits - lastbits > 0 && char.IsLetter(lasta))
            {
                args.Add(lasta, new GOOLArgument(lastv));
            }
        }

        public int Save()
        {
            return Value;
        }

        protected string GetArg(char a)
        {
            if (!args.ContainsKey(a))
                throw new ArgumentException("GetArg: Argument not found", "a");
            switch (args[a].Type)
            {
                case GOOLArgumentTypes.Ref:
                    return GetRefVal(args[a].Value);
                case GOOLArgumentTypes.ProcessField:
                    return ((ObjectFields)args[a].Value).ToString();
                default:
                    return args[a].Value.TransformedString();
            }
        }

        private string GetArguments()
        {
            if (Args.Count == 0) return "\t";
            string finalargs = string.Empty;

            bool multiple = false;
            foreach (char a in Args.Keys)
            {
                if (multiple)
                    finalargs += ",";
                finalargs += GetArg(a);
                multiple = true;
            }

            if (finalargs.Length < 5)
                finalargs += "\t";
            return finalargs;
        }

        private string GetRefVal(int val)
        {
            string r = string.Empty;
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
                    if (GOOL.Format == 1) // external GOOL entries will logically not have local data...
                    {
                        int cval = GOOL.GetConst(val & 0b1111111111);
                        if (cval >= 0x2000000 && (cval & 1) == 1)
                            r = $"({Entry.EIDToEName(cval)})";
                        else
                            r = $"({cval.TransformedString()})";
                    }
                    else
                    {
                        r = $"[pool$({(val & 0b1111111111).TransformedString()})]";
                    }
                }
                else // pool
                {
                    if (GOOL.Format == 0) // local GOOL entries will logically not have external data...
                    {
                        int cval = GOOL.GetConst(val & 0b1111111111);
                        if (cval >= 0x2000000 && (cval & 1) == 1)
                            r = $"({Entry.EIDToEName(cval)})";
                        else
                            r = $"({cval.TransformedString()})";
                    }
                    else
                    {
                        r = $"[ext$({(val & 0b1111111111).TransformedString()})]";
                    }
                }
            }
            else
            {
                int hi1 = val >> 9 & 0b11;
                if (hi1 == 0) // int
                {
                    r = $"{(BitConv.SignExtendInt32(val & 0x1FF, 9) * 0x100).TransformedString()}";
                }
                else if (hi1 == 1)
                {
                    if ((val >> 8 & 1) == 0) // frac
                        r = $"{(BitConv.SignExtendInt32(val, 8) * 0x10).TransformedString()}";
                    else // stack
                    {
                        int n = BitConv.SignExtendInt32(val, 7);
                        r = string.Format("{0}[{1}]", n >= 0 ? "stack" : "arg", (n < 0 ? -n - 1 : n).TransformedString());
                    }
                }
                else if (hi1 == 2) // reg
                {
                    r = $"{ObjectFields.self + (val >> 6 & 0b111)}->{(ObjectFields)(val & 0x3F)}";
                }
                else if (hi1 == 3) // var
                {
                    if (Enum.IsDefined(typeof(ObjectFields), val & 0x1FF))
                    {
                        r = ((ObjectFields)(val & 0x1FF)).ToString();
                    }
                    else
                        r = $"[var${(val & 0x1FF).TransformedString()}]";
                }
                else throw new Exception();
            }
            return r;
        }
    }
}
