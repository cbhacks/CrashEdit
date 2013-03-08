using Crash;
using Crash.Audio;

namespace CrashEdit
{
    public sealed class SpeechChunkController : Controller
    {
        private NSFController nsfcontroller;
        private SpeechChunk chunk;

        public SpeechChunkController(NSFController nsfcontroller,SpeechChunk chunk)
        {
            this.nsfcontroller = nsfcontroller;
            this.chunk = chunk;
            Node.Text = "Speech Chunk";
            Node.ImageKey = "speechchunk";
            Node.SelectedImageKey = "speechchunk";
            foreach (SpeechEntry entry in chunk.Entries)
            {
                AddNode(new LegacyController(entry));
            }
        }
    }
}
