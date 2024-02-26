using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(AnimationEntry))]
    public sealed class AnimationEntryController : EntryController
    {
        public AnimationEntryController(AnimationEntry animationentry, SubcontrollerGroup parentGroup) : base(animationentry, parentGroup)
        {
            AnimationEntry = animationentry;
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
                        texturechunks[i] = FindEID<TextureChunk>(BitConv.FromInt32(modelentry.Info, 0xC + i * 4));
                    }
                    return new AnimationEntryViewer(AnimationEntry.Frames, modelentry, texturechunks);
                }
                else
                {
                    return new AnimationEntryViewer(AnimationEntry.Frames, null, null);
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
