using CrashEdit.Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(ProtoAnimationEntry))]
    public sealed class ProtoAnimationEntryController : EntryController
    {
        public ProtoAnimationEntryController(ProtoAnimationEntry protoanimationentry, SubcontrollerGroup parentGroup) : base(protoanimationentry, parentGroup)
        {
            ProtoAnimationEntry = protoanimationentry;
            if (!ProtoAnimationEntry.NotProto)
                AddMenu("Export as Crash 1 SVTX", Menu_ExportAsC1);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            OldModelEntry modelentry = GetEntry<OldModelEntry>(ProtoAnimationEntry.Frames[0].ModelEID);
            Dictionary<int,TextureChunk> textures = new Dictionary<int,TextureChunk>();
            foreach (OldModelStruct str in modelentry.Structs)
                if (str is OldModelTexture tex && !textures.ContainsKey(tex.EID))
                    textures.Add(tex.EID,GetEntry<TextureChunk>(tex.EID));
            return new OldAnimationEntryViewer(ProtoAnimationEntry.Frames,false,modelentry,textures);
        }

        public ProtoAnimationEntry ProtoAnimationEntry { get; }

        private void Menu_ExportAsC1()
        {
            List<OldFrame> frames = new List<OldFrame>();
            foreach (var frame in ProtoAnimationEntry.Frames)
            {
                frames.Add(new OldFrame(frame.ModelEID,frame.XOffset,frame.YOffset,frame.ZOffset,frame.X1,frame.Y1,frame.Z1,frame.X2,frame.Y2,frame.Z2,0,0,0,frame.Vertices,frame.Unknown,null,false));
            }
            OldAnimationEntry newanim = new OldAnimationEntry(frames,ProtoAnimationEntry.EID);
            FileUtil.SaveFile(newanim.Save(), FileFilters.NSEntry, FileFilters.Any);
        }
    }
}
