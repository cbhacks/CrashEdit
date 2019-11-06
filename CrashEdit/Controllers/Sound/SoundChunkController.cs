using Crash;

namespace CrashEdit
{
    public sealed class SoundChunkController : EntryChunkController
    {
        public SoundChunkController(NSFController nsfcontroller,SoundChunk soundchunk) : base(nsfcontroller,soundchunk)
        {
            SoundChunk = soundchunk;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Sound Chunk {0}",NSFController.NSF.Chunks.IndexOf(SoundChunk) * 2 + 1);
            Node.ImageKey = "bluej";
            Node.SelectedImageKey = "bluej";
        }

        public SoundChunk SoundChunk { get; }
    }
}
