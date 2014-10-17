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
                imglist.Images.Add("default",OldResources.FileImage);
                imglist.Images.Add("nsf",new Icon(OldResources.NSFIcon,16,16));
                imglist.Images.Add("normalchunk",OldResources.YellowJournalImage);
                imglist.Images.Add("texturechunk",OldResources.ImageImage);
                imglist.Images.Add("oldsoundchunk",OldResources.BlueJournalImage);
                imglist.Images.Add("soundchunk",OldResources.BlueJournalImage);
                imglist.Images.Add("wavebankchunk",OldResources.MusicImage);
                imglist.Images.Add("speechchunk",OldResources.WhiteJournalImage);
                imglist.Images.Add("unprocessedchunk",OldResources.FileImage);
                imglist.Images.Add("oldanimationentry",OldResources.ThingImage);
                imglist.Images.Add("t1entry",OldResources.ThingImage);
                imglist.Images.Add("oldmodelentry",OldResources.ThingImage);
                imglist.Images.Add("modelentry",OldResources.ThingImage);
                imglist.Images.Add("oldsceneryentry",OldResources.ThingImage);
                imglist.Images.Add("sceneryentry",OldResources.ThingImage);
                imglist.Images.Add("t4entry",OldResources.ThingImage);
                imglist.Images.Add("t6entry",OldResources.ThingImage);
                imglist.Images.Add("oldzoneentry",OldResources.ThingImage);
                imglist.Images.Add("zoneentry",OldResources.ThingImage);
                imglist.Images.Add("t11entry",OldResources.ThingImage);
                imglist.Images.Add("soundentry",OldResources.SpeakerImage);
                imglist.Images.Add("oldmusicentry",OldResources.MusicImage);
                imglist.Images.Add("musicentry",OldResources.MusicImage);
                imglist.Images.Add("wavebankentry",OldResources.MusicImage);
                imglist.Images.Add("oldt15entry",OldResources.ThingImage);
                imglist.Images.Add("t15entry",OldResources.ThingImage);
                imglist.Images.Add("oldt17entry",OldResources.ThingImage);
                imglist.Images.Add("t17entry",OldResources.ThingImage);
                imglist.Images.Add("t18entry",OldResources.ThingImage);
                imglist.Images.Add("demoentry",OldResources.ThingImage);
                imglist.Images.Add("t20entry",OldResources.ThingImage);
                imglist.Images.Add("speechentry",OldResources.SpeakerImage);
                imglist.Images.Add("t21entry",OldResources.ThingImage);
                imglist.Images.Add("unprocessedentry",OldResources.ThingImage);
                imglist.Images.Add("oldframe",OldResources.ArrowImage);
                imglist.Images.Add("entity",OldResources.ArrowImage);
                imglist.Images.Add("vh",OldResources.ArrowImage);
                imglist.Images.Add("seq",OldResources.ArrowImage);
                imglist.Images.Add("t4item",OldResources.ArrowImage);
                imglist.Images.Add("item",OldResources.ArrowImage);
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
