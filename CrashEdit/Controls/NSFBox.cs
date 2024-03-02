using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class NSFBox : MainControl
    {
        public NSFBox(IUserInterface ui, LevelWorkspace ws) : base(ui, Controller.Make(ws, null))
        {
            Workspace = ws;
            NSF = ws.NSF;
            NSFController = (NSFController)RootController.SubcontrollerGroups[0].Members[0].Legacy;

            Sync();

            // this (should be) the NSF node. not very robust.
            ResourceTree.Nodes[1].Expand();
        }

        public LevelWorkspace Workspace { get; }
        public NSF NSF { get; }
        public NSFController NSFController { get; }

        public override void Sync()
        {
            RootController.Sync();
            ResourceTree.Sync();
            ResourceBox.Sync();
        }
    }
}
