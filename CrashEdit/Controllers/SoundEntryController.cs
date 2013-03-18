using Crash;
using Crash.Audio;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SoundEntryController : EntryController
    {
        private SoundEntry soundentry;

        public SoundEntryController(EntryChunkController entrychunkcontroller,SoundEntry soundentry) : base(entrychunkcontroller,soundentry)
        {
            this.soundentry = soundentry;
            Node.Text = "Sound Entry";
            Node.ImageKey = "soundentry";
            Node.SelectedImageKey = "soundentry";
        }

        protected override Control CreateEditor()
        {
            SoundBox box = new SoundBox(soundentry);
            box.Dock = DockStyle.Fill;
            return box;
        }

        public SoundEntry SoundEntry
        {
            get { return soundentry; }
        }
    }
}
