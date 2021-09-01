using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class UnprocessedEntryController : MysteryMultiItemEntryController
    {
        public UnprocessedEntryController(EntryChunkController entrychunkcontroller,UnprocessedEntry unprocessedentry) : base(entrychunkcontroller,unprocessedentry)
        {
            UnprocessedEntry = unprocessedentry;
            AddMenu(string.Format(CrashUI.Properties.Resources.UnprocessedEntryController_AcProcess,UnprocessedEntry.EName),Menu_Process_Entry);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.UnprocessedEntryController_Text,UnprocessedEntry.Type,UnprocessedEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "ThingOrange";
        }

        public UnprocessedEntry UnprocessedEntry { get; }

        private void Menu_Process_Entry()
        {
            Entry processedentry;
            try
            {
                processedentry = UnprocessedEntry.Process(EntryChunkController.NSFController.GameVersion);
            }
            catch (LoadAbortedException)
            {
                return;
            }
            int index = EntryChunkController.EntryChunk.Entries.IndexOf(UnprocessedEntry);
            EntryChunkController.EntryChunk.Entries[index] = processedentry;
            EntryController processedentrycontroller = EntryChunkController.CreateEntryController(processedentry);
            EntryChunkController.InsertNode(index,processedentrycontroller);
            RemoveSelf();
        }
    }
}
