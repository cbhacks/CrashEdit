using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ItemController : Controller
    {
        public ItemController(MysteryMultiItemEntryController mysteryentrycontroller,byte[] item)
        {
            MysteryEntryController = mysteryentrycontroller;
            Item = item;
            if (mysteryentrycontroller != null)
            {
                AddMenu("Replace Item",Menu_Replace_Item);
                AddMenu("Delete Item",Menu_Delete_Item);
            }
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = Crash.UI.Properties.Resources.ItemController_Text;
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new MysteryBox(Item);
        }

        public MysteryMultiItemEntryController MysteryEntryController { get; }
        public byte[] Item { get; private set; }

        private void Menu_Replace_Item()
        {
            int i = MysteryEntryController.MysteryEntry.Items.IndexOf(Item);
            byte[] data = FileUtil.OpenFile(FileFilters.Any);
            if (data != null)
            {
                Item = data;
                MysteryEntryController.MysteryEntry.Items[i] = data;
                InvalidateEditor();
            }
        }

        private void Menu_Delete_Item()
        {
            MysteryEntryController.MysteryEntry.Items.Remove(Item);
            Dispose();
        }
    }
}
