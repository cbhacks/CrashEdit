using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class T17EntryController : MysteryMultiItemEntryController
    {
        public T17EntryController(EntryChunkController entrychunkcontroller,T17Entry t17entry) : base(entrychunkcontroller,t17entry)
        {
            T17Entry = t17entry;
        }

        public T17Entry T17Entry { get; }
    }
}
