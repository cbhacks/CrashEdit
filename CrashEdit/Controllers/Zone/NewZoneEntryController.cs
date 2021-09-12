using CrashEdit.Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class NewZoneEntryController : EntryController
    {
        public NewZoneEntryController(EntryChunkController entrychunkcontroller,NewZoneEntry zoneentry) : base(entrychunkcontroller,zoneentry)
        {
            NewZoneEntry = zoneentry;
            foreach (Entity entity in zoneentry.Entities)
            {
                AddNode(new NewEntityController(this,entity));
            }
            AddMenu(CrashUI.Properties.Resources.ZoneEntryController_AcAddEntity,Menu_AddEntity);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format("Zone ({0})",NewZoneEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "ThingViolet";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            int linkedsceneryentrycount = BitConv.FromInt32(NewZoneEntry.Header,0);
            NewSceneryEntry[] linkedsceneryentries = new NewSceneryEntry [linkedsceneryentrycount];
            TextureChunk[][] totaltexturechunks = new TextureChunk[linkedsceneryentrycount][];
            for (int i = 0;i < linkedsceneryentrycount;i++)
            {
                linkedsceneryentries[i] = FindEID<NewSceneryEntry>(BitConv.FromInt32(NewZoneEntry.Header,4 + i * 48));
                TextureChunk[] texturechunks = new TextureChunk[BitConv.FromInt32(linkedsceneryentries[i].Info,0x28)];
                for (int j = 0; j < texturechunks.Length; ++j)
                {
                     texturechunks[j] = FindEID<TextureChunk>(BitConv.FromInt32(linkedsceneryentries[i].Info,0x2C+j*4));
                }
                totaltexturechunks[i] = texturechunks;
            }
            int linkedzoneentrycount = BitConv.FromInt32(NewZoneEntry.Header,400);
            NewZoneEntry[] linkedzoneentries = new NewZoneEntry [linkedzoneentrycount];
            for (int i = 0;i < linkedzoneentrycount;i++)
            {
                linkedzoneentries[i] = FindEID<NewZoneEntry>(BitConv.FromInt32(NewZoneEntry.Header,404 + i * 4));
            }
            return new NewZoneEntryViewer(NewZoneEntry,linkedsceneryentries,totaltexturechunks,linkedzoneentries);
        }

        public NewZoneEntry NewZoneEntry { get; }

        void Menu_AddEntity()
        {
            short id = 10;
            while (true)
            {
                foreach (ZoneEntry zone in EntryChunkController.NSFController.NSF.GetEntries<ZoneEntry>())
                {
                    foreach (Entity otherentity in zone.Entities)
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
            Entity newentity = Entity.Load(new Entity(new Dictionary<short,EntityProperty>()).Save());
            NewZoneEntry.Entities.Add(newentity);
            AddNode(new NewEntityController(this,newentity));
            NewZoneEntry.EntityCount = NewZoneEntry.Entities.Count;
        }
    }
}
