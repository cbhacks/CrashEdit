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
            if (value is NSF)
            {
                NSF nsf = (NSF)value;
                List<T1Entry> entries = new List<T1Entry>();
                foreach (Chunk chunk in nsf.Chunks)
                {
                    if (chunk is EntryChunk)
                    {
                        EntryChunk entrychunk = (EntryChunk)chunk;
                        foreach (Entry entry in entrychunk.Entries)
                        {
                            if (entry is T1Entry)
                            {
                                entries.Add((T1Entry)entry);
                            }
                        }
                    }
                }
                List<T1Entry> sourceentries = new List<T1Entry>();
                for (int i = 0;i < entries.Count;i++)
                {
                    sourceentries.Insert(random.Next(i),new T1Entry(entries[i].Items,entries[i].EID));
                }
                foreach (T1Entry entry in entries)
                {
                    for (int i = 0;i < sourceentries.Count;i++)
                    {
                        T1Entry sourceentry = sourceentries[i];
                        if (sourceentry.Items.Count == entry.Items.Count && sourceentry.Save().Length <= entry.Save().Length)
                        {
                            entry.Items.Clear();
                            foreach (byte[] item in sourceentry.Items)
                            {
                                entry.Items.Add(item);
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
