using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class T4Item0Box : UserControl
    {
        private ListBox lstValues;

        public T4Item0Box(T4Item0 t4item)
        {
            lstValues = new ListBox();
            lstValues.Dock = DockStyle.Fill;
            lstValues.Items.Add(string.Format("Count: {0}",t4item.Values.Count));
            lstValues.Items.Add(string.Format("Type: {0}",t4item.Unknown1));
            foreach (T4PolygonID value in t4item.Values)
            {
                lstValues.Items.Add(string.Format("Polygon {0} - World {1}",value.ID,value.World));
            }
            Controls.Add(lstValues);
        }
    }
}
