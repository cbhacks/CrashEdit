using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class MysteryMultiItemEntryController : EntryController
    {
        private MysteryMultiItemEntry mysteryentry;

        public MysteryMultiItemEntryController(EntryChunkController entrychunkcontroller,MysteryMultiItemEntry mysteryentry) : base(entrychunkcontroller,mysteryentry)
        {
            this.mysteryentry = mysteryentry;
            MysteryMultiItemEntryBox box = new MysteryMultiItemEntryBox(mysteryentry);
            box.Dock = DockStyle.Fill;
            Editor = box;
        }

        public MysteryMultiItemEntry MysteryEntry
        {
            get { return mysteryentry; }
        }
    }
}
