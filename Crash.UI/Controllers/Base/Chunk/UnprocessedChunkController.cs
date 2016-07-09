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

        private sealed class AcProcess : Action<UnprocessedChunkController>
        {
            protected override string GetText(UnprocessedChunkController c)
            {
                return Properties.Resources.UnprocessedChunkController_AcProcess;
            }

            protected override Command Activate(UnprocessedChunkController c)
            {
                int index = c.Up.NSF.Chunks.IndexOf(c.chunk);
                Chunk processedchunk = c.chunk.Process(index * 2 + 1);
                return c.Up.NSF.Chunks.CmSet(index,processedchunk);
            }
        }
    }
}
