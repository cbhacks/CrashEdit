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
            Node.Text = string.Format("Unprocessed Chunk {1} (T{0})",UnprocessedChunk.Type,NSFController.NSF.Chunks.IndexOf(UnprocessedChunk) * 2 + 1);
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
