using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(UnprocessedEntry))]
    public sealed class UnprocessedEntryController : EntryController
    {
        public UnprocessedEntryController(UnprocessedEntry unprocessedentry, SubcontrollerGroup parentGroup) : base(unprocessedentry, parentGroup)
        {
            UnprocessedEntry = unprocessedentry;
            AddMenu(string.Format(CrashUI.Properties.Resources.UnprocessedEntryController_AcProcess,UnprocessedEntry.EName),Menu_Process_Entry);
        }

        public UnprocessedEntry UnprocessedEntry { get; }

        private void Menu_Process_Entry()
        {
            Entry processedentry;
            try
            {
                processedentry = UnprocessedEntry.Process(GameVersion);
            }
            catch (LoadAbortedException)
            {
                return;
            }
            int index = EntryChunkController.EntryChunk.Entries.IndexOf(UnprocessedEntry);
            EntryChunkController.EntryChunk.Entries[index] = processedentry;
        }
    }
}
