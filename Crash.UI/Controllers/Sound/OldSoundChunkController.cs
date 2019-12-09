namespace Crash.UI
{
    public sealed class OldSoundChunkController : EntryChunkController
    {
        public OldSoundChunkController(NSFController up,OldSoundChunk chunk) : base(up,chunk)
        {
            Chunk = chunk;
        }

        public new OldSoundChunk Chunk { get; }

        public override string ToString() => Properties.Resources.OldSoundChunkController_Text;
    }
}
