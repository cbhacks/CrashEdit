using Crash;

namespace CrashHacks.Scripts.Zone
{
    public sealed class GlobalTestScript : Script
    {
        public override string Name
        {
            get { return "GlobalTestScript"; }
        }

        public override string Description
        {
            get { return base.Description; }
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
            return SupportLevel.Experimental;
        }

        public override void Run(object value,GameVersion gameversion)
        {
            if (value is ZoneEntry)
            {
                ZoneEntry entry = (ZoneEntry)value;
                foreach (Entity entity in entry.Entities)
                {
                    if (entity.ExtraProperties.ContainsKey(0x185))
                    {
                        EntityUInt32Property property = (EntityUInt32Property)entity.ExtraProperties[0x185];
                        foreach (EntityPropertyRow<uint> row in property.Rows)
                        {
                            if (row.Values.Count >= 1)
                                row.Values[0] |= 0x21212121;
                        }
                    }
                }
            }
        }
    }
}
