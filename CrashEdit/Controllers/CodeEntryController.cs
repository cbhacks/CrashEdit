using Crash;

namespace CrashEdit
{
    public sealed class CodeEntryController : MysteryMultiItemEntryController
    {
        public CodeEntryController(EntryChunkController entrychunkcontroller,CodeEntry codeentry) : base(entrychunkcontroller,codeentry)
        {
            CodeEntry = codeentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("GOOL ({0})",CodeEntry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public CodeEntry CodeEntry { get; }
    }
}
