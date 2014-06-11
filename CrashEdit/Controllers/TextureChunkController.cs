using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class TextureChunkController : ChunkController
    {
        private TextureChunk texturechunk;

        public TextureChunkController(NSFController nsfcontroller,TextureChunk texturechunk) : base(nsfcontroller,texturechunk)
        {
            this.texturechunk = texturechunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Texture Chunk ({0})",Entry.EIDToEName(texturechunk.EID));
            Node.ImageKey = "texturechunk";
            Node.SelectedImageKey = "texturechunk";
        }

        // MONO USERS
        // Comment out this function
        protected override Control CreateEditor()
        {
            return new TextureChunkBox(texturechunk);
        }

        public TextureChunk TextureChunk
        {
            get { return texturechunk; }
        }
    }
}
