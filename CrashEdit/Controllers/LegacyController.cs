using Crash;
using Crash.Game;
using Crash.Graphics;
using Crash.Audio;
using Crash.Unknown0;
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
            if (obj is Chunk)
            {
                return Populate((Chunk)obj);
            }
            else if (obj is Entry)
            {
                return Populate((Entry)obj);
            }
            else if (obj is T4Item)
            {
                return Populate((T4Item)obj,-1);
            }
            else if (obj is Entity)
            {
                return Populate((Entity)obj);
            }
            else
            {
                throw new Exception();
            }
        }

        private static TreeNode Populate(Chunk chunk)
        {
            if (chunk is TextureChunk)
            {
                return Populate((TextureChunk)chunk);
            }
            else
            {
                throw new Exception();
            }
        }

        private static TreeNode Populate(TextureChunk chunk)
        {
            TreeNode node = new TreeNode();
            node.Tag = chunk;
            node.Text = "Texture Chunk";
            node.ImageKey = "texturechunk";
            node.SelectedImageKey = "texturechunk";
            return node;
        }

        private static TreeNode Populate(Entry entry)
        {
            if (entry is T4Entry)
            {
                return Populate((T4Entry)entry);
            }
            else if (entry is EntityEntry)
            {
                return Populate((EntityEntry)entry);
            }
            else if (entry is SoundEntry)
            {
                return Populate((SoundEntry)entry);
            }
            else if (entry is WavebankEntry)
            {
                return Populate((WavebankEntry)entry);
            }
            else if (entry is SpeechEntry)
            {
                return Populate((SpeechEntry)entry);
            }
            else
            {
                throw new Exception();
            }
        }

        private static TreeNode Populate(T4Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T4 Entry";
            node.ImageKey = "t4entry";
            node.SelectedImageKey = "t4entry";
            for (int i = 0;i < entry.T4Items.Count;i++)
            {
                node.Nodes.Add(Populate(entry.T4Items[i],i));
            }
            return node;
        }

        private static TreeNode Populate(EntityEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Entity Entry";
            node.ImageKey = "entityentry";
            node.SelectedImageKey = "entityentry";
            foreach (Entity entity in entry.Entities)
            {
                node.Nodes.Add(Populate(entity));
            }
            return node;
        }

        private static TreeNode Populate(SoundEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Sound Entry";
            node.ImageKey = "soundentry";
            node.SelectedImageKey = "soundentry";
            return node;
        }

        private static TreeNode Populate(WavebankEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = string.Format("Wavebank Entry ({0})",entry.ID);
            node.ImageKey = "wavebankentry";
            node.SelectedImageKey = "wavebankentry";
            return node;
        }

        private static TreeNode Populate(SpeechEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Speech Entry";
            node.ImageKey = "speechentry";
            node.SelectedImageKey = "speechentry";
            return node;
        }

        private static TreeNode Populate(Entity entity)
        {
            TreeNode node = new TreeNode();
            node.Tag = entity;
            if (entity.Name != null)
            {
                node.Text = string.Format("Entity ({0})",entity.Name);
            }
            else
            {
                node.Text = "Unnamed Entity";
            }
            node.ImageKey = "entity";
            node.SelectedImageKey = "entity";
            return node;
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
