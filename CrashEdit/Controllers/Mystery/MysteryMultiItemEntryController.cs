using Crash;

namespace CrashEdit
{
    public abstract class MysteryMultiItemEntryController : EntryController
    {
        private MysteryMultiItemEntry mysteryentry;
        private byte item;

        public MysteryMultiItemEntryController(EntryChunkController entrychunkcontroller,MysteryMultiItemEntry mysteryentry) : base(entrychunkcontroller,mysteryentry)
        {
            this.mysteryentry = mysteryentry;
            AddMenu("Add Item",Menu_Add_Item);
            foreach (byte[] item in mysteryentry.Items)
            {
                AddNode(new ItemController(this,item));
            }
        }

        public MysteryMultiItemEntry MysteryEntry
        {
            get { return mysteryentry; }
        }

        private void Menu_Add_Item()
        {
            byte[] data = FileUtil.OpenFile(FileFilters.Any);
            if (data != null)
            {
                mysteryentry.Items.Add(data);
                AddNode(new ItemController(this, data));
            }
        }
    }
}
