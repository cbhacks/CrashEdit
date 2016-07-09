using Crash;

namespace CrashEdit
{
    public sealed class SLSTEntryController : EntryController
    {
        private SLSTEntry slstentry;

        public SLSTEntryController(EntryChunkController entrychunkcontroller,SLSTEntry slstentry) : base(entrychunkcontroller,slstentry)
        {
            this.slstentry = slstentry;
            //AddNode(new SLSTItem0Controller(null,slstentry.SLSTItemFirst));
            foreach (SLSTItem slstitem in slstentry.SLSTItems)
            {
                AddNode(new SLSTItemController(this,slstitem));
            }
            //AddNode(new SLSTItem0Controller(null,slstentry.SLSTItemLast));
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("SLST ({0})",slstentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public SLSTEntry SLSTEntry
        {
            get { return slstentry; }
        }
    }
}
