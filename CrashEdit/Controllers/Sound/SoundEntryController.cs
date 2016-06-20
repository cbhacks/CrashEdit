using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SoundEntryController : EntryController
    {
        private SoundEntry soundentry;

        public SoundEntryController(EntryChunkController entrychunkcontroller,SoundEntry soundentry) : base(entrychunkcontroller,soundentry)
        {
            this.soundentry = soundentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Sound ({0})",soundentry.EName);
            Node.ImageKey = "speaker";
            Node.SelectedImageKey = "speaker";
        }

        protected override Control CreateEditor()
        {
            return new SoundBox(soundentry);
        }

        public SoundEntry SoundEntry
        {
            get { return soundentry; }
        }
    }
}
