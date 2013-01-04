using Crash;
using System.Windows.Forms;

using IO = System.IO;

namespace CrashEdit
{
    public sealed class MainForm : Form
    {
        private static ImageList imglist;

        static MainForm()
        {
            imglist = new ImageList();
            try
            {
                imglist.Images.Add("default",Resources.FileIcon);
                imglist.Images.Add("tb_open",Resources.OpenIcon);
                imglist.Images.Add("tb_save",Resources.SaveIcon);
                imglist.Images.Add("tb_close",Resources.FolderIcon);
                imglist.Images.Add("tb_find",Resources.BinocularsIcon);
                imglist.Images.Add("tb_findnext",Resources.BinocularsNextIcon);
            }
            catch
            {
                imglist.Images.Clear();
            }
        }

        private ToolStrip tsToolbar;
        private ToolStripButton tbbOpen;
        private ToolStripButton tbbSave;
        private ToolStripButton tbbClose;
        private ToolStripSeparator tbsSeparator;
        private ToolStripButton tbbFind;
        private ToolStripButton tbbFindNext;
        private TabControl tbcTabs;

        public MainForm()
        {
            tbbOpen = new ToolStripButton();
            tbbOpen.Text = "Open";
            tbbOpen.ImageKey = "tb_open";
            tbbOpen.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbOpen.Click += new System.EventHandler(tbbOpen_Click);

            tbbSave = new ToolStripButton();
            tbbSave.Text = "Save";
            tbbSave.ImageKey = "tb_save";
            tbbSave.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbSave.Click += new System.EventHandler(tbbSave_Click);

            tbbClose = new ToolStripButton();
            tbbClose.Text = "Close";
            tbbClose.ImageKey = "tb_close";
            tbbClose.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbClose.Click += new System.EventHandler(tbbClose_Click);

            tbsSeparator = new ToolStripSeparator();

            tbbFind = new ToolStripButton();
            tbbFind.Text = "Find";
            tbbFind.ImageKey = "tb_find";
            tbbFind.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbFind.Click += new System.EventHandler(tbbFind_Click);

            tbbFindNext = new ToolStripButton();
            tbbFindNext.Text = "Find Next";
            tbbFindNext.ImageKey = "tb_findnext";
            tbbFindNext.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbFindNext.Click += new System.EventHandler(tbbFindNext_Click);

            tsToolbar = new ToolStrip();
            tsToolbar.Dock = DockStyle.Top;
            tsToolbar.ImageList = imglist;
            tsToolbar.Items.Add(tbbOpen);
            tsToolbar.Items.Add(tbbSave);
            tsToolbar.Items.Add(tbbClose);
            tsToolbar.Items.Add(tbsSeparator);
            tsToolbar.Items.Add(tbbFind);
            tsToolbar.Items.Add(tbbFindNext);

            tbcTabs = new TabControl();
            tbcTabs.Dock = DockStyle.Fill;

            this.Width = 640;
            this.Height = 480;
            this.Controls.Add(tbcTabs);
            this.Controls.Add(tsToolbar);
        }

        void tbbOpen_Click(object sender,System.EventArgs e)
        {
            OpenNSF();
        }

        void tbbSave_Click(object sender,System.EventArgs e)
        {
            SaveNSF();
        }

        void tbbClose_Click(object sender,System.EventArgs e)
        {
            CloseNSF();
        }

        void tbbFind_Click(object sender,System.EventArgs e)
        {
            Find();
        }

        void tbbFindNext_Click(object sender,System.EventArgs e)
        {
            FindNext();
        }

        public void OpenNSF()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "NSF Files (*.nsf)|*.nsf|All Files (*.*)|*.*";
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filename in dialog.FileNames)
                    {
                        OpenNSF(filename);
                    }
                }
            }
        }

        public void OpenNSF(string filename)
        {
            byte[] nsfdata = IO.File.ReadAllBytes(filename);
            NSF nsf = NSF.Load(nsfdata);
            OpenNSF(filename,nsf);
        }

        public void OpenNSF(string filename,NSF nsf)
        {
            NSFBox nsfbox = new NSFBox(nsf);
            nsfbox.Dock = DockStyle.Fill;

            TabPage nsftab = new TabPage(filename);
            nsftab.Tag = nsfbox;
            nsftab.Controls.Add(nsfbox);

            tbcTabs.TabPages.Add(nsftab);
            tbcTabs.SelectedTab = nsftab;
        }

        public void SaveNSF()
        {
            if (tbcTabs.SelectedTab != null)
            {
                string filename = tbcTabs.SelectedTab.Text;
                NSFBox nsfbox = (NSFBox)tbcTabs.SelectedTab.Tag;
                SaveNSF(filename,nsfbox.NSF);
            }
        }

        public void SaveNSF(string filename,NSF nsf)
        {
            byte[] nsfdata = nsf.Save();
            if (MessageBox.Show("Saving can (and in most cases will) produce an NSF file partially unusable by the game.\n\nContinue anyway?","Save Confirmation Prompt",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                IO.File.WriteAllBytes(filename,nsfdata);
            }
        }

        public void CloseNSF()
        {
            if (tbcTabs.SelectedTab != null)
            {
                tbcTabs.TabPages.Remove(tbcTabs.SelectedTab);
            }
        }

        public void Find()
        {
            if (tbcTabs.SelectedTab != null)
            {
                NSFBox nsfbox = (NSFBox)tbcTabs.SelectedTab.Tag;
                using (InputWindow inputwindow = new InputWindow())
                {
                    if (inputwindow.ShowDialog() == DialogResult.OK)
                    {
                        nsfbox.Find(inputwindow.Input);
                    }
                }
            }
        }

        public void FindNext()
        {
            if (tbcTabs.SelectedTab != null)
            {
                NSFBox nsfbox = (NSFBox)tbcTabs.SelectedTab.Tag;
                nsfbox.FindNext();
            }
        }
    }
}
