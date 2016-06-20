using Crash;

namespace CrashEdit
{
    public abstract class ChunkController : Controller
    {
        private NSFController nsfcontroller;
        private Chunk chunk;

        public ChunkController(NSFController nsfcontroller,Chunk chunk)
        {
            this.nsfcontroller = nsfcontroller;
            this.chunk = chunk;
            AddMenu("Delete Chunk",Menu_Delete_Chunk);
        }

        public NSFController NSFController
        {
            get { return nsfcontroller; }
        }
        
        public Chunk Chunk
        {
            get { return chunk; }
        }

        private void Menu_Delete_Chunk()
        {
            nsfcontroller.NSF.Chunks.Remove(chunk);
            Dispose();
        }
    }
}
