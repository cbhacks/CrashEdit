using CrashEdit.Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class MapEntryController : EntryController
    {
        public MapEntryController(EntryChunkController entrychunkcontroller,MapEntry mapentry) : base(entrychunkcontroller,mapentry)
        {
            MapEntry = mapentry;
            AddNode(new ItemController(this,mapentry.Header));
            AddNode(new ItemController(this,mapentry.Layout));
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
            NodeText = string.Format(CrashUI.Properties.Resources.MapEntryController_Text,MapEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "thing";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
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
