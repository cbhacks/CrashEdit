namespace CrashEdit.Crash
{
    public abstract class GObj
    {
        public abstract string Print();
    }

    public sealed class ListObj : GObj
    {
        public List<GObj> Forms { get; set; }

        public ListObj(params GObj[] forms)
        {
            Forms = new(forms);
        }

        public override string Print()
        {
            if (Forms.Count == 0)
            {
                return "()";
            }
            else
            {
                string res = "(";
                bool first = true;
                foreach (GObj obj in Forms)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res += " ";
                    }
                    res += obj.Print();
                }
                return res + ")";
            }
        }

        public string PrettyPrint()
        {
            return Print();
        }
    }

    public sealed class TokenObj(string val) : GObj
    {
        public string Value { get; set; } = val;

        public override string Print()
        {
            return Value;
        }
    }

    public sealed class SymbolObj(string val) : GObj
    {
        public string Value { get; set; } = val;

        public override string Print()
        {
            return "'" + Value;
        }
    }

    public sealed class StringObj(string val) : GObj
    {
        public string Value { get; set; } = val;

        public override string Print()
        {
            return '"' + Value + '"';
        }
    }

    public sealed class NumberObj(int val) : GObj
    {
        public int Value { get; set; } = val;

        public override string Print()
        {
            return Value.TransformedStringLisp();
        }
    }

    public sealed class EmptyObj : GObj
    {
        public override string Print()
        {
            return string.Empty;
        }
    }
}
