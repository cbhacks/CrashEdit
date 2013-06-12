using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    [EditorControl(typeof(T4Item))]
    public sealed class T4ItemBox : UserControl
    {
        private ListBox lstValues;

        public T4ItemBox(T4Item t4item)
        {
            lstValues = new ListBox();
            lstValues.Dock = DockStyle.Fill;
            lstValues.Items.Add(string.Format("Unknown1: {0}",t4item.Unknown1));
            foreach (short value in t4item.Values)
            {
                lstValues.Items.Add(value.ToString());
            }
            Controls.Add(lstValues);
        }
    }
}
