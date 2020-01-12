using Crash;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class NSFBox : UserControl
    {
        private static ImageList imglist;

        static NSFBox()
        {
            imglist = new ImageList { ColorDepth = ColorDepth.Depth32Bit };
            try
            {
                imglist.Images.Add("default",OldResources.FileImage);
                imglist.Images.Add("nsf",new Icon(OldResources.NSFIcon,16,16));
                imglist.Images.Add("yellowj",OldResources.YellowJournalImage);
                imglist.Images.Add("image",OldResources.ImageImage);
                imglist.Images.Add("bluej",OldResources.BlueJournalImage);
                imglist.Images.Add("music",OldResources.MusicImage);
                imglist.Images.Add("musicred",OldResources.MusicRedImage);
                imglist.Images.Add("musicyellow",OldResources.MusicYellowImage);
                imglist.Images.Add("whitej",OldResources.WhiteJournalImage);
                imglist.Images.Add("thing",OldResources.ThingImage);
                imglist.Images.Add("speaker",OldResources.SpeakerImage);
                imglist.Images.Add("arrow",OldResources.ArrowImage);
                imglist.Images.Add("greyb",OldResources.GreyBuckle);
                imglist.Images.Add("codeb",OldResources.CodeBuckle);
                imglist.Images.Add("crimsonb",OldResources.CrimsonBuckle);
                imglist.Images.Add("limeb",OldResources.LimeBuckle);
                imglist.Images.Add("blueb",OldResources.BlueBuckle);
                imglist.Images.Add("violetb",OldResources.VioletBuckle);
                imglist.Images.Add("redb",OldResources.RedBuckle);
                imglist.Images.Add("yellowb",OldResources.YellowBuckle);
            }
            catch
            {
                imglist.Images.Clear();
            }
        }

        private List<TreeNode> searchresults;

        private SplitContainer pnSplit;
        private TreeView trvMain;

        public NSFBox(NSF nsf, GameVersion gameversion)
        {
            NSF = nsf;
            NSFController = new NSFController(nsf, gameversion);

            searchresults = new List<TreeNode>();

            NSFController.Node.Expand();

            trvMain = new BufferedTreeView
            {
                Dock = DockStyle.Fill,
                ImageList = imglist,
                HideSelection = false,
                SelectedNode = NSFController.Node,
                AllowDrop = true
            };
            trvMain.Nodes.Add(NSFController.Node);
            trvMain.AfterSelect += new TreeViewEventHandler(trvMain_AfterSelect);
            trvMain.ItemDrag += new ItemDragEventHandler(trvMain_ItemDrag);
            trvMain.DragOver += new DragEventHandler(trvMain_DragOver);
            trvMain.DragDrop += new DragEventHandler(trvMain_DragDrop);

            pnSplit = new SplitContainer { Dock = DockStyle.Fill };
            pnSplit.Panel1.Controls.Add(trvMain);

            Controls.Add(pnSplit);
        }

        public NSF NSF { get; }
        public NSFController NSFController { get; }

        private void trvMain_AfterSelect(object sender,TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.Tag is Controller t)
                {
                    pnSplit.Panel2.Controls.Clear();
                    pnSplit.Panel2.Controls.Add(t.Editor);
                    t.Editor.Invalidate();
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
            TreeNode node = FindEID(eid,NSFController.Node);
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
                NSFController.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
