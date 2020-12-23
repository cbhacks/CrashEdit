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

        public OldEntityController(MapEntryController oldt17entrycontroller, OldEntity entity)
        {
            MapEntryController = oldt17entrycontroller;
            OldEntity = entity;
            AddMenu("Duplicate Entity",Menu_MapDuplicate);
            AddMenu("Delete Entity",Menu_MapDelete);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.OldEntityController_Text,OldEntity.ID,OldEntity.Type,OldEntity.Subtype);
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
        public MapEntryController MapEntryController { get; }

        public OldEntity OldEntity { get; }

        private void Menu_Duplicate()
        {
            short id = 6;
            while (true)
            {
                foreach (OldZoneEntry zone in OldZoneEntryController.EntryChunkController.NSFController.NSF.GetEntries<OldZoneEntry>())
                {
                    foreach (OldEntity otherentity in zone.Entities)
                    {
                        if (otherentity.ID == id)
                        {
                            goto FOUND_ID;
                        }
                    }
                }
                break;
            FOUND_ID:
                ++id;
                continue;
            }
            OldEntity newentity = OldEntity.Load(OldEntity.Save());
            newentity.ID = id;
            OldZoneEntryController.OldZoneEntry.Entities.Add(newentity);
            OldZoneEntryController.AddNode(new OldEntityController(OldZoneEntryController,newentity));
        }

        private void Menu_Delete()
        {
            OldZoneEntryController.OldZoneEntry.Entities.Remove(OldEntity);
            Dispose();
        }
        
        private void Menu_MapDuplicate()
        {
            short id = 6;
            while (true)
            {
                foreach (MapEntry zone in MapEntryController.EntryChunkController.NSFController.NSF.GetEntries<MapEntry>())
                {
                    foreach (OldEntity otherentity in zone.Entities)
                    {
                        if (otherentity.ID == id)
                        {
                            goto FOUND_ID;
                        }
                    }
                }
                break;
            FOUND_ID:
                ++id;
                continue;
            }
            OldEntity newentity = OldEntity.Load(OldEntity.Save());
            newentity.ID = id;
            MapEntryController.MapEntry.Entities.Add(newentity);
            MapEntryController.AddNode(new OldEntityController(MapEntryController,newentity));
        }

        private void Menu_MapDelete()
        {
            MapEntryController.MapEntry.Entities.Remove(OldEntity);
            Dispose();
        }
    }
}
