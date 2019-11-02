using Crash;

namespace CrashEdit
{
    public sealed class SpeechChunkController : EntryChunkController
    {
        public SpeechChunkController(NSFController nsfcontroller,SpeechChunk speechchunk) : base(nsfcontroller,speechchunk)
        {
            SpeechChunk = speechchunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Speech Chunk {0}", NSFController.chunkid);
            Node.ImageKey = "whitej";
            Node.SelectedImageKey = "whitej";
        }

        public SpeechChunk SpeechChunk { get; }
    }
}
