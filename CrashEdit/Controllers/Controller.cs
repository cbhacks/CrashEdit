using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class Controller : IDisposable
    {
        private TreeNode node;
        private ContextMenu contextmenu;
        private Control editor;

        public Controller()
        {
            this.node = new TreeNode();
            this.node.Tag = this;
            this.contextmenu = new ContextMenu();
            this.editor = null;
            this.node.ContextMenu = contextmenu;
        }

        public void AddNode(Controller controller)
        {
            node.Nodes.Add(controller.node);
        }

        public void InsertNode(int index,Controller controller)
        {
            node.Nodes.Insert(index,controller.node);
        }

        protected void AddMenu(string text,ControllerMenuDelegate proc)
        {
            EventHandler handler = delegate(object sender,EventArgs e)
            {
                try
                {
                    proc();
                }
                catch (GUIException ex)
                {
                    MessageBox.Show(ex.Message,text,MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            };
            contextmenu.MenuItems.Add(text,handler);
        }

        protected void AddMenuSeparator()
        {
            contextmenu.MenuItems.Add("-");
        }

        protected virtual Control CreateEditor()
        {
            Label label = new Label();
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Text = "No options available";
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
                if (node.IsSelected)
                {
                    node.TreeView.SelectedNode = null;
                    node.TreeView.SelectedNode = node;
                }
            }
        }

        public TreeNode Node
        {
            get { return node; }
        }

        public ContextMenu ContextMenu
        {
            get { return contextmenu; }
        }

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

        public virtual bool Move(Controller newcontroller,bool commit)
        {
            return false;
        }

        public virtual void Dispose()
        {
            foreach (TreeNode subnode in node.Nodes)
            {
                ((IDisposable)subnode.Tag).Dispose();
            }
            node.Remove();
            contextmenu.Dispose();
            if (editor != null)
            {
                editor.Dispose();
            }
        }
    }
}
