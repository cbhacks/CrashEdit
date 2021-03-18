using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public abstract class EntryController : LegacyController
    {
        public EntryController(EntryChunkController entrychunkcontroller,Entry entry) : base(entrychunkcontroller, entry)
        {
            EntryChunkController = entrychunkcontroller;
            Entry = entry;
            AddMenu(string.Format(CrashUI.Properties.Resources.EntryController_AcExport,entry.EName),Menu_Export_Entry);
            AddMenu(string.Format(CrashUI.Properties.Resources.EntryController_AcDelete,entry.EName),Menu_Delete_Entry);
            AddMenu(string.Format(CrashUI.Properties.Resources.EntryController_AcRename,entry.EName),Menu_Rename_Entry);
            if (!(this is UnprocessedEntryController))
            {
                AddMenu(string.Format(CrashUI.Properties.Resources.EntryController_AcDeprocess,entry.EName),Menu_Unprocess_Entry);
            }
        }

        public EntryChunkController EntryChunkController { get; private set; }
        public Entry Entry { get; }

        public override bool CanMoveTo(LegacyController newcontroller)
        {
            if (newcontroller is EntryChunkController) {
                return true;
            } else {
                return base.CanMoveTo(newcontroller);
            }
        }

        public override LegacyController MoveTo(LegacyController newcontroller)
        {
            if (newcontroller is EntryChunkController newecc)
            {
                EntryChunkController.EntryChunk.Entries.Remove(Entry);
                newecc.EntryChunk.Entries.Add(Entry);
                var replacement = newecc.CreateEntryController(Entry);
                newecc.AddNode(replacement);
                Dispose();
                return replacement;
            }
            else
            {
                return base.MoveTo(newcontroller);
            }
        }

        protected T FindEID<T>(int eid) where T : class,IEntry
        {
            return EntryChunkController.NSFController.NSF.GetEntry<T>(eid);
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
                trv.SelectedNode = unprocessedentrycontroller.Node;
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
                    ContextMenu.MenuItems[0].Text = string.Format(CrashUI.Properties.Resources.EntryController_AcExport,Entry.EName);
                    ContextMenu.MenuItems[1].Text = string.Format(CrashUI.Properties.Resources.EntryController_AcDelete,Entry.EName);
                    ContextMenu.MenuItems[2].Text = string.Format(CrashUI.Properties.Resources.EntryController_AcRename,Entry.EName);
                    if (!(this is UnprocessedEntryController))
                    {
                        ContextMenu.MenuItems[3].Text = string.Format(CrashUI.Properties.Resources.EntryController_AcDeprocess,Entry.EName);
                    }
                }
            }
        }
    }
}
