using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class AnimationEntryController : EntryController
    {
        public AnimationEntryController(EntryChunkController entrychunkcontroller,AnimationEntry animationentry) : base(entrychunkcontroller,animationentry)
        {
            AnimationEntry = animationentry;
            foreach (Frame frame in animationentry.Frames)
            {
                AddNode(new FrameController(this,frame));
            }
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.AnimationEntryController_Text,AnimationEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            if (!AnimationEntry.IsNew)
            {
                ModelEntry modelentry = FindEID<ModelEntry>(AnimationEntry.Frames[0].ModelEID);
                if (modelentry != null)
                {
                    TextureChunk[] texturechunks = new TextureChunk[modelentry.TPAGCount];
                    for (int i = 0; i < texturechunks.Length; ++i)
                    {
                        texturechunks[i] = FindEID<TextureChunk>(BitConv.FromInt32(modelentry.Info,0xC+i*4));
                    }
                    return new UndockableControl(new AnimationEntryViewer(AnimationEntry.Frames,modelentry,texturechunks));
                }
                else
                {
                    return new UndockableControl(new AnimationEntryViewer(AnimationEntry.Frames,null,null));
                }
            }
            else
            {
                return new Crash3AnimationSelector(AnimationEntry, EntryChunkController.NSFController.NSF);
            }
        }

        public AnimationEntry AnimationEntry { get; }
    }
}
