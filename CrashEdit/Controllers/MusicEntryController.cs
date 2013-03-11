using Crash;
using Crash.Audio;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class MusicEntryController : EntryController
    {
        private MusicEntry musicentry;

        public MusicEntryController(EntryChunkController entrychunkcontroller,MusicEntry musicentry) : base(entrychunkcontroller,musicentry)
        {
            this.musicentry = musicentry;
            Node.Text = "Music Entry";
            Node.ImageKey = "musicentry";
            Node.SelectedImageKey = "musicentry";
            foreach (SEQ seq in musicentry.SEP.SEQs)
            {
                AddNode(new SEQController(this,seq));
            }
            AddMenuSeparator();
            AddMenu("Export VH - VH",Menu_Export_VH);
            AddMenu("Export SEP - SEP",Menu_Export_SEP);
        }

        public MusicEntry MusicEntry
        {
            get { return musicentry; }
        }

        private void Menu_Export_VH(object sender,EventArgs e)
        {
            if (musicentry.VH == null)
            {
                MessageBox.Show("This music entry does not contain a VH file.");
                return;
            }
            byte[] data = musicentry.VH.Save();
            FileUtil.SaveFile(data,FileUtil.VHFilter + "|" + FileUtil.AnyFilter);
        }

        private void Menu_Export_SEP(object sender,EventArgs e)
        {
            byte[] data = musicentry.SEP.Save();
            FileUtil.SaveFile(data,FileUtil.SEPFilter + "|" + FileUtil.AnyFilter);
        }
    }
}
