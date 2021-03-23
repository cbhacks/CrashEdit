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

        public override bool CanMoveTo(CrashEdit.LegacyController newcontroller)
        {
            if (newcontroller is EntryChunkController) {
                return true;
            } else {
                return base.CanMoveTo(newcontroller);
            }
        }

        public override CrashEdit.LegacyController MoveTo(CrashEdit.LegacyController newcontroller)
        {
            if (newcontroller is EntryChunkController newecc)
            {
                EntryChunkController.EntryChunk.Entries.Remove(Entry);
                newecc.EntryChunk.Entries.Add(Entry);
                var replacement = newecc.CreateEntryController(Entry);
                newecc.AddNode(replacement);
                RemoveSelf();
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
            EntryChunkController.NeedsNewEditor = true;
            RemoveSelf();
        }

        private void Menu_Unprocess_Entry()
        {
            int index = EntryChunkController.EntryChunk.Entries.IndexOf(Entry);
            UnprocessedEntry unprocessedentry = Entry.Unprocess();
            EntryChunkController.EntryChunk.Entries[index] = unprocessedentry;
            UnprocessedEntryController unprocessedentrycontroller = new UnprocessedEntryController(EntryChunkController,unprocessedentry);
            EntryChunkController.InsertNode(index,unprocessedentrycontroller);
            RemoveSelf();
        }

        private void Menu_Rename_Entry()
        {
            using (NewEntryForm newentrywindow = new NewEntryForm(EntryChunkController.NSFController))
            {
                newentrywindow.Text = "Rename Entry";
                newentrywindow.SetRenameMode(Entry.EName);
                if (newentrywindow.ShowDialog() == DialogResult.OK)
                {
                    Entry.EID = newentrywindow.EID;
                    InvalidateNode();
                    EntryChunkController.NeedsNewEditor = true;
                    LegacyVerbs[0]._text = string.Format(CrashUI.Properties.Resources.EntryController_AcExport,Entry.EName);
                    LegacyVerbs[1]._text = string.Format(CrashUI.Properties.Resources.EntryController_AcDelete,Entry.EName);
                    LegacyVerbs[2]._text = string.Format(CrashUI.Properties.Resources.EntryController_AcRename,Entry.EName);
                    if (!(this is UnprocessedEntryController))
                    {
                        LegacyVerbs[3]._text = string.Format(CrashUI.Properties.Resources.EntryController_AcDeprocess,Entry.EName);
                    }
                }
            }
        }
    }
}
