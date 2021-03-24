using CrashEdit.Crash;

namespace CrashEdit.CE
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
            NodeText = string.Format(CrashUI.Properties.Resources.WavebankEntryController_Text,WavebankEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "MusicNoteYellow";
        }

        public WavebankEntry WavebankEntry { get; }
    }
}
