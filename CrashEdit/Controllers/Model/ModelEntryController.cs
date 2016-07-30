using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ModelEntryController : EntryController
    {
        private ModelEntry modelentry;

        public ModelEntryController(EntryChunkController entrychunkcontroller,ModelEntry modelentry) : base(entrychunkcontroller,modelentry)
        {
            this.modelentry = modelentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Model ({0})",modelentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        //protected override Control CreateEditor()
        //{
        //    return new ModelBox(modelentry);
        //}

        public ModelEntry ModelEntry
        {
            get { return modelentry; }
        }
    }
}
