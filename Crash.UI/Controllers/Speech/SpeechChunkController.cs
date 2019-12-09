namespace Crash.UI
{
    public sealed class SpeechChunkController : EntryChunkController
    {
        public SpeechChunkController(NSFController up,SpeechChunk chunk) : base(up,chunk)
        {
            Chunk = chunk;
        }

        public new SpeechChunk Chunk { get; }

        public override string ToString() => Properties.Resources.SpeechChunkController_Text;
    }
}
