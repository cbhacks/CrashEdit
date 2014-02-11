using Crash;

namespace CrashEdit
{
    public sealed class NormalChunkController : EntryChunkController
    {
        private NormalChunk normalchunk;

        public NormalChunkController(NSFController nsfcontroller,NormalChunk normalchunk) : base(nsfcontroller,normalchunk)
        {
            this.normalchunk = normalchunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Normal Chunk";
            Node.ImageKey = "normalchunk";
            Node.SelectedImageKey = "normalchunk";
        }
        
        public NormalChunk NormalChunk
        {
            get { return normalchunk; }
        }
    }
}
