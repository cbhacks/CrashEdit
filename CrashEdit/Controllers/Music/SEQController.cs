using Crash;

namespace CrashEdit
{
    public sealed class SEQController : Controller
    {
        public SEQController(MusicEntryController musicentrycontroller,SEQ seq)
        {
            MusicEntryController = musicentrycontroller;
            SEQ = seq;
            AddMenu("Replace SEQ",Menu_Replace_SEQ);
            AddMenu("Delete SEQ",Menu_Delete_SEQ);
            AddMenuSeparator();
            AddMenu("Export SEQ",Menu_Export_SEQ);
            AddMenu("Export SEQ as MIDI",Menu_Export_SEQ_MIDI);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = "SEQ";
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        public MusicEntryController MusicEntryController { get; }
        public SEQ SEQ { get; private set; }

        private void Menu_Replace_SEQ()
        {
            int i = MusicEntryController.MusicEntry.SEP.SEQs.IndexOf(SEQ);
            byte[] data = FileUtil.OpenFile(FileFilters.SEQ,FileFilters.Any);
            if (data != null)
            {
                SEQ = SEQ.Load(data);
                MusicEntryController.MusicEntry.SEP.SEQs[i] = SEQ;
            }
        }

        private void Menu_Delete_SEQ()
        {
            MusicEntryController.MusicEntry.SEP.SEQs.Remove(SEQ);
            Dispose();
        }

        private void Menu_Export_SEQ()
        {
            byte[] data = SEQ.Save();
            FileUtil.SaveFile(data,FileFilters.SEQ,FileFilters.Any);
        }

        private void Menu_Export_SEQ_MIDI()
        {
            byte[] data = SEQ.ToMIDI();
            FileUtil.SaveFile(data,FileFilters.MIDI,FileFilters.Any);
        }
    }
}
