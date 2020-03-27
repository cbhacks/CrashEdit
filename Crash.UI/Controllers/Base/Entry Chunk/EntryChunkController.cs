using System;

namespace Crash.UI
{
    public abstract class EntryChunkController : ChunkController
    {
        public EntryChunkController(NSFController up,EntryChunk chunk) : base(up,chunk)
        {
            Chunk = chunk;
            Chunk.Entries.ItemAdded += new EvListEventHandler<Entry>(Entries_ItemAdded);
            Chunk.Entries.ItemRemoved += new EvListEventHandler<Entry>(Entries_ItemRemoved);
            Chunk.Entries.Populate(Entries_ItemAdded);
        }

        public new EntryChunk Chunk { get; }

        private void Entries_ItemAdded(object sender,EvListEventArgs<Entry> e)
        {
            if (e.Item is OldAnimationEntry)
            {
                Subcontrollers.Insert(e.Index,new OldAnimationEntryController(this,(OldAnimationEntry)e.Item));
            }
            else if (e.Item is AnimationEntry)
            {
                Subcontrollers.Insert(e.Index,new AnimationEntryController(this,(AnimationEntry)e.Item));
            }
            else if (e.Item is OldModelEntry)
            {
                Subcontrollers.Insert(e.Index,new OldModelEntryController(this,(OldModelEntry)e.Item));
            }
            else if (e.Item is ModelEntry)
            {
                Subcontrollers.Insert(e.Index,new ModelEntryController(this,(ModelEntry)e.Item));
            }
            else if (e.Item is OldSceneryEntry)
            {
                Subcontrollers.Insert(e.Index,new OldSceneryEntryController(this,(OldSceneryEntry)e.Item));
            }
            else if (e.Item is SceneryEntry)
            {
                Subcontrollers.Insert(e.Index,new SceneryEntryController(this,(SceneryEntry)e.Item));
            }
            else if (e.Item is SLSTEntry)
            {
                Subcontrollers.Insert(e.Index,new SLSTEntryController(this,(SLSTEntry)e.Item));
            }
            else if (e.Item is T6Entry)
            {
                Subcontrollers.Insert(e.Index,new T6EntryController(this,(T6Entry)e.Item));
            }
            else if (e.Item is OldZoneEntry)
            {
                Subcontrollers.Insert(e.Index,new OldZoneEntryController(this,(OldZoneEntry)e.Item));
            }
            else if (e.Item is ZoneEntry)
            {
                Subcontrollers.Insert(e.Index,new ZoneEntryController(this,(ZoneEntry)e.Item));
            }
            else if (e.Item is GOOLEntry)
            {
                Subcontrollers.Insert(e.Index,new GOOLEntryController(this,(GOOLEntry)e.Item));
            }
            else if (e.Item is SoundEntry)
            {
                Subcontrollers.Insert(e.Index,new SoundEntryController(this,(SoundEntry)e.Item));
            }
            else if (e.Item is OldMusicEntry)
            {
                Subcontrollers.Insert(e.Index,new OldMusicEntryController(this,(OldMusicEntry)e.Item));
            }
            else if (e.Item is MusicEntry)
            {
                Subcontrollers.Insert(e.Index,new MusicEntryController(this,(MusicEntry)e.Item));
            }
            else if (e.Item is WavebankEntry)
            {
                Subcontrollers.Insert(e.Index,new WavebankEntryController(this,(WavebankEntry)e.Item));
            }
            else if (e.Item is ImageEntry)
            {
                Subcontrollers.Insert(e.Index,new ImageEntryController(this,(ImageEntry)e.Item));
            }
            else if (e.Item is T15Entry)
            {
                Subcontrollers.Insert(e.Index,new T15EntryController(this,(T15Entry)e.Item));
            }
            else if (e.Item is MapEntry)
            {
                Subcontrollers.Insert(e.Index,new MapEntryController(this,(MapEntry)e.Item));
            }
            else if (e.Item is T17Entry)
            {
                Subcontrollers.Insert(e.Index,new T17EntryController(this,(T17Entry)e.Item));
            }
            else if (e.Item is PaletteEntry)
            {
                Subcontrollers.Insert(e.Index,new PaletteEntryController(this,(PaletteEntry)e.Item));
            }
            else if (e.Item is DemoEntry)
            {
                Subcontrollers.Insert(e.Index,new DemoEntryController(this,(DemoEntry)e.Item));
            }
            else if (e.Item is ColoredAnimationEntry)
            {
                Subcontrollers.Insert(e.Index,new CutsceneAnimationEntryController(this,(ColoredAnimationEntry)e.Item));
            }
            else if (e.Item is SpeechEntry)
            {
                Subcontrollers.Insert(e.Index,new SpeechEntryController(this,(SpeechEntry)e.Item));
            }
            else if (e.Item is T21Entry)
            {
                Subcontrollers.Insert(e.Index,new T21EntryController(this,(T21Entry)e.Item));
            }
            else if (e.Item is UnprocessedEntry)
            {
                Subcontrollers.Insert(e.Index,new UnprocessedEntryController(this,(UnprocessedEntry)e.Item));
            }
            else
            {
                throw new NotImplementedException();
            }
            Invalidate();
        }

        private void Entries_ItemRemoved(object sender,EvListEventArgs<Entry> e)
        {
            Subcontrollers.RemoveAt(e.Index);
            Invalidate();
        }

        public override void Dispose()
        {
            Chunk.Entries.ItemAdded -= Entries_ItemAdded;
            Chunk.Entries.ItemRemoved -= Entries_ItemRemoved;
            base.Dispose();
        }
    }
}
