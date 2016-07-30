using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class EntryChunkBox : UserControl
    {
        private EntryChunkController controller;

        private int totalsize;

        private ListBox lstEntryList;

        public EntryChunkBox(EntryChunkController controller)
        {
            this.controller = controller;

            lstEntryList = new ListBox();
            lstEntryList.Dock = DockStyle.Fill;
            foreach (Entry entry in controller.EntryChunk.Entries)
            {
                lstEntryList.Items.Add(string.Format("{0}: {1} bytes",entry.EName,entry.Size));
                totalsize += entry.Size;
            }
            lstEntryList.Items.Add(string.Format("Total size: {0} bytes ({1} remaining)",totalsize + 16 + ((controller.EntryChunk.Entries.Count + 1) * 4),0x10000 - totalsize - 16 - ((controller.EntryChunk.Entries.Count + 1) * 4)));

            Controls.Add(lstEntryList);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                lstEntryList.Dispose();
            }
        }
    }
}
