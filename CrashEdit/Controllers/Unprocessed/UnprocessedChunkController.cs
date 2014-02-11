using Crash;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class UnprocessedChunkController : ChunkController
    {
        private UnprocessedChunk unprocessedchunk;

        public UnprocessedChunkController(NSFController nsfcontroller,UnprocessedChunk unprocessedchunk) : base(nsfcontroller,unprocessedchunk)
        {
            this.unprocessedchunk = unprocessedchunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Unprocessed Chunk (T{0})",unprocessedchunk.Type);
            Node.ImageKey = "unprocessedchunk";
            Node.SelectedImageKey = "unprocessedchunk";
        }

        protected override Control CreateEditor()
        {
            return new MysteryBox(unprocessedchunk.Data);
        }

        public UnprocessedChunk UnprocessedChunk
        {
            get { return unprocessedchunk; }
        }
    }
}
