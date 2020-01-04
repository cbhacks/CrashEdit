using System;

namespace Crash
{
    public sealed class ENameComparer : StringComparer
    {
        public override int Compare(string a, string b)
        {
            if (a.Length != 5 || b.Length != 5) throw new ArgumentException("Entry name is not 5 characters long.");
            int s = 0;
            for (int i = 0; i < 5 && s == 0; ++i)
            {
                s = Entry.ENameCharacterSet.IndexOf(a[i]) - Entry.ENameCharacterSet.IndexOf(b[i]);
            }
            return s;
        }

        public override int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }

        public override bool Equals(string x, string y)
        {
            return x == y;
        }
    }
}
