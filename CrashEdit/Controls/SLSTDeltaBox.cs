using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SLSTDeltaBox : UserControl
    {
        private ListBox lstValues;

        public SLSTDeltaBox(SLSTDelta slstitem)
        {
            lstValues = new ListBox
            {
                Dock = DockStyle.Fill
            };
            lstValues.Items.Add(string.Format("Remove Nodes: {0}",slstitem.RemoveNodes.Count));
            lstValues.Items.Add(string.Format("Add Nodes: {0}",slstitem.AddNodes.Count));
            lstValues.Items.Add(string.Format("Swap Nodes: {0}",slstitem.SwapNodes.Count));
            Controls.Add(lstValues);
        }
    }
}
