using Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoAnimationEntryController : EntryController
    {
        public ProtoAnimationEntryController(EntryChunkController entrychunkcontroller,ProtoAnimationEntry protoanimationentry) : base(entrychunkcontroller,protoanimationentry)
        {
            ProtoAnimationEntry = protoanimationentry;
            foreach (ProtoFrame frame in protoanimationentry.Frames)
            {
                AddNode(new ProtoFrameController(this,frame));
            }
            if (!ProtoAnimationEntry.NotProto)
                AddMenu("Export as Crash 1 SVTX", Menu_ExportAsC1);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("{1} Animation ({0})",ProtoAnimationEntry.EName, ProtoAnimationEntry.NotProto ? "Old" : "Proto"); // fucking hell
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(ProtoAnimationEntry.Frames[0].ModelEID);
            return new UndockableControl(new OldAnimationEntryViewer(ProtoAnimationEntry.Frames,modelentry));
        }

        public ProtoAnimationEntry ProtoAnimationEntry { get; }

        private void Menu_ExportAsC1()
        {
            List<OldFrame> frames = new List<OldFrame>();
            foreach (var frame in ProtoAnimationEntry.Frames)
            {
                frames.Add(new OldFrame(frame.ModelEID,frame.XOffset,frame.YOffset,frame.ZOffset,frame.X1,frame.Y1,frame.Z1,frame.X2,frame.Y2,frame.Z2,0,0,0,frame.Vertices,frame.Unknown,null));
            }
            OldAnimationEntry newanim = new OldAnimationEntry(frames,ProtoAnimationEntry.EID);
            FileUtil.SaveFile(newanim.Save(), FileFilters.NSEntry, FileFilters.Any);
        }
    }
}
