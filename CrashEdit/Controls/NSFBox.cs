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

            ws.Sync();
            RootController.Sync();
            Sync();

            ResourceTree.Nodes[1].Expand();
        }

        public LevelWorkspace Workspace { get; }
        public NSF NSF { get; }
        public NSFController NSFController { get; }
    }
}
