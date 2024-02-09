using System;
using System.Collections.Generic;
using System.Reflection;

namespace Crash
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

        private readonly Dictionary<char, GOOLArgument> args;
        private readonly Type type;

        public virtual string GetName()
        {
            var method = type.GetMethod("GetName", BindingFlags.Public | BindingFlags.Static);
            return (string)method.Invoke(null, new object[] { this });
        }

        public virtual string GetFormat()
        {
            var method = type.GetMethod("GetFormat", BindingFlags.Public | BindingFlags.Static);
            return (string)method.Invoke(null, null);
        }

        public virtual string GetComment()
        {
            var method = type.GetMethod("GetComment", BindingFlags.Public | BindingFlags.Static);
            return (string)method.Invoke(null, new object[] { this });
        }

        public virtual string Arguments => GetArguments();
        public GOOLEntry GOOL { get; }
        public Type Type => type;
        public int Value { get; set; }
        public int UnusedArg { get; private set; }
        public int Opcode => Value >> 24 & 0xFF;
        public IDictionary<char, GOOLArgument> Args => args;

        public GOOLInstruction(int value, GOOLEntry gool, Type type)
        {
            this.type = type;
            GOOL = gool;
            Value = value;
            args = new Dictionary<char, GOOLArgument>();
            if (!(this is MIPSInstruction))
                LoadFormat();
        }

        private void LoadFormat()
        {
            // [] means a GOOL ref, () means a process field, valid digits are any letter + 0, 1 and -, and each correspond to one bit. Bitfields must be contiguous. Spaces are removed in parsing.
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

        public string GetArg(char a)
        {
            if (!args.ContainsKey(a))
                throw new ArgumentException($"GetArg: Argument `{a}` not found", "a");
            switch (args[a].Type)
            {
                case GOOLArgumentTypes.Ref:
                    return GetRefVal(args[a].Value);
                case GOOLArgumentTypes.DestRef:
                    return GetDestRefVal(args[a].Value);
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
            if ((val & 0x800) == 0)
            {
                int off = val & 0x3FF;
                if ((val & 0x400) == 0)
                {
                    if (GOOL.Format == 1 && off < GOOL.Data.Length) // external GOOL entries will logically not have local data...
                    {
                        int cval = GOOL.Data[off];
                        if (off < GOOL.EntryCount)
                            return $"({Entry.EIDToEName(cval)})";
                        else
                            return $"({cval.TransformedString()})";
                    }
                    else
                    {
                        if (GOOL.ParentGOOL != null && GOOL.Format == 0 && off < GOOL.ParentGOOL.Data.Length)
                        {
                            int cval = GOOL.ParentGOOL.Data[off];
                            if (off < GOOL.ParentGOOL.EntryCount)
                                return $"({Entry.EIDToEName(cval)})";
                            else
                                return $"({cval.TransformedString()})";
                        }
                        else
                            return $"L({off.TransformedString()})";
                    }
                }
                else
                {
                    if (GOOL.Format == 0 && off < GOOL.Data.Length) // local GOOL entries will logically not have external data...
                    {
                        int cval = GOOL.Data[off];
                        if (off < GOOL.EntryCount)
                            return $"({Entry.EIDToEName(cval)})";
                        else
                            return $"({cval.TransformedString()})";
                    }
                    else
                    {
                        return $"EL({off.TransformedString()})";
                    }
                }
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
                    return string.Format("{0}[{1}]", n >= 0 ? "stack" : "arg", (n < 0 ? -n - 1 : n).TransformedString());
                }
                if (val != 0xBE0)
                {
                    if (val != 0xBF0)
                    {
                        return 0xBF0.TransformedString();
                    }
                    return "[sp2]";
                }
            }
            else
            {
                if ((val & 0x200) == 0)
                {
                    int link = val >> 6 & 0x7;
                    //if (link == 0)
                    //    return ((ObjectFields)(val & 0x3F)).ToString();
                    //else
                    return $"{ObjectFields.self + link}->{(ObjectFields)(val & 0x3F)}";
                }
                if ((val & 0x1FF) == 0x1F)
                    return "[sp]";
                else
                    return ((ObjectFields)(val & 0x1FF)).ToString();
            }
            return "[null]";
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
    }
}
