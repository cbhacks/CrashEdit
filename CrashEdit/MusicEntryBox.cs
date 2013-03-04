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
                TabPage page = new TabPage("VH");
                if (entry.VH != null)
                {
                    VHBox vhbox = new VHBox(entry.VH);
                    vhbox.Dock = DockStyle.Fill;
                    page.Controls.Add(vhbox);
                }
                tbcTabs.TabPages.Add(page);
            }
            Controls.Add(tbcTabs);
        }
    }
}
