using System;
using System.Reflection;

namespace Crash
{
    public static class Configuration
    {
        private static GameVersion gameversion;

        static Configuration()
        {
            GameVersion = GameVersion.None;
        }

        public static GameVersion GameVersion
        {
            get { return gameversion; }
            set { gameversion = value; }
        }
    }
}
