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
            foreach (byte[] item in mysteryentry.Items)
            {
                AddNode(new ItemController(this,item));
            }
        }

        public MysteryMultiItemEntry MysteryEntry
        {
            get { return mysteryentry; }
        }
    }
}
