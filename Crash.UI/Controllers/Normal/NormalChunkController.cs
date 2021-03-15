using CrashEdit.Crash;

namespace CrashEdit.CrashUI
{
    public sealed class NormalChunkController : EntryChunkController
    {
        public NormalChunkController(NSFController up,NormalChunk chunk) : base(up,chunk)
        {
            Chunk = chunk;
        }

        public new NormalChunk Chunk { get; }

        public override string ToString() => Properties.Resources.NormalChunkController_Text;
    }
}
