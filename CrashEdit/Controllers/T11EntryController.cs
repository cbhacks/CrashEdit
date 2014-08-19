using Crash;

namespace CrashEdit
{
    public sealed class T11EntryController : MysteryMultiItemEntryController
    {
        private T11Entry t11entry;

        public T11EntryController(EntryChunkController entrychunkcontroller,T11Entry t11entry) : base(entrychunkcontroller,t11entry)
        {
            this.t11entry = t11entry;
            Node.Text = string.Format("Code (GOOL) Entry ({0})",t11entry.EIDString);
            Node.ImageKey = "t11entry";
            Node.SelectedImageKey = "t11entry";
        }

        public T11Entry T11Entry
        {
            get { return t11entry; }
        }
    }
}
