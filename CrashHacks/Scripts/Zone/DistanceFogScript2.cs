using Crash;

namespace CrashHacks.Scripts.Zone
{
    public sealed class DistanceFogScript2 : Script
    {
        public override string Name
        {
            get { return "DistanceFogScript (add new props)"; }
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
                /*entry.Header[0x2AC] = 0x7F;
                entry.Header[0x2AD] = 0x0;
                entry.Header[0x2AE] = 0x0;
                entry.Header[0x2B0] = 0x0;
                entry.Header[0x2B1] = 0x7F;
                entry.Header[0x2B2] = 0x0;*/
                for (int i = 0;i < entry.Entities.Count;i++)
                {
                    Entity entity = entry.Entities[i];
                    if (i % 3 == 1 && entity.Name == null && !entity.ExtraProperties.ContainsKey(0x185))
                    {
                        EntityPropertyRow<uint> row = new EntityPropertyRow<uint>();
                        row.Values.Add(0x20);
                        row.MetaValue = 0;
                        entity.ExtraProperties[0x185] = new EntityUInt32Property(new EntityPropertyRow<uint>[] {row,row});
                    }
                    if (entity.LoadListA != null)
                    {
                        foreach (EntityPropertyRow<int> llrow in entity.LoadListA.Rows)
                        {
                            llrow.Values.Remove(0x7492AACD);
                            llrow.Values.Remove(0x6C8946CD);
                            llrow.Values.Remove(0x4E938CCD);
                            llrow.Values.Remove(0x4AC42E4D);
                            llrow.Values.Remove(0x66536ECD);
                        }
                    }
                }
            }
        }
    }
}
