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

        public override void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.SpeechChunkController_Text,NSFController.NSF.Chunks.IndexOf(SpeechChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "whitej";
        }

        public SpeechChunk SpeechChunk { get; }
    }
}
