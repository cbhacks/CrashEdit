#nullable enable

using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit {

    public class ResourceTreeView : TreeView {

        public ResourceTreeView(IVerbExecutor executor) {
            if (executor == null)
                throw new ArgumentNullException();

            Executor = executor;
            AllowDrop = true;
        }

        public IVerbExecutor Executor { get; }

        private Controller? _rootController;

        public Controller? RootController {
            get { return _rootController; }
            set {
                if (_rootController == value)
                    return;

                // Clear the existing selection.
                SelectedNode = null;
                if (_selectedController != null) {
                    _selectedController = null;
                    OnSelectedControllerChanged(EventArgs.Empty);
                }

                _rootController = value;
                Sync();
            }
        }

        public Controller? _selectedController;

        public Controller? SelectedController {
            get { return _selectedController; }
            set {
                if (_selectedController == value)
                    return;

                if (value == null) {
                    // AfterSelect won't raise when the new selection is null.
                    SelectedNode = null;

                    _selectedController = null;
                    OnSelectedControllerChanged(EventArgs.Empty);
                } else if (value == RootController) {
                    // Event will be raised here to set the selected controller.
                    SelectedNode = Nodes
                        .OfType<TreeNode>()
                        .Where(x => x.Tag == RootController)
                        .Single();
                } else {
                    // Find the controller path to the root.
                    var path = new List<Controller>();
                    for (var ctlr = value; ctlr != RootController; ctlr = ctlr?.Parent) {
                        if (ctlr == null) {
                            // No path was found.
                            throw new ArgumentException();
                        }

                        path.Add(ctlr);
                    }
                    path.Reverse();

                    // Keep in mind, the root controller's child controllers have *sibling*
                    // nodes, so the root controller is not in the node path.

                    // Walk the computed path backwards by tree node.
                    TreeNode? node = null;
                    var nodeGroup = Nodes;
                    foreach (var ctlr in path) {
                        // Unexpanded nodes have incomplete descendants, so they must be
                        // expanded before traversal can continue.
                        if (node != null) {
                            node.Expand();
                        }

                        node = nodeGroup
                            .OfType<TreeNode>()
                            .Where(x => x.Tag == ctlr)
                            .Single();
                        nodeGroup = node.Nodes;
                    }

                    // Event will be raised here to set the selected controller.
                    SelectedNode = node;
                }
            }
        }

        public event EventHandler? SelectedControllerChanged;

        public void Sync() {
            BeginUpdate();
            if (RootController == null) {
                Nodes.Clear();
            } else {
                SyncTree(Nodes, RootController, false);
            }
            EndUpdate();
        }

        private void SyncNode(TreeNode node, Controller ctlr) {
            node.Text = ctlr.Text;
            node.ImageKey = ctlr.ImageKey;
            node.SelectedImageKey = node.ImageKey;

            // Synchronize the child node list.
            if (ctlr != RootController) {
                // Don't infinitely recurse on root, which specially includes itself
                // in SyncTree().
                SyncTree(node.Nodes, ctlr, !node.IsExpanded);
            }
        }

        private void SyncTree(TreeNodeCollection tree, Controller ctlr, bool elide) {
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
                DisposeNode(node);
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
                    node.ContextMenuStrip = new VerbContextMenuStrip(Executor) {
                        Subject = subctlr
                    };
                    tree.Insert(i, node);
                }

                if (!elide) {
                    SyncNode(node, subctlr);
                }
                i++;
            }
        }

        private void DisposeNode(TreeNode node) {
            if (node.Tag is Controller) {
                node.ContextMenuStrip.Dispose();
                node.ContextMenuStrip = null;
                foreach (TreeNode subnode in node.Nodes) {
                    DisposeNode(subnode);
                }
            }
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e) {
            if (e.Node.Tag is Controller ctlr) {
                BeginUpdate();
                SyncTree(e.Node.Nodes, ctlr, false);
                EndUpdate();
            }
            base.OnBeforeExpand(e);
        }

        protected virtual void OnSelectedControllerChanged(EventArgs e) {
            if (SelectedControllerChanged != null) {
                SelectedControllerChanged(this, e);
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e) {
            if (e.Node.Tag is Controller ctlr) {
                if (_selectedController != ctlr) {
                    _selectedController = ctlr;
                    OnSelectedControllerChanged(EventArgs.Empty);
                }
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                foreach (TreeNode node in Nodes) {
                    DisposeNode(node);
                }
                Nodes.Clear();
            }
            base.Dispose(disposing);
        }

        protected class DragDropHelper {

            public DragDropHelper(Controller src, List<TransitiveVerb> verbs) {
                if (src == null)
                    throw new ArgumentNullException();
                if (verbs == null)
                    throw new ArgumentNullException();

                Source = src;
                Verbs = verbs;
            }

            public Controller Source { get; }

            public List<TransitiveVerb> Verbs { get; }

        }

        protected override void OnItemDrag(ItemDragEventArgs e) {
            if (e.Item is TreeNode node) {
                if (node.Tag is Controller ctlr) {
                    // Find all transitive verbs which can accept the to-be-dragged
                    // controller as a source controller.
                    var verbs = Verb.AllVerbs
                        .OfType<TransitiveVerb>()
                        .Where(x => x.ApplicableForSource(ctlr))
                        .ToList();

                    if (verbs.Count > 0) {
                        DoDragDrop(new DragDropHelper(ctlr, verbs), DragDropEffects.Move);

                        // We handled this drag attempt ourselves, don't raise the event.
                        return;
                    }
                }
            }

            base.OnItemDrag(e);
        }

        protected override void OnDragOver(DragEventArgs e) {
            if (!e.Data.GetDataPresent(typeof(DragDropHelper))) {
                // Not our drag operation.
                base.OnDragOver(e);
                return;
            }
            var helper = (DragDropHelper)e.Data.GetData(typeof(DragDropHelper));

            // Find the controller for the node being dragged over.
            var targetNode = GetNodeAt(PointToClient(new Point(e.X, e.Y)));
            var targetCtlr = targetNode?.Tag as Controller;
            if (targetCtlr == null) {
                // No node at the target location, or the node has no controller.
                e.Effect = DragDropEffects.None;
                return;
            }

            // Determine if there are any transitive verbs applicable to this drag.
            bool canDrop = helper.Verbs
                .Where(x => x.ApplicableForTransit(helper.Source, targetCtlr))
                .Any();

            e.Effect = canDrop ? DragDropEffects.Move : DragDropEffects.None;
        }

        protected override void OnDragDrop(DragEventArgs e) {
            if (!e.Data.GetDataPresent(typeof(DragDropHelper))) {
                // Not our drag operation.
                base.OnDragOver(e);
                return;
            }
            var helper = (DragDropHelper)e.Data.GetData(typeof(DragDropHelper));

            // Find the controller for the node being dragged over.
            var targetNode = GetNodeAt(PointToClient(new Point(e.X, e.Y)));
            var targetCtlr = targetNode?.Tag as Controller;
            if (targetCtlr == null) {
                // No node at the target location, or the node has no controller.
                e.Effect = DragDropEffects.None;
                return;
            }

            // Build the list of verbs available for the user to choose from.
            var choices = new List<Verb>();
            foreach (var verb in helper.Verbs) {
                if (!verb.ApplicableForTransit(helper.Source, targetCtlr))
                    continue;

                var newVerb = (TransitiveVerb)verb.Clone();
                newVerb.Source = helper.Source;
                newVerb.Destination = targetCtlr;
                choices.Add(newVerb);
            }

            // Present the options to the user and potentially execute one.
            Executor.ExecuteVerbChoice(choices);

            e.Effect = DragDropEffects.Move;
        }

    }

}
