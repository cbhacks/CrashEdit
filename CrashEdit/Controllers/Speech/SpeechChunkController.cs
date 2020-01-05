using Crash;

namespace CrashEdit
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
            Node.Text = string.Format(Crash.UI.Properties.Resources.SpeechChunkController_Text,NSFController.NSF.Chunks.IndexOf(SpeechChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "whitej";
            Node.SelectedImageKey = "whitej";
        }

        public SpeechChunk SpeechChunk { get; }
    }
}
