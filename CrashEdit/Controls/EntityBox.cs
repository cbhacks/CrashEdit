using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class EntityBox : UserControl
    {
        private Entity entity;

        private TabControl tbcTabs;

        public EntityBox(Entity entity)
        {
            this.entity = entity;

            tbcTabs = new TabControl();
            tbcTabs.Dock = DockStyle.Fill;
            foreach (EntityField field in entity.Fields)
            {
                TabPage tab = new TabPage(field.Type.ToString("X"));
                ListBox list = new ListBox();
                list.Dock = DockStyle.Fill;
                list.Items.Add(string.Format("Unknown 1: {0}",field.Unknown1));
                list.Items.Add(string.Format("Element Size: {0}",field.ElementSize));
                list.Items.Add(string.Format("Unknown 2: {0}",field.Unknown2));
                list.Items.Add(string.Format("Element Count: {0}",field.ElementCount));
                list.Items.Add(string.Format("Unknown 3: {0}",field.Unknown3));
                MysteryBox mysterybox = new MysteryBox(field.Data);
                mysterybox.Dock = DockStyle.Fill;
                MysteryBox extramysterybox = new MysteryBox(field.ExtraData);
                extramysterybox.Dock = DockStyle.Fill;
                TabControl tbcData = new TabControl();
                tbcData.Dock = DockStyle.Fill;
                TabPage datatab = new TabPage("Data");
                datatab.Controls.Add(mysterybox);
                tbcData.TabPages.Add(datatab);
                TabPage extradatatab = new TabPage("Extra Data");
                extradatatab.Controls.Add(extramysterybox);
                tbcData.TabPages.Add(extradatatab);
                SplitContainer split = new SplitContainer();
                split.Dock = DockStyle.Fill;
                split.Orientation = Orientation.Horizontal;
                split.SplitterDistance = 20;
                split.Panel1.Controls.Add(list);
                split.Panel2.Controls.Add(tbcData);
                tab.Controls.Add(split);
                tbcTabs.TabPages.Add(tab);
            }

            Controls.Add(tbcTabs);
        }
    }
}
