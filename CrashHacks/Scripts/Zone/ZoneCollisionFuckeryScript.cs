using Crash;

namespace CrashHacks.Scripts.Zone
{
    public sealed class ZoneCollisionFuckeryScript : Script
    {
        public override string Name
        {
            get { return "ZoneCollisionFuckeryScript"; }
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
                for (int i = 0x24;i < entry.Layout.Length;i += 2)
                {
                    short node = BitConv.FromInt16(entry.Layout,i);
                    if (node == 0x35)
                    {
                        //bear level water => nothin'
                        BitConv.ToInt16(entry.Layout,i,0);
                    }
                    else if (node == 0x129)
                    {
                        //water level water => burn your ass
                        BitConv.ToInt16(entry.Layout,i,0x41);
                    }
                    else if (node == 0xC1)
                    {
                        //bear level totem fucks => solid
                        BitConv.ToInt16(entry.Layout,i,3);
                    }
                    else if (node == 0x41)
                    {
                        //burning surfaces => solid
                        BitConv.ToInt16(entry.Layout,i,3);
                    }
                    else if (node == 0x5)
                    {
                        //weird borders on grates/etc => nothin'
                        BitConv.ToInt16(entry.Layout,i,0);
                    }
                }
            }
        }
    }
}
