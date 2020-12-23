using Crash;
using System;
using System.Collections.Generic;

namespace CrashHacks.Scripts.Animation
{
    public sealed class ShuffleScript : Script
    {
        private Random random;

        public ShuffleScript()
        {
            this.random = new Random();
        }

        public override string Name
        {
            get { return "Randomly swap around object animations"; }
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
                case GameVersion.Crash3:
                    return SupportLevel.Unsupported;
                default:
                    return SupportLevel.Untested;
            }
        }

        public override void Run(object value,GameVersion gameversion)
        {
            if (value is NSF nsf)
            {
                List<AnimationEntry> entries = nsf.GetEntries<AnimationEntry>();
                List<AnimationEntry> sourceentries = new List<AnimationEntry>();
                for (int i = 0;i < entries.Count;i++)
                {
                    sourceentries.Insert(random.Next(i),new AnimationEntry(entries[i].Frames,entries[i].IsNew,entries[i].EID));
                }
                foreach (AnimationEntry entry in entries)
                {
                    for (int i = 0;i < sourceentries.Count;i++)
                    {
                        AnimationEntry sourceentry = sourceentries[i];
                        if (sourceentry.Frames.Count == entry.Frames.Count && sourceentry.Save().Length <= entry.Save().Length)
                        {
                            entry.Frames.Clear();
                            foreach (Frame frame in sourceentry.Frames)
                            {
                                entry.Frames.Add(frame);
                            }
                            sourceentries.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }
    }
}
