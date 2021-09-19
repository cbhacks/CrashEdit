using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public abstract class ChunkController : LegacyController
    {
        public ChunkController(NSFController nsfcontroller,Chunk chunk) : base(nsfcontroller, chunk)
        {
            NSFController = nsfcontroller;
            Chunk = chunk;
            AddMenu(CrashUI.Properties.Resources.ChunkController_AcDelete,Menu_Delete_Chunk);
            if (!(this is UnprocessedChunkController))
            {
                AddMenu(CrashUI.Properties.Resources.ChunkController_AcDeprocess,Menu_Unprocess_Chunk);
            }
        }

        protected NSFController NSFController { get; }
        public Chunk Chunk { get; }

        private void Menu_Delete_Chunk()
        {
            NSFController.NSF.Chunks.Remove(Chunk);
            RemoveSelf();
        }

        private void Menu_Unprocess_Chunk()
        {
            int index = NSFController.NSF.Chunks.IndexOf(Chunk);
            UnprocessedChunk unprocessedchunk = Chunk.Unprocess(index * 2 + 1);
            NSFController.NSF.Chunks[index] = unprocessedchunk;
            UnprocessedChunkController unprocessedchunkcontroller = new UnprocessedChunkController(NSFController, unprocessedchunk);
            NSFController.InsertNode(index, unprocessedchunkcontroller);
            RemoveSelf();
        }
    }
}
