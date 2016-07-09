namespace Crash.UI
{
    public sealed class SoundChunkController : EntryChunkController
    {
        private SoundChunk chunk;

        public SoundChunkController(NSFController up,SoundChunk chunk) : base(up,chunk)
        {
            this.chunk = chunk;
        }

        public new SoundChunk Chunk
        {
            get { return chunk; }
        }

        public override string ToString()
        {
            return Properties.Resources.SoundChunkController_Text;
        }
    }
}
