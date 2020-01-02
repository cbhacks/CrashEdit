using Crash;

namespace CrashEdit
{
    public sealed class ModelEntryController : EntryController
    {
        public ModelEntryController(EntryChunkController entrychunkcontroller,ModelEntry modelentry) : base(entrychunkcontroller,modelentry)
        {
            ModelEntry = modelentry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            if (ModelEntry.Positions == null)
            {
                Node.Text = string.Format("Model ({0})", ModelEntry.EName);
            }
            else
            {
                Node.Text = string.Format("Compressed Model ({0})", ModelEntry.EName);
            }
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "crimsonb";
            Node.SelectedImageKey = "crimsonb";
        }

        public ModelEntry ModelEntry { get; }
    }
}
