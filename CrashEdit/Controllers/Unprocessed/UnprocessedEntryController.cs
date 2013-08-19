using Crash;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class UnprocessedEntryController : MysteryMultiItemEntryController
    {
        private UnprocessedEntry unprocessedentry;

        public UnprocessedEntryController(EntryChunkController entrychunkcontroller,UnprocessedEntry unprocessedentry) : base(entrychunkcontroller,unprocessedentry)
        {
            this.unprocessedentry = unprocessedentry;
            Node.Text = string.Format("Unprocessed Entry (T{0})",unprocessedentry.Type);
            Node.ImageKey = "unprocessedentry";
            Node.SelectedImageKey = "unprocessedentry";
            AddMenu("Process Entry",Menu_Process_Entry);
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
                processedentry = unprocessedentry.Process();
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
