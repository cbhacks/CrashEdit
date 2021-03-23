using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class OldVHController : LegacyController
    {
        public OldVHController(OldMusicEntryController oldmusicentrycontroller,VH vh) : base(oldmusicentrycontroller, vh)
        {
            OldMusicEntryController = oldmusicentrycontroller;
            VH = vh;
            AddMenu("Replace VH",Menu_Replace_VH);
            AddMenuSeparator();
            AddMenu("Export VH",Menu_Export_VH);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = "VH";
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "arrow";
        }

        public OldMusicEntryController OldMusicEntryController { get; }
        public VH VH { get; private set; }

        private void Menu_Replace_VH()
        {
            byte[] data = FileUtil.OpenFile(FileFilters.VH,FileFilters.Any);
            if (data != null)
            {
                VH = VH.Load(data);
                OldMusicEntryController.OldMusicEntry.VH = VH;
            }
        }

        private void Menu_Export_VH()
        {
            byte[] data = VH.Save();
            FileUtil.SaveFile(data,FileFilters.VH,FileFilters.Any);
        }
    }
}
