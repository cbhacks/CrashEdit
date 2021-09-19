using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class T21EntryController : MysteryMultiItemEntryController
    {
        public T21EntryController(EntryChunkController entrychunkcontroller,T21Entry t21entry) : base(entrychunkcontroller,t21entry)
        {
            T21Entry = t21entry;
        }

        public T21Entry T21Entry { get; }
    }
}
