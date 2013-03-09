using Crash;
using Crash.Audio;

namespace CrashEdit
{
    public sealed class SpeechChunkController : EntryChunkController
    {
        private SpeechChunk speechchunk;

        public SpeechChunkController(NSFController nsfcontroller,SpeechChunk speechchunk) : base(nsfcontroller,speechchunk)
        {
            this.speechchunk = speechchunk;
            Node.Text = "Speech Chunk";
            Node.ImageKey = "speechchunk";
            Node.SelectedImageKey = "speechchunk";
        }

        public SpeechChunk SpeechChunk
        {
            get { return speechchunk; }
        }
    }
}
