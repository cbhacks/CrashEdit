using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public abstract class ChunkController : LegacyController
    {
        public ChunkController(Chunk chunk, SubcontrollerGroup parentGroup) : base(parentGroup, chunk)
        {
            Chunk = chunk;
            if (!(this is UnprocessedChunkController))
            {
                AddMenu(CrashUI.Properties.Resources.ChunkController_AcDeprocess, Menu_Unprocess_Chunk);
            }
        }

        protected NSFController NSFController => (NSFController)Modern.Parent.Legacy;
        public Chunk Chunk { get; }

        private void Menu_Unprocess_Chunk()
        {
            int index = NSFController.NSF.Chunks.IndexOf(Chunk);
            UnprocessedChunk unprocessedchunk = Chunk.Unprocess();
            NSFController.NSF.Chunks[index] = unprocessedchunk;
        }
    }
}
