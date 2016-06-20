using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ItemController : Controller
    {
        private MysteryMultiItemEntryController mysteryentrycontroller;
        private byte[] item;
        
        public ItemController(MysteryMultiItemEntryController mysteryentrycontroller,byte[] item)
        {
            this.mysteryentrycontroller = mysteryentrycontroller;
            this.item = item;
            if (mysteryentrycontroller != null)
            {
                AddMenu("Replace Item",Menu_Replace_Item);
                AddMenu("Delete Item",Menu_Delete_Item);
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Item";
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new MysteryBox(item);
        }

        public MysteryMultiItemEntryController MysteryEntryController
        {
            get { return mysteryentrycontroller; }
        }

        public byte[] Item
        {
            get { return item; }
        }

        private void Menu_Replace_Item()
        {
            int i = mysteryentrycontroller.MysteryEntry.Items.IndexOf(item);
            byte[] data = FileUtil.OpenFile(FileFilters.Any);
            if (data != null)
            {
                item = data;
                mysteryentrycontroller.MysteryEntry.Items[i] = data;
                InvalidateEditor();
            }
        }

        private void Menu_Delete_Item()
        {
            mysteryentrycontroller.MysteryEntry.Items.Remove(item);
            Dispose();
        }
    }
}
