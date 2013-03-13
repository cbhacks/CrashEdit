using Crash;
using Crash.Game;
using Crash.Graphics;
using Crash.Audio;
using Crash.Unknown0;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class MysteryMultiItemEntryBox : UserControl
    {
        private TabControl tbcTabs;

        public MysteryMultiItemEntryBox(MysteryMultiItemEntry entry)
        {
            tbcTabs = new TabControl();
            tbcTabs.Dock = DockStyle.Fill;
            foreach (byte[] item in entry.Items)
            {
                MysteryBox mysterybox = new MysteryBox(item);
                mysterybox.Dock = DockStyle.Fill;
                TabPage tab = new TabPage("Item");
                tab.Controls.Add(mysterybox);
                tbcTabs.TabPages.Add(tab);
            }
            Controls.Add(tbcTabs);
        }
    }
}
