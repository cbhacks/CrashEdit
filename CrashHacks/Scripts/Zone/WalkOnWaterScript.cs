using Crash;
using System;
using System.Collections.Generic;

namespace CrashHacks.Scripts.Animation
{
    public sealed class WalkOnWaterScript : Script
    {
        public override string Name
        {
            get { return "Turn water surfaces solid"; }
        }

        public override string Description
        {
            get { return "Changes water surfaces into solid walkable ground."; }
        }

        public override string Author
        {
            get { return "chekwob"; }
        }

        public override string Category
        {
            get { return "zone"; }
        }

        public override SupportLevel CheckCompatibility(GameVersion gameversion)
        {
            switch (gameversion)
            {
                case GameVersion.Crash2:
                    return SupportLevel.Supported;
                default:
                    return SupportLevel.Untested;
            }
        }

        public override void Run(object value,GameVersion gameversion)
        {
            if (value is ZoneEntry)
            {
                ZoneEntry entry = (ZoneEntry)value;
                for (int i = 0x24;i < entry.Layout.Length;i += 2)
                {
                    short node = BitConv.FromInt16(entry.Layout,i);
                    if (node == 0x35)
                    {
                        //bear level water => solid
                        BitConv.ToInt16(entry.Layout,i,3);
                    }
                    else if (node == 0x129)
                    {
                        //water level water => solid
                        BitConv.ToInt16(entry.Layout,i,3);
                    }
                }
            }
        }
    }
}
