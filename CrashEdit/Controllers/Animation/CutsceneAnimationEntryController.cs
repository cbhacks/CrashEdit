using Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class CutsceneAnimationEntryController : EntryController
    {
        public CutsceneAnimationEntryController(EntryChunkController entrychunkcontroller,CutsceneAnimationEntry cutsceneanimationentry)
            : base(entrychunkcontroller,cutsceneanimationentry)
        {
            CutsceneAnimationEntry = cutsceneanimationentry;
            foreach (OldFrame frame in cutsceneanimationentry.Frames)
            {
                AddNode(new CutsceneFrameController(this,frame));
            }
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.CutsceneAnimationEntryController_Text,CutsceneAnimationEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(CutsceneAnimationEntry.Frames[0].ModelEID);
            Dictionary<int,TextureChunk> textures = new Dictionary<int,TextureChunk>();
            foreach (OldModelStruct str in modelentry.Structs)
                if (str is OldModelTexture tex && !textures.ContainsKey(tex.EID))
                    textures.Add(tex.EID,EntryChunkController.NSFController.NSF.FindEID<TextureChunk>(tex.EID));
            return new UndockableControl(new OldAnimationEntryViewer(CutsceneAnimationEntry.Frames,true,modelentry,textures));
        }

        public CutsceneAnimationEntry CutsceneAnimationEntry { get; }
    }
}

