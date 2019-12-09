namespace Crash.UI
{
    public sealed class WavebankChunkController : EntryChunkController
    {
        public WavebankChunkController(NSFController up,WavebankChunk chunk) : base(up,chunk)
        {
            Chunk = chunk;
        }

        public new WavebankChunk Chunk { get; }

        public override string ToString() => Properties.Resources.WavebankChunkController_Text;
    }
}
