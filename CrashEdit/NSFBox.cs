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
    public sealed class NSFBox : UserControl
    {
        private static ImageList imglist;
        private static Dictionary<Type,Type> editorcontrols;

        static NSFBox()
        {
            imglist = new ImageList();
            try
            {
                imglist.Images.Add("default",Resources.FileIcon);
                imglist.Images.Add("trv_nsf",Resources.FileIcon);
                imglist.Images.Add("trv_normalchunk",Resources.YellowJournalIcon);
                imglist.Images.Add("trv_texturechunk",Resources.ImageIcon);
                imglist.Images.Add("trv_soundchunk",Resources.BlueJournalIcon);
                imglist.Images.Add("trv_wavebankchunk",Resources.MusicIcon);
                imglist.Images.Add("trv_speechchunk",Resources.WhiteJournalIcon);
                imglist.Images.Add("trv_unknownchunk",Resources.FileIcon);
                imglist.Images.Add("trv_t1entry",Resources.ThingIcon);
                imglist.Images.Add("trv_t2entry",Resources.ThingIcon);
                imglist.Images.Add("trv_t3entry",Resources.ThingIcon);
                imglist.Images.Add("trv_t4entry",Resources.ThingIcon);
                imglist.Images.Add("trv_entityentry",Resources.ThingIcon);
                imglist.Images.Add("trv_t11entry",Resources.ThingIcon);
                imglist.Images.Add("trv_soundentry",Resources.SpeakerIcon);
                imglist.Images.Add("trv_musicentry",Resources.MusicIcon);
                imglist.Images.Add("trv_wavebankentry",Resources.MusicIcon);
                imglist.Images.Add("trv_t15entry",Resources.ThingIcon);
                imglist.Images.Add("trv_t17entry",Resources.ThingIcon);
                imglist.Images.Add("trv_demoentry",Resources.ThingIcon);
                imglist.Images.Add("trv_speechentry",Resources.SpeakerIcon);
                imglist.Images.Add("trv_t21entry",Resources.ThingIcon);
                imglist.Images.Add("trv_unknownentry",Resources.ThingIcon);
                imglist.Images.Add("trv_entity",Resources.ArrowIcon);
                imglist.Images.Add("trv_seq",Resources.ArrowIcon);
                imglist.Images.Add("trv_t4item",Resources.ArrowIcon);
            }
            catch
            {
                imglist.Images.Clear();
            }
            editorcontrols = new Dictionary<Type,Type>();
            foreach (Type editorcontrol in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (EditorControlAttribute attribute in editorcontrol.GetCustomAttributes(typeof(EditorControlAttribute),false))
                {
                    editorcontrols.Add(attribute.Type,editorcontrol);
                }
            }
        }

        private NSF nsf;

        private List<TreeNode> searchresults;

        private SplitContainer pnSplit;
        private TreeView trvMain;

        public NSFBox(NSF nsf)
        {
            this.nsf = nsf;

            this.searchresults = new List<TreeNode>();

            TreeNode rootnode = Populate(nsf);
            rootnode.Expand();

            trvMain = new TreeView();
            trvMain.Dock = DockStyle.Fill;
            trvMain.ImageList = imglist;
            trvMain.HideSelection = false;
            trvMain.Nodes.Add(rootnode);
            trvMain.SelectedNode = rootnode;
            trvMain.AfterSelect += new TreeViewEventHandler(trvMain_AfterSelect);

            pnSplit = new SplitContainer();
            pnSplit.Dock = DockStyle.Fill;
            pnSplit.Panel1.Controls.Add(trvMain);

            this.Controls.Add(pnSplit);
        }

        public NSF NSF
        {
            get { return nsf; }
        }

        void trvMain_AfterSelect(object sender,TreeViewEventArgs e)
        {
            Control control;
            if (e.Node != null)
            {
                control = Display(e.Node.Tag);
            }
            else
            {
                control = DisplayNothing();
            }
            foreach (Control c in pnSplit.Panel2.Controls)
            {
                c.Dispose();
            }
            pnSplit.Panel2.Controls.Clear();
            pnSplit.Panel2.Controls.Add(control);
        }

        public void Find(string term)
        {
            term = term.ToUpper();
            searchresults.Clear();
            foreach (TreeNode node in trvMain.Nodes)
            {
                FindNode(term,node);
            }
            FindNext();
        }

        public void FindNext()
        {
            if (searchresults.Count > 0)
            {
                trvMain.SelectedNode = searchresults[0];
                searchresults.RemoveAt(0);
            }
            else
            {
                MessageBox.Show("No results found.");
            }
        }

        private void FindNode(string term,TreeNode node)
        {
            if (node.Text.ToUpper().Contains(term))
            {
                searchresults.Add(node);
            }
            foreach (TreeNode childnode in node.Nodes)
            {
                FindNode(term,childnode);
            }
        }

        private TreeNode Populate(NSF nsf)
        {
            TreeNode node = new TreeNode();
            node.Tag = nsf;
            node.Text = "NSF File";
            node.ImageKey = "trv_nsf";
            node.SelectedImageKey = "trv_nsf";
            foreach (Chunk chunk in nsf.Chunks)
            {
                node.Nodes.Add(Populate(chunk));
            }
            return node;
        }

        private TreeNode Populate(Chunk chunk)
        {
            if (chunk is NormalChunk)
            {
                return Populate((NormalChunk)chunk);
            }
            else if (chunk is TextureChunk)
            {
                return Populate((TextureChunk)chunk);
            }
            else if (chunk is SoundChunk)
            {
                return Populate((SoundChunk)chunk);
            }
            else if (chunk is WavebankChunk)
            {
                return Populate((WavebankChunk)chunk);
            }
            else if (chunk is SpeechChunk)
            {
                return Populate((SpeechChunk)chunk);
            }
            else if (chunk is UnknownChunk)
            {
                return Populate((UnknownChunk)chunk);
            }
            else
            {
                throw new Exception();
            }
        }

        private TreeNode Populate(NormalChunk chunk)
        {
            TreeNode node = new TreeNode();
            node.Tag = chunk;
            node.Text = "Normal Chunk";
            node.ImageKey = "trv_normalchunk";
            node.SelectedImageKey = "trv_normalchunk";
            foreach (Entry entry in chunk.Entries)
            {
                node.Nodes.Add(Populate(entry));
            }
            return node;
        }

        private TreeNode Populate(TextureChunk chunk)
        {
            TreeNode node = new TreeNode();
            node.Tag = chunk;
            node.Text = "Texture Chunk";
            node.ImageKey = "trv_texturechunk";
            node.SelectedImageKey = "trv_texturechunk";
            return node;
        }

        private TreeNode Populate(SoundChunk chunk)
        {
            TreeNode node = new TreeNode();
            node.Tag = chunk;
            node.Text = "Sound Chunk";
            node.ImageKey = "trv_soundchunk";
            node.SelectedImageKey = "trv_soundchunk";
            foreach (SoundEntry entry in chunk.Entries)
            {
                node.Nodes.Add(Populate(entry));
            }
            return node;
        }

        private TreeNode Populate(WavebankChunk chunk)
        {
            TreeNode node = Populate(chunk.Entry);
            node.Tag = chunk;
            node.Text = string.Format("Wavebank Chunk ({0})",chunk.Entry.ID);
            node.ImageKey = "trv_wavebankchunk";
            node.SelectedImageKey = "trv_wavebankchunk";
            return node;
        }

        private TreeNode Populate(SpeechChunk chunk)
        {
            TreeNode node = new TreeNode();
            node.Tag = chunk;
            node.Text = "Speech Chunk";
            node.ImageKey = "trv_speechchunk";
            node.SelectedImageKey = "trv_speechchunk";
            foreach (SpeechEntry entry in chunk.Entries)
            {
                node.Nodes.Add(Populate(entry));
            }
            return node;
        }

        private TreeNode Populate(UnknownChunk chunk)
        {
            TreeNode node = new TreeNode();
            node.Tag = chunk;
            node.Text = "Unknown Chunk";
            node.ImageKey = "trv_unknownchunk";
            node.SelectedImageKey = "trv_unknownchunk";
            return node;
        }

        private TreeNode Populate(Entry entry)
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
            else if (entry is UnknownEntry)
            {
                return Populate((UnknownEntry)entry);
            }
            else
            {
                throw new Exception();
            }
        }

        private TreeNode Populate(T1Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T1 Entry";
            node.ImageKey = "trv_t1entry";
            node.SelectedImageKey = "trv_t1entry";
            return node;
        }

        private TreeNode Populate(T2Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T2 Entry";
            node.ImageKey = "trv_t2entry";
            node.SelectedImageKey = "trv_t2entry";
            return node;
        }

        private TreeNode Populate(T3Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T3 Entry";
            node.ImageKey = "trv_t3entry";
            node.SelectedImageKey = "trv_t3entry";
            return node;
        }

        private TreeNode Populate(T4Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T4 Entry";
            node.ImageKey = "trv_t4entry";
            node.SelectedImageKey = "trv_t4entry";
            for (int i = 0;i < entry.T4Items.Count;i++)
            {
                node.Nodes.Add(Populate(entry.T4Items[i],i));
            }
            return node;
        }

        private TreeNode Populate(EntityEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Entity Entry";
            node.ImageKey = "trv_entityentry";
            node.SelectedImageKey = "trv_entityentry";
            foreach (Entity entity in entry.Entities)
            {
                node.Nodes.Add(Populate(entity));
            }
            return node;
        }

        private TreeNode Populate(T11Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T11 Entry";
            node.ImageKey = "trv_t11entry";
            node.SelectedImageKey = "trv_t11entry";
            return node;
        }

        private TreeNode Populate(SoundEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Sound Entry";
            node.ImageKey = "trv_soundentry";
            node.SelectedImageKey = "trv_soundentry";
            return node;
        }

        private TreeNode Populate(MusicEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Music Entry";
            node.ImageKey = "trv_musicentry";
            node.SelectedImageKey = "trv_musicentry";
            for (int i = 0;i < entry.SEP.SEQs.Count;i++)
            {
                node.Nodes.Add(Populate(entry.SEP.SEQs[i],i));
            }
            return node;
        }

        private TreeNode Populate(WavebankEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = string.Format("Wavebank Entry ({0})",entry.ID);
            node.ImageKey = "trv_wavebankentry";
            node.SelectedImageKey = "trv_wavebankentry";
            return node;
        }

        private TreeNode Populate(T15Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T15 Entry";
            node.ImageKey = "trv_t15entry";
            node.SelectedImageKey = "trv_t15entry";
            return node;
        }

        private TreeNode Populate(T17Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T17 Entry";
            node.ImageKey = "trv_t17entry";
            node.SelectedImageKey = "trv_t17entry";
            return node;
        }

        private TreeNode Populate(DemoEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Demo Entry";
            node.ImageKey = "trv_demoentry";
            node.SelectedImageKey = "trv_demoentry";
            return node;
        }

        private TreeNode Populate(SpeechEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Speech Entry";
            node.ImageKey = "trv_speechentry";
            node.SelectedImageKey = "trv_speechentry";
            return node;
        }

        private TreeNode Populate(T21Entry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "T21 Entry";
            node.ImageKey = "trv_t21entry";
            node.SelectedImageKey = "trv_t21entry";
            return node;
        }

        private TreeNode Populate(UnknownEntry entry)
        {
            TreeNode node = new TreeNode();
            node.Tag = entry;
            node.Text = "Unknown Entry";
            node.ImageKey = "trv_unknownentry";
            node.SelectedImageKey = "trv_unknownentry";
            return node;
        }

        private TreeNode Populate(Entity entity)
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
            node.ImageKey = "trv_entity";
            node.SelectedImageKey = "trv_entity";
            return node;
        }

        private TreeNode Populate(SEQ seq,int id)
        {
            TreeNode node = new TreeNode();
            node.Tag = seq;
            node.Text = string.Format("SEQ ({0})",id);
            node.ImageKey = "trv_seq";
            node.SelectedImageKey = "trv_seq";
            return node;
        }

        private TreeNode Populate(T4Item t4item,int id)
        {
            TreeNode node = new TreeNode();
            node.Tag = t4item;
            node.Text = string.Format("Item ({0})",id);
            node.ImageKey = "trv_t4item";
            node.SelectedImageKey = "trv_t4item";
            return node;
        }

        private Control Display(object obj)
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

        private Control DisplayHex(byte[] data)
        {
            MysteryBox mysterybox = new MysteryBox(data);
            mysterybox.Dock = DockStyle.Fill;
            return mysterybox;
        }

        private Control DisplayNothing()
        {
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Text = "No options available.";
            return label;
        }
    }
}
