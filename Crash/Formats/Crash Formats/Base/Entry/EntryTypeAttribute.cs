using System;

namespace Crash
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public sealed class EntryTypeAttribute : Attribute
    {
        private int type;
        private GameVersion gameversion;

        public EntryTypeAttribute(int type,GameVersion gameversion)
        {
            this.type = type;
            this.gameversion = gameversion;
        }

        public int Type
        {
            get { return type; }
        }

        public GameVersion GameVersion
        {
            get { return gameversion; }
        }
    }
}
