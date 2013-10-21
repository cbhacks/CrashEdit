using Crash;
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

        static NSFBox()
        {
            imglist = new ImageList();
            try
            {
                imglist.Images.Add("default",Resources.FileImage);
                imglist.Images.Add("nsf",new Icon(Resources.NSFIcon,16,16));
                imglist.Images.Add("normalchunk",Resources.YellowJournalImage);
                imglist.Images.Add("texturechunk",Resources.ImageImage);
                imglist.Images.Add("oldsoundchunk",Resources.BlueJournalImage);
                imglist.Images.Add("soundchunk",Resources.BlueJournalImage);
                imglist.Images.Add("wavebankchunk",Resources.MusicImage);
                imglist.Images.Add("speechchunk",Resources.WhiteJournalImage);
                imglist.Images.Add("unprocessedchunk",Resources.FileImage);
                imglist.Images.Add("t1entry",Resources.ThingImage);
                imglist.Images.Add("oldmodelentry",Resources.ThingImage);
                imglist.Images.Add("modelentry",Resources.ThingImage);
                imglist.Images.Add("oldsceneryentry",Resources.ThingImage);
                imglist.Images.Add("sceneryentry",Resources.ThingImage);
                imglist.Images.Add("t4entry",Resources.ThingImage);
                imglist.Images.Add("t6entry",Resources.ThingImage);
                imglist.Images.Add("oldentityentry",Resources.ThingImage);
                imglist.Images.Add("entityentry",Resources.ThingImage);
                imglist.Images.Add("t11entry",Resources.ThingImage);
                imglist.Images.Add("soundentry",Resources.SpeakerImage);
                imglist.Images.Add("oldmusicentry",Resources.MusicImage);
                imglist.Images.Add("musicentry",Resources.MusicImage);
                imglist.Images.Add("wavebankentry",Resources.MusicImage);
                imglist.Images.Add("oldt15entry",Resources.ThingImage);
                imglist.Images.Add("t15entry",Resources.ThingImage);
                imglist.Images.Add("oldt17entry",Resources.ThingImage);
                imglist.Images.Add("t17entry",Resources.ThingImage);
                imglist.Images.Add("t18entry",Resources.ThingImage);
                imglist.Images.Add("demoentry",Resources.ThingImage);
                imglist.Images.Add("t20entry",Resources.ThingImage);
                imglist.Images.Add("speechentry",Resources.SpeakerImage);
                imglist.Images.Add("t21entry",Resources.ThingImage);
                imglist.Images.Add("unprocessedentry",Resources.ThingImage);
                imglist.Images.Add("entity",Resources.ArrowImage);
                imglist.Images.Add("vh",Resources.ArrowImage);
                imglist.Images.Add("seq",Resources.ArrowImage);
                imglist.Images.Add("t4item",Resources.ArrowImage);
                imglist.Images.Add("item",Resources.ArrowImage);
            }
            catch
            {
                imglist.Images.Clear();
            }
        }

        private NSF nsf;
        private NSFController controller;

        private List<TreeNode> searchresults;

        private SplitContainer pnSplit;
        private TreeView trvMain;

        public NSFBox(NSF nsf,GameVersion gameversion)
        {
            this.nsf = nsf;
            this.controller = new NSFController(nsf,gameversion);

            this.searchresults = new List<TreeNode>();

            controller.Node.Expand();

            trvMain = new TreeView();
            trvMain.Dock = DockStyle.Fill;
            trvMain.ImageList = imglist;
            trvMain.HideSelection = false;
            trvMain.Nodes.Add(controller.Node);
            trvMain.SelectedNode = controller.Node;
            trvMain.AllowDrop = true;
            trvMain.AfterSelect += new TreeViewEventHandler(trvMain_AfterSelect);
            trvMain.ItemDrag += new ItemDragEventHandler(trvMain_ItemDrag);
            trvMain.DragOver += new DragEventHandler(trvMain_DragOver);
            trvMain.DragDrop += new DragEventHandler(trvMain_DragDrop);

            pnSplit = new SplitContainer();
            pnSplit.Dock = DockStyle.Fill;
            pnSplit.Panel1.Controls.Add(trvMain);

            this.Controls.Add(pnSplit);
        }

        public NSF NSF
        {
            get { return nsf; }
        }

        private void trvMain_AfterSelect(object sender,TreeViewEventArgs e)
        {
            pnSplit.Panel2.Controls.Clear();
            if (e.Node != null)
            {
                object tag = e.Node.Tag;
                if (tag is Controller)
                {
                    pnSplit.Panel2.Controls.Add(((Controller)tag).Editor);
                }
            }
        }

        private void trvMain_ItemDrag(object sender,ItemDragEventArgs e)
        {
            DoDragDrop(e.Item,DragDropEffects.All);
        }

        private void trvMain_DragOver(object sender,DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            TreeNode node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            Controller item = (Controller)node.Tag;
            Point droppoint = trvMain.PointToClient(new Point(e.X,e.Y));
            TreeNode dropnode = trvMain.GetNodeAt(droppoint);
            if (dropnode == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            if (node.TreeView != dropnode.TreeView)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            Controller destination = (Controller)dropnode.Tag;
            if (item == destination)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            if (item.Move(destination,false))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void trvMain_DragDrop(object sender,DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeNode)))
            {
                return;
            }
            TreeNode node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            Controller item = (Controller)node.Tag;
            Point droppoint = trvMain.PointToClient(new Point(e.X,e.Y));
            TreeNode dropnode = trvMain.GetNodeAt(droppoint);
            if (dropnode == null)
            {
                return;
            }
            if (node.TreeView != dropnode.TreeView)
            {
                return;
            }
            Controller destination = (Controller)dropnode.Tag;
            if (item == destination)
            {
                return;
            }
            item.Move(destination,true);
            item.Node.EnsureVisible();
            trvMain.SelectedNode = item.Node;
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

        public void GotoEID(int eid)
        {
            TreeNode node = FindEID(eid,controller.Node);
            if (node != null)
            {
                node.EnsureVisible();
                trvMain.SelectedNode = node;
            }
            else
            {
                MessageBox.Show("No results found.");
            }
        }

        private TreeNode FindEID(int eid,TreeNode node)
        {
            if (node.Tag is EntryController)
            {
                if (((EntryController)node.Tag).Entry.EID == eid)
                {
                    return node;
                }
            }
            foreach (TreeNode childnode in node.Nodes)
            {
                TreeNode result = FindEID(eid,childnode);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                controller.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
