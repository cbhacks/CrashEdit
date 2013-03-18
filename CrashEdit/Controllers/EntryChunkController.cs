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
                    AddNode(new T1EntryController(this,(T1Entry)entry));
                }
                else if (entry is T2Entry)
                {
                    AddNode(new T2EntryController(this,(T2Entry)entry));
                }
                else if (entry is T3Entry)
                {
                    AddNode(new T3EntryController(this,(T3Entry)entry));
                }
                else if (entry is T4Entry)
                {
                    AddNode(new T4EntryController(this,(T4Entry)entry));
                }
                else if (entry is EntityEntry)
                {
                    AddNode(new EntityEntryController(this,(EntityEntry)entry));
                }
                else if (entry is T11Entry)
                {
                    AddNode(new T11EntryController(this,(T11Entry)entry));
                }
                else if (entry is SoundEntry)
                {
                    AddNode(new SoundEntryController(this,(SoundEntry)entry));
                }
                else if (entry is MusicEntry)
                {
                    AddNode(new MusicEntryController(this,(MusicEntry)entry));
                }
                else if (entry is WavebankEntry)
                {
                    AddNode(new WavebankEntryController(this,(WavebankEntry)entry));
                }
                else if (entry is T15Entry)
                {
                    AddNode(new T15EntryController(this,(T15Entry)entry));
                }
                else if (entry is T17Entry)
                {
                    AddNode(new T17EntryController(this,(T17Entry)entry));
                }
                else if (entry is DemoEntry)
                {
                    AddNode(new DemoEntryController(this,(DemoEntry)entry));
                }
                else if (entry is SpeechEntry)
                {
                    AddNode(new SpeechEntryController(this,(SpeechEntry)entry));
                }
                else if (entry is T21Entry)
                {
                    AddNode(new T21EntryController(this,(T21Entry)entry));
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
