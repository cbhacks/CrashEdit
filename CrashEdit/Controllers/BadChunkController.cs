using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class BadChunkController : ChunkController
    {
        private BadChunk badchunk;

        public BadChunkController(NSFController nsfcontroller,BadChunk badchunk) : base(nsfcontroller,badchunk)
        {
            this.badchunk = badchunk;
            Node.Text = "Bad Chunk";
            Node.ImageKey = "badchunk";
            Node.SelectedImageKey = "badchunk";
        }

        protected override Control CreateEditor()
        {
            return new MysteryBox(badchunk.Data);
        }

        public BadChunk BadChunk
        {
            get { return badchunk; }
        }
    }
}
