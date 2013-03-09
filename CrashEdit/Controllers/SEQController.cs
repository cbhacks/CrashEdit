using Crash;
using Crash.Audio;
using System;

namespace CrashEdit
{
    public sealed class SEQController : Controller
    {
        private MusicEntryController musicentrycontroller;
        private SEQ seq;

        public SEQController(MusicEntryController musicentrycontroller,SEQ seq)
        {
            this.musicentrycontroller = musicentrycontroller;
            this.seq = seq;
            Node.Text = "SEQ";
            Node.ImageKey = "seq";
            Node.SelectedImageKey = "seq";
            AddMenu("Delete SEQ",Menu_Delete_SEQ);
        }

        public MusicEntryController MusicEntryController
        {
            get { return musicentrycontroller; }
        }

        public SEQ SEQ
        {
            get { return seq; }
        }

        private void Menu_Delete_SEQ(object sender,EventArgs e)
        {
            musicentrycontroller.MusicEntry.SEP.SEQs.Remove(seq);
            Dispose();
        }
    }
}
