using Crash;

namespace CrashEdit
{
    public sealed class WavebankChunkController : EntryChunkController
    {
        public WavebankChunkController(NSFController nsfcontroller,WavebankChunk wavebankchunk) : base(nsfcontroller,wavebankchunk)
        {
            WavebankChunk = wavebankchunk;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Wavebank Chunk {0}", NSFController.NSF.Chunks.IndexOf(WavebankChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "musicred";
            Node.SelectedImageKey = "musicred";
        }

        public WavebankChunk WavebankChunk { get; }
    }
}
