namespace Crash.UI
{
    public sealed class TextureChunkController : ChunkController,IEntryController
    {
        private TextureChunk chunk;

        public TextureChunkController(NSFController up,TextureChunk chunk) : base(up,chunk)
        {
            this.chunk = chunk;
        }

        public new TextureChunk Chunk
        {
            get { return chunk; }
        }

        IEntry IEntryController.Entry
        {
            get { return chunk; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.TextureChunkController_Text,Entry.EIDToEName(chunk.EID));
        }
    }
}
