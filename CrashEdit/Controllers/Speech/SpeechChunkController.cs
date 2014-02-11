using Crash;

namespace CrashEdit
{
    public sealed class SpeechChunkController : EntryChunkController
    {
        private SpeechChunk speechchunk;

        public SpeechChunkController(NSFController nsfcontroller,SpeechChunk speechchunk) : base(nsfcontroller,speechchunk)
        {
            this.speechchunk = speechchunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
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
