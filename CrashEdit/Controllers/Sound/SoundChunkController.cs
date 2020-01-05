using Crash;

namespace CrashEdit
{
    public sealed class SoundChunkController : EntryChunkController
    {
        public SoundChunkController(NSFController nsfcontroller,SoundChunk soundchunk) : base(nsfcontroller,soundchunk)
        {
            SoundChunk = soundchunk;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.SoundChunkController_Text,NSFController.NSF.Chunks.IndexOf(SoundChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "bluej";
            Node.SelectedImageKey = "bluej";
        }

        public SoundChunk SoundChunk { get; }
    }
}
