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
            Node.ImageKey = "codeb";
            Node.SelectedImageKey = "codeb";
        }

        public CodeEntry CodeEntry { get; }
    }
}
