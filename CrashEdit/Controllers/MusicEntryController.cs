using Crash;
using Crash.Audio;

namespace CrashEdit
{
    public sealed class MusicEntryController : Controller
    {
        private NormalChunkController chunkcontroller;
        private MusicEntry entry;

        public MusicEntryController(NormalChunkController chunkcontroller,MusicEntry entry)
        {
            this.chunkcontroller = chunkcontroller;
            this.entry = entry;
            Node.Text = "Music Entry";
            Node.ImageKey = "musicentry";
            Node.SelectedImageKey = "musicentry";
            foreach (SEQ seq in entry.SEP.SEQs)
            {
                AddNode(new LegacyController(seq));
            }
        }
    }
}
