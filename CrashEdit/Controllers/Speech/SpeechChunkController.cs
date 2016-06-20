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
            Node.Text = string.Format("Speech Chunk {0}", NSFController.chunkid);
            Node.ImageKey = "whitej";
            Node.SelectedImageKey = "whitej";
        }

        public SpeechChunk SpeechChunk
        {
            get { return speechchunk; }
        }
    }
}
