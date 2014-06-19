using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crash.UI
{
    public abstract class EntryChunkController : ChunkController
    {
        private EntryChunk chunk;

        public EntryChunkController(NSFController up,EntryChunk chunk) : base(up,chunk)
        {
            this.chunk = chunk;
            this.chunk.Entries.ItemAdded += new EvListEventHandler<Entry>(Entries_ItemAdded);
            this.chunk.Entries.ItemRemoved += new EvListEventHandler<Entry>(Entries_ItemRemoved);
            this.chunk.Entries.Populate(Entries_ItemAdded);
        }

        public new EntryChunk Chunk
        {
            get { return chunk; }
        }

        private void Entries_ItemAdded(object sender,EvListEventArgs<Entry> e)
        {
            if (e.Item is OldAnimationEntry)
            {
                Subcontrollers.Insert(e.Index,new OldAnimationEntryController(this,(OldAnimationEntry)e.Item));
            }
            else if (e.Item is T1Entry)
            {
                Subcontrollers.Insert(e.Index,new T1EntryController(this,(T1Entry)e.Item));
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
            else if (e.Item is T4Entry)
            {
                Subcontrollers.Insert(e.Index,new T4EntryController(this,(T4Entry)e.Item));
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
            else if (e.Item is T11Entry)
            {
                Subcontrollers.Insert(e.Index,new T11EntryController(this,(T11Entry)e.Item));
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
            else if (e.Item is OldT15Entry)
            {
                Subcontrollers.Insert(e.Index,new OldT15EntryController(this,(OldT15Entry)e.Item));
            }
            else if (e.Item is T15Entry)
            {
                Subcontrollers.Insert(e.Index,new T15EntryController(this,(T15Entry)e.Item));
            }
            else if (e.Item is OldT17Entry)
            {
                Subcontrollers.Insert(e.Index,new OldT17EntryController(this,(OldT17Entry)e.Item));
            }
            else if (e.Item is T17Entry)
            {
                Subcontrollers.Insert(e.Index,new T17EntryController(this,(T17Entry)e.Item));
            }
            else if (e.Item is T18Entry)
            {
                Subcontrollers.Insert(e.Index,new T18EntryController(this,(T18Entry)e.Item));
            }
            else if (e.Item is DemoEntry)
            {
                Subcontrollers.Insert(e.Index,new DemoEntryController(this,(DemoEntry)e.Item));
            }
            else if (e.Item is T20Entry)
            {
                Subcontrollers.Insert(e.Index,new T20EntryController(this,(T20Entry)e.Item));
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
            Subcontrollers[e.Index].Dispose();
            Subcontrollers.RemoveAt(e.Index);
            Invalidate();
        }

        public override void Dispose()
        {
            chunk.Entries.ItemAdded -= Entries_ItemAdded;
            chunk.Entries.ItemRemoved -= Entries_ItemRemoved;
            base.Dispose();
        }
    }
}
