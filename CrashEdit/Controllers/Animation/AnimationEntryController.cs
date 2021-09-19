using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
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
            NodeText = string.Format(CrashUI.Properties.Resources.AnimationEntryController_Text,AnimationEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "ThingLime";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
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
                    return new AnimationEntryViewer(AnimationEntry.Frames,modelentry,texturechunks);
                }
                else
                {
                    return new AnimationEntryViewer(AnimationEntry.Frames,null,null);
                }
            }
            else
            {
                return new Crash3AnimationSelector(AnimationEntry, GetNSF());
            }
        }

        public AnimationEntry AnimationEntry { get; }
    }
}
