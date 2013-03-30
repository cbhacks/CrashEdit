using Crash;
using Crash.Unknown2;

namespace CrashEdit
{
    public sealed class T2ChunkController : EntryChunkController
    {
        private T2Chunk t2chunk;

        public T2ChunkController(NSFController nsfcontroller,T2Chunk t2chunk) : base(nsfcontroller,t2chunk)
        {
            this.t2chunk = t2chunk;
            Node.Text = "T2 Chunk";
            Node.ImageKey = "t2chunk";
            Node.SelectedImageKey = "t2chunk";
        }
        
        public T2Chunk T2Chunk
        {
            get { return t2chunk; }
        }
    }
}
