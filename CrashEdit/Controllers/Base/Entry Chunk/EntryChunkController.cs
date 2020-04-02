using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class EntryChunkController : ChunkController
    {
        public EntryChunkController(NSFController nsfcontroller,EntryChunk entrychunk) : base(nsfcontroller,entrychunk)
        {
            EntryChunk = entrychunk;
            PopulateNodes();
            AddMenu(Crash.UI.Properties.Resources.EntryChunkController_AcImport,Menu_Import_Entry);
            AddMenu(Crash.UI.Properties.Resources.EntryChunkController_AcAddNew,Menu_Add_Entry);
        }

        public void PopulateNodes()
        {
            foreach (Entry entry in EntryChunk.Entries)
            {
                AddNode(CreateEntryController(entry));
            }
        }

        public EntryChunk EntryChunk { get; }

        protected override Control CreateEditor() => new EntryChunkBox(this);

        internal EntryController CreateEntryController(Entry entry)
        {
            if (entry is ProtoAnimationEntry)
            {
                return new ProtoAnimationEntryController(this, (ProtoAnimationEntry)entry);
            }
            else if (entry is OldAnimationEntry)
            {
                return new OldAnimationEntryController(this,(OldAnimationEntry)entry);
            }
            else if (entry is OldModelEntry)
            {
                return new OldModelEntryController(this,(OldModelEntry)entry);
            }
            else if (entry is ModelEntry)
            {
                return new ModelEntryController(this, (ModelEntry)entry);
            }
            else if (entry is AnimationEntry)
            {
                return new AnimationEntryController(this, (AnimationEntry)entry);
            }
            else if (entry is ProtoSceneryEntry)
            {
                return new ProtoSceneryEntryController(this,(ProtoSceneryEntry)entry);
            }
            else if (entry is OldSceneryEntry)
            {
                return new OldSceneryEntryController(this,(OldSceneryEntry)entry);
            }
            else if (entry is SceneryEntry)
            {
                return new SceneryEntryController(this,(SceneryEntry)entry);
            }
            else if (entry is NewSceneryEntry)
            {
                return new NewSceneryEntryController(this,(NewSceneryEntry)entry);
            }
            else if (entry is SLSTEntry)
            {
                return new SLSTEntryController(this,(SLSTEntry)entry);
            }
            else if (entry is OldSLSTEntry)
            {
                return new OldSLSTEntryController(this,(OldSLSTEntry)entry);
            }
            else if (entry is T6Entry)
            {
                return new T6EntryController(this,(T6Entry)entry);
            }
            else if (entry is ProtoZoneEntry)
            {
                return new ProtoZoneEntryController(this,(ProtoZoneEntry)entry);
            }
            else if (entry is OldZoneEntry)
            {
                return new OldZoneEntryController(this,(OldZoneEntry)entry);
            }
            else if (entry is ZoneEntry)
            {
                return new ZoneEntryController(this,(ZoneEntry)entry);
            }
            else if (entry is NewZoneEntry)
            {
                return new NewZoneEntryController(this,(NewZoneEntry)entry);
            }
            else if (entry is GOOLEntry)
            {
                return new GOOLEntryController(this,(GOOLEntry)entry);
            }
            else if (entry is SoundEntry)
            {
                return new SoundEntryController(this,(SoundEntry)entry);
            }
            else if (entry is OldMusicEntry)
            {
                return new OldMusicEntryController(this,(OldMusicEntry)entry);
            }
            else if (entry is MusicEntry)
            {
                return new MusicEntryController(this,(MusicEntry)entry);
            }
            else if (entry is WavebankEntry)
            {
                return new WavebankEntryController(this,(WavebankEntry)entry);
            }
            else if (entry is ImageEntry)
            {
                return new ImageEntryController(this,(ImageEntry)entry);
            }
            else if (entry is T15Entry)
            {
                return new T15EntryController(this,(T15Entry)entry);
            }
            else if (entry is MapEntry)
            {
                return new MapEntryController(this,(MapEntry)entry);
            }
            else if (entry is T17Entry)
            {
                return new T17EntryController(this,(T17Entry)entry);
            }
            else if (entry is PaletteEntry)
            {
                return new PaletteEntryController(this,(PaletteEntry)entry);
            }
            else if (entry is DemoEntry)
            {
                return new DemoEntryController(this,(DemoEntry)entry);
            }
            else if (entry is ColoredAnimationEntry)
            {
                return new ColoredAnimationEntryController(this,(ColoredAnimationEntry)entry);
            }
            else if (entry is SpeechEntry)
            {
                return new SpeechEntryController(this,(SpeechEntry)entry);
            }
            else if (entry is T21Entry)
            {
                return new T21EntryController(this,(T21Entry)entry);
            }
            else if (entry is UnprocessedEntry)
            {
                return new UnprocessedEntryController(this,(UnprocessedEntry)entry);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void Menu_Import_Entry()
        {
            byte[][] datas = FileUtil.OpenFiles(FileFilters.NSEntry,FileFilters.Any);
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
                        AddNode(CreateEntryController(processedentry));
                    }
                    else
                    {
                        EntryChunk.Entries.Add(entry);
                        AddNode(new UnprocessedEntryController(this,entry));
                    }
                }
                catch (LoadAbortedException)
                {
                }
            }
            InvalidateEditor();
        }

        private void Menu_Add_Entry()
        {
            using (NewEntryForm newentrywindow = new NewEntryForm(NSFController))
            {
                if (newentrywindow.ShowDialog(Node.TreeView.TopLevelControl) == DialogResult.OK)
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
                                    BitConv.ToInt32(items[0],0x214,newentrywindow.EID);
                                    BitConv.ToInt32(items[0],0x2E0,0x53);
                                    BitConv.ToInt32(items[0],0x304,Entry.NullEID);
                                    BitConv.ToInt16(items[0],0x318,-4096); // 0xF000
                                    BitConv.ToInt16(items[0],0x31A,0x800);
                                    BitConv.ToInt16(items[0],0x31C,0x1000);
                                    BitConv.ToInt16(items[0],0x31E,-3563); // 0xF000
                                    BitConv.ToInt16(items[0],0x320,0x800);
                                    BitConv.ToInt16(items[0],0x322,0x1000);
                                    BitConv.ToInt16(items[0],0x324,0x1000);
                                    BitConv.ToInt16(items[0],0x326,-2048); // 0xF800
                                    BitConv.ToInt16(items[0],0x328,0);
                                    BitConv.ToInt16(items[0],0x32A,0x400);
                                    BitConv.ToInt16(items[0],0x32C,0x400);
                                    BitConv.ToInt16(items[0],0x32E,0x400);
                                    for (int i = 0; i < 12; ++i)
                                    {
                                        BitConv.ToInt16(items[0],0x330+i*2,0xFF);
                                        BitConv.ToInt16(items[0],0x360+i*2,0xFF);
                                    }
                                    BitConv.ToInt16(items[0],0x348,-4096); // 0xF000
                                    BitConv.ToInt16(items[0],0x34A,0x800);
                                    BitConv.ToInt16(items[0],0x34C,0x1000);
                                    BitConv.ToInt16(items[0],0x34E,-3563); // 0xF000
                                    BitConv.ToInt16(items[0],0x350,0x800);
                                    BitConv.ToInt16(items[0],0x352,0x1000);
                                    BitConv.ToInt16(items[0],0x354,0x1000);
                                    BitConv.ToInt16(items[0],0x356,-2048); // 0xF800
                                    BitConv.ToInt16(items[0],0x358,0);
                                    BitConv.ToInt16(items[0],0x35A,0x266);
                                    BitConv.ToInt16(items[0],0x35C,0x266);
                                    BitConv.ToInt16(items[0],0x35E,0x266);
                                    BitConv.ToInt16(items[0],0x36C,0x64);
                                    BitConv.ToInt16(items[0],0x36E,0x64);
                                    newentry = new OldZoneEntry(items[0],items[1],new OldCamera[0],new OldEntity[0],newentrywindow.EID);
                                    ((OldZoneEntry)newentry).HeaderCount = 2;
                                    ((OldZoneEntry)newentry).ZoneCount = 1;
                                    //newentry = ((OldZoneEntryLoader)loaders[newentry.Type]).Load(new List<byte[]>(newentry.Unprocess().Items).ToArray(),newentry.EID);
                                    break;
                            }
                            break;
                        case 11:
                            items[0] = new byte[0x18];
                            BitConv.ToInt32(items[0],0x4,0x100); // default category: 1 (0 is Crash?)
                            BitConv.ToInt32(items[0],0x8,1); // default format: complete GOOL entry
                            BitConv.ToInt32(items[0],0xC,(int)ObjectFields.mem4); // mem0-3 can be used as local variables
                            var goolver = GOOLInterpreter.GetVersion(NSFController.GameVersion);
                            newentry = new GOOLEntry(goolver,
                                items[0],
                                goolver == GOOLVersion.Version2 || goolver == GOOLVersion.Version3 ? new byte[4] { 0x0, 0x40, 0x89, 0x31 } : new byte[4] { 0x0, 0x40, 0x89, 0x82 }, // RET instruction
                                new int[1] { newentrywindow.EID },
                                new short[1] { 0 },
                                new List<GOOLStateDescriptor>() { new GOOLStateDescriptor(0x1,0x0,0x0,0x3FFF,0x3FFF,0x0) },
                                new byte[0],
                                newentrywindow.EID);
                            break;
                    }
                    if (newentry == null)
                        throw new Exception("An error occurred making a new entry.");
                    else
                    {
                        EntryChunk.Entries.Add(newentry);
                        AddNode(CreateEntryController(newentry));
                        Editor.Invalidate();
                    }
                }
            }
        }
    }
}
