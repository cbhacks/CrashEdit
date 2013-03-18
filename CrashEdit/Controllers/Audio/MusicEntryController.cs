using Crash;
using Crash.Audio;
using System;
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
            Node.Text = "Music Entry";
            Node.ImageKey = "musicentry";
            Node.SelectedImageKey = "musicentry";
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
            WavebankEntry vb0entry = FindEID<WavebankEntry>(musicentry.VB0EID);
            WavebankEntry vb1entry = FindEID<WavebankEntry>(musicentry.VB1EID);
            WavebankEntry vb2entry = FindEID<WavebankEntry>(musicentry.VB2EID);
            WavebankEntry vb3entry = FindEID<WavebankEntry>(musicentry.VB3EID);
            if (vb0entry == null || vb1entry == null || vb2entry == null || vb3entry == null)
            {
                throw new GUIException("One or more of the linked wavebank entries could not be found.");
            }
            List<SampleLine> samples = new List<SampleLine>();
            samples.AddRange(vb0entry.Samples.SampleLines);
            samples.AddRange(vb1entry.Samples.SampleLines);
            samples.AddRange(vb2entry.Samples.SampleLines);
            samples.AddRange(vb3entry.Samples.SampleLines);
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
            byte[] data = FileUtil.OpenFile(FileUtil.VHFilter + "|" + FileUtil.VABFilter + "|" + FileUtil.AnyFilter);
            if (data != null)
            {
                VH vh = VH.Load(data);
                musicentry.VH = vh;
                InsertNode(0,new VHController(this,vh));
            }
        }

        private void Menu_Import_SEQ()
        {
            byte[] data = FileUtil.OpenFile(FileUtil.SEQFilter + "|" + FileUtil.AnyFilter);
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
            FileUtil.SaveFile(data,FileUtil.SEPFilter + "|" + FileUtil.AnyFilter);
        }

        private void Menu_Export_Linked_VH()
        {
            VH vh = FindLinkedVH();
            byte[] data = vh.Save();
            FileUtil.SaveFile(data,FileUtil.VHFilter + "|" + FileUtil.AnyFilter);
        }

        private void Menu_Export_Linked_VB()
        {
            SampleLine[] vb = FindLinkedVB();
            byte[] data = new SampleSet(vb).Save();
            FileUtil.SaveFile(data,FileUtil.VBFilter + "|" + FileUtil.AnyFilter);
        }

        private void Menu_Export_Linked_VAB()
        {
            VAB vab = FindLinkedVAB();
            byte[] data = vab.Save();
            FileUtil.SaveFile(data,FileUtil.VABFilter + "|" + FileUtil.AnyFilter);
        }

        private void Menu_Export_Linked_VAB_DLS()
        {
            if (MessageBox.Show("Exporting to DLS is experimental.\n\nContinue anyway?","Export Linked VAB as DLS",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            VAB vab = FindLinkedVAB();
            byte[] data = vab.ToDLS().Save();
            FileUtil.SaveFile(data,FileUtil.DLSFilter + "|" + FileUtil.AnyFilter);
        }
    }
}
