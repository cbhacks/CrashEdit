using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class SpeechChunkController : EntryChunkController
    {
        public SpeechChunkController(NSFController nsfcontroller,SpeechChunk speechchunk) : base(nsfcontroller,speechchunk)
        {
            SpeechChunk = speechchunk;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.SpeechChunkController_Text,NSFController.NSF.Chunks.IndexOf(SpeechChunk) * 2 + 1);
        }

        public void InvalidateNodeImage()
        {
            NodeImageKey = "JournalWhite";
        }

        public SpeechChunk SpeechChunk { get; }
    }
}
