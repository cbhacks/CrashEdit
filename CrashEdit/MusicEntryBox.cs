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
            for (int i = 0;i < entry.SEP.SEQs.Count;i++)
            {
                MysteryBox mystery = new MysteryBox(entry.SEP.SEQs[i].Data);
                mystery.Dock = DockStyle.Fill;
                TabPage page = new TabPage(string.Format("SEQ {0}",i));
                page.Controls.Add(mystery);
                tbcTabs.TabPages.Add(page);
            }
            Controls.Add(tbcTabs);
        }
    }
}
