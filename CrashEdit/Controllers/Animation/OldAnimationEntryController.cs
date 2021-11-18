using CrashEdit.Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldAnimationEntry))]
    public sealed class OldAnimationEntryController : EntryController
    {
        public OldAnimationEntryController(OldAnimationEntry oldanimationentry, SubcontrollerGroup parentGroup) : base(oldanimationentry, parentGroup)
        {
            OldAnimationEntry = oldanimationentry;
            foreach (OldFrame frame in oldanimationentry.Frames)
            {
                AddNode(new OldFrameController(this,frame));
            }
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            OldModelEntry modelentry = GetEntry<OldModelEntry>(OldAnimationEntry.Frames[0].ModelEID);
            Dictionary<int,TextureChunk> textures = new Dictionary<int,TextureChunk>();
            if (modelentry != null)
                foreach (OldModelStruct str in modelentry.Structs)
                    if (str is OldModelTexture tex && !textures.ContainsKey(tex.EID))
                        textures.Add(tex.EID,GetEntry<TextureChunk>(tex.EID));
            return new OldAnimationEntryViewer(OldAnimationEntry.Frames,false,modelentry,textures);
        }

        public OldAnimationEntry OldAnimationEntry { get; }
    }
}
