#nullable enable

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit {

    public class MainControl : UserControl, IVerbExecutor {

        public MainControl() {
            Split = new SplitContainer {
                Dock = DockStyle.Fill
            };
            Controls.Add(Split);

            ResourceTree = new ResourceTreeView(this) {
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

        public void ExecuteVerb(Verb verb) {
            if (verb == null)
                throw new ArgumentNullException();

            verb.Execute();

            if (RootController != null) {
                RootController.Sync();
                Sync();
            }
        }

        public void ExecuteVerbChoice(List<Verb> verbs) {
            if (verbs == null)
                throw new ArgumentNullException();

            // Don't bother if there are no choices.
            if (verbs.Count == 0)
                return;

            // TODO let the user choose one
            ExecuteVerb(verbs[0]);
        }

    }

}
