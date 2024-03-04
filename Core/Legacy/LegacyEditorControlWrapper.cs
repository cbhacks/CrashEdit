using System.Windows.Forms;

namespace CrashEdit
{

    public sealed class LegacyEditorControlWrapper : UserControl
    {

        public LegacyEditorControlWrapper(LegacyController legacyCtlr)
        {
            ArgumentNullException.ThrowIfNull(legacyCtlr);

            LegacyController = legacyCtlr;
            legacyCtlr.NeedsNewEditor = false;
            InnerControl = legacyCtlr.CreateEditor();
            InnerControl.Dock = DockStyle.Fill;
            Controls.Add(InnerControl);
        }

        public LegacyController LegacyController { get; }

        public Control InnerControl { get; private set; }

        public void Sync()
        {
            if (LegacyController.NeedsNewEditor)
            {
                LegacyController.NeedsNewEditor = false;
                Controls.Remove(InnerControl);
                InnerControl.Dispose();
                InnerControl = LegacyController.CreateEditor();
                InnerControl.Dock = DockStyle.Fill;
                Controls.Add(InnerControl);
            }
            else
            {
                InnerControl.Invalidate(true);
            }
        }

    }

}
