using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(EntryChunk))]
    public class EntryChunkController : ChunkController
    {
        public EntryChunkController(EntryChunk entrychunk, SubcontrollerGroup parentGroup) : base(entrychunk, parentGroup)
        {
            EntryChunk = entrychunk;
            AddMenu(CrashUI.Properties.Resources.EntryChunkController_AcImport, Menu_Import_Entry);
            AddMenu(CrashUI.Properties.Resources.EntryChunkController_AcAddNew, Menu_Add_Entry);
        }

        public EntryChunk EntryChunk { get; }

        public override bool EditorAvailable => true;

        public override Control CreateEditor() => new EntryChunkBox(this);

        private void Menu_Import_Entry()
        {
            byte[][] datas = FileUtil.OpenFiles(FileFilters.NSEntry, FileFilters.Any);
            if (datas == null)
                return;
            bool process = MessageBox.Show("Do you want to process the imported entries?", "Import Entry", MessageBoxButtons.YesNo) == DialogResult.Yes;
            foreach (var data in datas)
            {
                try
                {
                    UnprocessedEntry entry = Entry.Load(data);
                    if (process)
                    {
                        Entry processedentry = entry.Process(NSFController.GameVersion);
                        EntryChunk.Entries.Add(processedentry);
                    }
                    else
                    {
                        EntryChunk.Entries.Add(entry);
                    }
                }
                catch (LoadAbortedException)
                {
                }
            }
            NeedsNewEditor = true;
        }

        private void Menu_Add_Entry()
        {
            using (NewEntryForm newentrywindow = new NewEntryForm(GetNSF(), GameVersion))
            {
                if (newentrywindow.ShowDialog() == DialogResult.OK)
                {
                    //Dictionary<int, EntryLoader> loaders = Entry.GetLoaders(NSFController.GameVersion);
                    Entry newentry = null;
                    byte[][] items = new byte[6][];
                    switch (newentrywindow.Type)
                    {
                        case -1: // unprocessed
                            newentry = Entry.Load(new UnprocessedEntry(new byte[0][], newentrywindow.EID, newentrywindow.UnprocessedType).Save());
                            break;
                        case 7:
                            switch (NSFController.GameVersion)
                            {
                                case GameVersion.Crash1BetaMAR08:
                                case GameVersion.Crash1BetaMAY11:
                                case GameVersion.Crash1:
                                    items[0] = new byte[0x378];
                                    items[1] = new byte[0x24];
                                    BitConv.ToInt32(items[0], 0x214, newentrywindow.EID);
                                    BitConv.ToInt32(items[0], 0x2E0, 0x53);
                                    BitConv.ToInt32(items[0], 0x304, Entry.NullEID);
                                    BitConv.ToInt16(items[0], 0x318, -4096); // 0xF000
                                    BitConv.ToInt16(items[0], 0x31A, 0x800);
                                    BitConv.ToInt16(items[0], 0x31C, 0x1000);
                                    BitConv.ToInt16(items[0], 0x31E, -3563); // 0xF000
                                    BitConv.ToInt16(items[0], 0x320, 0x800);
                                    BitConv.ToInt16(items[0], 0x322, 0x1000);
                                    BitConv.ToInt16(items[0], 0x324, 0x1000);
                                    BitConv.ToInt16(items[0], 0x326, -2048); // 0xF800
                                    BitConv.ToInt16(items[0], 0x328, 0);
                                    BitConv.ToInt16(items[0], 0x32A, 0x400);
                                    BitConv.ToInt16(items[0], 0x32C, 0x400);
                                    BitConv.ToInt16(items[0], 0x32E, 0x400);
                                    for (int i = 0; i < 12; ++i)
                                    {
                                        BitConv.ToInt16(items[0], 0x330 + i * 2, 0xFF);
                                        BitConv.ToInt16(items[0], 0x360 + i * 2, 0xFF);
                                    }
                                    BitConv.ToInt16(items[0], 0x348, -4096); // 0xF000
                                    BitConv.ToInt16(items[0], 0x34A, 0x800);
                                    BitConv.ToInt16(items[0], 0x34C, 0x1000);
                                    BitConv.ToInt16(items[0], 0x34E, -3563); // 0xF000
                                    BitConv.ToInt16(items[0], 0x350, 0x800);
                                    BitConv.ToInt16(items[0], 0x352, 0x1000);
                                    BitConv.ToInt16(items[0], 0x354, 0x1000);
                                    BitConv.ToInt16(items[0], 0x356, -2048); // 0xF800
                                    BitConv.ToInt16(items[0], 0x358, 0);
                                    BitConv.ToInt16(items[0], 0x35A, 0x266);
                                    BitConv.ToInt16(items[0], 0x35C, 0x266);
                                    BitConv.ToInt16(items[0], 0x35E, 0x266);
                                    BitConv.ToInt16(items[0], 0x36C, 0x64);
                                    BitConv.ToInt16(items[0], 0x36E, 0x64);
                                    newentry = new OldZoneEntry(items[0], items[1], new OldCamera[0], new OldEntity[0], newentrywindow.EID);
                                    ((OldZoneEntry)newentry).HeaderCount = 2;
                                    ((OldZoneEntry)newentry).ZoneCount = 1;
                                    //newentry = ((OldZoneEntryLoader)loaders[newentry.Type]).Load(new List<byte[]>(newentry.Unprocess().Items).ToArray(),newentry.EID);
                                    break;
                            }
                            break;
                        case 11:
                            items[0] = new byte[0x18];
                            BitConv.ToInt32(items[0], 0x4, 0x100); // default category: 1 (0 is Crash?)
                            BitConv.ToInt32(items[0], 0x8, 1); // default format: complete GOOL entry
                            BitConv.ToInt32(items[0], 0xC, (int)ObjectFields.mem4); // mem0-3 can be used as local variables
                            var goolver = GOOLInterpreter.GetVersion(NSFController.GameVersion);
                            newentry = new GOOLEntry(goolver,
                                items[0],
                                goolver == GOOLVersion.Version2 || goolver == GOOLVersion.Version3 ? new byte[4] { 0x0, 0x40, 0x89, 0x31 } : new byte[4] { 0x0, 0x40, 0x89, 0x82 }, // RET instruction
                                new int[1] { newentrywindow.EID },
                                new short[1] { 0 },
                                new List<GOOLStateDescriptor>() { new GOOLStateDescriptor(0x1, 0x0, 0x0, 0x3FFF, 0x3FFF, 0x0) },
                                new byte[0],
                                newentrywindow.EID);
                            break;
                    }
                    if (newentry == null)
                        throw new Exception("An error occurred making a new entry.");
                    else
                    {
                        EntryChunk.Entries.Add(newentry);
                        NeedsNewEditor = true;
                    }
                }
            }
        }
    }
}
