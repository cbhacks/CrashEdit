using Crash;
using Crash.Unknown0;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class UnprocessedEntryController : MysteryMultiItemEntryController
    {
        private UnprocessedEntry unprocessedentry;

        public UnprocessedEntryController(EntryChunkController entrychunkcontroller,UnprocessedEntry unprocessedentry) : base(entrychunkcontroller,unprocessedentry)
        {
            this.unprocessedentry = unprocessedentry;
            Node.Text = string.Format("Unprocessed Entry (T{0})",unprocessedentry.Type);
            Node.ImageKey = "unprocessedentry";
            Node.SelectedImageKey = "unprocessedentry";
        }

        public UnprocessedEntry UnprocessedEntry
        {
            get { return unprocessedentry; }
        }
    }
}
