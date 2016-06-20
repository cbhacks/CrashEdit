using Crash;

namespace CrashEdit
{
    public abstract class EntryController : Controller
    {
        private EntryChunkController entrychunkcontroller;
        private Entry entry;

        public EntryController(EntryChunkController entrychunkcontroller,Entry entry)
        {
            this.entrychunkcontroller = entrychunkcontroller;
            this.entry = entry;
            AddMenu("Export Entry",Menu_Export_Entry);
            AddMenu("Delete Entry",Menu_Delete_Entry);
            if (!(this is UnprocessedEntryController))
            {
                AddMenu("Unprocess Entry",Menu_Unprocess_Entry);
            }
        }

        public EntryChunkController EntryChunkController
        {
            get { return entrychunkcontroller; }
        }

        public Entry Entry
        {
            get { return entry; }
        }

        public override bool Move(Controller newcontroller,bool commit)
        {
            if (newcontroller is EntryChunkController)
            {
                if (commit)
                {
                    entrychunkcontroller.EntryChunk.Entries.Remove(entry);
                    Node.Remove();
                    entrychunkcontroller = (EntryChunkController)newcontroller;
                    entrychunkcontroller.EntryChunk.Entries.Add(entry);
                    entrychunkcontroller.Node.Nodes.Add(Node);
                }
                return true;
            }
            else
            {
                return base.Move(newcontroller,commit);
            }
        }

        protected T FindEID<T>(int eid) where T : Entry
        {
            return entrychunkcontroller.NSFController.NSF.FindEID<T>(eid);
        }

        private void Menu_Export_Entry()
        {
            FileUtil.SaveFile(entry.Save(),FileFilters.NSEntry,FileFilters.Any);
        }

        private void Menu_Delete_Entry()
        {
            entrychunkcontroller.EntryChunk.Entries.Remove(entry);
            Dispose();
        }

        private void Menu_Unprocess_Entry()
        {
            int index = entrychunkcontroller.EntryChunk.Entries.IndexOf(entry);
            UnprocessedEntry unprocessedentry = entry.Unprocess();
            entrychunkcontroller.EntryChunk.Entries[index] = unprocessedentry;
            UnprocessedEntryController unprocessedentrycontroller = new UnprocessedEntryController(entrychunkcontroller,unprocessedentry);
            entrychunkcontroller.InsertNode(index,unprocessedentrycontroller);
            if (Node.IsSelected)
            {
                Node.TreeView.SelectedNode = unprocessedentrycontroller.Node;
            }
            Dispose();
        }
    }
}
