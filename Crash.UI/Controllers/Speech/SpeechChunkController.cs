namespace Crash.UI
{
    public sealed class SpeechChunkController : EntryChunkController
    {
        private SpeechChunk chunk;

        public SpeechChunkController(NSFController up,SpeechChunk chunk) : base(up,chunk)
        {
            this.chunk = chunk;
        }

        public new SpeechChunk Chunk
        {
            get { return chunk; }
        }

        public override string ToString()
        {
            return Properties.Resources.SpeechChunkController_Text;
        }
    }
}
