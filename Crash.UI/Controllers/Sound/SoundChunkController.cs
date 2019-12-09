namespace Crash.UI
{
    public sealed class SoundChunkController : EntryChunkController
    {
        public SoundChunkController(NSFController up,SoundChunk chunk) : base(up,chunk)
        {
            Chunk = chunk;
        }

        public new SoundChunk Chunk { get; }

        public override string ToString() => Properties.Resources.SoundChunkController_Text;
    }
}
