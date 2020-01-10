using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldEntityController : Controller
    {
        public OldEntityController(OldZoneEntryController oldzoneentrycontroller,OldEntity entity)
        {
            OldZoneEntryController = oldzoneentrycontroller;
            OldEntity = entity;
            AddMenu("Duplicate Entity",Menu_Duplicate);
            AddMenu("Delete Entity",Menu_Delete);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public OldEntityController(OldT17EntryController oldt17entrycontroller, OldEntity entity)
        {
            OldT17EntryController = oldt17entrycontroller;
            OldEntity = entity;
            AddMenu("Duplicate Entity",Menu_DuplicateT17);
            AddMenu("Delete Entity",Menu_DeleteT17);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.OldEntityController_Text,OldEntity.ID,OldEntity.Positions.Count);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new OldEntityBox(this);
        }

        public OldZoneEntryController OldZoneEntryController { get; }
        public OldT17EntryController OldT17EntryController { get; }

        public OldEntity OldEntity { get; }

        private void Menu_Duplicate()
        {
            short id = 6;
            while (true)
            {
                foreach (Chunk chunk in OldZoneEntryController.EntryChunkController.NSFController.NSF.Chunks)
                {
                    if (chunk is EntryChunk entrychunk)
                    {
                        foreach (Entry entry in entrychunk.Entries)
                        {
                            if (entry is OldZoneEntry zone)
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
            ++OldZoneEntryController.OldZoneEntry.EntityCount;
            OldEntity newentity = OldEntity.Load(OldEntity.Save());
            newentity.ID = id;
            OldZoneEntryController.OldZoneEntry.Entities.Add(newentity);
            OldZoneEntryController.AddNode(new OldEntityController(OldZoneEntryController,newentity));
        }

        private void Menu_Delete()
        {
            --OldZoneEntryController.OldZoneEntry.EntityCount;
            OldZoneEntryController.OldZoneEntry.Entities.Remove(OldEntity);
            Dispose();
        }
        
        private void Menu_DuplicateT17()
        {
            short id = 6;
            while (true)
            {
                foreach (Chunk chunk in OldT17EntryController.EntryChunkController.NSFController.NSF.Chunks)
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
            ++OldT17EntryController.OldT17Entry.EntityCount;
            OldEntity newentity = OldEntity.Load(OldEntity.Save());
            newentity.ID = id;
            OldT17EntryController.OldT17Entry.Entities.Add(newentity);
            OldT17EntryController.AddNode(new OldEntityController(OldT17EntryController,newentity));
        }

        private void Menu_DeleteT17()
        {
            --OldT17EntryController.OldT17Entry.EntityCount;
            OldT17EntryController.OldT17Entry.Entities.Remove(OldEntity);
            Dispose();
        }
    }
}
