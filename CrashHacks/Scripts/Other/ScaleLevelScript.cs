using Crash;

namespace CrashHacks.Scripts.Other
{
    public sealed class ScaleLevelScript : Script
    {
        public override string Name
        {
            get { return "ScaleLevelScript"; }
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
            get { return "other"; }
        }

        public override SupportLevel CheckCompatibility(GameVersion gameversion)
        {
            return SupportLevel.Experimental;
        }

        public override void Run(object value,GameVersion gameversion)
        {
            const double xscale = 1;
            const double yscale = 2;
            const double zscale = 1;
            if (value is SceneryEntry)
            {
                SceneryEntry entry = (SceneryEntry)value;
                BitConv.ToInt32(entry.Info,0,(int)(entry.XOffset * xscale));
                BitConv.ToInt32(entry.Info,4,(int)(entry.YOffset * yscale));
                BitConv.ToInt32(entry.Info,8,(int)(entry.ZOffset * zscale));
                for (int i = 0;i < entry.Vertices.Count;i++)
                {
                    SceneryVertex v = entry.Vertices[i];
                    int x = (int)(v.X * xscale);
                    int y = (int)(v.Y * yscale);
                    int z = (int)(v.Z * zscale);
                    entry.Vertices[i] = new SceneryVertex(x,y,z,v.UnknownX,v.UnknownY,v.UnknownZ);
                }
            }
            else if (value is SceneryEntry)
            {
                SceneryEntry entry = (SceneryEntry)value;
                for (int i = 0;i < entry.Vertices.Count;i++)
                {
                    SceneryVertex v = entry.Vertices[i];
                    int x = (int)(v.X * xscale);
                    int y = (int)(v.Y * yscale);
                    int z = (int)(v.Z * zscale);
                    entry.Vertices[i] = new SceneryVertex(x,y,z,v.UnknownX,v.UnknownY,v.UnknownZ);
                }
            }
        }
    }
}
