namespace Crash.UI
{
    public sealed class WavebankEntryController : EntryController
    {
        public WavebankEntryController(EntryChunkController up,WavebankEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new WavebankEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.WavebankEntryController_Text,Entry.EName);
    }
}
