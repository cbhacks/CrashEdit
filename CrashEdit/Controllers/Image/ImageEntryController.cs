using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class ImageEntryController : MysteryMultiItemEntryController
    {
        public ImageEntryController(EntryChunkController entrychunkcontroller,ImageEntry imageentry) : base(entrychunkcontroller,imageentry)
        {
            ImageEntry = imageentry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.ImageEntryController_Text,ImageEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "image";
        }

        public ImageEntry ImageEntry { get; }
    }
}
