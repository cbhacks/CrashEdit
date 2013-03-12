using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class MysteryUniItemEntryController : EntryController
    {
        private MysteryUniItemEntry mysteryentry;

        public MysteryUniItemEntryController(EntryChunkController entrychunkcontroller,MysteryUniItemEntry mysteryentry) : base(entrychunkcontroller,mysteryentry)
        {
            this.mysteryentry = mysteryentry;
            MysteryBox box = new MysteryBox(mysteryentry);
            box.Dock = DockStyle.Fill;
            Editor = box;
        }

        public MysteryUniItemEntry MysteryEntry
        {
            get { return mysteryentry; }
        }
    }
}
