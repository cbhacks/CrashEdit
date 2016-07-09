using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SLSTItem0Box : UserControl
    {
        private ListBox lstValues;

        public SLSTItem0Box(SLSTItem0 slstitem)
        {
            lstValues = new ListBox();
            lstValues.Dock = DockStyle.Fill;
            lstValues.Items.Add(string.Format("Count: {0}",slstitem.Values.Count));
            lstValues.Items.Add(string.Format("Type: {0}",slstitem.Unknown1));
            foreach (SLSTPolygonID value in slstitem.Values)
            {
                lstValues.Items.Add(string.Format("Polygon {0} - World {1}",value.ID,value.World));
            }
            Controls.Add(lstValues);
        }
    }
}
