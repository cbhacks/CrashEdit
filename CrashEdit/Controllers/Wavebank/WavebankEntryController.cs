using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class WavebankEntryController : EntryController
    {
        public WavebankEntryController(EntryChunkController entrychunkcontroller,WavebankEntry wavebankentry) : base(entrychunkcontroller,wavebankentry)
        {
            WavebankEntry = wavebankentry;
        }

        public WavebankEntry WavebankEntry { get; }
    }
}
