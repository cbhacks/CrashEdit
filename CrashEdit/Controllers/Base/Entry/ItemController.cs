using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class ItemController : LegacyController
    {
        public ItemController(LegacyController parent,byte[] item) : base(parent, item)
        {
            MysteryEntryController = parent as MysteryMultiItemEntryController;
            Item = item;
            if (MysteryEntryController != null)
            {
                AddMenu("Replace Item",Menu_Replace_Item);
                AddMenu("Delete Item",Menu_Delete_Item);
            }
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = CrashUI.Properties.Resources.ItemController_Text;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new MysteryBox(Item);
        }

        public MysteryMultiItemEntryController MysteryEntryController { get; }
        public byte[] Item { get; }

        private void Menu_Replace_Item()
        {
            int i = MysteryEntryController.MysteryEntry.Items.IndexOf(Item);
            byte[] data = FileUtil.OpenFile(FileFilters.Any);
            if (data != null)
            {
                MysteryEntryController.MysteryEntry.Items[i] = data;
                MysteryEntryController.LegacySubcontrollers[i] = new ItemController(MysteryEntryController, data);
            }
        }

        private void Menu_Delete_Item()
        {
            MysteryEntryController.MysteryEntry.Items.Remove(Item);
            RemoveSelf();
        }
    }
}
