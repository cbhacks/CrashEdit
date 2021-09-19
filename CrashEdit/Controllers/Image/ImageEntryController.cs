using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class ImageEntryController : MysteryMultiItemEntryController
    {
        public ImageEntryController(EntryChunkController entrychunkcontroller,ImageEntry imageentry) : base(entrychunkcontroller,imageentry)
        {
            ImageEntry = imageentry;
        }

        public ImageEntry ImageEntry { get; }
    }
}
