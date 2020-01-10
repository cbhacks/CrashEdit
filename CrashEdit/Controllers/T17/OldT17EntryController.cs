using Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldT17EntryController : EntryController
    {
        public OldT17EntryController(EntryChunkController entrychunkcontroller,OldT17Entry oldt17entry) : base(entrychunkcontroller,oldt17entry)
        {
            OldT17Entry = oldt17entry;
            AddNode(new ItemController(null,oldt17entry.Header));
            AddNode(new ItemController(null,oldt17entry.Layout));
            foreach (OldEntity entity in oldt17entry.Entities)
            {
                AddNode(new OldEntityController(this,entity));
            }
            AddMenu("Add Entity",Menu_AddEntity);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.OldT17EntryController_Text,OldT17Entry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        protected override Control CreateEditor()
        {
            return new MapEntryViewer(this);
        }

        public OldT17Entry OldT17Entry { get; }

        void Menu_AddEntity()
        {
            short id = 1;
            while (true)
            {
                foreach (Chunk chunk in EntryChunkController.NSFController.NSF.Chunks)
                {
                    if (chunk is EntryChunk entrychunk)
                    {
                        foreach (Entry entry in entrychunk.Entries)
                        {
                            if (entry is OldT17Entry zone)
                            {
                                foreach (OldEntity otherentity in zone.Entities)
                                {
                                    if (otherentity.ID == id)
                                    {
                                        goto FOUND_ID;
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            FOUND_ID:
                ++id;
                continue;
            }
            OldEntity newentity = OldEntity.Load(new OldEntity(0,0x00030018,id,0,0,0,0,0,new List<EntityPosition>() { new EntityPosition(0,0,0) },0).Save());
            OldT17Entry.Entities.Add(newentity);
            AddNode(new OldEntityController(this,newentity));
            OldT17Entry.EntityCount = OldT17Entry.Entities.Count;
        }
    }
}
