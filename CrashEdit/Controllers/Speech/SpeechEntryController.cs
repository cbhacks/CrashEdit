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
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Speech Entry ({0})",speechentry.EIDString);
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
