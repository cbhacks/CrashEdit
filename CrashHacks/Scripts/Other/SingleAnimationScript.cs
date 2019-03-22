using Crash;

namespace CrashHacks.Scripts.Other
{
    public sealed class SingleAnimationScript : Script
    {
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
            if (value is T1Entry)
            {
                T1Entry entry = (T1Entry)value;
                byte[] data = System.IO.File.ReadAllBytes(@"U:\T1.bin");
                for (int i = 0;i < entry.Items.Count;i++)
                {
                    if (entry.Items[i].Length >= data.Length)
                    {
                        entry.Items[i] = data;
                    }
                }
            }
        }
    }
}
