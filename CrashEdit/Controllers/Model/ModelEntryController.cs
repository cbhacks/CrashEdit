using CrashEdit.Crash;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit.CE
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
                NodeText = string.Format(CrashUI.Properties.Resources.ModelEntryController_Text,ModelEntry.EName);
            }
            else
            {
                NodeText = string.Format(CrashUI.Properties.Resources.ModelEntryController_Compressed_Text,ModelEntry.EName);
            }
        }

        public override void InvalidateNodeImage()
        {
            if (ModelEntry.Positions == null)
            {
                NodeImageKey = "crimsonb";
            }
            else
            {
                NodeImageKey = "redb";
            }
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            if (ModelEntry.Positions == null)
                return new Label { Text = string.Format("Polygon count: {0}\nVertex count: {1}",ModelEntry.PolyCount,ModelEntry.VertexCount), TextAlign = ContentAlignment.MiddleCenter };
            else
            {
                int totalbits = ModelEntry.Positions.Count * 8 * 3;
                int bits = 0;
                foreach (ModelPosition pos in ModelEntry.Positions)
                {
                    bits += 1+pos.XBits;
                    bits += 1+pos.YBits;
                    bits += 1+pos.ZBits;
                }
                return new Label { Text = string.Format("Polygon count: {0}\nVertex count: {1}\nCompression ratio: {2:P1} ({3}/{4})",ModelEntry.PolyCount,ModelEntry.VertexCount,(float)bits/totalbits,bits,totalbits), TextAlign = ContentAlignment.MiddleCenter };
            }
        }

        public ModelEntry ModelEntry { get; }
    }
}
