using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class EntryChunkController : ChunkController
    {
        private EntryChunk entrychunk;

        public EntryChunkController(NSFController nsfcontroller,EntryChunk entrychunk) : base(nsfcontroller,entrychunk)
        {
            this.entrychunk = entrychunk;
            foreach (Entry entry in entrychunk.Entries)
            {
                AddNode(CreateEntryController(entry));
            }
            AddMenu("Import Entry",Menu_Import_Entry);
        }

        public EntryChunk EntryChunk
        {
            get { return entrychunk; }
        }

        protected override Control CreateEditor()
        {
            return new EntryChunkBox(this);
        }

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
            else if (entry is T1Entry)
            {
                return new T1EntryController(this,(T1Entry)entry);
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
            else if (entry is T11Entry)
            {
                return new T11EntryController(this,(T11Entry)entry);
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
            else if (entry is OldT15Entry)
            {
                return new OldT15EntryController(this,(OldT15Entry)entry);
            }
            else if (entry is T15Entry)
            {
                return new T15EntryController(this,(T15Entry)entry);
            }
            else if (entry is OldT17Entry)
            {
                return new OldT17EntryController(this,(OldT17Entry)entry);
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
            else if (entry is CutsceneAnimationEntry)
            {
                return new CutsceneAnimationEntryController(this,(CutsceneAnimationEntry)entry);
            }
            else if (entry is SpeechEntry)
            {
                return new SpeechEntryController(this,(SpeechEntry)entry);
            }
            else if (entry is T21Entry)
            {
                return new T21EntryController(this,(T21Entry)entry);
            }
            else if (entry is T2Entry)
            {
                return new T2EntryController(this, (T2Entry)entry);
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
            byte[] data = FileUtil.OpenFile(FileFilters.NSEntry,FileFilters.Any);
            if (data == null)
                return;
            try
            {
                UnprocessedEntry entry = Entry.Load(data);
                entrychunk.Entries.Add(entry);
                AddNode(new UnprocessedEntryController(this,entry));
            }
            catch (LoadAbortedException)
            {
            }
        }
    }
}
