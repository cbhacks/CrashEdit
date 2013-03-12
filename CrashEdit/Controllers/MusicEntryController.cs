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
            AddMenu("Import VH",Menu_Import_VH);
            AddMenu("Import SEQ",Menu_Import_SEQ);
            AddMenuSeparator();
            AddMenu("Export VH - VH",Menu_Export_VH);
            AddMenu("Export SEP - SEP",Menu_Export_SEP);
        }

        public MusicEntry MusicEntry
        {
            get { return musicentry; }
        }

        private void Menu_Import_VH(object sender,EventArgs e)
        {
            if (musicentry.VH != null)
            {
                if (MessageBox.Show("This music entry already contains a VH file. This operation will replace the VH file.","Import VH",MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    return;
                }
            }
            byte[] data = FileUtil.OpenFile(FileUtil.VHFilter + "|" + FileUtil.VABFilter + "|" + FileUtil.AnyFilter);
            if (data != null)
            {
                musicentry.VH = VH.Load(data);
            }
        }

        private void Menu_Import_SEQ(object sender,EventArgs e)
        {
            byte[] data = FileUtil.OpenFile(FileUtil.SEQFilter + "|" + FileUtil.AnyFilter);
            if (data != null)
            {
                SEQ seq = SEQ.Load(data);
                musicentry.SEP.SEQs.Add(seq);
                AddNode(new SEQController(this,seq));
            }
        }

        private void Menu_Export_VH(object sender,EventArgs e)
        {
            if (musicentry.VH == null)
            {
                MessageBox.Show("This music entry does not contain a VH file.","Export VH");
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
