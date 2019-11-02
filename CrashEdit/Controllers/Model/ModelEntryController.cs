using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ModelEntryController : EntryController
    {
        public ModelEntryController(EntryChunkController entrychunkcontroller,ModelEntry modelentry) : base(entrychunkcontroller,modelentry)
        {
            ModelEntry = modelentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Model ({0})",ModelEntry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        protected override Control CreateEditor()
        {
            if (ModelEntry.Positions != null)
                return new AnimationEntryViewer(ModelEntry);
            else
            {
                Label label = new Label
                {
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Text = "No options available"
                };
                return label;
            }
        }

        public ModelEntry ModelEntry { get; }
    }
}
