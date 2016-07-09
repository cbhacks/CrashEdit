namespace Crash.UI
{
    public sealed class WavebankEntryController : EntryController
    {
        private WavebankEntry entry;

        public WavebankEntryController(EntryChunkController up,WavebankEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new WavebankEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.WavebankEntryController_Text,entry.EName);
        }
    }
}
