using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class TextureChunkController : ChunkController
    {
        private TextureChunk texturechunk;

        public TextureChunkController(NSFController nsfcontroller,TextureChunk texturechunk) : base(nsfcontroller,texturechunk)
        {
            this.texturechunk = texturechunk;
            AddMenu("Recalculate Checksum",Menu_Recalculate_Checksum);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Texture Chunk {1} ({0})",Entry.EIDToEName(texturechunk.EID),NSFController.chunkid);
            Node.ImageKey = "image";
            Node.SelectedImageKey = "image";
        }

        protected override Control CreateEditor()
        {
            // Hack for Mono so it doesn't crash.
            if (Type.GetType("Mono.Runtime") != null)
                return base.CreateEditor();
            return new TextureChunkBox(texturechunk);
        }

        public TextureChunk TextureChunk
        {
            get { return texturechunk; }
        }

        private void Menu_Recalculate_Checksum()
        {
            int current_checksum = BitConv.FromInt32(texturechunk.Data, 12);
            int correct_checksum = Chunk.CalculateChecksum(texturechunk.Data);
            if (current_checksum == correct_checksum)
            {
                MessageBox.Show("Checksum was already correct.");
                return;
            }
            BitConv.ToInt32(texturechunk.Data, 12, correct_checksum);
            MessageBox.Show("Checksum was incorrect and has been corrected.");
        }
    }
}
