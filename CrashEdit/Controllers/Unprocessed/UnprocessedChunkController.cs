using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class UnprocessedChunkController : ChunkController
    {
        public UnprocessedChunkController(NSFController nsfcontroller,UnprocessedChunk unprocessedchunk) : base(nsfcontroller,unprocessedchunk)
        {
            UnprocessedChunk = unprocessedchunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Unprocessed Chunk (T{0})",UnprocessedChunk.Type);
            Node.ImageKey = "yellowj";
            Node.SelectedImageKey = "yellowj";
        }

        protected override Control CreateEditor()
        {
            return new MysteryBox(UnprocessedChunk.Data);
        }

        public UnprocessedChunk UnprocessedChunk { get; }
    }
}
