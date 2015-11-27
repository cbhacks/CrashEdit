namespace CrashEdit
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uxTabs = new System.Windows.Forms.TabControl();
            this.uxStartPage = new System.Windows.Forms.TabPage();
            this.uxToolbar = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.uxStatusbar = new System.Windows.Forms.StatusStrip();
            this.sbiStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbiProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.uxMenubar = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFindEntryName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFindObjectName = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mniUndoCurrentState = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tbiOpen = new System.Windows.Forms.ToolStripButton();
            this.tbiSave = new System.Windows.Forms.ToolStripButton();
            this.tbiClose = new System.Windows.Forms.ToolStripButton();
            this.tbiPatchNSD = new System.Windows.Forms.ToolStripButton();
            this.tbiFind = new System.Windows.Forms.ToolStripButton();
            this.tbiFindNext = new System.Windows.Forms.ToolStripButton();
            this.tbiGotoEID = new System.Windows.Forms.ToolStripButton();
            this.tbiUndo = new System.Windows.Forms.ToolStripButton();
            this.tbiRedo = new System.Windows.Forms.ToolStripButton();
            this.mniFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFilePatchNSD = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFindFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFindFindNext = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFindEntryID = new System.Windows.Forms.ToolStripMenuItem();
            this.mniUndoRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.mniUndoUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpenNSF = new System.Windows.Forms.OpenFileDialog();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.uxTabs.SuspendLayout();
            this.uxToolbar.SuspendLayout();
            this.uxStatusbar.SuspendLayout();
            this.uxMenubar.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxTabs
            // 
            this.uxTabs.Controls.Add(this.uxStartPage);
            this.uxTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTabs.Location = new System.Drawing.Point(0,49);
            this.uxTabs.Name = "uxTabs";
            this.uxTabs.SelectedIndex = 0;
            this.uxTabs.Size = new System.Drawing.Size(632,380);
            this.uxTabs.TabIndex = 0;
            this.uxTabs.Selected += new System.Windows.Forms.TabControlEventHandler(this.uxTabs_Selected);
            // 
            // uxStartPage
            // 
            this.uxStartPage.Location = new System.Drawing.Point(4,22);
            this.uxStartPage.Name = "uxStartPage";
            this.uxStartPage.Size = new System.Drawing.Size(624,354);
            this.uxStartPage.TabIndex = 0;
            this.uxStartPage.Text = "CrashEdit";
            this.uxStartPage.UseVisualStyleBackColor = true;
            // 
            // uxToolbar
            // 
            this.uxToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbiOpen,
            this.tbiSave,
            this.tbiClose,
            this.toolStripSeparator5,
            this.tbiPatchNSD,
            this.toolStripSeparator6,
            this.tbiFind,
            this.tbiFindNext,
            this.tbiGotoEID,
            this.toolStripSeparator9,
            this.tbiUndo,
            this.tbiRedo});
            this.uxToolbar.Location = new System.Drawing.Point(0,24);
            this.uxToolbar.Name = "uxToolbar";
            this.uxToolbar.Size = new System.Drawing.Size(632,25);
            this.uxToolbar.TabIndex = 1;
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6,25);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6,25);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6,25);
            // 
            // uxStatusbar
            // 
            this.uxStatusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbiStatus,
            this.sbiProgress});
            this.uxStatusbar.Location = new System.Drawing.Point(0,429);
            this.uxStatusbar.Name = "uxStatusbar";
            this.uxStatusbar.Size = new System.Drawing.Size(632,22);
            this.uxStatusbar.TabIndex = 2;
            // 
            // sbiStatus
            // 
            this.sbiStatus.Name = "sbiStatus";
            this.sbiStatus.Size = new System.Drawing.Size(515,17);
            this.sbiStatus.Spring = true;
            this.sbiStatus.Text = "<READY>";
            this.sbiStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sbiProgress
            // 
            this.sbiProgress.Name = "sbiProgress";
            this.sbiProgress.Size = new System.Drawing.Size(100,16);
            // 
            // uxMenubar
            // 
            this.uxMenubar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuFind,
            this.mnuUndo});
            this.uxMenubar.Location = new System.Drawing.Point(0,0);
            this.uxMenubar.Name = "uxMenubar";
            this.uxMenubar.Size = new System.Drawing.Size(632,24);
            this.uxMenubar.TabIndex = 3;
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileOpen,
            this.toolStripSeparator1,
            this.mniFileSave,
            this.mniFileSaveAs,
            this.toolStripSeparator2,
            this.mniFilePatchNSD,
            this.toolStripSeparator3,
            this.mniFileClose,
            this.toolStripSeparator4,
            this.mniFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(56,20);
            this.mnuFile.Text = "<FILE>";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161,6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(161,6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(161,6);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(161,6);
            // 
            // mniFileExit
            // 
            this.mniFileExit.Name = "mniFileExit";
            this.mniFileExit.Size = new System.Drawing.Size(164,22);
            this.mniFileExit.Text = "<EXIT>";
            this.mniFileExit.Click += new System.EventHandler(this.mniFileExit_Click);
            // 
            // mnuFind
            // 
            this.mnuFind.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFindFind,
            this.mniFindFindNext,
            this.toolStripSeparator7,
            this.mniFindEntryID,
            this.mniFindEntryName,
            this.toolStripSeparator8,
            this.mniFindObjectName});
            this.mnuFind.Name = "mnuFind";
            this.mnuFind.Size = new System.Drawing.Size(59,20);
            this.mnuFind.Text = "<FIND>";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(172,6);
            // 
            // mniFindEntryName
            // 
            this.mniFindEntryName.Name = "mniFindEntryName";
            this.mniFindEntryName.Size = new System.Drawing.Size(175,22);
            this.mniFindEntryName.Text = "<ENTRY NAME>";
            this.mniFindEntryName.Click += new System.EventHandler(this.mniFindEntryName_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(172,6);
            // 
            // mniFindObjectName
            // 
            this.mniFindObjectName.Name = "mniFindObjectName";
            this.mniFindObjectName.Size = new System.Drawing.Size(175,22);
            this.mniFindObjectName.Text = "<OBJECT NAME>";
            this.mniFindObjectName.Click += new System.EventHandler(this.mniFindObjectName_Click);
            // 
            // mnuUndo
            // 
            this.mnuUndo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniUndoRedo,
            this.mniUndoCurrentState,
            this.mniUndoUndo});
            this.mnuUndo.Name = "mnuUndo";
            this.mnuUndo.Size = new System.Drawing.Size(64,20);
            this.mnuUndo.Text = "<UNDO>";
            // 
            // mniUndoCurrentState
            // 
            this.mniUndoCurrentState.Enabled = false;
            this.mniUndoCurrentState.Image = global::CrashEdit.Properties.Resources.objects_028;
            this.mniUndoCurrentState.Name = "mniUndoCurrentState";
            this.mniUndoCurrentState.Size = new System.Drawing.Size(171,22);
            this.mniUndoCurrentState.Text = "<CURRENT STATE>";
            // 
            // mniFileSaveAs
            // 
            this.mniFileSaveAs.Name = "mniFileSaveAs";
            this.mniFileSaveAs.Size = new System.Drawing.Size(164,22);
            this.mniFileSaveAs.Text = "<SAVE AS>";
            this.mniFileSaveAs.Click += new System.EventHandler(this.mniFileSaveAs_Click);
            // 
            // tbiOpen
            // 
            this.tbiOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiOpen.Image = global::CrashEdit.Properties.Resources.Computer_File_064;
            this.tbiOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiOpen.Name = "tbiOpen";
            this.tbiOpen.Size = new System.Drawing.Size(23,22);
            this.tbiOpen.Text = "<OPEN>";
            this.tbiOpen.Click += new System.EventHandler(this.tbiOpen_Click);
            // 
            // tbiSave
            // 
            this.tbiSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiSave.Image = global::CrashEdit.Properties.Resources.objects_029;
            this.tbiSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiSave.Name = "tbiSave";
            this.tbiSave.Size = new System.Drawing.Size(23,22);
            this.tbiSave.Text = "<SAVE>";
            this.tbiSave.Click += new System.EventHandler(this.tbiSave_Click);
            // 
            // tbiClose
            // 
            this.tbiClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiClose.Image = global::CrashEdit.Properties.Resources.Computer_File_063;
            this.tbiClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiClose.Name = "tbiClose";
            this.tbiClose.Size = new System.Drawing.Size(23,22);
            this.tbiClose.Text = "<CLOSE>";
            this.tbiClose.Click += new System.EventHandler(this.tbiClose_Click);
            // 
            // tbiPatchNSD
            // 
            this.tbiPatchNSD.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiPatchNSD.Image = global::CrashEdit.Properties.Resources.objects_070;
            this.tbiPatchNSD.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiPatchNSD.Name = "tbiPatchNSD";
            this.tbiPatchNSD.Size = new System.Drawing.Size(23,22);
            this.tbiPatchNSD.Text = "<PATCH NSD>";
            this.tbiPatchNSD.Click += new System.EventHandler(this.tbiPatchNSD_Click);
            // 
            // tbiFind
            // 
            this.tbiFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiFind.Image = global::CrashEdit.Properties.Resources.objects_036;
            this.tbiFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiFind.Name = "tbiFind";
            this.tbiFind.Size = new System.Drawing.Size(23,22);
            this.tbiFind.Text = "<FIND>";
            this.tbiFind.Click += new System.EventHandler(this.tbiFind_Click);
            // 
            // tbiFindNext
            // 
            this.tbiFindNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiFindNext.Image = global::CrashEdit.Properties.Resources.objects_037;
            this.tbiFindNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiFindNext.Name = "tbiFindNext";
            this.tbiFindNext.Size = new System.Drawing.Size(23,22);
            this.tbiFindNext.Text = "<FIND NEXT>";
            this.tbiFindNext.Click += new System.EventHandler(this.tbiFindNext_Click);
            // 
            // tbiGotoEID
            // 
            this.tbiGotoEID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiGotoEID.Image = global::CrashEdit.Properties.Resources.objects_028;
            this.tbiGotoEID.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiGotoEID.Name = "tbiGotoEID";
            this.tbiGotoEID.Size = new System.Drawing.Size(23,22);
            this.tbiGotoEID.Text = "<GOTO EID>";
            this.tbiGotoEID.Click += new System.EventHandler(this.tbiGotoEID_Click);
            // 
            // tbiUndo
            // 
            this.tbiUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiUndo.Image = global::CrashEdit.Properties.Resources.objects_079;
            this.tbiUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiUndo.Name = "tbiUndo";
            this.tbiUndo.Size = new System.Drawing.Size(23,22);
            this.tbiUndo.Text = "<UNDO>";
            this.tbiUndo.Click += new System.EventHandler(this.tbiUndo_Click);
            // 
            // tbiRedo
            // 
            this.tbiRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbiRedo.Image = global::CrashEdit.Properties.Resources.objects_020;
            this.tbiRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbiRedo.Name = "tbiRedo";
            this.tbiRedo.Size = new System.Drawing.Size(23,22);
            this.tbiRedo.Text = "<REDO>";
            this.tbiRedo.Click += new System.EventHandler(this.tbiRedo_Click);
            // 
            // mniFileOpen
            // 
            this.mniFileOpen.Image = global::CrashEdit.Properties.Resources.Computer_File_064;
            this.mniFileOpen.Name = "mniFileOpen";
            this.mniFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mniFileOpen.Size = new System.Drawing.Size(164,22);
            this.mniFileOpen.Text = "<OPEN>";
            this.mniFileOpen.Click += new System.EventHandler(this.mniFileOpen_Click);
            // 
            // mniFileSave
            // 
            this.mniFileSave.Image = global::CrashEdit.Properties.Resources.objects_029;
            this.mniFileSave.Name = "mniFileSave";
            this.mniFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mniFileSave.Size = new System.Drawing.Size(164,22);
            this.mniFileSave.Text = "<SAVE>";
            this.mniFileSave.Click += new System.EventHandler(this.mniFileSave_Click);
            // 
            // mniFilePatchNSD
            // 
            this.mniFilePatchNSD.Image = global::CrashEdit.Properties.Resources.objects_070;
            this.mniFilePatchNSD.Name = "mniFilePatchNSD";
            this.mniFilePatchNSD.Size = new System.Drawing.Size(164,22);
            this.mniFilePatchNSD.Text = "<PATCH NSD>";
            this.mniFilePatchNSD.Click += new System.EventHandler(this.mniFilePatchNSD_Click);
            // 
            // mniFileClose
            // 
            this.mniFileClose.Image = global::CrashEdit.Properties.Resources.Computer_File_063;
            this.mniFileClose.Name = "mniFileClose";
            this.mniFileClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mniFileClose.Size = new System.Drawing.Size(164,22);
            this.mniFileClose.Text = "<CLOSE>";
            this.mniFileClose.Click += new System.EventHandler(this.mniFileClose_Click);
            // 
            // mniFindFind
            // 
            this.mniFindFind.Image = global::CrashEdit.Properties.Resources.objects_036;
            this.mniFindFind.Name = "mniFindFind";
            this.mniFindFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.mniFindFind.Size = new System.Drawing.Size(175,22);
            this.mniFindFind.Text = "<FIND>";
            this.mniFindFind.Click += new System.EventHandler(this.mniFindFind_Click);
            // 
            // mniFindFindNext
            // 
            this.mniFindFindNext.Image = global::CrashEdit.Properties.Resources.objects_037;
            this.mniFindFindNext.Name = "mniFindFindNext";
            this.mniFindFindNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.mniFindFindNext.Size = new System.Drawing.Size(175,22);
            this.mniFindFindNext.Text = "<FIND NEXT>";
            this.mniFindFindNext.Click += new System.EventHandler(this.mniFindFindNext_Click);
            // 
            // mniFindEntryID
            // 
            this.mniFindEntryID.Image = global::CrashEdit.Properties.Resources.objects_028;
            this.mniFindEntryID.Name = "mniFindEntryID";
            this.mniFindEntryID.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.mniFindEntryID.Size = new System.Drawing.Size(175,22);
            this.mniFindEntryID.Text = "<ENTRY ID>";
            this.mniFindEntryID.Click += new System.EventHandler(this.mniFindEntryID_Click);
            // 
            // mniUndoRedo
            // 
            this.mniUndoRedo.Image = global::CrashEdit.Properties.Resources.objects_020;
            this.mniUndoRedo.Name = "mniUndoRedo";
            this.mniUndoRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.mniUndoRedo.Size = new System.Drawing.Size(171,22);
            this.mniUndoRedo.Text = "<REDO>";
            this.mniUndoRedo.Click += new System.EventHandler(this.mniUndoRedo_Click);
            // 
            // mniUndoUndo
            // 
            this.mniUndoUndo.Image = global::CrashEdit.Properties.Resources.objects_079;
            this.mniUndoUndo.Name = "mniUndoUndo";
            this.mniUndoUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mniUndoUndo.Size = new System.Drawing.Size(171,22);
            this.mniUndoUndo.Text = "<UNDO>";
            this.mniUndoUndo.Click += new System.EventHandler(this.mniUndoUndo_Click);
            // 
            // dlgOpenNSF
            // 
            this.dlgOpenNSF.Filter = "NSF Files (*.nsf)|*.nsf|All Files (*.*)|*.*";
            this.dlgOpenNSF.Multiselect = true;
            // 
            // mnuEdit
            // 
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(59,20);
            this.mnuEdit.Text = "<EDIT>";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632,451);
            this.Controls.Add(this.uxTabs);
            this.Controls.Add(this.uxStatusbar);
            this.Controls.Add(this.uxToolbar);
            this.Controls.Add(this.uxMenubar);
            this.MainMenuStrip = this.uxMenubar;
            this.Name = "MainForm";
            this.Text = "CrashEdit";
            this.uxTabs.ResumeLayout(false);
            this.uxToolbar.ResumeLayout(false);
            this.uxToolbar.PerformLayout();
            this.uxStatusbar.ResumeLayout(false);
            this.uxStatusbar.PerformLayout();
            this.uxMenubar.ResumeLayout(false);
            this.uxMenubar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl uxTabs;
        private System.Windows.Forms.ToolStrip uxToolbar;
        private System.Windows.Forms.StatusStrip uxStatusbar;
        private System.Windows.Forms.MenuStrip uxMenubar;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniFileClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniFileSave;
        private System.Windows.Forms.ToolStripMenuItem mniFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniFilePatchNSD;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniFileExit;
        private System.Windows.Forms.ToolStripButton tbiOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tbiSave;
        private System.Windows.Forms.ToolStripButton tbiPatchNSD;
        private System.Windows.Forms.ToolStripButton tbiClose;
        private System.Windows.Forms.ToolStripMenuItem mnuFind;
        private System.Windows.Forms.ToolStripMenuItem mniFindFind;
        private System.Windows.Forms.ToolStripMenuItem mniFindFindNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem mniFindEntryID;
        private System.Windows.Forms.ToolStripMenuItem mniFindEntryName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem mniFindObjectName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tbiFind;
        private System.Windows.Forms.ToolStripButton tbiFindNext;
        private System.Windows.Forms.ToolStripButton tbiGotoEID;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton tbiUndo;
        private System.Windows.Forms.ToolStripButton tbiRedo;
        private System.Windows.Forms.ToolStripMenuItem mnuUndo;
        private System.Windows.Forms.ToolStripMenuItem mniUndoRedo;
        private System.Windows.Forms.ToolStripMenuItem mniUndoCurrentState;
        private System.Windows.Forms.ToolStripMenuItem mniUndoUndo;
        private System.Windows.Forms.ToolStripStatusLabel sbiStatus;
        private System.Windows.Forms.ToolStripProgressBar sbiProgress;
        private System.Windows.Forms.TabPage uxStartPage;
        private System.Windows.Forms.OpenFileDialog dlgOpenNSF;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
    }
}