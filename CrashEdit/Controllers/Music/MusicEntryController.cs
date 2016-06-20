using Crash;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class MusicEntryController : EntryController
    {
        private MusicEntry musicentry;

        public MusicEntryController(EntryChunkController entrychunkcontroller,MusicEntry musicentry) : base(entrychunkcontroller,musicentry)
        {
            this.musicentry = musicentry;
            if (musicentry.VH != null)
            {
                AddNode(new VHController(this,musicentry.VH));
            }
            foreach (SEQ seq in musicentry.SEP.SEQs)
            {
                AddNode(new SEQController(this,seq));
            }
            AddMenuSeparator();
            AddMenu("Import VH",Menu_Import_VH);
            AddMenu("Import SEQ",Menu_Import_SEQ);
            AddMenuSeparator();
            AddMenu("Export SEP",Menu_Export_SEP);
            AddMenuSeparator();
            AddMenu("Export Linked VH",Menu_Export_Linked_VH);
            AddMenu("Export Linked VB",Menu_Export_Linked_VB);
            AddMenu("Export Linked VAB",Menu_Export_Linked_VAB);
            AddMenu("Export Linked VAB as DLS",Menu_Export_Linked_VAB_DLS);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Music ({0})",musicentry.EName);
            Node.ImageKey = "music";
            Node.SelectedImageKey = "music";
        }

        public MusicEntry MusicEntry
        {
            get { return musicentry; }
        }

        private VH FindLinkedVH()
        {
            MusicEntry vhentry = FindEID<MusicEntry>(musicentry.VHEID);
            if (vhentry == null)
            {
                throw new GUIException("The linked music entry could not be found.");
            }
            if (vhentry.VH == null)
            {
                throw new GUIException("The linked music entry was found but does not contain a VH file.");
            }
            return vhentry.VH;
        }

        private SampleLine[] FindLinkedVB()
        {
            List<SampleLine> samples = new List<SampleLine>();
            WavebankEntry vb0entry = FindEID<WavebankEntry>(musicentry.VB0EID);
            WavebankEntry vb1entry = FindEID<WavebankEntry>(musicentry.VB1EID);
            WavebankEntry vb2entry = FindEID<WavebankEntry>(musicentry.VB2EID);
            WavebankEntry vb3entry = FindEID<WavebankEntry>(musicentry.VB3EID);
            WavebankEntry vb4entry = FindEID<WavebankEntry>(musicentry.VB4EID);
            WavebankEntry vb5entry = FindEID<WavebankEntry>(musicentry.VB5EID);
            WavebankEntry vb6entry = FindEID<WavebankEntry>(musicentry.VB6EID);
            if (vb0entry != null)
                samples.AddRange(vb0entry.Samples.SampleLines);
            if (vb1entry != null)
                samples.AddRange(vb1entry.Samples.SampleLines);
            if (vb2entry != null)
                samples.AddRange(vb2entry.Samples.SampleLines);
            if (vb3entry != null)
                samples.AddRange(vb3entry.Samples.SampleLines);
            if (vb4entry != null)
                samples.AddRange(vb4entry.Samples.SampleLines);
            if (vb5entry != null)
                samples.AddRange(vb5entry.Samples.SampleLines);
            if (vb6entry != null)
                samples.AddRange(vb6entry.Samples.SampleLines);
            return samples.ToArray();
        }

        private VAB FindLinkedVAB()
        {
            VH vh = FindLinkedVH();
            SampleLine[] vb = FindLinkedVB();
            return VAB.Join(vh,vb);
        }

        private void Menu_Import_VH()
        {
            if (musicentry.VH != null)
            {
                throw new GUIException("This music entry already contains a VH file.");
            }
            byte[] data = FileUtil.OpenFile(FileFilters.VH,FileFilters.VAB,FileFilters.Any);
            if (data != null)
            {
                VH vh = VH.Load(data);
                musicentry.VH = vh;
                InsertNode(0,new VHController(this,vh));
            }
        }

        private void Menu_Import_SEQ()
        {
            byte[] data = FileUtil.OpenFile(FileFilters.SEQ,FileFilters.Any);
            if (data != null)
            {
                SEQ seq = SEQ.Load(data);
                musicentry.SEP.SEQs.Add(seq);
                AddNode(new SEQController(this,seq));
            }
        }

        private void Menu_Export_SEP()
        {
            byte[] data = musicentry.SEP.Save();
            FileUtil.SaveFile(data,FileFilters.SEP,FileFilters.Any);
        }

        private void Menu_Export_Linked_VH()
        {
            VH vh = FindLinkedVH();
            byte[] data = vh.Save();
            FileUtil.SaveFile(data,FileFilters.VH,FileFilters.Any);
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
