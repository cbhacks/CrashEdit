using Crash;

namespace CrashEdit
{
    public sealed class WavebankChunkController : EntryChunkController
    {
        private WavebankChunk wavebankchunk;

        public WavebankChunkController(NSFController nsfcontroller,WavebankChunk wavebankchunk) : base(nsfcontroller,wavebankchunk)
        {
            this.wavebankchunk = wavebankchunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Wavebank Chunk";
            Node.ImageKey = "wavebankchunk";
            Node.SelectedImageKey = "wavebankchunk";
        }

        public WavebankChunk WavebankChunk
        {
            get { return wavebankchunk; }
        }
    }
}
