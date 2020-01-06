using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class TextureChunkController : ChunkController
    {
        public TextureChunkController(NSFController nsfcontroller,TextureChunk texturechunk) : base(nsfcontroller,texturechunk)
        {
            TextureChunk = texturechunk;
            AddMenu(Crash.UI.Properties.Resources.TextureChunkController_AcRecalcChecksum,Menu_Recalculate_Checksum);
            AddMenu(Crash.UI.Properties.Resources.TextureChunkController_AcRename,Menu_Rename_Entry);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.TextureChunkController_Text,Entry.EIDToEName(TextureChunk.EID),NSFController.NSF.Chunks.IndexOf(TextureChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "image";
            Node.SelectedImageKey = "image";
        }

        protected override Control CreateEditor()
        {
            // Hack for Mono so it doesn't crash.
            if (Type.GetType("Mono.Runtime") != null)
                return base.CreateEditor();
            return new TextureChunkBox(TextureChunk);
        }

        public TextureChunk TextureChunk { get; }

        private void Menu_Recalculate_Checksum()
        {
            int current_checksum = BitConv.FromInt32(TextureChunk.Data, 12);
            int correct_checksum = Chunk.CalculateChecksum(TextureChunk.Data);
            if (current_checksum == correct_checksum)
            {
                MessageBox.Show("Checksum was already correct.");
                return;
            }
            BitConv.ToInt32(TextureChunk.Data, 12, correct_checksum);
            MessageBox.Show("Checksum was incorrect and has been corrected.");
        }

        private void Menu_Rename_Entry()
        {
            using (NewEntryForm newentrywindow = new NewEntryForm(NSFController))
            {
                newentrywindow.Text = "Rename Entry";
                newentrywindow.SetRenameMode(TextureChunk.EName);
                if (newentrywindow.ShowDialog(Node.TreeView.TopLevelControl) == DialogResult.OK)
                {
                    TextureChunk.EID = newentrywindow.EID;
                    InvalidateNode();
                }
            }
        }
    }
}
