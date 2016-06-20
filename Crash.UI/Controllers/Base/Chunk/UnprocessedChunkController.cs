namespace Crash.UI
{
    public sealed class UnprocessedChunkController : ChunkController
    {
        private UnprocessedChunk chunk;

        public UnprocessedChunkController(NSFController up,UnprocessedChunk chunk) : base(up,chunk)
        {
            this.chunk = chunk;
        }

        public new UnprocessedChunk Chunk
        {
            get { return chunk; }
        }

        public override string ToString()
        {
            return Properties.Resources.UnprocessedChunkController_Text;
        }
    }
}
