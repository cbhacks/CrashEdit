using Crash;

namespace CrashEdit
{
    public sealed class VHController : Controller
    {
        public VHController(MusicEntryController musicentrycontroller,VH vh)
        {
            MusicEntryController = musicentrycontroller;
            VH = vh;
            AddMenu("Replace VH",Menu_Replace_VH);
            AddMenu("Delete VH",Menu_Delete_VH);
            AddMenuSeparator();
            AddMenu("Export VH",Menu_Export_VH);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = "VH";
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        public MusicEntryController MusicEntryController { get; }
        public VH VH { get; private set; }

        private void Menu_Replace_VH()
        {
            byte[] data = FileUtil.OpenFile(FileFilters.VH,FileFilters.Any);
            if (data != null)
            {
                VH = VH.Load(data);
                MusicEntryController.MusicEntry.VH = VH;
            }
        }

        private void Menu_Delete_VH()
        {
            MusicEntryController.MusicEntry.VH = null;
            Dispose();
        }

        private void Menu_Export_VH()
        {
            byte[] data = VH.Save();
            FileUtil.SaveFile(data,FileFilters.VH,FileFilters.Any);
        }
    }
}
