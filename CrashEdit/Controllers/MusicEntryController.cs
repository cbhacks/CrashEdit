using Crash;
using Crash.Audio;

namespace CrashEdit
{
    public sealed class MusicEntryController : EntryController
    {
        private MusicEntry musicentry;

        public MusicEntryController(EntryChunkController entrychunkcontroller,MusicEntry musicentry) : base(entrychunkcontroller,musicentry)
        {
            this.musicentry = musicentry;
            Node.Text = "Music Entry";
            Node.ImageKey = "musicentry";
            Node.SelectedImageKey = "musicentry";
            foreach (SEQ seq in musicentry.SEP.SEQs)
            {
                AddNode(new SEQController(this,seq));
            }
        }

        public MusicEntry MusicEntry
        {
            get { return musicentry; }
        }
    }
}
