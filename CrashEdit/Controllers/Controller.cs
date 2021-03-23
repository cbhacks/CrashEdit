using CrashEdit.Crash;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public abstract class LegacyController : CrashEdit.LegacyController, IDisposable
    {
        private Control editor;

        public LegacyController(LegacyController parent, object resource) : base(parent, resource)
        {
            ContextMenu = new ContextMenu();
            Node = new TreeNode
            {
                Tag = this,
                ContextMenu = ContextMenu
            };
            editor = null;
        }

        public void AddNode(LegacyController controller)
        {
            if (controller.Parent != this) {
                throw new Exception();
            }
            Node.Nodes.Add(controller.Node);
            LegacySubcontrollers.Add(controller);
        }

        public void InsertNode(int index,LegacyController controller)
        {
            if (controller.Parent != this) {
                throw new Exception();
            }
            Node.Nodes.Insert(index,controller.Node);
            LegacySubcontrollers.Insert(index, controller);
        }

        protected void AddMenu(string text,ControllerMenuDelegate proc)
        {
            void handler(object sender, EventArgs e)
            {
                try
                {
                    ErrorManager.EnterSubject(new Object());
                    proc();
                }
                catch (GUIException ex)
                {
                    MessageBox.Show(ex.Message,text,MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                finally
                {
                    ErrorManager.ExitSubject();
                }
            }
            LegacyVerbs.Add(new LegacyVerb(text, new Action(proc)));
            ContextMenu.MenuItems.Add(text,handler);
        }

        protected void AddMenuSeparator()
        {
            ContextMenu.MenuItems.Add("-");
        }

        public abstract void InvalidateNode();
        public abstract void InvalidateNodeImage();

        public override string NodeText => Node.Text;
        public override string NodeImage => Node.ImageKey;

        public override Control CreateEditor()
        {
            Label label = new Label
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "No options available"
            };
            return label;
        }

        protected void InvalidateEditor()
        {
            if (editor != null)
            {
                Control container = editor.Parent;
                if (container != null)
                {
                    container.Controls.Remove(editor);
                }
                editor.Dispose();
                editor = null;
                if (Node.IsSelected)
                {
                    Node.TreeView.SelectedNode = null;
                    Node.TreeView.SelectedNode = Node;
                }
            }
        }

        public TreeNode Node { get; }
        public ContextMenu ContextMenu { get; }

        public Control Editor
        {
            get
            {
                if (editor == null)
                {
                    editor = CreateEditor();
                    editor.Dock = DockStyle.Fill;
                }
                return editor;
            }
        }

        public virtual void Dispose()
        {
            Parent?.LegacySubcontrollers?.Remove(this);
            TreeNode[] nodes = new TreeNode[Node.Nodes.Count];
            int i = 0;
            foreach(TreeNode node in Node.Nodes)
            {
                nodes[i++] = node;
            }
            for (i = 0; i < nodes.Length; ++i)
            {
                if (nodes[i].Tag != null)
                {
                    if (nodes[i].Tag is LegacyController t)
                    {
                        t.Dispose();
                    }
                }
            }
            Node.Remove(); // <-- this line makes the TreeNodeCollection volatile, so the node references must be copies onto a separate list beforehand
            ContextMenu.Dispose();
            if (editor != null)
            {
                editor.Dispose();
            }
        }
    }
}
