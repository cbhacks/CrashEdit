using Crash;
using System;

namespace CrashEdit
{
    public sealed class OldVHController : Controller
    {
        private OldMusicEntryController oldmusicentrycontroller;
        private VH vh;

        public OldVHController(OldMusicEntryController oldmusicentrycontroller,VH vh)
        {
            this.oldmusicentrycontroller = oldmusicentrycontroller;
            this.vh = vh;
            Node.Text = "VH";
            Node.ImageKey = "vh";
            Node.SelectedImageKey = "vh";
            AddMenu("Replace VH",Menu_Replace_VH);
            AddMenuSeparator();
            AddMenu("Export VH",Menu_Export_VH);
        }

        public OldMusicEntryController OldMusicEntryController
        {
            get { return oldmusicentrycontroller; }
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
                oldmusicentrycontroller.OldMusicEntry.VH = vh;
            }
        }

        private void Menu_Export_VH()
        {
            byte[] data = vh.Save();
            FileUtil.SaveFile(data,FileFilters.VH,FileFilters.Any);
        }
    }
}
