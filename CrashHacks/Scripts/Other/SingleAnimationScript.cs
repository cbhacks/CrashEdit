using Crash;
using System;

namespace CrashHacks.Scripts.Other
{
    public sealed class SingleAnimationScript : Script
    {
        private Random random;

        public SingleAnimationScript()
        {
            this.random = new Random();
        }

        public override string Name
        {
            get { return "SingleAnimationScript"; }
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
            if (value is AnimationEntry entry)
            {
                int f = random.Next(entry.Frames.Count);
                for (int i = 0;i < entry.Frames.Count;i++)
                {
                    entry.Frames[i] = entry.Frames[f];
                }
            }
        }
    }
}
