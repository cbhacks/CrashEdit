using Crash;
using Crash.Game;
using Crash.Graphics;
using Crash.Audio;
using Crash.Unknown0;

namespace CrashEdit
{
    public sealed class NormalChunkController : Controller
    {
        private NSFController nsfcontroller;
        private NormalChunk chunk;

        public NormalChunkController(NSFController nsfcontroller,NormalChunk chunk)
        {
            this.nsfcontroller = nsfcontroller;
            this.chunk = chunk;
            Node.Text = "Normal Chunk";
            Node.ImageKey = "normalchunk";
            Node.SelectedImageKey = "normalchunk";
            foreach (Entry entry in chunk.Entries)
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
                else if (entry is MusicEntry)
                {
                    AddNode(new MusicEntryController(this,(MusicEntry)entry));
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
    }
}
