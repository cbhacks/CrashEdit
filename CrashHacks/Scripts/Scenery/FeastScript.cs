using Crash;
using System;

namespace CrashHacks.Scripts.Scenery
{
    public sealed class FeastScript : Script
    {
        public override string Name
        {
            get { return "FeastScript"; }
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
                SceneryEntry entry = (SceneryEntry)value;
                if (entry.Item6.Length < 4)
                    return;
                int feast = 0;
                for (int i = 0;i < entry.Item6.Length;i += 4)
                {
                    int newfeast = BitConv.FromInt32(entry.Item6,i);
                    if (i != 0)
                        BitConv.ToInt32(entry.Item6,i,feast);
                    feast = newfeast;
                }
                BitConv.ToInt32(entry.Item6,0,feast);
            }
        }
    }
}
