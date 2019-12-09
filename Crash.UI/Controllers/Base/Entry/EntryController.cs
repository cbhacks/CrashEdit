namespace Crash.UI
{
    public abstract class EntryController : Controller,IEntryController
    {
        private EntryChunkController up;

        public EntryController(EntryChunkController up,Entry entry)
        {
            this.up = up;
            Entry = entry;
        }

        public Entry Entry { get; }

        IEntry IEntryController.Entry => Entry;

        private sealed class AcDelete : Action<EntryController>
        {
            protected override string GetText(EntryController c)
            {
                return string.Format(Properties.Resources.EntryController_AcDelete,c.Entry.EName);
            }

            protected override Command Activate(EntryController c)
            {
                return c.up.Chunk.Entries.CmRemove(c.Entry);
            }
        }

        private sealed class AcDeprocess : Action<EntryController>
        {
            protected override bool CheckCompatibility(EntryController c)
            {
                if (c is UnprocessedEntryController)
                    return false;
                return true;
            }

            protected override string GetText(EntryController c) => string.Format(Properties.Resources.EntryController_AcDeprocess,c.Entry.EName);

            protected override Command Activate(EntryController c)
            {
                UnprocessedEntry unprocessedentry = c.Entry.Unprocess();
                return c.up.Chunk.Entries.CmSet(c.up.Chunk.Entries.IndexOf(c.Entry),unprocessedentry);
            }
        }
    }
}
