using Crash;
using System;
using System.Collections.Generic;

namespace CrashHacks.Scripts.Animation
{
    public sealed class ReverseScript : Script
    {
        public override string Name
        {
            get { return "Reverse all object animations"; }
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
            get { return "animation"; }
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
                List<Frame> frames = new List<Frame>(entry.Frames);
                frames.Reverse();
                entry.Frames.Clear();
                foreach (Frame frame in frames)
                {
                    entry.Frames.Add(frame);
                }
            }
        }
    }
}
