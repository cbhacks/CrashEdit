using Crash;
using Crash.Game;
using Crash.Graphics;
using Crash.Audio;
using Crash.Unknown0;
using Crash.Unknown5;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    [EditorControl(typeof(T1Entry))]
    [EditorControl(typeof(T2Entry))]
    [EditorControl(typeof(T3Entry))]
    [EditorControl(typeof(T4Entry))]
    [EditorControl(typeof(T11Entry))]
    [EditorControl(typeof(T17Entry))]
    [EditorControl(typeof(T21Entry))]
    [EditorControl(typeof(UnknownEntry))]
    public sealed class MysteryMultiItemEntryBox : UserControl
    {
        private TabControl tbcTabs;

        public MysteryMultiItemEntryBox(IMysteryMultiItemEntry entry)
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
