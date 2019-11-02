using System;

namespace Crash
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public sealed class EntryTypeAttribute : Attribute
    {
        public EntryTypeAttribute(int type,GameVersion gameversion)
        {
            Type = type;
            GameVersion = gameversion;
        }

        public int Type { get; }

        public GameVersion GameVersion { get; }
    }
}
