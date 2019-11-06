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
                if (entry.Colors.Count < 1)
                    return;
                SceneryColor feast = new SceneryColor(0, 0, 0, 0);
                for (int i = 0;i < entry.Colors.Count;++i)
                {
                    SceneryColor newfeast = entry.Colors[i];
                    if (i != 0)
                        entry.Colors[i] = new SceneryColor(feast.Red, feast.Green, feast.Blue, feast.Extra);
                    feast = newfeast;
                }
                entry.Colors[0] = new SceneryColor(feast.Red, feast.Green, feast.Blue, feast.Extra);
            }
        }
    }
}
