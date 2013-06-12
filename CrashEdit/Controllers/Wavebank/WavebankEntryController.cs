using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class WavebankEntryController : EntryController
    {
        private WavebankEntry wavebankentry;

        public WavebankEntryController(EntryChunkController entrychunkcontroller,WavebankEntry wavebankentry) : base(entrychunkcontroller,wavebankentry)
        {
            this.wavebankentry = wavebankentry;
            Node.Text = "Wavebank Entry";
            Node.ImageKey = "wavebankentry";
            Node.SelectedImageKey = "wavebankentry";
        }

        public WavebankEntry WavebankEntry
        {
            get { return wavebankentry; }
        }
    }
}
