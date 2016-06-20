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
            Node.Text = string.Format("Normal Chunk {0}",NSFController.chunkid);
            Node.ImageKey = "yellowj";
            Node.SelectedImageKey = "yellowj";
        }
        
        public NormalChunk NormalChunk
        {
            get { return normalchunk; }
        }
    }
}
