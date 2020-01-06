using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SoundEntryController : EntryController
    {
        public SoundEntryController(EntryChunkController entrychunkcontroller,SoundEntry soundentry) : base(entrychunkcontroller,soundentry)
        {
            SoundEntry = soundentry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.SoundEntryController_Text,SoundEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "speaker";
            Node.SelectedImageKey = "speaker";
        }

        protected override Control CreateEditor()
        {
            return new SoundBox(SoundEntry);
        }

        public SoundEntry SoundEntry { get; }
    }
}
