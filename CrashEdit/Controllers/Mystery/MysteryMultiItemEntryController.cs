using Crash;

namespace CrashEdit
{
    public abstract class MysteryMultiItemEntryController : EntryController
    {
        public MysteryMultiItemEntryController(EntryChunkController entrychunkcontroller,MysteryMultiItemEntry mysteryentry) : base(entrychunkcontroller,mysteryentry)
        {
            MysteryEntry = mysteryentry;
            AddMenu("Add Item",Menu_Add_Item);
            foreach (byte[] item in mysteryentry.Items)
            {
                AddNode(new ItemController(this,item));
            }
        }

        public MysteryMultiItemEntry MysteryEntry { get; }

        private void Menu_Add_Item()
        {
            byte[] data = FileUtil.OpenFile(FileFilters.Any);
            if (data != null)
            {
                MysteryEntry.Items.Add(data);
                AddNode(new ItemController(this, data));
            }
        }
    }
}
