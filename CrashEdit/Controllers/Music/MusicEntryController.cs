using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(MusicEntry))]
    public sealed class MusicEntryController : EntryController
    {
        public MusicEntryController(MusicEntry musicentry, SubcontrollerGroup parentGroup) : base(musicentry, parentGroup)
        {
            MusicEntry = musicentry;
            AddMenuSeparator();
            AddMenu("Export Linked VH", Menu_Export_Linked_VH);
            AddMenu("Export Linked VB", Menu_Export_Linked_VB);
            AddMenu("Export Linked VAB", Menu_Export_Linked_VAB);
            AddMenu("Export Linked VAB as DLS", Menu_Export_Linked_VAB_DLS);
            AddMenuSeparator();
            AddMenu("Replace Linked VB", Menu_Replace_Linked_VB);
            AddMenu("Replace Linked VAB", Menu_Replace_Linked_VAB);
        }

        public MusicEntry MusicEntry { get; }

        private VH FindLinkedVH()
        {
            MusicEntry vhentry = FindEID<MusicEntry>(MusicEntry.VHEID);
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
            WavebankEntry vb0entry = FindEID<WavebankEntry>(MusicEntry.VB0EID);
            WavebankEntry vb1entry = FindEID<WavebankEntry>(MusicEntry.VB1EID);
            WavebankEntry vb2entry = FindEID<WavebankEntry>(MusicEntry.VB2EID);
            WavebankEntry vb3entry = FindEID<WavebankEntry>(MusicEntry.VB3EID);
            WavebankEntry vb4entry = FindEID<WavebankEntry>(MusicEntry.VB4EID);
            WavebankEntry vb5entry = FindEID<WavebankEntry>(MusicEntry.VB5EID);
            WavebankEntry vb6entry = FindEID<WavebankEntry>(MusicEntry.VB6EID);
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
            return VAB.Join(vh, vb);
        }

        private void Menu_Export_Linked_VH()
        {
            VH vh = FindLinkedVH();
            byte[] data = vh.Save();
            FileUtil.SaveFile(data, FileFilters.VH, FileFilters.Any);
        }

        private void Menu_Export_Linked_VB()
        {
            SampleLine[] vb = FindLinkedVB();
            byte[] data = new SampleSet(vb).Save();
            FileUtil.SaveFile(data, FileFilters.VB, FileFilters.Any);
        }

        private void Menu_Export_Linked_VAB()
        {
            VAB vab = FindLinkedVAB();
            byte[] data = vab.Save();
            FileUtil.SaveFile(data, FileFilters.VAB, FileFilters.Any);
        }

        private void Menu_Export_Linked_VAB_DLS()
        {
            if (MessageBox.Show("Exporting to DLS is experimental.\n\nContinue anyway?", "Export Linked VAB as DLS", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            VAB vab = FindLinkedVAB();
            byte[] data = vab.ToDLS().Save();
            FileUtil.SaveFile(data, FileFilters.DLS, FileFilters.Any);
        }

        private void Menu_Replace_Linked_VB()
        {
            byte[] data = FileUtil.OpenFile(FileFilters.VB, FileFilters.Any);
            if (data == null) return;
            ReplaceLinkedVB(SampleSet.Load(data).SampleLines);
        }

        private void Menu_Replace_Linked_VAB()
        {
            try
            {
                byte[] vab_data = FileUtil.OpenFile(FileFilters.VAB, FileFilters.Any);

                if (vab_data == null) throw new LoadAbortedException();

                VH vh = VH.Load(vab_data);

                int vb_offset = 2592 + 32 * 16 * vh.Programs.Count;
                if ((vab_data.Length - vb_offset) % 16 != 0)
                {
                    ErrorManager.SignalIgnorableError("extra feature: VB size is invalid");
                }
                vh.VBSize = (vab_data.Length - vb_offset) / 16;
                var vb = new List<SampleLine>();
                byte[] line_data = new byte[16];
                for (int i = 0; i < vh.VBSize; i++)
                {
                    Array.Copy(vab_data, vb_offset + i * 16, line_data, 0, 16);
                    vb.Add(SampleLine.Load(line_data));
                }

                MusicEntry vhentry = FindEID<MusicEntry>(MusicEntry.VHEID);
                if (vhentry == null)
                {
                    throw new GUIException("The linked music entry could not be found.");
                }
                if (vhentry.VH == null)
                {
                    throw new GUIException("The linked music entry was found but does not contain a VH file.");
                }

                vhentry.VH = vh;
                ReplaceLinkedVB(vb);
            }
            catch (LoadAbortedException)
            {
            }
        }

        private void ReplaceLinkedVB(List<SampleLine> samples)
        {
            var vbEntries = new List<WavebankEntry>();
            WavebankEntry vb0entry = FindEID<WavebankEntry>(MusicEntry.VB0EID);
            WavebankEntry vb1entry = FindEID<WavebankEntry>(MusicEntry.VB1EID);
            WavebankEntry vb2entry = FindEID<WavebankEntry>(MusicEntry.VB2EID);
            WavebankEntry vb3entry = FindEID<WavebankEntry>(MusicEntry.VB3EID);
            WavebankEntry vb4entry = FindEID<WavebankEntry>(MusicEntry.VB4EID);
            WavebankEntry vb5entry = FindEID<WavebankEntry>(MusicEntry.VB5EID);
            WavebankEntry vb6entry = FindEID<WavebankEntry>(MusicEntry.VB6EID);
            if (vb0entry != null)
                vbEntries.Add(vb0entry);
            if (vb1entry != null)
                vbEntries.Add(vb1entry);
            if (vb2entry != null)
                vbEntries.Add(vb2entry);
            if (vb3entry != null)
                vbEntries.Add(vb3entry);
            if (vb4entry != null)
                vbEntries.Add(vb4entry);
            if (vb5entry != null)
                vbEntries.Add(vb5entry);
            if (vb6entry != null)
                vbEntries.Add(vb6entry);

            foreach (var vbEntry in vbEntries)
            {
                vbEntry.Samples.SampleLines.Clear();
                if (samples.Count > 0)
                {
                    if (samples.Count <= WavebankEntry.MaxSampleLines)
                    {
                        vbEntry.Samples.SampleLines.AddRange(samples);
                        samples.Clear();
                    }
                    else
                    {
                        vbEntry.Samples.SampleLines.AddRange(samples.GetRange(0, WavebankEntry.MaxSampleLines));
                        samples.RemoveRange(0, WavebankEntry.MaxSampleLines);
                    }
                }
            }

            if (samples.Count > 0)
            {
                throw new GUIException("VB too large for the number of linked wavebank entries.\n\nThe imported data has been truncated.");
            }
        }

    }
}
