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
            NodeText = string.Format(CrashUI.Properties.Resources.NormalChunkController_Text,NSFController.NSF.Chunks.IndexOf(NormalChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "yellowj";
        }

        public NormalChunk NormalChunk { get; }
    }
}
