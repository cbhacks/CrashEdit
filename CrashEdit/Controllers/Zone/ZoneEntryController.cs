using Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ZoneEntryController : EntryController
    {
        public ZoneEntryController(EntryChunkController entrychunkcontroller,ZoneEntry zoneentry) : base(entrychunkcontroller,zoneentry)
        {
            ZoneEntry = zoneentry;
            AddNode(new ItemController(null,zoneentry.Header));
            AddNode(new ItemController(null,zoneentry.Layout));
            foreach (Entity entity in zoneentry.Entities)
            {
                AddNode(new EntityController(this,entity));
            }
            AddMenu(Crash.UI.Properties.Resources.ZoneEntryController_AcAddEntity,Menu_AddEntity);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.ZoneEntryController_Text,ZoneEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "violetb";
            Node.SelectedImageKey = "violetb";
        }

        protected override Control CreateEditor()
        {
            int linkedsceneryentrycount = BitConv.FromInt32(ZoneEntry.Header,0);
            SceneryEntry[] linkedsceneryentries = new SceneryEntry [linkedsceneryentrycount];
            TextureChunk[][] totaltexturechunks = new TextureChunk[linkedsceneryentrycount][];
            for (int i = 0;i < linkedsceneryentrycount;i++)
            {
                linkedsceneryentries[i] = FindEID<SceneryEntry>(BitConv.FromInt32(ZoneEntry.Header,4 + i * 48));
                TextureChunk[] texturechunks = new TextureChunk[BitConv.FromInt32(linkedsceneryentries[i].Info,0x28)];
                for (int j = 0; j < texturechunks.Length; ++j)
                {
                     texturechunks[j] = FindEID<TextureChunk>(BitConv.FromInt32(linkedsceneryentries[i].Info,0x2C+j*4));
                }
                totaltexturechunks[i] = texturechunks;
            }
            int linkedzoneentrycount = BitConv.FromInt32(ZoneEntry.Header,400);
            ZoneEntry[] linkedzoneentries = new ZoneEntry [linkedzoneentrycount];
            for (int i = 0;i < linkedzoneentrycount;i++)
            {
                linkedzoneentries[i] = FindEID<ZoneEntry>(BitConv.FromInt32(ZoneEntry.Header,404 + i * 4));
            }
            return new UndockableControl(new ZoneEntryViewer(ZoneEntry,linkedsceneryentries,totaltexturechunks,linkedzoneentries));
        }

        public ZoneEntry ZoneEntry { get; }

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
            newentity.ID = id;
            ZoneEntry.Entities.Add(newentity);
            AddNode(new EntityController(this,newentity));
            ++ZoneEntry.EntityCount;
        }
    }
}
