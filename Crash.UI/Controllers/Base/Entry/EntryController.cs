using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crash.UI
{
    public abstract class EntryController : Controller,IEntryController
    {
        private EntryChunkController up;
        private Entry entry;

        public EntryController(EntryChunkController up,Entry entry)
        {
            this.up = up;
            this.entry = entry;
        }

        public Entry Entry
        {
            get { return entry; }
        }

        IEntry IEntryController.Entry
        {
            get { return entry; }
        }
    }
}
