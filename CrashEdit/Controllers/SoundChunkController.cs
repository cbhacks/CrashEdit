using Crash;
using Crash.Audio;

namespace CrashEdit
{
    public sealed class SoundChunkController : Controller
    {
        private NSFController nsfcontroller;
        private SoundChunk chunk;

        public SoundChunkController(NSFController nsfcontroller,SoundChunk chunk)
        {
            this.nsfcontroller = nsfcontroller;
            this.chunk = chunk;
            Node.Text = "Sound Chunk";
            Node.ImageKey = "soundchunk";
            Node.SelectedImageKey = "soundchunk";
            foreach (SoundEntry entry in chunk.Entries)
            {
                AddNode(new LegacyController(entry));
            }
        }
    }
}
