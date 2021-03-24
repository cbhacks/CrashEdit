using CrashEdit.Crash;

namespace CrashEdit.CE
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
            NodeText = string.Format(CrashUI.Properties.Resources.SoundChunkController_Text,NSFController.NSF.Chunks.IndexOf(SoundChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "JournalBlue";
        }

        public SoundChunk SoundChunk { get; }
    }
}
