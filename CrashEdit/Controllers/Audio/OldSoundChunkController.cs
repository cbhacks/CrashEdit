using Crash;
using Crash.Audio;

namespace CrashEdit
{
    public sealed class OldSoundChunkController : EntryChunkController
    {
        private OldSoundChunk oldsoundchunk;

        public OldSoundChunkController(NSFController nsfcontroller,OldSoundChunk oldsoundchunk) : base(nsfcontroller,oldsoundchunk)
        {
            this.oldsoundchunk = oldsoundchunk;
            Node.Text = "Old Sound Chunk";
            Node.ImageKey = "oldsoundchunk";
            Node.SelectedImageKey = "oldsoundchunk";
        }
        
        public OldSoundChunk OldSoundChunk
        {
            get { return oldsoundchunk; }
        }
    }
}
