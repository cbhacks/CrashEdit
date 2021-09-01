using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class T11EntryController : MysteryMultiItemEntryController
    {
        private T11Entry t11entry;

        public T11EntryController(EntryChunkController entrychunkcontroller,T11Entry t11entry) : base(entrychunkcontroller,t11entry)
        {
            this.t11entry = t11entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format("T11 Entry ({0})",t11entry.EName);
            NodeImageKey = "ThingOrange";
        }

        public T11Entry T11Entry
        {
            get { return t11entry; }
        }
    }
}
