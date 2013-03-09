using Crash;
using System;

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
            AddMenu("Delete Entry",Menu_Delete_Entry);
        }

        public EntryChunkController EntryChunkController
        {
            get { return entrychunkcontroller; }
        }

        public Entry Entry
        {
            get { return entry; }
        }

        private void Menu_Delete_Entry(object sender,EventArgs e)
        {
            entrychunkcontroller.EntryChunk.Entries.Remove(entry);
            Dispose();
        }
    }
}
