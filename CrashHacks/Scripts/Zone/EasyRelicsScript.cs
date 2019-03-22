using Crash;
using System;

namespace CrashHacks.Scripts.Zone
{
    public sealed class EasyRelicsScript : Script
    {
        public override string Name
        {
            get { return "EasyRelicsScript"; }
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
                    if (entity.Name == null || !entity.Name.StartsWith("obj_box"))
                        continue;
                    entity.ExtraProperties.Remove(0x30E);
                    EntityInt32Property property = new EntityInt32Property();
                    EntityPropertyRow<int> row = new EntityPropertyRow<int>();
                    row.Values.Add(0x7100);
                    property.Rows.Add(row);
                    entity.ExtraProperties[0x336] = property;
                    for (int i = 0;i < entity.Positions.Count;i++)
                    {
                        EntityPosition position = entity.Positions[i];
                        short x = (short)(position.X / 4);
                        short y = (short)(position.Y / 4);
                        short z = (short)(position.Z / 4);
                        entity.Positions[i] = new EntityPosition(x,y,z);
                    }
                }
            }
        }
    }
}
