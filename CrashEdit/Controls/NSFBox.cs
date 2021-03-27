using CrashEdit.Crash;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit.CE
{
    public sealed class NSFBox : MainControl
    {
        public NSFBox(IUserInterface ui, LevelWorkspace ws) : base(ui, Controller.Make(ws, null))
        {
            NSF = ws.NSF;
            NSFController = (NSFController)RootController.Legacy;

            RootController.Sync();
            Sync();
        }

        public NSF NSF { get; }
        public NSFController NSFController { get; }
    }
}
