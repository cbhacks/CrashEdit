using Crash;
using Crash.Game;
using Crash.Graphics;
using Crash.Audio;
using Crash.Unknown0;

namespace CrashEdit
{
    public abstract class EntryChunkController : ChunkController
    {
        private EntryChunk entrychunk;

        public EntryChunkController(NSFController nsfcontroller,EntryChunk entrychunk) : base(nsfcontroller,entrychunk)
        {
            this.entrychunk = entrychunk;
            foreach (Entry entry in entrychunk.Entries)
            {
                if (entry is T1Entry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is T2Entry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is T3Entry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is T4Entry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is EntityEntry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is T11Entry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is SoundEntry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is MusicEntry)
                {
                    AddNode(new MusicEntryController(this,(MusicEntry)entry));
                }
                else if (entry is WavebankEntry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is T15Entry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is T17Entry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is DemoEntry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is SpeechEntry)
                {
                    AddNode(new LegacyController(entry));
                }
                else if (entry is T21Entry)
                {
                    AddNode(new LegacyController(entry));
                }
                else
                {
                    AddNode(new ErrorController());
                }
            }
        }

        public EntryChunk EntryChunk
        {
            get { return entrychunk; }
        }
    }
}
