using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class EntryController : Controller
    {
        public EntryController(EntryChunkController entrychunkcontroller,Entry entry)
        {
            EntryChunkController = entrychunkcontroller;
            Entry = entry;
            AddMenu("Export Entry",Menu_Export_Entry);
            AddMenu("Delete Entry",Menu_Delete_Entry);
            AddMenu("Rename Entry",Menu_Rename_Entry);
            if (!(this is UnprocessedEntryController))
            {
                AddMenu("Unprocess Entry",Menu_Unprocess_Entry);
            }
        }

        public EntryChunkController EntryChunkController { get; private set; }
        public Entry Entry { get; }

        public override bool Move(Controller newcontroller,bool commit)
        {
            if (newcontroller is EntryChunkController)
            {
                if (commit)
                {
                    EntryChunkController.EntryChunk.Entries.Remove(Entry);
                    Node.Remove();
                    EntryChunkController = (EntryChunkController)newcontroller;
                    EntryChunkController.EntryChunk.Entries.Add(Entry);
                    EntryChunkController.Node.Nodes.Add(Node);
                }
                return true;
            }
            else
            {
                return base.Move(newcontroller,commit);
            }
        }

        protected T FindEID<T>(int eid) where T : class,IEntry
        {
            return EntryChunkController.NSFController.NSF.FindEID<T>(eid);
        }

        private void Menu_Export_Entry()
        {
            FileUtil.SaveFile(Entry.Save(),FileFilters.NSEntry,FileFilters.Any);
        }

        private void Menu_Delete_Entry()
        {
            EntryChunkController.EntryChunk.Entries.Remove(Entry);
            EntryChunkController.Editor.Invalidate();
            Dispose();
        }

        private void Menu_Unprocess_Entry()
        {
            var trv = Node.TreeView;
            trv.BeginUpdate();
            int index = EntryChunkController.EntryChunk.Entries.IndexOf(Entry);
            UnprocessedEntry unprocessedentry = Entry.Unprocess();
            EntryChunkController.EntryChunk.Entries[index] = unprocessedentry;
            UnprocessedEntryController unprocessedentrycontroller = new UnprocessedEntryController(EntryChunkController,unprocessedentry);
            EntryChunkController.InsertNode(index,unprocessedentrycontroller);
            if (Node.IsSelected)
            {
                Node.TreeView.SelectedNode = unprocessedentrycontroller.Node;
            }
            Dispose();
            trv.EndUpdate();
        }

        private void Menu_Rename_Entry()
        {
            using (NewEntryForm newentrywindow = new NewEntryForm(EntryChunkController.NSFController))
            {
                newentrywindow.Text = "Rename Entry";
                newentrywindow.SetRenameMode(Entry.EName);
                if (newentrywindow.ShowDialog(Node.TreeView.TopLevelControl) == DialogResult.OK)
                {
                    Entry.EID = newentrywindow.EID;
                    InvalidateNode();
                    EntryChunkController.Editor.Invalidate();
                }
            }
        }
    }
}
