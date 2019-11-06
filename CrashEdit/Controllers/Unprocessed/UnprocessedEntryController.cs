using Crash;

namespace CrashEdit
{
    public sealed class UnprocessedEntryController : MysteryMultiItemEntryController
    {
        public UnprocessedEntryController(EntryChunkController entrychunkcontroller,UnprocessedEntry unprocessedentry) : base(entrychunkcontroller,unprocessedentry)
        {
            UnprocessedEntry = unprocessedentry;
            AddMenu("Process Entry",Menu_Process_Entry);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Unprocessed T{0} Entry ({1})",UnprocessedEntry.Type,UnprocessedEntry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
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
            var trv = Node.TreeView;
            trv.BeginUpdate();
            int index = EntryChunkController.EntryChunk.Entries.IndexOf(UnprocessedEntry);
            EntryChunkController.EntryChunk.Entries[index] = processedentry;
            EntryController processedentrycontroller = EntryChunkController.CreateEntryController(processedentry);
            EntryChunkController.InsertNode(index,processedentrycontroller);
            if (Node.IsSelected)
            {
                Node.TreeView.SelectedNode = processedentrycontroller.Node;
            }
            Dispose();
            trv.EndUpdate();
        }
    }
}
