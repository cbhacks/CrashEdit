using Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class MapEntryController : EntryController
    {
        public MapEntryController(EntryChunkController entrychunkcontroller,MapEntry mapentry) : base(entrychunkcontroller,mapentry)
        {
            MapEntry = mapentry;
            AddNode(new ItemController(null,mapentry.Header));
            AddNode(new ItemController(null,mapentry.Layout));
            foreach (OldEntity entity in mapentry.Entities)
            {
                AddNode(new OldEntityController(this,entity));
            }
            AddMenu("Add Entity",Menu_AddEntity);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.MapEntryController_Text,MapEntry.EName);
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

        public MapEntry MapEntry { get; }

        void Menu_AddEntity()
        {
            short id = 1;
            while (true)
            {
                foreach (MapEntry zone in EntryChunkController.NSFController.NSF.GetEntries<MapEntry>())
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
            OldEntity newentity = OldEntity.Load(new OldEntity(0x0018,3,0,id,0,0,0,0,0,new List<EntityPosition>() { new EntityPosition(0,0,0) },0).Save());
            MapEntry.Entities.Add(newentity);
            AddNode(new OldEntityController(this,newentity));
        }
    }
}
