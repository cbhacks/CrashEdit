using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class WavebankChunkController : EntryChunkController
    {
        public WavebankChunkController(NSFController nsfcontroller,WavebankChunk wavebankchunk) : base(nsfcontroller,wavebankchunk)
        {
            WavebankChunk = wavebankchunk;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.WavebankChunkController_Text,NSFController.NSF.Chunks.IndexOf(WavebankChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "MusicNoteRed";
        }

        public WavebankChunk WavebankChunk { get; }
    }
}
