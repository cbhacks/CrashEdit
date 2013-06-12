using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SpeechEntryController : EntryController
    {
        private SpeechEntry speechentry;

        public SpeechEntryController(EntryChunkController entrychunkcontroller,SpeechEntry speechentry) : base(entrychunkcontroller,speechentry)
        {
            this.speechentry = speechentry;
            Node.Text = "Speech Entry";
            Node.ImageKey = "speechentry";
            Node.SelectedImageKey = "speechentry";
        }

        protected override Control CreateEditor()
        {
            return new SoundBox(speechentry);
        }

        public SpeechEntry SpeechEntry
        {
            get { return speechentry; }
        }
    }
}
