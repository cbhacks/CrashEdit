using CrashEdit.Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(ZoneEntry))]
    public sealed class ZoneEntryController : EntryController
    {
        public ZoneEntryController(ZoneEntry zoneentry, SubcontrollerGroup parentGroup) : base(zoneentry, parentGroup)
        {
            ZoneEntry = zoneentry;
            AddMenu(CrashUI.Properties.Resources.ZoneEntryController_AcAddEntity,Menu_AddEntity);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
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
            return new ZoneEntryViewer(ZoneEntry,linkedsceneryentries,totaltexturechunks,linkedzoneentries);
        }

        public ZoneEntry ZoneEntry { get; }

        void Menu_AddEntity()
        {
            short id = 10;
            while (true)
            {
                foreach (ZoneEntry zone in GetEntries<ZoneEntry>())
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
            ++ZoneEntry.EntityCount;
        }
    }
}
