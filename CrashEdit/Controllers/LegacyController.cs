using Crash;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class LegacyController : Controller
    {
        private static Dictionary<Type,Type> editorcontrols;

        static LegacyController()
        {
            editorcontrols = new Dictionary<Type,Type>();
            foreach (Type editorcontrol in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (EditorControlAttribute attribute in editorcontrol.GetCustomAttributes(typeof(EditorControlAttribute),false))
                {
                    editorcontrols.Add(attribute.Type,editorcontrol);
                }
            }
        }

        private static TreeNode Populate(object obj)
        {
            if (obj is T4Item)
            {
                return Populate((T4Item)obj,-1);
            }
            else
            {
                throw new Exception();
            }
        }

        private static TreeNode Populate(T4Item t4item,int id)
        {
            TreeNode node = new TreeNode();
            node.Tag = t4item;
            node.Text = string.Format("Item ({0})",id);
            node.ImageKey = "t4item";
            node.SelectedImageKey = "t4item";
            return node;
        }

        private static Control Display(object obj)
        {
            if (editorcontrols.ContainsKey(obj.GetType()))
            {
                Control control = (Control)Activator.CreateInstance(editorcontrols[obj.GetType()],obj);
                control.Dock = DockStyle.Fill;
                return control;
            }
            else
            {
                return DisplayNothing();
            }
        }

        private static Control DisplayHex(byte[] data)
        {
            MysteryBox mysterybox = new MysteryBox(data);
            mysterybox.Dock = DockStyle.Fill;
            return mysterybox;
        }

        private static Control DisplayNothing()
        {
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Text = "No options available.";
            return label;
        }

        private object obj;

        public LegacyController(object obj)
        {
            this.obj = obj;
            TreeNode legacynode = Populate(obj);
            Node.Text = legacynode.Text;
            Node.ImageKey = legacynode.ImageKey;
            Node.SelectedImageKey = legacynode.SelectedImageKey;
            foreach (TreeNode subnode in legacynode.Nodes)
            {
                Node.Nodes.Add(new LegacyController(subnode.Tag).Node);
            }
        }

        protected override Control CreateEditor()
        {
            return Display(obj);
        }
    }
}
