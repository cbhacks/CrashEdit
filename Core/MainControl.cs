#nullable enable

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit {

    public class MainControl : UserControl, IVerbExecutor {

        public MainControl(Controller rootController) {
            if (rootController == null)
                throw new ArgumentNullException();

            RootController = rootController;

            Split = new SplitContainer {
                Dock = DockStyle.Fill
            };
            Controls.Add(Split);

            ResourceTree = new ResourceTreeView(this) {
                Dock = DockStyle.Fill,
                RootController = RootController
            };
            Split.Panel1.Controls.Add(ResourceTree);
        }

        public Controller RootController { get; }

        public SplitContainer Split { get; }

        public ResourceTreeView ResourceTree { get; }

        public void Sync() {
            ResourceTree.Sync();
        }

        public void ExecuteVerb(Verb verb) {
            if (verb == null)
                throw new ArgumentNullException();

            verb.Execute();

            RootController.Sync();
            Sync();
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
