using Crash;

namespace CrashEdit
{
    public sealed class WavebankEntryController : EntryController
    {
        public WavebankEntryController(EntryChunkController entrychunkcontroller,WavebankEntry wavebankentry) : base(entrychunkcontroller,wavebankentry)
        {
            WavebankEntry = wavebankentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Wavebank ({0})",WavebankEntry.EName);
            Node.ImageKey = "music";
            Node.SelectedImageKey = "music";
        }

        public WavebankEntry WavebankEntry { get; }
    }
}
