namespace Crash.UI
{
    public sealed class TextureChunkController : ChunkController,IEntryController
    {
        public TextureChunkController(NSFController up,TextureChunk chunk) : base(up,chunk)
        {
            Chunk = chunk;
        }

        public new TextureChunk Chunk { get; }

        IEntry IEntryController.Entry => Chunk;

        public override string ToString() => string.Format(Properties.Resources.TextureChunkController_Text,Entry.EIDToEName(Chunk.EID));
    }
}
