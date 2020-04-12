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

            lstEntryList = new ListBox { Dock = DockStyle.Fill };

            Controls.Add(lstEntryList);
            
            Invalidated += EntryChunkBox_Invalidated;
        }

        private void EntryChunkBox_Invalidated(object sender, InvalidateEventArgs e)
        {
            PopulateList();
        }

        private void PopulateList()
        {
            totalsize = 0;
            lstEntryList.Items.Clear();
            foreach (Entry entry in controller.EntryChunk.Entries)
            {
                var this_size = Aligner.Align(entry.Save().Length, controller.EntryChunk.Alignment);
                lstEntryList.Items.Add(string.Format("{0}: {1} bytes", entry.EName, this_size));
                totalsize += this_size;
            }
            lstEntryList.Items.Add(string.Format("Total size: {2} entries, {0} bytes ({1} remaining)", totalsize + 16 + ((controller.EntryChunk.Entries.Count + 1) * 4), Chunk.Length - (totalsize + 16 + ((controller.EntryChunk.Entries.Count + 1) * 4)), controller.EntryChunk.Entries.Count));
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
