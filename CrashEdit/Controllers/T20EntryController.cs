using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class T20EntryController : MysteryMultiItemEntryController
    {
        private T20Entry t20entry;

        public T20EntryController(EntryChunkController entrychunkcontroller,T20Entry t20entry) : base(entrychunkcontroller,t20entry)
        {
            this.t20entry = t20entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format("T20 Entry ({0})",t20entry.EName);
            NodeImageKey = "ThingOrange";
        }

        public T20Entry T20Entry
        {
            get { return t20entry; }
        }
    }
}
