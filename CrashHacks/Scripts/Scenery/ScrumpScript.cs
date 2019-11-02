using Crash;
using System;

namespace CrashHacks.Scripts.Scenery
{
    public sealed class ScrumpScript : Script
    {
        public override string Name
        {
            get { return "ScrumpScript"; }
        }
        public override string Author
        {
            get { return "chekwob"; }
        }

        public override string Category
        {
            get { return "scenery"; }
        }

        public override SupportLevel CheckCompatibility(GameVersion gameversion)
        {
            return SupportLevel.Experimental;
        }

        public override void Run(object value,GameVersion gameversion)
        {
            if (value is SceneryEntry)
            {
                UnprocessedEntry entry = ((SceneryEntry)value).Unprocess();
                if (entry.Items[3].Length < 11)
                    return;
                byte[] scrump = new byte [11];
                for (int i = 0;i + 11 <= entry.Items[3].Length;i += 11)
                {
                    byte[] newscrump = new byte [11];
                    Array.Copy(entry.Items[3], i,newscrump,0,11);
                    if (i != 0)
                        scrump.CopyTo(entry.Items[3], i);
                    scrump = newscrump;
                }
                scrump.CopyTo(entry.Items[3], 0);
                value = entry.Process(gameversion);
            }
        }
    }
}
