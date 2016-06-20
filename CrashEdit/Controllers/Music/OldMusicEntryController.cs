using Crash;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class OldMusicEntryController : EntryController
    {
        private OldMusicEntry oldmusicentry;

        public OldMusicEntryController(EntryChunkController entrychunkcontroller,OldMusicEntry oldmusicentry) : base(entrychunkcontroller,oldmusicentry)
        {
            this.oldmusicentry = oldmusicentry;
            AddNode(new OldVHController(this,oldmusicentry.VH));
            foreach (SEQ seq in oldmusicentry.SEP.SEQs)
            {
                AddNode(new OldSEQController(this,seq));
            }
            AddMenuSeparator();
            AddMenu("Import SEQ",Menu_Import_SEQ);
            AddMenuSeparator();
            AddMenu("Export SEP",Menu_Export_SEP);
            AddMenuSeparator();
            AddMenu("Export Linked VB",Menu_Export_Linked_VB);
            AddMenu("Export Linked VAB",Menu_Export_Linked_VAB);
            AddMenu("Export Linked VAB as DLS",Menu_Export_Linked_VAB_DLS);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Music ({0})",oldmusicentry.EName);
            Node.ImageKey = "music";
            Node.SelectedImageKey = "music";
        }

        public OldMusicEntry OldMusicEntry
        {
            get { return oldmusicentry; }
        }

        private SampleLine[] FindLinkedVB()
        {
            List<SampleLine> samples = new List<SampleLine>();
            WavebankEntry vb0entry = FindEID<WavebankEntry>(oldmusicentry.VB0EID);
            WavebankEntry vb1entry = FindEID<WavebankEntry>(oldmusicentry.VB1EID);
            WavebankEntry vb2entry = FindEID<WavebankEntry>(oldmusicentry.VB2EID);
            WavebankEntry vb3entry = FindEID<WavebankEntry>(oldmusicentry.VB3EID);
            if (vb0entry != null)
                samples.AddRange(vb0entry.Samples.SampleLines);
            if (vb1entry != null)
                samples.AddRange(vb1entry.Samples.SampleLines);
            if (vb2entry != null)
                samples.AddRange(vb2entry.Samples.SampleLines);
            if (vb3entry != null)
                samples.AddRange(vb3entry.Samples.SampleLines);
            return samples.ToArray();
        }

        private VAB FindLinkedVAB()
        {
            SampleLine[] vb = FindLinkedVB();
            return VAB.Join(oldmusicentry.VH,vb);
        }

        private void Menu_Import_SEQ()
        {
            byte[] data = FileUtil.OpenFile(FileFilters.SEQ,FileFilters.Any);
            if (data != null)
            {
                SEQ seq = SEQ.Load(data);
                oldmusicentry.SEP.SEQs.Add(seq);
                AddNode(new OldSEQController(this,seq));
            }
        }

        private void Menu_Export_SEP()
        {
            byte[] data = oldmusicentry.SEP.Save();
            FileUtil.SaveFile(data,FileFilters.SEP,FileFilters.Any);
        }

        private void Menu_Export_Linked_VB()
        {
            SampleLine[] vb = FindLinkedVB();
            byte[] data = new SampleSet(vb).Save();
            FileUtil.SaveFile(data,FileFilters.VB,FileFilters.Any);
        }

        private void Menu_Export_Linked_VAB()
        {
            VAB vab = FindLinkedVAB();
            byte[] data = vab.Save();
            FileUtil.SaveFile(data,FileFilters.VAB,FileFilters.Any);
        }

        private void Menu_Export_Linked_VAB_DLS()
        {
            if (MessageBox.Show("Exporting to DLS is experimental.\n\nContinue anyway?","Export Linked VAB as DLS",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            VAB vab = FindLinkedVAB();
            byte[] data = vab.ToDLS().Save();
            FileUtil.SaveFile(data,FileFilters.DLS,FileFilters.Any);
        }
    }
}
