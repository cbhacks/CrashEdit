using Crash;

namespace CrashEdit
{
    public abstract class ChunkController : Controller
    {
        public ChunkController(NSFController nsfcontroller,Chunk chunk)
        {
            NSFController = nsfcontroller;
            Chunk = chunk;
            AddMenu("Delete Chunk",Menu_Delete_Chunk);
        }

        public NSFController NSFController { get; }
        public Chunk Chunk { get; }

        private void Menu_Delete_Chunk()
        {
            NSFController.NSF.Chunks.Remove(Chunk);
            Dispose();
        }
    }
}
