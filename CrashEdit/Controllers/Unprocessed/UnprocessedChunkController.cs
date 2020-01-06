using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class UnprocessedChunkController : ChunkController
    {
        public UnprocessedChunkController(NSFController nsfcontroller,UnprocessedChunk unprocessedchunk) : base(nsfcontroller,unprocessedchunk)
        {
            UnprocessedChunk = unprocessedchunk;
            AddMenu(Crash.UI.Properties.Resources.UnprocessedChunkController_AcProcess,Menu_Process_Chunk);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.UnprocessedChunkController_Text,UnprocessedChunk.Type,NSFController.NSF.Chunks.IndexOf(UnprocessedChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "yellowj";
            Node.SelectedImageKey = "yellowj";
        }

        protected override Control CreateEditor()
        {
            return new MysteryBox(UnprocessedChunk.Data);
        }

        public UnprocessedChunk UnprocessedChunk { get; }

        private void Menu_Process_Chunk()
        {
            Chunk processedchunk;
            try
            {
                processedchunk = UnprocessedChunk.Process(NSFController.NSF.Chunks.IndexOf(UnprocessedChunk) * 2 + 1);
            }
            catch (LoadAbortedException)
            {
                return;
            }
            var trv = Node.TreeView;
            trv.BeginUpdate();
            int index = NSFController.NSF.Chunks.IndexOf(UnprocessedChunk);
            NSFController.NSF.Chunks[index] = processedchunk;
            if (processedchunk is EntryChunk)
            {
                ((EntryChunk)processedchunk).ProcessAll(NSFController.GameVersion);
            }
            ChunkController processedchunkcontroller = NSFController.CreateChunkController(processedchunk);
            NSFController.InsertNode(index, processedchunkcontroller);
            if (Node.IsSelected)
            {
                Node.TreeView.SelectedNode = processedchunkcontroller.Node;
            }
            Dispose();
            trv.EndUpdate();
        }
    }
}
