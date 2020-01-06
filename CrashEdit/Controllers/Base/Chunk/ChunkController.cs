using Crash;

namespace CrashEdit
{
    public abstract class ChunkController : Controller
    {
        public ChunkController(NSFController nsfcontroller,Chunk chunk)
        {
            NSFController = nsfcontroller;
            Chunk = chunk;
            AddMenu(Crash.UI.Properties.Resources.ChunkController_AcDelete,Menu_Delete_Chunk);
            if (!(this is UnprocessedChunkController))
            {
                AddMenu(Crash.UI.Properties.Resources.ChunkController_AcDeprocess,Menu_Unprocess_Chunk);
            }
        }

        public NSFController NSFController { get; }
        public Chunk Chunk { get; }

        private void Menu_Delete_Chunk()
        {
            NSFController.NSF.Chunks.Remove(Chunk);
            Dispose();
        }

        private void Menu_Unprocess_Chunk()
        {
            var trv = Node.TreeView;
            trv.BeginUpdate();
            int index = NSFController.NSF.Chunks.IndexOf(Chunk);
            UnprocessedChunk unprocessedchunk = Chunk.Unprocess(index * 2 + 1);
            NSFController.NSF.Chunks[index] = unprocessedchunk;
            UnprocessedChunkController unprocessedchunkcontroller = new UnprocessedChunkController(NSFController, unprocessedchunk);
            NSFController.InsertNode(index, unprocessedchunkcontroller);
            if (Node.IsSelected)
            {
                Node.TreeView.SelectedNode = unprocessedchunkcontroller.Node;
            }
            Dispose();
            trv.EndUpdate();
        }
    }
}
