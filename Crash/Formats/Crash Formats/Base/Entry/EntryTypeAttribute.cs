using System;

namespace Crash
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EntryTypeAttribute : Attribute
    {
        private int type;

        public EntryTypeAttribute(int type)
        {
            this.type = type;
        }

        public int Type
        {
            get { return type; }
        }
    }
}
