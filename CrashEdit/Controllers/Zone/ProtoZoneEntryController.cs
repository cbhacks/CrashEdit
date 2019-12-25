using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoZoneEntryController : EntryController
    {
        public ProtoZoneEntryController(EntryChunkController entrychunkcontroller,ProtoZoneEntry zoneentry)
            : base(entrychunkcontroller,zoneentry)
        {
            ZoneEntry = zoneentry;
            AddNode(new ItemController(null,zoneentry.Header));
            AddNode(new ItemController(null,zoneentry.Layout));
            foreach (OldCamera camera in zoneentry.Cameras)
            {
                AddNode(new OldCameraController(this,camera));
            }
            foreach (ProtoEntity entity in zoneentry.Entities)
            {
                AddNode(new ProtoEntityController(this,entity));
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Prototype Zone ({0})",ZoneEntry.EName);
            Node.ImageKey = "violetb";
            Node.SelectedImageKey = "violetb";
        }

        protected override Control CreateEditor()
        {
            int linkedsceneryentrycount = BitConv.FromInt32(ZoneEntry.Header,0);
            ProtoSceneryEntry[] linkedsceneryentries = new ProtoSceneryEntry[linkedsceneryentrycount];
            for (int i = 0; i < linkedsceneryentrycount; i++)
            {
                linkedsceneryentries[i] = FindEID<ProtoSceneryEntry>(BitConv.FromInt32(ZoneEntry.Header,4 + i * 64));
            }
            int linkedzoneentrycount = BitConv.FromInt32(ZoneEntry.Header,528);
            ProtoZoneEntry[] linkedzoneentries = new ProtoZoneEntry[linkedzoneentrycount];
            for (int i = 0; i < linkedzoneentrycount; i++)
            {
                linkedzoneentries[i] = FindEID<ProtoZoneEntry>(BitConv.FromInt32(ZoneEntry.Header,532 + i * 4));
            }
            return new UndockableControl(new ProtoZoneEntryViewer(ZoneEntry,linkedsceneryentries,linkedzoneentries));
        }

        public ProtoZoneEntry ZoneEntry { get; }
    }
}
