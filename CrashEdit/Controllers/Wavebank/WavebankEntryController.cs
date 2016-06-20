using Crash;

namespace CrashEdit
{
    public sealed class WavebankEntryController : EntryController
    {
        private WavebankEntry wavebankentry;

        public WavebankEntryController(EntryChunkController entrychunkcontroller,WavebankEntry wavebankentry) : base(entrychunkcontroller,wavebankentry)
        {
            this.wavebankentry = wavebankentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Wavebank ({0})",wavebankentry.EName);
            Node.ImageKey = "music";
            Node.SelectedImageKey = "music";
        }

        public WavebankEntry WavebankEntry
        {
            get { return wavebankentry; }
        }
    }
}
