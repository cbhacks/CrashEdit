using Crash;
using System;

namespace CrashHacks.Scripts.Scenery.Color
{
    public sealed class SwizzlePerMapScript : Script
    {
        private static Random rand = new Random();

        public override string Name
        {
            get { return "Apply random color swizzle (random per-map)."; }
        }

        public override string Category
        {
            get { return "scenery"; }
        }

        public override string Author
        {
            get { return "chekwob"; }
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
            if (value is NSF nsf)
            {
                int r_r = rand.Next(2);
                int r_g = rand.Next(2);
                int r_b = rand.Next(2);
                int r_s = r_r + r_g + r_b;
                int g_r = rand.Next(2);
                int g_g = rand.Next(2);
                int g_b = rand.Next(2);
                int g_s = g_r + g_g + g_b;
                int b_r = rand.Next(2);
                int b_g = rand.Next(2);
                int b_b = rand.Next(2);
                int b_s = b_r + b_g + b_b;

                if (r_s == 0) r_s = 1;
                if (g_s == 0) g_s = 1;
                if (b_s == 0) b_s = 1;
                foreach (SceneryEntry entry in nsf.GetEntries<SceneryEntry>())
                {
                    for (int i = 0;i < entry.Colors.Count;i++)
                    {
                        SceneryColor color = entry.Colors[i];
                        int r = color.Red;
                        int g = color.Green;
                        int b = color.Blue;
                        entry.Colors[i] = new SceneryColor(
                            (byte)((r_r * r + r_g * g + r_b * b) / r_s),
                            (byte)((g_r * r + g_g * g + g_b * b) / g_s),
                            (byte)((b_r * r + b_g * g + b_b * b) / b_s),
                            color.Extra
                        );
                    }
                }
            }
        }
    }
}
