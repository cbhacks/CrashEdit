#nullable enable

using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit {

    public class ResourceBox : UserControl {

        public ResourceBox() {
            NoResourceLabel = new Label {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Nothing is selected."
            };
            Controls.Add(NoResourceLabel);

            UndockedLabel = new Label {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "This editor is currently undocked.\n\nClick to focus.",
                Visible = false
            };
            UndockedLabel.Click += (sender, e) => {
                if (ActivePanelUndockForm != null) {
                    ActivePanelUndockForm.Focus();
                }
            };
            Controls.Add(UndockedLabel);
        }

        private Controller? _activeController;

        public Controller? ActiveController {
            get { return _activeController; }
            set {
                if (_activeController == value)
                    return;

                if (value != null) {
                    if (!AllPanels.TryGetValue(value, out var panel)) {
                        // This controller does not have a panel yet, so make one.
                        panel = new ResourcePanel(value) {
                            Dock = DockStyle.Fill,
                            Visible = false
                        };
                        Controls.Add(panel);
                        AllPanels.Add(value, panel);
                    }

                    ActivePanel = panel;
                } else {
                    ActivePanel = null;
                }

                _activeController = value;
            }
        }

        private ResourcePanel? _activePanel;

        public ResourcePanel? ActivePanel {
            get { return _activePanel; }
            private set {
                if (_activePanel == value)
                    return;

                // Hide the controls for the current value.
                if (_activePanel != null) {
                    if (ActivePanelUndockForm != null) {
                        // The panel was active but undocked.
                        UndockedLabel.Visible = false;
                        ActivePanelUndockForm = null;
                    } else {
                        // The panel was active and visible.
                        _activePanel.Visible = false;
                    }
                } else {
                    // No panel was active.
                    NoResourceLabel.Visible = false;
                }

                _activePanel = value;

                // Show the controls for the new value.
                if (value != null) {
                    if (UndockForms.TryGetValue(value, out var form)) {
                        // The panel is now active, but is currently undocked.
                        UndockedLabel.Visible = true;
                        ActivePanelUndockForm = form;
                    } else {
                        // The panel is now active and visible.
                        value.Visible = true;
                    }
                } else {
                    // No panel is now active.
                    NoResourceLabel.Visible = true;
                }
            }
        }

        public Form? ActivePanelUndockForm { get; private set; }

        public bool CanToggleUndock =>
            ActivePanel != null &&
            !ActivePanel.IsEmpty;

        public void ToggleUndock() {
            if (ActivePanel == null)
                throw new InvalidOperationException();
            if (ActivePanel.IsEmpty)
                throw new InvalidOperationException();

            if (ActivePanelUndockForm == null) {
                // Create the new undock form.
                ActivePanelUndockForm = new Form() {
                    Text = ActiveController!.Text,
                    Icon = Embeds.GetIcon(ActiveController.ImageKey),
                    ClientSize = ActivePanel.Size,
                    Tag = ActivePanel
                };
                ActivePanelUndockForm.FormClosing += UndockForm_FormClosing;
                UndockForms.Add(ActivePanel, ActivePanelUndockForm);

                // Move the panel to the form.
                Controls.Remove(ActivePanel);
                ActivePanelUndockForm.Controls.Add(ActivePanel);

                // Show the undocked message in its place.
                UndockedLabel.Visible = true;

                // Display the form.
                ActivePanelUndockForm.Show(this);
                ActivePanelUndockForm.Focus();
            } else {
                // Panel is already undocked, so redock it.
                // The closing event handler will do the rest of the work.
                ActivePanelUndockForm.Close();
            }
        }

        private void UndockForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (sender == ActivePanelUndockForm) {
                // Hide the undocked message.
                UndockedLabel.Visible = false;

                // Move the panel back into this container.
                ActivePanelUndockForm.Controls.Remove(ActivePanel);
                Controls.Add(ActivePanel);

                // Discard the undock form.
                ActivePanelUndockForm = null;
                UndockForms.Remove(ActivePanel!);
            } else {
                var form = (Form)sender;
                var panel = (ResourcePanel)form.Tag;
                panel.Visible = false;
                form.Controls.Remove(panel);
                Controls.Add(panel);
                UndockForms.Remove(panel);
            }
        }

        public Dictionary<Controller, ResourcePanel> AllPanels { get; } =
            new Dictionary<Controller, ResourcePanel>();

        public Dictionary<ResourcePanel, Form> UndockForms { get; } =
            new Dictionary<ResourcePanel, Form>();

        public Label NoResourceLabel { get; }

        public Label UndockedLabel { get; }

        public void Sync() {
            // Destroy panels for controllers which have died.
            foreach (var kvp in AllPanels.ToList().Where(x => x.Key.Dead)) {
                var ctlr = kvp.Key;
                var panel = kvp.Value;

                // Deselect the controller if it is selected.
                if (ctlr == ActiveController) {
                    ActiveController = null;
                }

                // Redock the panel if it is undocked.
                if (UndockForms.TryGetValue(panel, out var form)) {
                    form.Close();
                }

                // Remove the panel and destroy it.
                Controls.Remove(panel);
                AllPanels.Remove(ctlr);
                panel.Dispose();
            }

            // Update undock form titlebars and icons.
            foreach (var kvp in UndockForms) {
                var ctlr = kvp.Key.Controller;
                var form = kvp.Value;
                form.Text = ctlr.Text;
                form.Icon = Embeds.GetIcon(ctlr.ImageKey);
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                while (UndockForms.Count > 0) {
                    UndockForms.Values.First().Close();
                }
            }
            base.Dispose(disposing);
        }

    }

    public class ResourcePanel : UserControl {

        public ResourcePanel(Controller ctlr) {
            if (ctlr == null)
                throw new ArgumentNullException();

            Controller = ctlr;

            TabControl = new TabControl {
                Dock = DockStyle.Fill
            };
            Controls.Add(TabControl);

            Editors = Editor.AllEditors
                .Where(x => x.ApplicableForSubject(ctlr))
                .Select(x => (Editor)Activator.CreateInstance(x.GetType()))
                .ToList();

            if (Editors.Count == 0) {
                // No editors available for this resource.
                TabControl.TabPages.Add("None");
                TabControl.TabPages[0].Controls.Add(new Label {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = "No editors are available for this resource."
                });
                return;
            }

            foreach (var editor in Editors) {
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

        public TabControl TabControl { get; }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                foreach (var editor in Editors) {
                    editor.Dispose();
                }
            }
            base.Dispose(disposing);
        }

    }

}
