using Crash;

namespace CrashHacks.Scripts.Scenery.Color
{
    public abstract class BaseScript : Script
    {
        public override string Category
        {
            get { return "scenery"; }
        }

        public override SupportLevel CheckCompatibility(GameVersion gameversion)
        {
            switch (gameversion)
            {
                case GameVersion.Crash1:
                case GameVersion.Crash1BetaMAR08:
                case GameVersion.Crash1BetaMAY11:
                    return SupportLevel.Unsupported;
                case GameVersion.Crash2:
                case GameVersion.Crash3:
                    return SupportLevel.Supported;
                default:
                    return SupportLevel.Untested;
            }
        }

        public override void Run(object value,GameVersion gameversion)
        {
            if (value is SceneryEntry)
            {
                SceneryEntry entry = (SceneryEntry)value;
                for (int i = 0;i < entry.Colors.Count;i++)
                {
                    SceneryColor color = entry.Colors[i];
                    byte r = color.Red;
                    byte g = color.Green;
                    byte b = color.Blue;
                    Run(ref r,ref g,ref b);
                    entry.Colors[i] = new SceneryColor(r,g,b,color.Extra);
                }
            }
        }

        public abstract void Run(ref byte r,ref byte g,ref byte b);
    }
}
