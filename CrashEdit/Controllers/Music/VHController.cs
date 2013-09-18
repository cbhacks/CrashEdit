using Crash;
using System;

namespace CrashEdit
{
    public sealed class VHController : Controller
    {
        private MusicEntryController musicentrycontroller;
        private VH vh;

        public VHController(MusicEntryController musicentrycontroller,VH vh)
        {
            this.musicentrycontroller = musicentrycontroller;
            this.vh = vh;
            Node.Text = "VH";
            Node.ImageKey = "vh";
            Node.SelectedImageKey = "vh";
            AddMenu("Replace VH",Menu_Replace_VH);
            AddMenu("Delete VH",Menu_Delete_VH);
            AddMenuSeparator();
            AddMenu("Export VH",Menu_Export_VH);
        }

        public MusicEntryController MusicEntryController
        {
            get { return musicentrycontroller; }
        }

        public VH VH
        {
            get { return vh; }
        }

        private void Menu_Replace_VH()
        {
            byte[] data = FileUtil.OpenFile(FileFilters.VH,FileFilters.Any);
            if (data != null)
            {
                vh = VH.Load(data);
                musicentrycontroller.MusicEntry.VH = vh;
            }
        }

        private void Menu_Delete_VH()
        {
            musicentrycontroller.MusicEntry.VH = null;
            Dispose();
        }

        private void Menu_Export_VH()
        {
            byte[] data = vh.Save();
            FileUtil.SaveFile(data,FileFilters.VH,FileFilters.Any);
        }
    }
}
