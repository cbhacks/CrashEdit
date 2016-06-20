namespace Crash.UI
{
    public abstract class EntryController : Controller
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
    }
}
