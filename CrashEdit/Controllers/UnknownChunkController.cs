using Crash;
using Crash.Unknown0;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class UnknownChunkController : ChunkController
    {
        private UnknownChunk unknownchunk;

        public UnknownChunkController(NSFController nsfcontroller,UnknownChunk unknownchunk) : base(nsfcontroller,unknownchunk)
        {
            this.unknownchunk = unknownchunk;
            Node.Text = string.Format("Unknown Chunk (T{0})",unknownchunk.Type);
            Node.ImageKey = "unknownchunk";
            Node.SelectedImageKey = "unknownchunk";
        }

        public UnknownChunk UnknownChunk
        {
            get { return unknownchunk; }
        }
    }
}
