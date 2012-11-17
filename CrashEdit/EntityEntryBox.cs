using Crash;
using Crash.Game;
using System.Windows.Forms;

namespace CrashEdit
{
    [EditorControl(typeof(EntityEntry))]
    public sealed class EntityEntryBox : UserControl
    {
        private TabControl tbcTabs;

        public EntityEntryBox(EntityEntry entry)
        {
            tbcTabs = new TabControl();
            tbcTabs.Dock = DockStyle.Fill;
            {
                MysteryBox mystery = new MysteryBox(entry.Unknown1);
                mystery.Dock = DockStyle.Fill;
                TabPage page = new TabPage("Unknown 1");
                page.Controls.Add(mystery);
                tbcTabs.TabPages.Add(page);
            }
            {
                MysteryBox mystery = new MysteryBox(entry.Unknown2);
                mystery.Dock = DockStyle.Fill;
                TabPage page = new TabPage("Unknown 2");
                page.Controls.Add(mystery);
                tbcTabs.TabPages.Add(page);
            }
            Controls.Add(tbcTabs);
        }
    }
}
