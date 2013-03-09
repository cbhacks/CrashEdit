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
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Text = "No options available";
            this.node = new TreeNode();
            this.node.Tag = this;
            this.contextmenu = new ContextMenu();
            this.editor = label;
            this.node.ContextMenu = contextmenu;
        }

        protected void AddNode(Controller controller)
        {
            node.Nodes.Add(controller.node);
        }

        protected void AddMenu(string text,EventHandler handler)
        {
            MenuItem menuitem = new MenuItem();
            menuitem.Text = text;
            menuitem.Click += handler;
            contextmenu.MenuItems.Add(menuitem);
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
            get { return editor; }
            protected set
            {
                editor.Dispose();
                editor = value;
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
            editor.Dispose();
        }
    }
}
