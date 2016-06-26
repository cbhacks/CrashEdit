using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crash.UI
{
    public sealed class T20EntryController : MysteryMultiItemEntryController
    {
        private T20Entry entry;

        public T20EntryController(EntryChunkController up,T20Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new T20Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.T20EntryController_Text,entry.EName);
        }
    }
}
