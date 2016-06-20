using Crash;

namespace CrashEdit
{
    public sealed class SoundChunkController : EntryChunkController
    {
        private SoundChunk soundchunk;

        public SoundChunkController(NSFController nsfcontroller,SoundChunk soundchunk) : base(nsfcontroller,soundchunk)
        {
            this.soundchunk = soundchunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Sound Chunk {0}", NSFController.chunkid);
            Node.ImageKey = "bluej";
            Node.SelectedImageKey = "bluej";
        }

        public SoundChunk SoundChunk
        {
            get { return soundchunk; }
        }
    }
}
