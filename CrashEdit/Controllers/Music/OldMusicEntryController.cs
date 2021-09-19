using CrashEdit.Crash;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit.CE
{
    public sealed class OldMusicEntryController : EntryController
    {
        public OldMusicEntryController(EntryChunkController entrychunkcontroller,OldMusicEntry oldmusicentry) : base(entrychunkcontroller,oldmusicentry)
        {
            OldMusicEntry = oldmusicentry;
            AddMenuSeparator();
            AddMenu("Export Linked VB",Menu_Export_Linked_VB);
            AddMenu("Export Linked VAB",Menu_Export_Linked_VAB);
            AddMenu("Export Linked VAB as DLS",Menu_Export_Linked_VAB_DLS);
        }

        public OldMusicEntry OldMusicEntry { get; }

        private SampleLine[] FindLinkedVB()
        {
            List<SampleLine> samples = new List<SampleLine>();
            WavebankEntry vb0entry = FindEID<WavebankEntry>(OldMusicEntry.VB0EID);
            WavebankEntry vb1entry = FindEID<WavebankEntry>(OldMusicEntry.VB1EID);
            WavebankEntry vb2entry = FindEID<WavebankEntry>(OldMusicEntry.VB2EID);
            WavebankEntry vb3entry = FindEID<WavebankEntry>(OldMusicEntry.VB3EID);
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
            return VAB.Join(OldMusicEntry.VH,vb);
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
