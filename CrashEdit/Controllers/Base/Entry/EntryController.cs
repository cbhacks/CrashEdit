using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(Entry))]
    public class EntryController : LegacyController
    {
        public EntryController(Entry entry, SubcontrollerGroup parentGroup) : base(parentGroup, entry)
        {
            Entry = entry;
            AddMenu(string.Format(CrashUI.Properties.Resources.EntryController_AcRename,entry.EName),Menu_Rename_Entry);
            if (!(this is UnprocessedEntryController))
            {
                AddMenu(string.Format(CrashUI.Properties.Resources.EntryController_AcDeprocess,entry.EName),Menu_Unprocess_Entry);
            }
        }

        protected EntryChunkController EntryChunkController => (EntryChunkController)Modern.Parent.Legacy;
        public Entry Entry { get; }

        public override bool CanMoveTo(CrashEdit.LegacyController newcontroller)
        {
            if (newcontroller is EntryChunkController) {
                return true;
            } else {
                return base.CanMoveTo(newcontroller);
            }
        }

        public override void MoveTo(CrashEdit.LegacyController newcontroller)
        {
            if (newcontroller is EntryChunkController newecc)
            {
                EntryChunkController.EntryChunk.Entries.Remove(Entry);
                newecc.EntryChunk.Entries.Add(Entry);
            }
            else
            {
                base.MoveTo(newcontroller);
            }
        }

        protected T FindEID<T>(int eid) where T : class,IEntry
        {
            return GetEntry<T>(eid);
        }

        private void Menu_Unprocess_Entry()
        {
            int index = EntryChunkController.EntryChunk.Entries.IndexOf(Entry);
            UnprocessedEntry unprocessedentry = Entry.Unprocess();
            EntryChunkController.EntryChunk.Entries[index] = unprocessedentry;
        }

        private void Menu_Rename_Entry()
        {
            using (NewEntryForm newentrywindow = new NewEntryForm(GetNSF(), GameVersion))
            {
                newentrywindow.Text = "Rename Entry";
                newentrywindow.SetRenameMode(Entry.EName);
                if (newentrywindow.ShowDialog() == DialogResult.OK)
                {
                    Entry.EID = newentrywindow.EID;
                    EntryChunkController.NeedsNewEditor = true;
                    LegacyVerbs[0]._text = string.Format(CrashUI.Properties.Resources.EntryController_AcDelete,Entry.EName);
                    LegacyVerbs[1]._text = string.Format(CrashUI.Properties.Resources.EntryController_AcRename,Entry.EName);
                    if (!(this is UnprocessedEntryController))
                    {
                        LegacyVerbs[2]._text = string.Format(CrashUI.Properties.Resources.EntryController_AcDeprocess,Entry.EName);
                    }
                }
            }
        }
    }
}
