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

        public void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.OldSoundChunkController_Text,NSFController.NSF.Chunks.IndexOf(OldSoundChunk) * 2 + 1);
        }

        public void InvalidateNodeImage()
        {
            NodeImageKey = "JournalBlue";
        }

        public OldSoundChunk OldSoundChunk { get; }
    }
}
