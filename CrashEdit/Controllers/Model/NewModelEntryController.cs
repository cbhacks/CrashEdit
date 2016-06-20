using Crash;

namespace CrashEdit
{
    public sealed class NewModelEntryController : MysteryMultiItemEntryController
    {
        private NewModelEntry newmodelentry;

        public NewModelEntryController(EntryChunkController entrychunkcontroller,NewModelEntry newmodelentry) : base(entrychunkcontroller,newmodelentry)
        {
            this.newmodelentry = newmodelentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Model ({0})",newmodelentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public NewModelEntry NewModelEntry
        {
            get { return newmodelentry; }
        }
    }
}
