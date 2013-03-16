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
        }

        protected override Control CreateEditor()
        {
            MysteryMultiItemEntryBox box = new MysteryMultiItemEntryBox(mysteryentry);
            box.Dock = DockStyle.Fill;
            return box;
        }

        public MysteryMultiItemEntry MysteryEntry
        {
            get { return mysteryentry; }
        }
    }
}
