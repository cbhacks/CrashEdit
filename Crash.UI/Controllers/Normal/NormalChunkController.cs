namespace Crash.UI
{
    public sealed class NormalChunkController : EntryChunkController
    {
        private NormalChunk chunk;

        public NormalChunkController(NSFController up,NormalChunk chunk) : base(up,chunk)
        {
            this.chunk = chunk;
        }

        public new NormalChunk Chunk
        {
            get { return chunk; }
        }

        public override string ToString()
        {
            return Properties.Resources.NormalChunkController_Text;
        }
    }
}
