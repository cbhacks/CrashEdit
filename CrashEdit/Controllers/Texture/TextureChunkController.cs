using CrashEdit.Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class TextureChunkController : ChunkController
    {
        public TextureChunkController(NSFController nsfcontroller,TextureChunk texturechunk) : base(nsfcontroller,texturechunk)
        {
            TextureChunk = texturechunk;
            AddMenu(CrashUI.Properties.Resources.TextureChunkController_AcRecalcChecksum,Menu_Recalculate_Checksum);
            AddMenu(CrashUI.Properties.Resources.TextureChunkController_AcRename,Menu_Rename_Entry);
            AddMenu(CrashUI.Properties.Resources.TextureChunkController_AcOpenViewer,Menu_Open_Viewer);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.TextureChunkController_Text,Entry.EIDToEName(TextureChunk.EID),NSFController.NSF.Chunks.IndexOf(TextureChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "image";
        }

        public override bool EditorAvailable => Type.GetType("Mono.Runtime") == null;

        public override Control CreateEditor()
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
                if (newentrywindow.ShowDialog() == DialogResult.OK)
                {
                    TextureChunk.EID = newentrywindow.EID;
                    InvalidateNode();
                }
            }
        }

        private TextureViewer frmViewer = null;

        private void Menu_Open_Viewer()
        {
            if (frmViewer == null)
            {
                frmViewer = new TextureViewer(TextureChunk);
                frmViewer.FormClosing += delegate (object sender2, FormClosingEventArgs e2)
                {
                    frmViewer = null;
                };
                frmViewer.Show();
            }
            else
                frmViewer.Select();
        }
    }
}
