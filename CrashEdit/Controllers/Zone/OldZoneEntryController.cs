using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldZoneEntryController : EntryController
    {
        private OldZoneEntry zoneentry;

        public OldZoneEntryController(EntryChunkController entrychunkcontroller,OldZoneEntry zoneentry)
            : base(entrychunkcontroller,zoneentry)
        {
            this.zoneentry = zoneentry;
            AddNode(new ItemController(null,zoneentry.Unknown1));
            AddNode(new ItemController(null,zoneentry.Unknown2));
            foreach (OldCamera camera in zoneentry.Cameras)
            {
                AddNode(new OldCameraController(this,camera));
            }
            foreach (OldEntity entity in zoneentry.Entities)
            {
                AddNode(new OldEntityController(this,entity));
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Zone ({0})",zoneentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        protected override Control CreateEditor()
        {
            int linkedsceneryentrycount = BitConv.FromInt32(zoneentry.Unknown1,0);
            OldSceneryEntry[] linkedsceneryentries = new OldSceneryEntry[linkedsceneryentrycount];
            for (int i = 0; i < linkedsceneryentrycount; i++)
            {
                linkedsceneryentries[i] = FindEID<OldSceneryEntry>(BitConv.FromInt32(zoneentry.Unknown1,4 + i * 64));
            }
            int linkedzoneentrycount = BitConv.FromInt32(zoneentry.Unknown1,528);
            OldZoneEntry[] linkedzoneentries = new OldZoneEntry[linkedzoneentrycount];
            for (int i = 0; i < linkedzoneentrycount; i++)
            {
                linkedzoneentries[i] = FindEID<OldZoneEntry>(BitConv.FromInt32(zoneentry.Unknown1,532 + i * 4));
            }
            return new UndockableControl(new OldZoneEntryViewer(zoneentry,linkedsceneryentries,linkedzoneentries));
        }

        public OldZoneEntry ZoneEntry
        {
            get { return zoneentry; }
        }
    }
}
