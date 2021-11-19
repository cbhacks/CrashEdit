using CrashEdit.Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(ColoredAnimationEntry))]
    public sealed class ColoredAnimationEntryController : EntryController
    {
        public ColoredAnimationEntryController(ColoredAnimationEntry cutsceneanimationentry, SubcontrollerGroup parentGroup)
            : base(cutsceneanimationentry, parentGroup)
        {
            CutsceneAnimationEntry = cutsceneanimationentry;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            OldModelEntry modelentry = GetEntry<OldModelEntry>(CutsceneAnimationEntry.Frames[0].ModelEID);
            Dictionary<int,TextureChunk> textures = new Dictionary<int,TextureChunk>();
            foreach (OldModelStruct str in modelentry.Structs)
                if (str is OldModelTexture tex && !textures.ContainsKey(tex.EID))
                    textures.Add(tex.EID,GetEntry<TextureChunk>(tex.EID));
            return new OldAnimationEntryViewer(CutsceneAnimationEntry.Frames,true,modelentry,textures);
        }

        public ColoredAnimationEntry CutsceneAnimationEntry { get; }
    }
}

