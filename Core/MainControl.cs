#nullable enable

using System;
using System.Windows.Forms;

namespace CrashEdit {

    public class MainControl : UserControl {

        public MainControl() {
            Split = new SplitContainer {
                Dock = DockStyle.Fill
            };
            Controls.Add(Split);

            ResourceTree = new ResourceTreeView {
                Dock = DockStyle.Fill
            };
            Split.Panel1.Controls.Add(ResourceTree);
        }

        private Controller? _rootController;

        public Controller? RootController {
            get { return _rootController; }
            set {
                if (_rootController == value)
                    return;

                _rootController = value;
                ResourceTree.RootController = value;
            }
        }

        public SplitContainer Split { get; }

        public ResourceTreeView ResourceTree { get; }

        public void Sync() {
            ResourceTree.Sync();
        }

    }

}
