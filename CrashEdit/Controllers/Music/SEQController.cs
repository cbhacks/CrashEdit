using Crash;

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
            AddMenu("Replace SEQ",Menu_Replace_SEQ);
            AddMenu("Delete SEQ",Menu_Delete_SEQ);
            AddMenuSeparator();
            AddMenu("Export SEQ",Menu_Export_SEQ);
            AddMenu("Export SEQ as MIDI",Menu_Export_SEQ_MIDI);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "SEQ";
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        public MusicEntryController MusicEntryController
        {
            get { return musicentrycontroller; }
        }

        public SEQ SEQ
        {
            get { return seq; }
        }

        private void Menu_Replace_SEQ()
        {
            int i = musicentrycontroller.MusicEntry.SEP.SEQs.IndexOf(seq);
            byte[] data = FileUtil.OpenFile(FileFilters.SEQ,FileFilters.Any);
            if (data != null)
            {
                seq = SEQ.Load(data);
                musicentrycontroller.MusicEntry.SEP.SEQs[i] = seq;
            }
        }

        private void Menu_Delete_SEQ()
        {
            musicentrycontroller.MusicEntry.SEP.SEQs.Remove(seq);
            Dispose();
        }

        private void Menu_Export_SEQ()
        {
            byte[] data = seq.Save();
            FileUtil.SaveFile(data,FileFilters.SEQ,FileFilters.Any);
        }

        private void Menu_Export_SEQ_MIDI()
        {
            byte[] data = seq.ToMIDI();
            FileUtil.SaveFile(data,FileFilters.MIDI,FileFilters.Any);
        }
    }
}
