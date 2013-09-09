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
            set
            {
                Entry.loaders.Clear();
                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    foreach (EntryTypeAttribute attribute in type.GetCustomAttributes(typeof(EntryTypeAttribute),false))
                    {
                        if (attribute.GameVersion == value)
                        {
                            EntryLoader loader = (EntryLoader)Activator.CreateInstance(type);
                            Entry.loaders.Add(attribute.Type,loader);
                        }
                    }
                }
                gameversion = value;
            }
        }
    }
}
