using Crash;

namespace CrashEdit
{
    public sealed class WavebankChunkController : EntryChunkController
    {
        public WavebankChunkController(NSFController nsfcontroller,WavebankChunk wavebankchunk) : base(nsfcontroller,wavebankchunk)
        {
            WavebankChunk = wavebankchunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Wavebank Chunk {0}", NSFController.chunkid);
            Node.ImageKey = "music";
            Node.SelectedImageKey = "music";
        }

        public WavebankChunk WavebankChunk { get; }
    }
}
