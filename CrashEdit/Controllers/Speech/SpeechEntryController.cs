using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class SpeechEntryController : EntryController
    {
        public SpeechEntryController(EntryChunkController entrychunkcontroller,SpeechEntry speechentry) : base(entrychunkcontroller,speechentry)
        {
            SpeechEntry = speechentry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(CrashUI.Properties.Resources.SpeechEntryController_Text,SpeechEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "speaker";
            Node.SelectedImageKey = "speaker";
        }

        protected override Control CreateEditor()
        {
            return new SoundBox(SpeechEntry);
        }

        public SpeechEntry SpeechEntry { get; }
    }
}
