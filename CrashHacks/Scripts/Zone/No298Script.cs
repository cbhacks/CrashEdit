using Crash;

namespace CrashHacks.Scripts.Zone
{
    public sealed class No298Script : Script
    {
        public override string Name
        {
            get { return "No298Script"; }
        }

        /*public override string Description
        {
            get { return "Disables the darkness effect used in levels such as Totally Fly and Totally Bear."; }
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
            return SupportLevel.Experimental;
        }

        public override void Run(object value,GameVersion gameversion)
        {
            if (value is ZoneEntry)
            {
                ZoneEntry entry = (ZoneEntry)value;
                foreach (Entity entity in entry.Entities)
                {
                    entity.ExtraProperties.Remove(0x142);
                }
            }
        }
    }
}
