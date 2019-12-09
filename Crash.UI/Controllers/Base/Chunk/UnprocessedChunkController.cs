namespace Crash.UI
{
    public sealed class UnprocessedChunkController : ChunkController
    {
        public UnprocessedChunkController(NSFController up,UnprocessedChunk chunk) : base(up,chunk)
        {
            Chunk = chunk;
        }

        public new UnprocessedChunk Chunk { get; }

        public override string ToString() => Properties.Resources.UnprocessedChunkController_Text;

        private sealed class AcProcess : Action<UnprocessedChunkController>
        {
            protected override string GetText(UnprocessedChunkController c) => Properties.Resources.UnprocessedChunkController_AcProcess;

            protected override Command Activate(UnprocessedChunkController c)
            {
                int index = c.Up.NSF.Chunks.IndexOf(c.Chunk);
                Chunk processedchunk = c.Chunk.Process(index * 2 + 1);
                return c.Up.NSF.Chunks.CmSet(index,processedchunk);
            }
        }
    }
}
