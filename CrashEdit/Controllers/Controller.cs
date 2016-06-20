using System;
using System.Drawing;
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
            node = new TreeNode();
            node.Tag = this;
            contextmenu = new ContextMenu();
            editor = null;
            node.ContextMenu = contextmenu;
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

        public abstract void InvalidateNode();

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
            node.Remove();
            contextmenu.Dispose();
            if (editor != null)
            {
                editor.Dispose();
            }
        }
    }
}
