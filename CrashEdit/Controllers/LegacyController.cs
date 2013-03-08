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
            else if (obj is SEQ)
            {
                return Populate((SEQ)obj,-1);
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
            if (chunk is NormalChunk)
            {
                return Populate((NormalChunk)chunk);
            }
            else if (chunk is TextureChunk)
            {
                return Populate((TextureChunk)chunk);
            }
            else if (chunk is WavebankChunk)
            {
                return Populate((WavebankChunk)chunk);
            }
            else
            {
                throw new Exception();
            }
        }

        private static TreeNode Populate(NormalChunk chunk)
        {
            TreeNode node = new TreeNode();
            node.Tag = chunk;
            node.Text = "Normal Chunk";
            node.ImageKey = "normalchunk";
            node.SelectedImageKey = "normalchunk";
            foreach (Entry entry in chunk.Entries)
            {
                node.Nodes.Add(Populate(entry));
            }
            return node;
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

        private static TreeNode Populate(WavebankChunk chunk)
        {
            TreeNode node = Populate(chunk.Entry);
            node.Tag = chunk;
            node.Text = string.Format("Wavebank Chunk ({0})",chunk.Entry.ID);
            node.ImageKey = "wavebankchunk";
            node.SelectedImageKey = "wavebankchunk";
            return node;
        }

        private static TreeNode Populate(Entry entry)
        {
            if (entry is T1Entry)
            {
                return Populate((T1Entry)entry);
            }
            else if (entry is T2Entry)
            {
                return Populate((T2Entry)entry);
            }
            else if (entry is T3Entry)
            {
                return Populate((T3Entry)entry);
            }
            else if (entry is T4Entry)
            {
                return Populate((T4Entry)entry);
            }
            else if (entry is EntityEntry)
            {
                return Populate((EntityEntry)entry);
            }
            else if (entry is T11Entry)
            {
                return Populate((T11Entry)entry);
            }
            else if (entry is SoundEntry)
            {
                return Populate((SoundEntry)entry);
            }
            else if (entry is MusicEntry)
            {
                return Populate((MusicEntry)entry);
            }
            else if (entry is WavebankEntry)
            {
                return Populate((WavebankEntry)entry);
            }
            else if (entry is T15Entry)
            {
                return Populate((T15Entry)entry);
            }
            else if (entry is T17Entry)
            {
                return Populate((T17Entry)entry);
            }
            else if (entry is DemoEntry)
            {
                return Populate((DemoEntry)entry);
            }
            else if (entry is SpeechEntry)
            {
                return Populate((SpeechEntry)entry);
            }
            else if (entry is T21Entry)
            {
                return Populate((T21Entry)entry);
            }
            else
            {
                throw new Exception();
            }
        }

        private static TreeNode Populate(T1Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T1 Entry";
            node.ImageKey = "t1entry";
            node.SelectedImageKey = "t1entry";
            return node;
        }

        private static TreeNode Populate(T2Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T2 Entry";
            node.ImageKey = "t2entry";
            node.SelectedImageKey = "t2entry";
            return node;
        }

        private static TreeNode Populate(T3Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T3 Entry";
            node.ImageKey = "t3entry";
            node.SelectedImageKey = "t3entry";
            return node;
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

        private static TreeNode Populate(T11Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T11 Entry";
            node.ImageKey = "t11entry";
            node.SelectedImageKey = "t11entry";
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

        private static TreeNode Populate(MusicEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Music Entry";
            node.ImageKey = "musicentry";
            node.SelectedImageKey = "musicentry";
            for (int i = 0;i < entry.SEP.SEQs.Count;i++)
            {
                node.Nodes.Add(Populate(entry.SEP.SEQs[i],i));
            }
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

        private static TreeNode Populate(T15Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T15 Entry";
            node.ImageKey = "t15entry";
            node.SelectedImageKey = "t15entry";
            return node;
        }

        private static TreeNode Populate(T17Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T17 Entry";
            node.ImageKey = "t17entry";
            node.SelectedImageKey = "t17entry";
            return node;
        }

        private static TreeNode Populate(DemoEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Demo Entry";
            node.ImageKey = "demoentry";
            node.SelectedImageKey = "demoentry";
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

        private static TreeNode Populate(T21Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T21 Entry";
            node.ImageKey = "t21entry";
            node.SelectedImageKey = "t21entry";
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

        private static TreeNode Populate(SEQ seq,int id)
        {
            TreeNode node = new TreeNode();
            node.Tag = seq;
            node.Text = string.Format("SEQ ({0})",id);
            node.ImageKey = "seq";
            node.SelectedImageKey = "seq";
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
            else if (obj is WavebankChunk)
            {
                return Display(((WavebankChunk)obj).Entry);
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

        public LegacyController(object obj)
        {
            TreeNode legacynode = Populate(obj);
            Node.Text = legacynode.Text;
            Node.ImageKey = legacynode.ImageKey;
            Node.SelectedImageKey = legacynode.SelectedImageKey;
            foreach (TreeNode subnode in legacynode.Nodes)
            {
                Node.Nodes.Add(new LegacyController(subnode.Tag).Node);
            }
            Editor = Display(obj);
        }
    }
}
