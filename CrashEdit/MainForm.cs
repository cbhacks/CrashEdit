using Crash;
using Crash.UI;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class MainForm : Form
    {
        private List<ToolStripItem> syncstripitems;

        public MainForm()
        {
            InitializeComponent();

            syncstripitems = new List<ToolStripItem>();
            syncstripitems.Add(mniFileSave);
            syncstripitems.Add(mniFileSaveAs);
            syncstripitems.Add(mniFilePatchNSD);
            syncstripitems.Add(mniFileClose);
            syncstripitems.Add(mniFindFind);
            syncstripitems.Add(mniFindFindNext);
            syncstripitems.Add(mniFindEntryID);
            syncstripitems.Add(mniFindEntryName);
            syncstripitems.Add(mniFindObjectName);
            syncstripitems.Add(tbiSave);
            syncstripitems.Add(tbiClose);
            syncstripitems.Add(tbiPatchNSD);
            syncstripitems.Add(tbiFind);
            syncstripitems.Add(tbiFindNext);
            syncstripitems.Add(tbiGotoEID);

            // Set menu strings
            mnuFile.Text = Properties.Resources.Menu_File;
            mniFileOpen.Text = Properties.Resources.Menu_File_Open;
            mniFileSave.Text = Properties.Resources.Menu_File_Save;
            mniFileSaveAs.Text = Properties.Resources.Menu_File_SaveAs;
            mniFilePatchNSD.Text = Properties.Resources.Menu_File_PatchNSD;
            mniFileClose.Text = Properties.Resources.Menu_File_Close;
            mniFileExit.Text = Properties.Resources.Menu_File_Exit;
            mnuFind.Text = Properties.Resources.Menu_Find;
            mniFindFind.Text = Properties.Resources.Menu_Find_Find;
            mniFindFindNext.Text = Properties.Resources.Menu_Find_FindNext;
            mniFindEntryID.Text = Properties.Resources.Menu_Find_EntryID;
            mniFindEntryName.Text = Properties.Resources.Menu_Find_EntryName;
            mniFindObjectName.Text = Properties.Resources.Menu_Find_ObjectName;
            mnuUndo.Text = Properties.Resources.Menu_Undo;
            mniUndoCurrentState.Text = Properties.Resources.Menu_Undo_CurrentState;

            // Set toolbar strings
            tbiOpen.Text = Properties.Resources.Toolbar_Open;
            tbiSave.Text = Properties.Resources.Toolbar_Save;
            tbiClose.Text = Properties.Resources.Toolbar_Close;
            tbiPatchNSD.Text = Properties.Resources.Toolbar_PatchNSD;
            tbiFind.Text = Properties.Resources.Toolbar_Find;
            tbiFindNext.Text = Properties.Resources.Toolbar_FindNext;
            tbiGotoEID.Text = Properties.Resources.Toolbar_GotoEID;

            SyncUI();
        }

        public void SyncUI()
        {
            foreach (ToolStripItem stripitem in syncstripitems)
            {
                stripitem.Enabled = false;
            }
            mniUndoUndo.Enabled = false;
            mniUndoUndo.Text = Properties.Resources.Menu_Undo_UndoNone;
            mniUndoRedo.Enabled = false;
            mniUndoRedo.Text = Properties.Resources.Menu_Undo_RedoNone;
            tbiUndo.Enabled = false;
            tbiUndo.Text = Properties.Resources.Toolbar_UndoNone;
            tbiRedo.Enabled = false;
            tbiRedo.Text = Properties.Resources.Toolbar_RedoNone;
            sbiStatus.Text = "";
            sbiProgress.Visible = false;
            if (uxTabs.SelectedIndex == -1)
                return;
            if (uxTabs.SelectedTab.Tag is MainControl)
            {
                foreach (ToolStripItem stripitem in syncstripitems)
                {
                    stripitem.Enabled = true;
                }
            }
            else if (uxTabs.SelectedTab.Tag is NSFBox)
            {
                mniFileClose.Enabled = true;
                tbiClose.Enabled = true;
            }
        }

        private void MainControl_SyncMasterUI(object sender,EventArgs e)
        {
            SyncUI();
        }

        private void uxTabs_Selected(object sender,TabControlEventArgs e)
        {
            switch (e.Action)
            {
                case TabControlAction.Selected:
                    if (e.TabPage.Tag is MainControl)
                    {
                        ((MainControl)e.TabPage.Tag).SyncMasterUI += MainControl_SyncMasterUI;
                    }
                    break;
                case TabControlAction.Deselected:
                    if (e.TabPage.Tag is MainControl)
                    {
                        ((MainControl)e.TabPage.Tag).SyncMasterUI -= MainControl_SyncMasterUI;
                    }
                    break;
            }
            SyncUI();
        }

        private void mniFileOpen_Click(object sender,EventArgs e)
        {
            // ShowDialog has major re-entrancy issues here
            // https://connect.microsoft.com/VisualStudio/feedback/details/697252
            if (dlgOpenNSF.Tag != null)
                return;
            DialogResult openresult;
            try
            {
                dlgOpenNSF.Tag = true;
                openresult = dlgOpenNSF.ShowDialog();
            }
            finally
            {
                dlgOpenNSF.Tag = null;
            }
            if (openresult == DialogResult.OK)
            {
                GameVersion gameversion;
                using (GameVersionForm gameversionform = new GameVersionForm())
                {
                    if (gameversionform.ShowDialog(this) != DialogResult.OK)
                        return;
                    gameversion = gameversionform.SelectedVersion;
                }
                foreach (string filename in dlgOpenNSF.FileNames)
                {
                    MainControl control = new MainControl(new FileInfo(filename),gameversion);
                    control.Dock = DockStyle.Fill;
                    TabPage tab = new TabPage(filename);
                    tab.Tag = control;
                    tab.Controls.Add(control);
                    uxTabs.TabPages.Add(tab);
                    NSFBox nsfbox = new NSFBox(control.NSFController.NSF,gameversion);
                    nsfbox.Dock = DockStyle.Fill;
                    TabPage oldtab = new TabPage("[OLD] " + filename);
                    oldtab.Tag = nsfbox;
                    oldtab.Controls.Add(nsfbox);
                    uxTabs.TabPages.Add(oldtab);
                    uxTabs.SelectedTab = tab;
                }
            }
        }

        private void mniFileSave_Click(object sender,EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mniFileSaveAs_Click(object sender,EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mniFilePatchNSD_Click(object sender,EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mniFileClose_Click(object sender,EventArgs e)
        {
            TabPage tab = uxTabs.SelectedTab;
            uxTabs.TabPages.RemoveAt(uxTabs.SelectedIndex);
            tab.Dispose();
        }

        private void mniFileExit_Click(object sender,EventArgs e)
        {
            Close();
        }

        private void mniFindFind_Click(object sender,EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mniFindFindNext_Click(object sender,EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mniFindEntryID_Click(object sender,EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mniFindEntryName_Click(object sender,EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mniFindObjectName_Click(object sender,EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mniUndoRedo_Click(object sender,EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mniUndoUndo_Click(object sender,EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void tbiOpen_Click(object sender,EventArgs e)
        {
            mniFileOpen_Click(null,null);
        }

        private void tbiSave_Click(object sender,EventArgs e)
        {
            mniFileSave_Click(null,null);
        }

        private void tbiClose_Click(object sender,EventArgs e)
        {
            mniFileClose_Click(null,null);
        }

        private void tbiPatchNSD_Click(object sender,EventArgs e)
        {
            mniFilePatchNSD_Click(null,null);
        }

        private void tbiFind_Click(object sender,EventArgs e)
        {
            mniFindFind_Click(null,null);
        }

        private void tbiFindNext_Click(object sender,EventArgs e)
        {
            mniFindFindNext_Click(null,null);
        }

        private void tbiGotoEID_Click(object sender,EventArgs e)
        {
            mniFindEntryID_Click(null,null);
        }

        private void tbiUndo_Click(object sender,EventArgs e)
        {
            mniUndoUndo_Click(null,null);
        }

        private void tbiRedo_Click(object sender,EventArgs e)
        {
            mniUndoRedo_Click(null,null);
        }
    }
}
