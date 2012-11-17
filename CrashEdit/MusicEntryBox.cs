using Crash;
using Crash.Audio;
using System.Windows.Forms;

namespace CrashEdit
{
    [EditorControl(typeof(MusicEntry))]
    public sealed class MusicEntryBox : UserControl
    {
        private TabControl tbcTabs;

        public MusicEntryBox(MusicEntry entry)
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
                MysteryBox mystery = new MysteryBox(entry.VAB);
                mystery.Dock = DockStyle.Fill;
                TabPage page = new TabPage("VAB");
                page.Controls.Add(mystery);
                tbcTabs.TabPages.Add(page);
            }
            {
                MysteryBox mystery = new MysteryBox(entry.SEQ);
                mystery.Dock = DockStyle.Fill;
                TabPage page = new TabPage("SEQ");
                page.Controls.Add(mystery);
                tbcTabs.TabPages.Add(page);
            }
            Controls.Add(tbcTabs);
        }
    }
}
