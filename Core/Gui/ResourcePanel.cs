using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{

    public class ResourcePanel : UserControl
    {

        public ResourcePanel(Controller ctlr)
        {
            ArgumentNullException.ThrowIfNull(ctlr);

            Controller = ctlr;

            InactiveTimer = new();

            Editors = Editor.AllEditors
                .Where(x => x.ApplicableForSubject(ctlr))
                .Select(x => (Editor)Activator.CreateInstance(x.GetType()))
                .ToList();

            if (Editors.Count == 0)
            {
                // No editors available for this resource.
                Controls.Add(new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = "No editors are available for this resource."
                });
                return;
            }
            else if (Editors.Count == 1)
            {
                // Only one editor available for this resource. Do not make a whole tab control for it.
                var editor = Editors[0];
                editor.Initialize(ctlr);
                editor.Control.Dock = DockStyle.Fill;
                Controls.Add(editor.Control);
                return;
            }

            // multiple editors, make a tab
            TabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(TabControl);

            foreach (var editor in Editors)
            {
                editor.Initialize(ctlr);
                editor.Control.Dock = DockStyle.Fill;

                var tabPage = new TabPage();
                tabPage.Tag = editor;
                tabPage.Text = editor.Text;
                tabPage.Controls.Add(editor.Control);
                TabControl.TabPages.Add(tabPage);
            }
        }

        public bool IsEmpty => Editors.Count == 0;

        public Controller Controller { get; }

        public List<Editor> Editors { get; }

        public TabControl? TabControl { get; }

        public Stopwatch InactiveTimer { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var editor in Editors)
                {
                    editor.Dispose();
                }
            }
            base.Dispose(disposing);
        }

    }

}
