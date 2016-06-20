using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class T4Item1Box : UserControl
    {
        private ListBox lstValues;

        public T4Item1Box(T4Item1 t4item)
        {
            lstValues = new ListBox();
            lstValues.Dock = DockStyle.Fill;
            lstValues.Items.Add(string.Format("Count: {0}",t4item.Values.Count));
            lstValues.Items.Add(string.Format("Type: {0}",t4item.Unknown1));
            lstValues.Items.Add(string.Format("Remove Node Index: {0}",t4item.RemoveNodeIndex));
            lstValues.Items.Add(string.Format("Swap Node Index: {0}",t4item.SwapNodeIndex));
            foreach (short value in t4item.Values)
            {
                lstValues.Items.Add(string.Format("Value {0}",value));
            }
            Controls.Add(lstValues);
        }
    }
}
