using CrashEdit.Crash.GOOLIns;

namespace CrashEdit.Crash
{
    public class GOOLInstruction
    {
        public const string DefaultFormat = "[AAAAAAAAAAAA] [BBBBBBBBBBBB]";
        public const string DefaultFormatLR = "[LLLLLLLLLLLL] [RRRRRRRRRRRR]";
        public const string DefaultFormatDS = "[DDDDDDDDDDDD] [SSSSSSSSSSSS]";
        public const string DefaultFormatDS2 = "{DDDDDDDDDDDD} [SSSSSSSSSSSS]";
        public const string NullFormat = "000001111101";
        public const int NullRef = 0xBE0;
        public const int DoubleStackRef = 0xBF0;
        public const int StackRef = 0xE1F;

        public static bool ParensOnPool = false;

        private readonly Dictionary<char, GOOLArgument> args;
        private readonly GOOLInsOpcode opcode;

        public virtual string GetName()
        {
            return opcode.GetName(this);
        }

        public virtual string GetFormat()
        {
            return opcode.GetFormat();
        }

        public virtual string GetComment()
        {
            return opcode.GetComment(this);
        }

        public int GetStackPop() => opcode.StackPop(this);
        public int GetStackPush() => opcode.StackPush(this);

        public virtual string Arguments => GetArguments();
        public GOOLEntry GOOL { get; }
        public Type Type => opcode.GetType();
        public int Value { get; set; }
        public int UnusedArg { get; private set; }
        public int ID => Value >> 24 & 0xFF;
        public IDictionary<char, GOOLArgument> Args => args;

        public bool DecompFakeInstruction { get; private set; }

        public GOOLInstruction(int value, GOOLEntry gool, Type? type, bool fake = false)
        {
            opcode = type != null ? (GOOLInsOpcode)Activator.CreateInstance(type)! : null;
            GOOL = gool;
            Value = value;
            args = [];
            if (this is not MIPSInstruction)
                LoadFormat();
            DecompFakeInstruction = fake;
        }

