using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class T6EntryController : MysteryUniItemEntryController
    {
        public T6EntryController(EntryChunkController entrychunkcontroller,T6Entry t6entry) : base(entrychunkcontroller,t6entry)
        {
            T6Entry = t6entry;
        }

        public T6Entry T6Entry { get; }
    }
}
