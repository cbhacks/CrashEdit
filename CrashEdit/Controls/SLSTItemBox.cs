using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SLSTItemBox : UserControl
    {
        private ListBox lstValues;

        public SLSTItemBox(SLSTItem slstitem)
        {
            lstValues = new ListBox();
            lstValues.Dock = DockStyle.Fill;
            lstValues.Items.Add(string.Format("Count: {0}",slstitem.Values.Count));
            lstValues.Items.Add(string.Format("Type: {0}",slstitem.Unknown1));
            //lstValues.Items.Add(string.Format("Remove Node Index: {0}",slstitem.RemoveNodeIndex));
            //lstValues.Items.Add(string.Format("Swap Node Index: {0}",slstitem.SwapNodeIndex));
            foreach (short value in slstitem.Values)
            {
                lstValues.Items.Add(string.Format("Value {0}",value));
            }
            Controls.Add(lstValues);
        }
    }
}
