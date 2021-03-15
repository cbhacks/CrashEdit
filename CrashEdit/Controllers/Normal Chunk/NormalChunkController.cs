using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class NormalChunkController : EntryChunkController
    {
        public NormalChunkController(NSFController nsfcontroller,NormalChunk normalchunk) : base(nsfcontroller,normalchunk)
        {
            NormalChunk = normalchunk;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(CrashUI.Properties.Resources.NormalChunkController_Text,NSFController.NSF.Chunks.IndexOf(NormalChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "yellowj";
            Node.SelectedImageKey = "yellowj";
        }

        public NormalChunk NormalChunk { get; }
    }
}
