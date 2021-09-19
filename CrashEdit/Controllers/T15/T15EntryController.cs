using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class T15EntryController : MysteryUniItemEntryController
    {
        public T15EntryController(EntryChunkController entrychunkcontroller,T15Entry t15entry) : base(entrychunkcontroller,t15entry)
        {
            T15Entry = t15entry;
        }

        public T15Entry T15Entry { get; }
    }
}
