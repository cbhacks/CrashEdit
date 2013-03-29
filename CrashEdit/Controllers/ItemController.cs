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
            Node.Text = "Item";
            Node.ImageKey = "item";
            Node.SelectedImageKey = "item";
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
    }
}
