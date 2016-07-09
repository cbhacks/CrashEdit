namespace Crash.UI
{
    public sealed class WavebankChunkController : EntryChunkController
    {
        private WavebankChunk chunk;

        public WavebankChunkController(NSFController up,WavebankChunk chunk) : base(up,chunk)
        {
            this.chunk = chunk;
        }

        public new WavebankChunk Chunk
        {
            get { return chunk; }
        }

        public override string ToString()
        {
            return Properties.Resources.WavebankChunkController_Text;
        }
    }
}
