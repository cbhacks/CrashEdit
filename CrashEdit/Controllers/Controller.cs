using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class Controller : IDisposable
    {
        private Control editor;

        public Controller()
        {
            Node = new TreeNode();
            Node.Tag = this;
            ContextMenu = new ContextMenu();
            editor = null;
            Node.ContextMenu = ContextMenu;
        }

        public void AddNode(Controller controller)
        {
            Node.Nodes.Add(controller.Node);
        }

        public void InsertNode(int index,Controller controller)
        {
            Node.Nodes.Insert(index,controller.Node);
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
            ContextMenu.MenuItems.Add(text,handler);
        }

        protected void AddMenuSeparator()
        {
            ContextMenu.MenuItems.Add("-");
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

        public virtual bool Move(Controller newcontroller,bool commit)
        {
            return false;
        }

        public virtual void Dispose()
        {
            Node.Remove();
            ContextMenu.Dispose();
            if (editor != null)
            {
                editor.Dispose();
            }
        }
    }
}
