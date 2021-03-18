#nullable enable

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit {

    public class ResourceTreeView : TreeView {

        private Controller? _rootController;

        public Controller? RootController {
            get { return _rootController; }
            set {
                if (_rootController == value)
                    return;

                _rootController = value;
                Sync();
            }
        }

        public void Sync() {
            BeginUpdate();
            if (RootController == null) {
                Nodes.Clear();
            } else {
                SyncTree(Nodes, RootController);
            }
            EndUpdate();
        }

        private void SyncNode(TreeNode node, Controller ctlr) {
            node.Text = ctlr.Legacy?.NodeText ?? ctlr.Resource.GetType().ToString();
            node.ImageKey = ctlr.Legacy?.NodeImage ?? "";
            node.SelectedImageKey = node.ImageKey;

            // Synchronize the child node list.
            if (ctlr != RootController) {
                // Don't infinitely recurse on root, which specially includes itself
                // in SyncTree().
                SyncTree(node.Nodes, ctlr);
            }
        }

        private void SyncTree(TreeNodeCollection tree, Controller ctlr) {
            var existingNodes = new Dictionary<Controller, TreeNode>();

            // Find all existing nodes and their matching controllers.
            foreach (TreeNode node in tree) {
                if (node.Tag is Controller subctlr) {
                    existingNodes.Add(subctlr, node);
                }
            }

            // Build a list of current subcontrollers.
            var subctlrs = new List<Controller>();
            foreach (var subctlrGroup in ctlr.SubcontrollerGroups) {
                foreach (var subctlr in subctlrGroup.Members) {
                    subctlrs.Add(subctlr);
                }
            }

            // Also account for the root controller, placing it at the top and
            // at the same level as its subcontrollers.
            if (ctlr == RootController) {
                subctlrs.Insert(0, ctlr);
            }

            // Remove nodes for no-longer-present subcontrollers.
            var obsoleteNodes = new HashSet<TreeNode>(existingNodes.Values);
            foreach (var subctlr in subctlrs) {
                if (existingNodes.TryGetValue(subctlr, out var existingNode)) {
                    obsoleteNodes.Remove(existingNode);
                }
            }
            foreach (var node in obsoleteNodes) {
                node.Remove();
            }

            // Build new node list.
            int i = 0;
            foreach (var subctlr in subctlrs) {
                TreeNode node;
                if (existingNodes.TryGetValue(subctlr, out node)) {
                    if (node.Index == i) {
                        // OK, node is already in the correct positon.
                    } else {
                        // Node exists, but needs to be moved.
                        node.Remove();
                        tree.Insert(i, node);
                    }
                } else {
                    // This subcontroller is new, so a new node must be made.
                    node = new TreeNode();
                    node.Tag = subctlr;
                    tree.Insert(i, node);
                }

                SyncNode(node, subctlr);
                i++;
            }
        }

    }

}