        private void LoadFormat()
        {
            // [] means a GOOL ref, () means a process field, valid characters are any letter + 0, 1 and -, and each correspond to one bit. Bitfields must be contiguous. Spaces are removed in parsing.
            args.Clear();
            int vbits = 0;
            int lastv = 0;
            int lastbits = 0;
            char lasta = '\0';
            GOOLArgumentTypes type = GOOLArgumentTypes.None;
            string format = GetFormat();
            for (int i = 0; i < format.Length; ++i)
            {
                char c = format[i];
                if (char.IsWhiteSpace(c) || (!char.IsLetter(c) && (c != '-' && c != '0' && c != '1' && c != '[' && c != ']' && c != '(' && c != ')' && c != '{' && c != '}' && c != '<' && c != '>'))) continue;
                if (c == '[' || c == '{' || c == '(' || c == '<')
                {
                    if (type != GOOLArgumentTypes.None)
                        ErrorManager.SignalError("GOOLInstruction: Bad format");
                    if (c != lasta)
                    {
                        if (vbits - lastbits > 0)
                            args.Add(lasta, new GOOLArgument(lastv));

                        lastbits = vbits;
                        lasta = c;
                        lastv = 0;
                    }
                    switch (c)
                    {
                        case '[': type = GOOLArgumentTypes.Ref; break;
                        case '{': type = GOOLArgumentTypes.DestRef; break;
                        case '(': type = GOOLArgumentTypes.ProcessField; break;
                        case '<': type = GOOLArgumentTypes.Signed; break;
                    }
                }
                else if (c == ']' || c == '}' || c == ')' || c == '>')
                {
                    var want_type = GOOLArgumentTypes.None;
                    switch (c)
                    {
                        case ']': want_type = GOOLArgumentTypes.Ref; break;
                        case '}': want_type = GOOLArgumentTypes.DestRef; break;
                        case ')': want_type = GOOLArgumentTypes.ProcessField; break;
                        case '>': want_type = GOOLArgumentTypes.Signed; break;
                    }

                    if (type != want_type)
                        ErrorManager.SignalError("GOOLInstruction: Bad format");
                    if ((type == GOOLArgumentTypes.Ref || type == GOOLArgumentTypes.DestRef) && vbits - lastbits < 12)
                        ErrorManager.SignalError("GOOLInstruction: Bad format, GOOL ref must be at least 12 bits long");
                    if (vbits - lastbits == 0)
                        ErrorManager.SignalError("GOOLInstruction: Bad format, argument was 0 bits long");

                    if (type == GOOLArgumentTypes.Signed)
                        lastv = BitConv.SignExtendInt32(lastv, vbits - lastbits);
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
                        //if ((c == '0' && (Value >> vbits & 1) != 0) || (c == '1' && (Value >> vbits & 1) != 1))
                        //    ErrorManager.SignalIgnorableError("GOOLInstruction: Constant bit had unexpected value.");
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
                ErrorManager.SignalError($"GOOLInstruction: Bad format, read {vbits} bits instead of 24");
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

        public bool IsStackRef(char a)
        {
            if (!args.TryGetValue(a, out GOOLArgument value))
                throw new ArgumentException($"GetArg: Argument `{a}` not found", nameof(a));
            switch (value.Type)
            {
                case GOOLArgumentTypes.Ref:
                case GOOLArgumentTypes.DestRef:
                    return value.Value == StackRef;
                case GOOLArgumentTypes.ProcessField:
                    return (ObjectFields)value.Value == ObjectFields.sp;
                default:
                    return false;
            }
        }

        public bool IsDoubleStackRef(char a)
        {
            if (!args.TryGetValue(a, out GOOLArgument value))
                throw new ArgumentException($"GetArg: Argument `{a}` not found", nameof(a));
            switch (value.Type)
            {
                case GOOLArgumentTypes.Ref:
                    return value.Value == DoubleStackRef;
                default:
                    return false;
            }
        }

        public bool IsNullRef(char a)
        {
            if (!args.TryGetValue(a, out GOOLArgument value))
                throw new ArgumentException($"GetArg: Argument `{a}` not found", nameof(a));
            switch (value.Type)
            {
                case GOOLArgumentTypes.Ref:
                    return value.Value == NullRef;
                default:
                    return false;
            }
        }

        public string GetArg(char a)
        {
            if (!args.TryGetValue(a, out GOOLArgument value))
                throw new ArgumentException($"GetArg: Argument `{a}` not found", nameof(a));
            switch (value.Type)
            {
                case GOOLArgumentTypes.Ref:
                    return GetRefVal(value.Value);
                case GOOLArgumentTypes.DestRef:
                    return GetDestRefVal(value.Value);
                case GOOLArgumentTypes.ProcessField:
                    return ((ObjectFields)value.Value).ToString();
                default:
                    return value.Value.TransformedString();
            }
        }

        public string GetArgNoHex(char a)
        {
            if (!args.TryGetValue(a, out GOOLArgument value))
                throw new ArgumentException($"GetArg: Argument `{a}` not found", nameof(a));
            switch (value.Type)
            {
                case GOOLArgumentTypes.Ref:
                    return GetRefVal(value.Value);
                case GOOLArgumentTypes.DestRef:
                    return GetDestRefVal(value.Value);
                case GOOLArgumentTypes.ProcessField:
                    return ((ObjectFields)value.Value).ToString();
                default:
                    return value.Value.ToString();
            }
        }

        public bool TryGetImmediate(char a, out int val)
        {
            if (!args.TryGetValue(a, out GOOLArgument value))
                throw new ArgumentException($"GetArg: Argument `{a}` not found", nameof(a));
            switch (value.Type)
            {
                case GOOLArgumentTypes.Ref:
                    return GetRefValImm(value.Value, out val);
                default:
                    val = 0;
                    return false;
            }
        }

        private string GetArguments()
        {
            if (Args.Count == 0) return "";
            string finalargs = string.Empty;

            bool multiple = false;
            foreach (char a in Args.Keys)
            {
                if (multiple)
                    finalargs += ",";
                finalargs += GetArg(a);
                multiple = true;
            }

            return finalargs;
        }

        private string GetRefVal(int val)
        {
            if (val == StackRef)
                return "[sp]";
            if ((val & 0x800) == 0)
            {
                int off = val & 0x3FF;
                int cval;
                if ((val & 0x400) == 0)
                {
                    if (GOOL.Format == 1 && off < GOOL.Data.Length) // external GOOL entries will logically not have local data...
                    {
                        cval = GOOL.Data[off];
                    }
                    else
                    {
                        if (GOOL.ParentGOOL != null && GOOL.Format == 0 && off < GOOL.ParentGOOL.Data.Length)
                        {
                            cval = GOOL.ParentGOOL.Data[off];
                        }
                        else
                            return $"L({off.TransformedString()})";
                    }
                }
                else
                {
                    if (GOOL.Format == 0 && off < GOOL.Data.Length) // local GOOL entries will logically not have external data...
                    {
                        cval = GOOL.Data[off];
                    }
                    else
                    {
                        return $"EL({off.TransformedString()})";
                    }
                }
                if (off < GOOL.EntryCount)
                    return $"({Entry.EIDToEName(cval)})";
                else
                    return ParensOnPool ? $"({cval.TransformedString()})" : cval.TransformedString();
            }
            if ((val & 0x400) == 0)
            {
                if ((val & 0x200) == 0)
                {
                    return $"{(val << 0x17 >> 0xF).TransformedString()}";
                }
                if ((val & 0x100) == 0)
                {
                    return $"{(val << 0x18 >> 0x14).TransformedString()}";
                }
                if ((val & 0x80) == 0)
                {
                    int n = BitConv.SignExtendInt32(val, 7);
                    return string.Format("{0}[{1}]", n >= 0 ? "stack" : "arg", (n < 0 ? -n - 1 : n));
                }
                if (val != 0xBE0)
                {
                    if (val != 0xBF0)
                    {
                        throw new Exception("could not resolve null or double stack reference");
                    }
                    return "[sp-1]";
                }
            }
            else
            {
                if ((val & 0x200) == 0)
                {
                    int link = val >> 6 & 0x7;
                    return $"{ObjectFields.self + link}->{(ObjectFields)(val & 0x3F)}";
                }
                return ((ObjectFields)(val & 0x1FF)).ToString();
            }
            return "[null]";
        }

        private bool GetRefValImm(int val, out int ret)
        {
            if ((val & 0x800) == 0)
            {
                int off = val & 0x3FF;
                if ((val & 0x400) == 0)
                {
                    if (GOOL.Format == 1 && off < GOOL.Data.Length) // external GOOL entries will logically not have local data...
                    {
                        ret = GOOL.Data[off];
                        if (off < GOOL.EntryCount)
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        if (GOOL.ParentGOOL != null && GOOL.Format == 0 && off < GOOL.ParentGOOL.Data.Length)
                        {
                            ret = GOOL.ParentGOOL.Data[off];
                            if (off < GOOL.ParentGOOL.EntryCount)
                                return false;
                            else
                                return true;
                        }
                    }
                }
                else
                {
                    if (GOOL.Format == 0 && off < GOOL.Data.Length) // local GOOL entries will logically not have external data...
                    {
                        ret = GOOL.Data[off];
                        if (off < GOOL.EntryCount)
                            return false;
                        else
                            return true;
                    }
                }
            }
            if ((val & 0x400) == 0)
            {
                if ((val & 0x200) == 0)
                {
                    ret = val << 0x17 >> 0xF;
                    return true;
                }
                if ((val & 0x100) == 0)
                {
                    ret = val << 0x18 >> 0x14;
                    return true;
                }
            }
            ret = 0;
            return false;
        }

        private string GetDestRefVal(int val)
        {
            if ((val & 0x400) == 0)
            {
                int n = BitConv.SignExtendInt32(val, 7);
                return string.Format("{0}[{1}]", n >= 0 ? "stack" : "arg", (n < 0 ? -n - 1 : n).TransformedString());
            }
            else
            {
                if ((val & 0x200) == 0)
                {
                    int link = val >> 6 & 0x7;
                    if (link == 0)
                        return ((ObjectFields)(val & 0x3F)).ToString();
                    else
                        return $"{ObjectFields.self + link}->{(ObjectFields)(val & 0x3F)}";
                }
                if ((val & 0x1FF) == 0x1F)
                    return "[sp]";
                else
                    return ((ObjectFields)(val & 0x1FF)).ToString();
            }
        }

        private GObj GetRefValLisp(int val)
        {
            if (val == StackRef)
                return new SymbolObj("sp");
            if ((val & 0x800) == 0)
            {
                int off = val & 0x3FF;
                int cval;
                if ((val & 0x400) == 0)
                {
                    if (GOOL.Format == 1 && off < GOOL.Data.Length) // external GOOL entries will logically not have local data...
                    {
                        cval = GOOL.Data[off];
                    }
                    else
                    {
                        if (GOOL.ParentGOOL != null && GOOL.Format == 0 && off < GOOL.ParentGOOL.Data.Length)
                        {
                            cval = GOOL.ParentGOOL.Data[off];
                        }
                        else
                            throw new Exception("could not resolve static pool ref in " + GOOL.EName);
                    }
                }
                else
                {
                    if (GOOL.Format == 0 && off < GOOL.Data.Length) // local GOOL entries will logically not have external data...
                    {
                        cval = GOOL.Data[off];
                    }
                    else
                    {
                        throw new Exception("could not resolve external static pool ref in " + GOOL.EName);
                    }
                }
                if (off < GOOL.EntryCount)
                    return new ListObj(new TokenObj("file"), new StringObj(Entry.EIDToEName(cval)));
                else
                    return new NumberObj(cval);
            }
            if ((val & 0x400) == 0)
            {
                if ((val & 0x200) == 0)
                {
                    return new NumberObj(val << 0x17 >> 0xF);
                }
                if ((val & 0x100) == 0)
                {
                    return new NumberObj(val << 0x18 >> 0x14);
                }
                if ((val & 0x80) == 0)
                {
                    int n = BitConv.SignExtendInt32(val, 7);
                    return new TokenObj(string.Format("{0}{1}", n >= 0 ? "stack" : "arg", n < 0 ? -n - 1 : n));
                }
                if (val != 0xBE0)
                {
                    if (val != 0xBF0)
                    {
                        throw new Exception("could not resolve null or double stack reference");
                    }
                    return new SymbolObj("sp-double");
                }
            }
            else
            {
                if ((val & 0x200) == 0)
                {
                    int link = val >> 6 & 0x7;
                    return new ListObj(new TokenObj($"{ObjectFields.self + link}"), new TokenObj($"{(ObjectFields)(val & 0x3F)}"));
                }
                return new TokenObj(((ObjectFields)(val & 0x1FF)).ToString());
            }
            return new SymbolObj("null");
        }

        public GObj ArgToLisp(char a)
        {
            if (!args.TryGetValue(a, out GOOLArgument value))
                throw new ArgumentException($"GetArg: Argument `{a}` not found", nameof(a));
            switch (value.Type)
            {
                case GOOLArgumentTypes.Ref:
                case GOOLArgumentTypes.DestRef:
                    return GetRefValLisp(value.Value);
                case GOOLArgumentTypes.ProcessField:
                    return new TokenObj(((ObjectFields)value.Value).ToString());
                default:
                    return new NumberObj(value.Value);
            }
        }

        public GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            return opcode.DecompileToLisp(statement, ref i);
        }
    }
}
