using Crash;

namespace CrashEdit
{
    public sealed class WavebankEntryController : EntryController
    {
        public WavebankEntryController(EntryChunkController entrychunkcontroller,WavebankEntry wavebankentry) : base(entrychunkcontroller,wavebankentry)
        {
            WavebankEntry = wavebankentry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.WavebankEntryController_Text,WavebankEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "musicyellow";
            Node.SelectedImageKey = "musicyellow";
        }

        public WavebankEntry WavebankEntry { get; }
    }
}
