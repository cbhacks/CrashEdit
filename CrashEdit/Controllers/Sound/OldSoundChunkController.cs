using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class OldSoundChunkController : EntryChunkController
    {
        public OldSoundChunkController(NSFController nsfcontroller,OldSoundChunk oldsoundchunk) : base(nsfcontroller,oldsoundchunk)
        {
            OldSoundChunk = oldsoundchunk;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(CrashUI.Properties.Resources.OldSoundChunkController_Text,NSFController.NSF.Chunks.IndexOf(OldSoundChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "bluej";
            Node.SelectedImageKey = "bluej";
        }

        public OldSoundChunk OldSoundChunk { get; }
    }
}
