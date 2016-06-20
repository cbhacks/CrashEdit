namespace Crash.UI
{
    public abstract class ChunkController : Controller
    {
        private NSFController up;
        private Chunk chunk;

        public ChunkController(NSFController up,Chunk chunk)
        {
            this.up = up;
            this.chunk = chunk;
        }

        public Chunk Chunk
        {
            get { return chunk; }
        }
    }
}
