using CrashEdit.Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class OldAnimationEntryController : EntryController
    {
        public OldAnimationEntryController(EntryChunkController entrychunkcontroller,OldAnimationEntry oldanimationentry) : base(entrychunkcontroller,oldanimationentry)
        {
            OldAnimationEntry = oldanimationentry;
            foreach (OldFrame frame in oldanimationentry.Frames)
            {
                AddNode(new OldFrameController(this,frame));
            }
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(CrashUI.Properties.Resources.OldAnimationEntryController_Text,OldAnimationEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            OldModelEntry modelentry = EntryChunkController.NSFController.NSF.GetEntry<OldModelEntry>(OldAnimationEntry.Frames[0].ModelEID);
            Dictionary<int,TextureChunk> textures = new Dictionary<int,TextureChunk>();
            if (modelentry != null)
                foreach (OldModelStruct str in modelentry.Structs)
                    if (str is OldModelTexture tex && !textures.ContainsKey(tex.EID))
                        textures.Add(tex.EID,EntryChunkController.NSFController.NSF.GetEntry<TextureChunk>(tex.EID));
            return new UndockableControl(new OldAnimationEntryViewer(OldAnimationEntry.Frames,false,modelentry,textures));
        }

        public OldAnimationEntry OldAnimationEntry { get; }
    }
}
