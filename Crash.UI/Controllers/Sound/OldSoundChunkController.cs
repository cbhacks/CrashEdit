namespace Crash.UI
{
    public sealed class OldSoundChunkController : EntryChunkController
    {
        private OldSoundChunk chunk;

        public OldSoundChunkController(NSFController up,OldSoundChunk chunk) : base(up,chunk)
        {
            this.chunk = chunk;
        }

        public new OldSoundChunk Chunk
        {
            get { return chunk; }
        }

        public override string ToString()
        {
            return Properties.Resources.OldSoundChunkController_Text;
        }
    }
}
