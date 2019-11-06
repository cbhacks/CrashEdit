using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldSLSTSourceBox : UserControl
    {
        private ListBox lstValues;

        public OldSLSTSourceBox(OldSLSTSource slstitem)
        {
            lstValues = new ListBox
            {
                Dock = DockStyle.Fill
            };
            lstValues.Items.Add(string.Format("Count: {0}",slstitem.Polygons.Count));
            lstValues.Items.Add(string.Format("Type: {0}",0));
            foreach (OldSLSTPolygonID value in slstitem.Polygons)
            {
                lstValues.Items.Add(string.Format("Polygon {0} (World {1})",value.ID,value.World));
            }
            Controls.Add(lstValues);
        }
    }
}
