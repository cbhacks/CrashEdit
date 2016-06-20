using Crash;

namespace CrashEdit
{
    public sealed class UnprocessedEntryController : MysteryMultiItemEntryController
    {
        private UnprocessedEntry unprocessedentry;

        public UnprocessedEntryController(EntryChunkController entrychunkcontroller,UnprocessedEntry unprocessedentry) : base(entrychunkcontroller,unprocessedentry)
        {
            this.unprocessedentry = unprocessedentry;
            AddMenu("Process Entry",Menu_Process_Entry);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Unprocessed T{0} Entry ({1})",unprocessedentry.Type,unprocessedentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public UnprocessedEntry UnprocessedEntry
        {
            get { return unprocessedentry; }
        }

        private void Menu_Process_Entry()
        {
            Entry processedentry;
            try
            {
                processedentry = unprocessedentry.Process(EntryChunkController.NSFController.GameVersion);
            }
            catch (LoadAbortedException)
            {
                return;
            }
            int index = EntryChunkController.EntryChunk.Entries.IndexOf(unprocessedentry);
            EntryChunkController.EntryChunk.Entries[index] = processedentry;
            EntryController processedentrycontroller = EntryChunkController.CreateEntryController(processedentry);
            EntryChunkController.InsertNode(index,processedentrycontroller);
            if (Node.IsSelected)
            {
                Node.TreeView.SelectedNode = processedentrycontroller.Node;
            }
            Dispose();
        }
    }
}
