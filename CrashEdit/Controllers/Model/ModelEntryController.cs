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

        protected override Control CreateEditor()
        {
            if (modelentry.Positions != null)
                return new AnimationEntryViewer(modelentry);
            else
            {
                Label label = new Label();
                label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                label.Text = "No options available";
                return label;
            }
        }

        public ModelEntry ModelEntry
        {
            get { return modelentry; }
        }
    }
}
