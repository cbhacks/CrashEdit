using Crash;
using System;

namespace CrashHacks.Scripts.Zone
{
    public sealed class NoWarpBarriersScript : Script
    {
        public override string Name
        {
            get { return "NoWarpBarriersScript"; }
        }

        /*public override string Description
        {
            get { return "Clears out a camera distance field, bringing the camera significantly closer to the player."; }
        }*/

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
                case GameVersion.Crash1:
                case GameVersion.Crash1BetaMAR08:
                case GameVersion.Crash1BetaMAY11:
                case GameVersion.Crash2:
                    return SupportLevel.Unsupported;
                case GameVersion.Crash3:
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
                foreach (Entity entity in entry.Entities)
                {
                    if (entity.Name != null && entity.Name.StartsWith("obj_barrier"))
                    {
                        entity.Type = 63;
                    }
                }
            }
        }
    }
}
