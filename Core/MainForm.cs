#nullable enable

using System;
using System.Windows.Forms;

namespace CrashEdit {

    public abstract class MainForm : Form, ICommandHost {

        public MainForm() {
            TabControl = new TabControl {
                Dock = DockStyle.Fill
            };
            TabControl.SelectedIndexChanged += (sender, e) => {
                OnResyncSuggested(EventArgs.Empty);
            };
            Controls.Add(TabControl);

            // Toolbar
            ToolStrip = new ToolStrip {
                ImageList = Resources2.ImageList
            };
            Controls.Add(ToolStrip);

            // Right-side toolbar items below -- they must be added in
            // reverse order (i.e. right to left) !

            // Toolbar -> Find Last
            ToolStrip.Items.Add(new ToolStripCommandButton {
                Alignment = ToolStripItemAlignment.Right,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Command = new FindLastCommand(this)
            });

            // Toolbar -> Find Next
            ToolStrip.Items.Add(new ToolStripCommandButton {
                Alignment = ToolStripItemAlignment.Right,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Command = new FindNextCommand(this)
            });

            // Toolbar -> Find Previous
            ToolStrip.Items.Add(new ToolStripCommandButton {
                Alignment = ToolStripItemAlignment.Right,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Command = new FindPreviousCommand(this)
            });

            // Toolbar -> Find First
            ToolStrip.Items.Add(new ToolStripCommandButton {
                Alignment = ToolStripItemAlignment.Right,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Command = new FindFirstCommand(this)
            });

            // Toolbar -> Search box
            SearchBox = new ToolStripTextBox {
                Alignment = ToolStripItemAlignment.Right,
                Enabled = false
            };
            SearchBox.TextChanged += (sender, e) => {
                if (ActiveWorkspaceHost is MainControl mainCtl) {
                    if (mainCtl.SearchQuery != SearchBox.Text) {
                        mainCtl.SearchQuery = SearchBox.Text;
                        OnResyncSuggested(EventArgs.Empty);
                    }
                }
            };
            SearchBox.KeyPress += (sender, e) => {
                if (e.KeyChar == '\r') {
                    // Start a search if the user pressed enter, if valid.
                    var findFirst = new FindFirstCommand(this);
                    if (findFirst.Ready) {
                        e.Handled = true;
                        if (findFirst.Execute()) {
                            // Select the tree view after successful search.
                            if (ActiveWorkspaceHost is MainControl mainCtl) {
                                mainCtl.ResourceTree.Focus();
                            }
                        } else {
                            // Reselect the search field otherwise.
                            SearchBox.Focus();
                            SearchBox.SelectAll();
                        }
                    }
                }
            };
            ToolStrip.Items.Add(SearchBox);

            // Toolbar -> Find (label and icon)
            ToolStrip.Items.Add(new ToolStripLabel {
                Alignment = ToolStripItemAlignment.Right,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Text = "Find",
                ImageKey = "MagnifyingGlass"
            });

            // Menubar
            MenuStrip = new MenuStrip {
                ImageList = Resources2.ImageList
            };
            Controls.Add(MenuStrip);

            // Menubar -> Edit
            EditMenu = new ToolStripMenuItem {
                Text = "&Edit"
            };
            EditMenu.DropDown.ImageList = Resources2.ImageList;
            MenuStrip.Items.Add(EditMenu);

            // Menubar -> Edit -> Find
            var findMenuItem = new ToolStripMenuItem {
                Text = "&Find",
                ImageKey = "MagnifyingGlass",
                ShortcutKeys = Keys.Control | Keys.F
            };
            findMenuItem.Click += (sender, e) => {
                SearchBox.Focus();
                SearchBox.SelectAll();
            };
            EditMenu.DropDownItems.Add(findMenuItem);

            // Menubar -> Edit -> Find Next
            EditMenu.DropDownItems.Add(new ToolStripCommandMenuItem {
                Command = new FindNextCommand(this),
                ShortcutKeys = Keys.F3
            });

            // Menubar -> Edit -> Find Previous
            EditMenu.DropDownItems.Add(new ToolStripCommandMenuItem {
                Command = new FindPreviousCommand(this),
                ShortcutKeys = Keys.Shift | Keys.F3
            });
        }

        public TabControl TabControl { get; }

        public MenuStrip MenuStrip { get; }

        public ToolStripMenuItem EditMenu { get; }

        public ToolStrip ToolStrip { get; }

        public ToolStripTextBox SearchBox { get; }

        public abstract IWorkspaceHost? ActiveWorkspaceHost { get; }

        public void ShowError(string msg) {
            MessageBox.Show(
                this,
                msg,
                "CrashEdit Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public event EventHandler? ResyncSuggested;

        protected virtual void OnResyncSuggested(EventArgs e) {
            if (ActiveWorkspaceHost is MainControl mainCtl) {
                SearchBox.Enabled = true;
                SearchBox.Text = mainCtl.SearchQuery;
            } else {
                SearchBox.Enabled = false;
                SearchBox.Text = "";
            }
            if (ResyncSuggested != null) {
                ResyncSuggested(this, e);
            }
        }

        protected void MainControl_ActiveControllerChanged(object sender, EventArgs e) {
            if (sender == ActiveWorkspaceHost) {
                OnResyncSuggested(EventArgs.Empty);
            }
        }

    }

}
