using Crash;

namespace CrashEdit
{
    public sealed class OldSoundChunkController : EntryChunkController
    {
        private OldSoundChunk oldsoundchunk;

        public OldSoundChunkController(NSFController nsfcontroller,OldSoundChunk oldsoundchunk) : base(nsfcontroller,oldsoundchunk)
        {
            this.oldsoundchunk = oldsoundchunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Sound Chunk {0}", NSFController.chunkid);
            Node.ImageKey = "bluej";
            Node.SelectedImageKey = "bluej";
        }
        
        public OldSoundChunk OldSoundChunk
        {
            get { return oldsoundchunk; }
        }
    }
}
