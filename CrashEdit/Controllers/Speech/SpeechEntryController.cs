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
            NodeText = string.Format(CrashUI.Properties.Resources.SpeechEntryController_Text,SpeechEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "speaker";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new SoundBox(SpeechEntry);
        }

        public SpeechEntry SpeechEntry { get; }
    }
}
